using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class EnumSelectOneParameterComponent : IParameterComponentDefinition
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
            return parameter.Type == ParameterType.Enum || parameter.Type == ParameterType.SelectOne;
        }

        public static void UpdateControl(StackPanel stackPanel, Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue) 
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
            var comboBox = new ComboBox
            {
                IsEnabled = !options.ReadOnly
            };
            foreach(var item in MakePretty(parameter.GetChoices(), additionalInfo)) 
            {
                comboBox.Items.Add(item);
            }
            var parameterValue = parameter.GetValue<string>(customConverters);
            comboBox.SelectedValue = MakePretty(parameterValue, additionalInfo);

            var stackPanelInBorder = new StackPanel();
            stackPanelInBorder.Children.Add(new TextBlock {
                Text = parameterName,
                FontWeight = FontWeight.Bold,
            });
            
            border.Child = stackPanelInBorder;

            if (HasExtraInfo(additionalInfo, parameter.GetChoices()) || additionalInfo.HasKeyAndCanConvertTo("parametersIf:null", typeof(ParameterCollection)))
            {
                comboBox.SelectionChanged += (sender, args) => 
                {
                    updateParameterValue(ReversePretty((string)((ComboBox)sender).SelectedValue, additionalInfo), null);
                    UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
                };
                ParameterCollectionView extraParameters = null;
                var localOptions = options;
                string valueAsRefText = null;
                var extraParametersName = string.Format(options.ExtraParametersName, parameterName, comboBox.SelectedValue);
                if (additionalInfo.HasKeyAndCanConvertTo($"parametersIf:{parameterValue}", typeof(ParameterCollection)))
                {
                    valueAsRefText = parameterValue;
                }
                else if (parameterValue == string.Empty && additionalInfo.HasKeyAndCanConvertTo("parametersIf:null", typeof(ParameterCollection)))
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
                    extraParameters = new ParameterCollectionView(additionalInfo.GetByKey<ParameterCollection>($"parametersIf:{valueAsRefText}", customConverters), localOptions, customParameterComponents, customConverters);
                    extraParameters.ValueChanged += (sender, e) => {
                        additionalInfo.GetParameterByKey($"parametersIf:{valueAsRefText}").SetValue(e.NewParameterCollection, customConverters);
                        updateParameterValue(ReversePretty((string)comboBox.SelectedValue, additionalInfo), additionalInfo);
                    };
                }
                
                if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.None) 
                {
                    stackPanelInBorder.Children.Add(comboBox);
                    stackPanel.Children.Add(border);
                    if (extraParameters != null)
                    {
                        stackPanel.Children.Add(extraParameters);
                    }
                }
                else if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.ExpanderOnOnlyCollection)
                {
                    stackPanelInBorder.Children.Add(comboBox);
                    stackPanel.Children.Add(border);
                    if (extraParameters != null)
                    {
                        stackPanel.Children.Add(new Expander
                        {
                            Background = localOptions.ExpanderOptions.Background,
                            BorderBrush = localOptions.ExpanderOptions.BorderBrush,
                            BorderThickness = localOptions.ExpanderOptions.BorderThickness,
                            CornerRadius = localOptions.ExpanderOptions.CornerRadius,
                            Margin = localOptions.ExpanderOptions.Margin,
                            Padding = localOptions.ExpanderOptions.Padding,
                            ExpandDirection = localOptions.ExpanderOptions.ExpandDirection,
                            IsExpanded = localOptions.ExpanderOptions.IsExpanded,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Header = new TextBlock {
                                Text = extraParametersName,
                                FontWeight = FontWeight.Bold,
                            },
                            Content = extraParameters
                        });
                    }
                }
                else if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.ExpanderOverWholeParameter)
                {
                    var contentStackPanel = new StackPanel();
                    contentStackPanel.Children.Add(comboBox);
                    if (extraParameters != null)
                    {
                        contentStackPanel.Children.Add(extraParameters);
                    }
                    stackPanel.Children.Add(new Expander
                    {
                        Background = localOptions.ExpanderOptions.Background,
                        BorderBrush = localOptions.ExpanderOptions.BorderBrush,
                        BorderThickness = localOptions.ExpanderOptions.BorderThickness,
                        CornerRadius = localOptions.ExpanderOptions.CornerRadius,
                        Margin = localOptions.ExpanderOptions.Margin,
                        Padding = localOptions.ExpanderOptions.Padding,
                        ExpandDirection = localOptions.ExpanderOptions.ExpandDirection,
                        IsExpanded = localOptions.ExpanderOptions.IsExpanded,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Header = new TextBlock {
                            Text = parameterName,
                            FontWeight = FontWeight.Bold,
                        },
                        Content = contentStackPanel
                    });
                }
            }
            else 
            {
                comboBox.SelectionChanged += (sender, args) => 
                {
                    updateParameterValue(ReversePretty((string)((ComboBox)sender).SelectedValue, additionalInfo), null);
                };
                stackPanelInBorder.Children.Add(comboBox);
                stackPanel.Children.Add(border);
            }
        }

        private static string ReversePretty(string selectedValue, ParameterCollection additionalInfo)
        {
            if (additionalInfo.HasKeyAndCanConvertTo("prettyValues", typeof(ParameterCollection)))
            {
                foreach(var parameter in additionalInfo.GetByKey<ParameterCollection>("prettyValues"))
                {
                    if (parameter.GetValue<string>() == selectedValue) 
                    {
                        return parameter.Key;
                    }
                }
            }
            return selectedValue;
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

        private static IEnumerable<string> MakePretty(IEnumerable<string> enumerable, ParameterCollection additionalInfo)
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
            return enumerable;
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
