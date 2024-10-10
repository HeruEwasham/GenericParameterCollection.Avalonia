using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class SelectManyParameterComponent : IParameterComponentDefinition
    {
        public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            var stackPanel = new StackPanel();
            UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
            return stackPanel;
        }

        public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return ComponentParentType.None;
        }

        public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return parameter.Type == ParameterType.SelectMany;
        }

        private static void UpdateControl(StackPanel stackPanel, Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue) 
        {
            stackPanel.Children.Clear();
            var border = new Border
            {
                Background = options.BorderOptions.Background,
                BorderBrush = options.BorderOptions.BorderBrush,
                BorderThickness = options.BorderOptions.BorderThickness,
                CornerRadius = options.BorderOptions.CornerRadius,
                Margin = options.BorderOptions.Margin,
                Padding = options.BorderOptions.Padding,
                BoxShadow = options.BorderOptions.BoxShadow
            };
            var listBox = new ListBox 
            {
                SelectionMode = SelectionMode.Multiple | SelectionMode.Toggle,
                IsEnabled = !options.ReadOnly
            };
            AutomationProperties.SetName(listBox, parameterName);
            AutomationProperties.SetName(border, parameterName);
            foreach(var item in MakePretty(parameter.GetChoices(), additionalInfo)) 
            {
                listBox.Items.Add(item);
            }
            var parameterValue = parameter.GetValue<string[]>(customConverters);
            listBox.SelectedItems = MakePretty(parameterValue, additionalInfo);

            var stackPanelInBorder = new StackPanel();
            stackPanelInBorder.Children.Add(new TextBlock {
                Text = parameterName,
                FontWeight = FontWeight.Bold,
            });
            
            border.Child = stackPanelInBorder;

            if (HasExtraInfo(additionalInfo, parameter.GetChoices()) || additionalInfo.HasKeyAndCanConvertTo("parametersIf:null", typeof(ParameterCollection)))
            {
                listBox.SelectionChanged += (sender, args) => 
                {
                    updateParameterValue(ReversePretty((IEnumerable<string>)((ListBox)sender).SelectedItems, additionalInfo), null);
                    UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
                };
                var extraParameters = new Dictionary<string, ParameterCollectionView>();
                var extraParametersOptions = new Dictionary<string, ParameterCollectionViewOptions>();
                var extraParametersNames = new Dictionary<string, string>();
                foreach(string item in parameterValue) {
                    var localOptions = options;
                    string valueAsRefText = null;
                    var extraParametersName = string.Format(options.ExtraParametersName, parameterName, MakePretty(item, additionalInfo));
                    if (additionalInfo.HasKeyAndCanConvertTo($"parametersIf:{item}", typeof(ParameterCollection)))
                    {
                        valueAsRefText = item;
                    }
                    else if (item == string.Empty && additionalInfo.HasKeyAndCanConvertTo("parametersIf:null", typeof(ParameterCollection)))
                    {
                        valueAsRefText = "null";
                    }

                    if (valueAsRefText != null) 
                    {
                        if (additionalInfo.HasKeyAndCanConvertTo($"parametersIf:{valueAsRefText}:name", typeof(string)))
                        {
                            extraParametersName = additionalInfo.GetByKey<string>($"parametersIf:{valueAsRefText}:name");
                        }
                        if (options.AdditionalInfoWillOverride && additionalInfo.HasKeyAndCanConvertTo($"parametersIf:{valueAsRefText}:options", typeof(ParameterCollection)))
                        {
                            localOptions = ParameterCollectionViewOptions.CreateFromParameterCollection(additionalInfo.GetByKey<ParameterCollection>($"parametersIf:{valueAsRefText}:options"), options);
                        }
                        var extraParametersItem = new ParameterCollectionView(additionalInfo.GetByKey<ParameterCollection>($"parametersIf:{valueAsRefText}", customConverters), localOptions, customParameterComponents, customConverters);
                        extraParametersItem.ValueChanged += (sender, e) => {
                            additionalInfo.GetParameterByKey($"parametersIf:{valueAsRefText}").SetValue(e.NewParameterCollection, customConverters);
                            updateParameterValue(listBox.SelectedItems, additionalInfo);
                        };
                        extraParameters.Add(item, extraParametersItem);
                        extraParametersOptions.Add(item, localOptions);
                        extraParametersNames.Add(item, extraParametersName);
                    }
                }
                
                
                if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.None) 
                {
                    stackPanelInBorder.Children.Add(listBox);
                    stackPanel.Children.Add(border);
                    if (options.SelectManyExtraParametersGetOwnParent)
                    {
                        foreach (var item in extraParameters)
                        {
                            var expander = new Expander
                            {
                                Background = extraParametersOptions[item.Key].ExpanderOptions.Background,
                                BorderBrush = extraParametersOptions[item.Key].ExpanderOptions.BorderBrush,
                                BorderThickness = extraParametersOptions[item.Key].ExpanderOptions.BorderThickness,
                                CornerRadius = extraParametersOptions[item.Key].ExpanderOptions.CornerRadius,
                                Margin = extraParametersOptions[item.Key].ExpanderOptions.Margin,
                                Padding = extraParametersOptions[item.Key].ExpanderOptions.Padding,
                                ExpandDirection = extraParametersOptions[item.Key].ExpanderOptions.ExpandDirection,
                                IsExpanded = extraParametersOptions[item.Key].ExpanderOptions.IsExpanded,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Header = new TextBlock {
                                    Text = extraParametersNames[item.Key],
                                    FontWeight = FontWeight.Bold,
                                },
                                Content = item.Value
                            };
                            AutomationProperties.SetName(expander, extraParametersNames[item.Key]);
                            stackPanel.Children.Add(expander);
                        }
                    }
                    else 
                    {
                        foreach (var item in extraParameters)
                        {
                            stackPanel.Children.Add(item.Value);
                        }
                    }
                }
                else if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.ExpanderOnOnlyCollection)
                {
                    stackPanelInBorder.Children.Add(listBox);
                    stackPanel.Children.Add(border);
                    var stackPanelInParentExpander = new StackPanel();
                    var expander = new Expander
                    {
                        Background = options.ExpanderOptions.Background,
                        BorderBrush = options.ExpanderOptions.BorderBrush,
                        BorderThickness = options.ExpanderOptions.BorderThickness,
                        CornerRadius = options.ExpanderOptions.CornerRadius,
                        Margin = options.ExpanderOptions.Margin,
                        Padding = options.ExpanderOptions.Padding,
                        ExpandDirection = options.ExpanderOptions.ExpandDirection,
                        IsExpanded = options.ExpanderOptions.IsExpanded,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Header = new TextBlock {
                            Text = string.Format(options.SelectManyExtraParametersName, parameterName),
                            FontWeight = FontWeight.Bold,
                        },
                        Content = stackPanelInParentExpander
                    };
                    AutomationProperties.SetName(expander, string.Format(options.SelectManyExtraParametersName, parameterName));
                    if (options.SelectManyExtraParametersGetOwnParent)
                    {
                        foreach (var item in extraParameters)
                        {
                            var childExpander = new Expander
                            {
                                Background = extraParametersOptions[item.Key].ExpanderOptions.Background,
                                BorderBrush = extraParametersOptions[item.Key].ExpanderOptions.BorderBrush,
                                BorderThickness = extraParametersOptions[item.Key].ExpanderOptions.BorderThickness,
                                CornerRadius = extraParametersOptions[item.Key].ExpanderOptions.CornerRadius,
                                Margin = extraParametersOptions[item.Key].ExpanderOptions.Margin,
                                Padding = extraParametersOptions[item.Key].ExpanderOptions.Padding,
                                ExpandDirection = extraParametersOptions[item.Key].ExpanderOptions.ExpandDirection,
                                IsExpanded = extraParametersOptions[item.Key].ExpanderOptions.IsExpanded,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Header = new TextBlock {
                                    Text = extraParametersNames[item.Key],
                                    FontWeight = FontWeight.Bold,
                                },
                                Content = item.Value
                            };
                            AutomationProperties.SetName(childExpander,extraParametersNames[item.Key]);
                            stackPanelInParentExpander.Children.Add(childExpander);
                        }
                    }
                    else 
                    {
                        foreach (var item in extraParameters)
                        {
                            stackPanelInParentExpander.Children.Add(item.Value);
                        }
                    }
                    stackPanel.Children.Add(expander);
                }
                else if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.ExpanderOverWholeParameter)
                {
                    var contentStackPanel = new StackPanel();
                    contentStackPanel.Children.Add(listBox);
                    if (options.SelectManyExtraParametersGetOwnParent)
                    {
                        foreach (var item in extraParameters)
                        {
                            var childExpander = new Expander
                            {
                                Background = extraParametersOptions[item.Key].ExpanderOptions.Background,
                                BorderBrush = extraParametersOptions[item.Key].ExpanderOptions.BorderBrush,
                                BorderThickness = extraParametersOptions[item.Key].ExpanderOptions.BorderThickness,
                                CornerRadius = extraParametersOptions[item.Key].ExpanderOptions.CornerRadius,
                                Margin = extraParametersOptions[item.Key].ExpanderOptions.Margin,
                                Padding = extraParametersOptions[item.Key].ExpanderOptions.Padding,
                                ExpandDirection = extraParametersOptions[item.Key].ExpanderOptions.ExpandDirection,
                                IsExpanded = extraParametersOptions[item.Key].ExpanderOptions.IsExpanded,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Header = new TextBlock {
                                    Text = extraParametersNames[item.Key],
                                    FontWeight = FontWeight.Bold,
                                },
                                Content = item.Value
                            };
                            AutomationProperties.SetName(childExpander, extraParametersNames[item.Key]);
                            contentStackPanel.Children.Add(childExpander);
                        }
                    }
                    else 
                    {
                        foreach (var item in extraParameters)
                        {
                            contentStackPanel.Children.Add(item.Value);
                        }
                    }
                    var expander = new Expander
                    {
                        Background = options.ExpanderOptions.Background,
                        BorderBrush = options.ExpanderOptions.BorderBrush,
                        BorderThickness = options.ExpanderOptions.BorderThickness,
                        CornerRadius = options.ExpanderOptions.CornerRadius,
                        Margin = options.ExpanderOptions.Margin,
                        Padding = options.ExpanderOptions.Padding,
                        ExpandDirection = options.ExpanderOptions.ExpandDirection,
                        IsExpanded = options.ExpanderOptions.IsExpanded,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Header = new TextBlock {
                            Text = parameterName,
                            FontWeight = FontWeight.Bold,
                        },
                        Content = contentStackPanel
                    };
                    AutomationProperties.SetName(expander, parameterName);
                    stackPanel.Children.Add(expander);
                }
            }
            else 
            {
                listBox.SelectionChanged += (sender, args) => 
                {
                    updateParameterValue(ReversePretty((IEnumerable<string>)((ListBox)sender).SelectedItems, additionalInfo), null);
                };
                stackPanelInBorder.Children.Add(listBox);
                stackPanel.Children.Add(border);
            }
        }

        private static IEnumerable<string> ReversePretty(IEnumerable<string> selectedValues, ParameterCollection additionalInfo)
        {
            if (additionalInfo.HasKeyAndCanConvertTo("prettyValues", typeof(ParameterCollection)))
            {
                var prettyValues = new Dictionary<string, string>();
                foreach(var parameter in additionalInfo.GetByKey<ParameterCollection>("prettyValues"))
                {
                    prettyValues.Add(parameter.GetValue<string>(), parameter.Key);
                }

                var result = new List<string>();
                foreach(var value in selectedValues) 
                {
                    if (prettyValues.ContainsKey(value))
                    {
                        result.Add(prettyValues[value]);
                    }
                    else
                    {
                        result.Add(value);
                    }
                }
                return result;
            }
            return selectedValues;
        }

        private static string MakePretty(string parameterValue, ParameterCollection additionalInfo)
        {
            if (additionalInfo.HasKeyAndCanConvertTo("prettyValues", typeof(ParameterCollection)))
            {
                var prettyValues = additionalInfo.GetByKey<ParameterCollection>("prettyValues");
                if (prettyValues.HasKeyAndCanConvertTo(parameterValue, typeof(string)))
                {
                    return prettyValues.GetByKey<string>(parameterValue);
                }
            }

            return parameterValue;
        }

        private static List<string> MakePretty(IEnumerable<string> enumerable, ParameterCollection additionalInfo)
        {
            if (additionalInfo.HasKeyAndCanConvertTo("prettyValues", typeof(ParameterCollection))) 
            {
                var prettyValues = additionalInfo.GetByKey<ParameterCollection>("prettyValues");
                var result = new List<string>();
                foreach (var value in enumerable)
                {
                    if (prettyValues.HasKeyAndCanConvertTo(value, typeof(string)))
                    {
                        result.Add(prettyValues.GetByKey<string>(value));
                    }
                    else
                    {
                        result.Add(value);
                    }
                }
                return result;
            }
            return enumerable.ToList();
        }

        private static bool HasExtraInfo(ParameterCollection additionalInfo, IEnumerable<string> choices)
        {
            foreach(var choice in choices)
            {
                if (additionalInfo.HasKeyAndCanConvertTo($"parametersIf:{choice}", typeof(ParameterCollection)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
