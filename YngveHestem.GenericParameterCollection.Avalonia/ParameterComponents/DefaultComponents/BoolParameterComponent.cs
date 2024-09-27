using System;
using Avalonia.Controls;
using Avalonia.Media;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class BoolParameterComponent : IParameterComponentDefinition
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
            return parameter.Type == ParameterType.Bool;
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
            var checkBox = new CheckBox 
            {
                IsThreeState = options.IsNullable,
                IsChecked = parameter.GetValue<bool?>(customConverters),
                IsEnabled = !options.ReadOnly
            };

            var stackPanelInBorder = new StackPanel();
            stackPanelInBorder.Children.Add(new TextBlock {
                Text = parameterName,
                FontWeight = FontWeight.Bold,
            });
            stackPanelInBorder.Children.Add(checkBox);
            border.Child = stackPanelInBorder;

            if (additionalInfo.HasKeyAndCanConvertTo("parametersIf:true", typeof(ParameterCollection)) || additionalInfo.HasKeyAndCanConvertTo("parametersIf:false", typeof(ParameterCollection)) || additionalInfo.HasKeyAndCanConvertTo("parametersIf:null", typeof(ParameterCollection)))
            {
                checkBox.IsCheckedChanged += (sender, args) => 
                {
                    updateParameterValue(((CheckBox)sender).IsChecked, null);
                    UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
                };
                ParameterCollectionView extraParameters = null;
                var localOptions = options;
                string valueAsRefText = null;
                var extraParametersName = string.Format(options.ExtraParametersName, parameterName, checkBox.IsChecked);
                if (checkBox.IsChecked.HasValue && checkBox.IsChecked.Value && additionalInfo.HasKeyAndCanConvertTo("parametersIf:true", typeof(ParameterCollection)))
                {
                    valueAsRefText = "true";
                }
                else if (checkBox.IsChecked.HasValue && !checkBox.IsChecked.Value && additionalInfo.HasKeyAndCanConvertTo("parametersIf:false", typeof(ParameterCollection)))
                {
                    valueAsRefText = "false";
                }
                else if (!checkBox.IsChecked.HasValue && additionalInfo.HasKeyAndCanConvertTo("parametersIf:null", typeof(ParameterCollection)))
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
                        updateParameterValue(checkBox.IsChecked, additionalInfo);
                    };
                }
                
                if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.None) 
                {
                    stackPanel.Children.Add(border);
                    if (extraParameters != null)
                    {
                        stackPanel.Children.Add(extraParameters);
                    }
                }
                else if (options.ParentTypeWhenHavingExtraParameters == ExtraParametersParentType.ExpanderOnOnlyCollection)
                {
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
                    contentStackPanel.Children.Add(checkBox);
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
                checkBox.IsCheckedChanged += (sender, args) => 
                {
                    updateParameterValue(((CheckBox)sender).IsChecked, null);
                };
                stackPanel.Children.Add(border);
            }
        }
    }
}
