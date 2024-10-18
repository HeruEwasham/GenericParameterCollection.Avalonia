using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Layout;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;
using Avalonia.Automation;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    public class ParameterCollectionView : UserControl
    {
        public ParameterCollection ParameterCollection 
        {
            get
            {
                return _currentParameterCollection;
            }
            set
            {
                _currentParameterCollection = value.DeepCopyJson();
                InitializeForm();
            }
        }

        public ParameterCollectionViewOptions Options 
        {
            get 
            {
                return _options;
            }
            set
            {
                _options = value;
                InitializeForm();
            }
        }

        /// <summary>
        /// Do you have any special components to handle a parameter instead of the default ones? Here you can define them.
        /// </summary>
        public IParameterComponentDefinition[] CustomParameterComponents 
        {
            get 
            {
                return _customParameterComponents;
            } 
            set 
            {
                SetCustomParameterComponents(value);
                InitializeForm();
            }
        }

        /// <summary>
        /// Do you have any custom converters you want to use to convert the values? Here you can define them.
        /// </summary>
        public IParameterValueConverter[] CustomConverters 
        { 
            get
            {
                return _customConverters;
            }
            set
            {
                _customConverters = value;
                InitializeForm();
            } 
        }

        public static readonly RoutedEvent<ParameterCollectionViewOnChangeEventArgs> ValueChangedEvent =
        RoutedEvent.Register<ParameterCollectionView, ParameterCollectionViewOnChangeEventArgs>(nameof(ValueChanged), RoutingStrategies.Direct);

        public event EventHandler<ParameterCollectionViewOnChangeEventArgs> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }

        protected virtual void OnValueChanged(ParameterCollection parameters, string parameterKey)
        {
            var args = new ParameterCollectionViewOnChangeEventArgs(ValueChangedEvent, parameters, parameterKey);
            RaiseEvent(args);
        }


        private ParameterCollection _currentParameterCollection;
        private ParameterCollectionViewOptions _options;
        private IParameterComponentDefinition[] _customParameterComponents;
        private IParameterComponentDefinition[] _parameterComponents = Extensions.DefaultParameterComponents;
        private IParameterValueConverter[] _customConverters;

        private StackPanel _parameterComponentList;

        public ParameterCollectionView() {}

        public ParameterCollectionView(ParameterCollection parameters, ParameterCollectionViewOptions options = null, IParameterComponentDefinition[] customParameterComponents = null, IParameterValueConverter[] customConverters = null)
        {
            _currentParameterCollection = parameters.DeepCopyJson();
            _options = options;
            SetCustomParameterComponents(customParameterComponents);
            _customConverters = customConverters;
            InitializeForm();
        }

        private void InitializeForm() 
        {
            if (_options == null)
            {
                _options = new ParameterCollectionViewOptions();
            }
            _parameterComponentList = new StackPanel();
            _options.MakeValid();
            if (_currentParameterCollection != null)
            {
                foreach (var parameter in _currentParameterCollection) 
                {
                    var additionalInfo = parameter.GetAdditionalInfo();
                    var localOptions = _options;
                    if (Options.AdditionalInfoWillOverride && additionalInfo != null)
                    {
                        localOptions = ParameterCollectionViewOptions.CreateFromParameterCollection(additionalInfo, _options);
                        localOptions.MakeValid();
                    }

                    if (additionalInfo == null)
                    {
                        additionalInfo = new ParameterCollection();
                    }

                    var parameterText = parameter.Key;
                    if (additionalInfo.HasKeyAndCanConvertTo(localOptions.ReadableParameterTextKey, typeof(string)))
                    {
                        parameterText = additionalInfo.GetByKey<string>(localOptions.ReadableParameterTextKey);
                    }
                    else if (localOptions.ShowParameterNameAsHumanReadable)
                    {
                        parameterText = parameterText.HumanReadable();
                    }

                    if (_parameterComponents.Any(p => p.ShouldComponentBeUsed(parameter, additionalInfo, localOptions, CustomConverters)))
                    {
                        var def = _parameterComponents.First(p => p.ShouldComponentBeUsed(parameter, additionalInfo, localOptions, CustomConverters));
                        var parentType = def.GetHowParameterNameIsShown(parameter, additionalInfo, localOptions, CustomConverters);
                        if (parentType == ComponentParentType.None)
                        {
                            var component = def.GetComponent(parameter, parameterText, additionalInfo, localOptions, CustomConverters, CustomParameterComponents, (value, adInfo) => ChangeParameter(parameter.Key, value, adInfo));
                            if (additionalInfo.HasKeyAndCanConvertTo(localOptions.TooltipParameterTextKey, typeof(string))) 
                            {
                                ToolTip.SetTip(component, additionalInfo.GetByKey<string>(localOptions.TooltipParameterTextKey));
                            }
                            AutomationProperties.SetName(component, parameterText);
                            _parameterComponentList.Children.Add(component);
                        }
                        else if (parentType == ComponentParentType.Border || parentType == ComponentParentType.BorderWithoutName)
                        {
                            var component = def.GetComponent(parameter, parameterText, additionalInfo, localOptions, CustomConverters, CustomParameterComponents, (value, adInfo) => ChangeParameter(parameter.Key, value, adInfo));
                            var border = new Border
                            {
                                Background = localOptions.BorderOptions.Background,
                                BorderBrush = localOptions.BorderOptions.BorderBrush,
                                BorderThickness = localOptions.BorderOptions.BorderThickness,
                                CornerRadius = localOptions.BorderOptions.CornerRadius,
                                Margin = localOptions.BorderOptions.Margin,
                                Padding = localOptions.BorderOptions.Padding,
                                BoxShadow = localOptions.BorderOptions.BoxShadow
                            };

                            if (parentType == ComponentParentType.Border)
                            {
                                var stackPanel = new StackPanel();
                                stackPanel.Children.Add(new TextBlock {
                                    Text = parameterText,
                                    FontWeight = FontWeight.Bold,
                                });
                                stackPanel.Children.Add(component);
                                border.Child = stackPanel;
                            }
                            else 
                            {
                                border.Child = component;
                            }

                            if (additionalInfo.HasKeyAndCanConvertTo(localOptions.TooltipParameterTextKey, typeof(string))) 
                            {
                                ToolTip.SetTip(border, additionalInfo.GetByKey<string>(localOptions.TooltipParameterTextKey));
                            }

                            _parameterComponentList.Children.Add(border);
                        }
                        else if (parentType == ComponentParentType.Expander)
                        {
                            var expander = new Expander
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
                                    Text = parameterText,
                                    FontWeight = FontWeight.Bold,
                                }
                            };
                            if (!localOptions.ExpanderOptions.LoadContentOnlyWhenExpanding)
                            {
                                expander.Content = def.GetComponent(parameter, parameterText, additionalInfo, localOptions, CustomConverters, CustomParameterComponents, (value, adInfo) => ChangeParameter(parameter.Key, value, adInfo));
                            }
                            else
                            {
                                expander.Expanding += (s, e) =>
                                {
                                    var exp = (Expander)s;
                                    if (exp.Content == null)
                                    {
                                        exp.Content = def.GetComponent(parameter, parameterText, additionalInfo, localOptions, CustomConverters, CustomParameterComponents, (value, adInfo) => ChangeParameter(parameter.Key, value, adInfo));
                                    }
                                };

                                if (localOptions.ExpanderOptions.UnloadContentWhenCollapsed)
                                {
                                    expander.Collapsed += (s, e) =>
                                    {
                                        ((Expander)s).Content = null;
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                    };
                                }
                                
                                if (expander.IsExpanded && expander.Content == null)
                                {
                                    expander.Content = def.GetComponent(parameter, parameterText, additionalInfo, localOptions, CustomConverters, CustomParameterComponents, (value, adInfo) => ChangeParameter(parameter.Key, value, adInfo));
                                }
                            }

                            if (additionalInfo.HasKeyAndCanConvertTo(localOptions.TooltipParameterTextKey, typeof(string))) 
                            {
                                ToolTip.SetTip(expander, additionalInfo.GetByKey<string>(localOptions.TooltipParameterTextKey));
                            }
                            _parameterComponentList.Children.Add(expander);
                        }
                    }
                }

                Content = new ScrollViewer 
                {
                    Content = _parameterComponentList
                };
            }
        }

        private void ChangeParameter(string parameterKey, object value)
        {
            if (_currentParameterCollection.GetParameterByKey(parameterKey).SetValue(value, CustomConverters))
            {
                OnValueChanged(_currentParameterCollection, parameterKey);
            }
        }

        private void ChangeParameter(string parameterKey, object value, ParameterCollection additionalInfo)
        {
            if (additionalInfo == null)
            {
                ChangeParameter(parameterKey, value);
            }
            else if (_currentParameterCollection.GetParameterByKey(parameterKey).SetValue(value, additionalInfo, CustomConverters))
            {
                OnValueChanged(_currentParameterCollection, parameterKey);
            }
        }

        private void SetCustomParameterComponents(IParameterComponentDefinition[] value)
        {
            _customParameterComponents = value;
            if (value != null)
            {
                _parameterComponents = value.Concat(Extensions.DefaultParameterComponents).ToArray();
            }
            else 
            {
                _parameterComponents = Extensions.DefaultParameterComponents;
            }
        }
    }
}
