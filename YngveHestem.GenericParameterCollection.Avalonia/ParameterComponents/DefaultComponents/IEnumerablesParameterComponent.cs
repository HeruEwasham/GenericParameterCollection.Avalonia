using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class IEnumerablesParameterComponent : IParameterComponentDefinition
    {
        public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            var stackPanel = new StackPanel();
            UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
            return stackPanel;
        }

        public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return ComponentParentType.Expander;
        }

        public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return parameter.Type == ParameterType.String_IEnumerable ||
            parameter.Type == ParameterType.String_Multiline_IEnumerable ||
            parameter.Type == ParameterType.Int_IEnumerable ||
            parameter.Type == ParameterType.Decimal_IEnumerable ||
            parameter.Type == ParameterType.Bool_IEnumerable ||
            parameter.Type == ParameterType.DateTime_IEnumerable ||
            parameter.Type == ParameterType.Date_IEnumerable ||
            parameter.Type == ParameterType.ParameterCollection_IEnumerable;
        }

        private static void UpdateControl(StackPanel stackPanel, Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            stackPanel.Children.Clear();
            var componentsList = GetComponentList(customParameterComponents);
            foreach(var item in GetListItemsAsParameters(parameter, additionalInfo, customConverters))
            {
                var itemControlDef = componentsList.First(p => p.ShouldComponentBeUsed(item, additionalInfo, options, customConverters));
                var parentType = itemControlDef.GetHowParameterNameIsShown(parameter, additionalInfo, options, customConverters);
                var componentArea = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                if (parentType == ComponentParentType.None)
                {
                    var component = itemControlDef.GetComponent(item, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key), additionalInfo, options, customConverters, customParameterComponents, (value, adInfo) => 
                    {
                        var valueListType = typeof(List<>).MakeGenericType(new[] { value.GetType() });
                        var itemValues = (IList)parameter.GetValue(valueListType, customConverters);
                        var keyAsInt = int.Parse(item.Key) - 1;
                        if (itemValues.Count > keyAsInt) {
                            itemValues[keyAsInt] = value;
                            updateParameterValue(itemValues, adInfo);
                        }
                    });
                    AutomationProperties.SetName(component, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key));
                    componentArea.Children.Add(component);
                }
                else if (parentType == ComponentParentType.Border || parentType == ComponentParentType.BorderWithoutName)
                {
                    var component = itemControlDef.GetComponent(item, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key), additionalInfo, options, customConverters, customParameterComponents, (value, adInfo) => 
                    {
                        var valueListType = typeof(List<>).MakeGenericType(new[] { value.GetType() });
                        var itemValues = (IList)parameter.GetValue(valueListType, customConverters);
                        var keyAsInt = int.Parse(item.Key) - 1;
                        if (itemValues.Count > keyAsInt) {
                            itemValues[keyAsInt] = value;
                            updateParameterValue(itemValues, adInfo);
                        }
                    });
                    AutomationProperties.SetName(component, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key));
                    var border = new Border
                    {
                        Background = options.BorderOptions.Background,
                        BorderBrush = options.BorderOptions.BorderBrush,
                        BorderThickness = options.BorderOptions.BorderThickness,
                        CornerRadius = options.BorderOptions.CornerRadius,
                        Margin = options.BorderOptions.Margin,
                        Padding = options.BorderOptions.Padding,
                        BoxShadow = options.BorderOptions.BoxShadow,
                        Child = component
                    };
                    componentArea.Children.Add(border);
                }
                else if (parentType == ComponentParentType.Expander)
                {
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
                            Text = string.Format(options.IEnumerableSingleItemName, parameterName, item.Key),
                            FontWeight = FontWeight.Bold,
                        }
                    };
                    if (!options.ExpanderOptions.LoadContentOnlyWhenExpanding)
                    {
                        var component = itemControlDef.GetComponent(item, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key), additionalInfo, options, customConverters, customParameterComponents, (value, adInfo) => 
                        {
                            var valueListType = typeof(List<>).MakeGenericType(new[] { value.GetType() });
                            var itemValues = (IList)parameter.GetValue(valueListType, customConverters);
                            var keyAsInt = int.Parse(item.Key) - 1;
                            if (itemValues.Count > keyAsInt) {
                                itemValues[keyAsInt] = value;
                                updateParameterValue(itemValues, adInfo);
                            }
                        });
                        AutomationProperties.SetName(component, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key));
                        expander.Content = component;
                    }
                    else
                    {
                        expander.Expanding += (s, e) =>
                        {
                            var exp = (Expander)s;
                            if (exp.Content == null)
                            {
                                var component = itemControlDef.GetComponent(item, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key), additionalInfo, options, customConverters, customParameterComponents, (value, adInfo) => 
                                {
                                    var valueListType = typeof(List<>).MakeGenericType(new[] { value.GetType() });
                                    var itemValues = (IList)parameter.GetValue(valueListType, customConverters);
                                    var keyAsInt = int.Parse(item.Key) - 1;
                                    if (itemValues.Count > keyAsInt) {
                                        itemValues[keyAsInt] = value;
                                        updateParameterValue(itemValues, adInfo);
                                    }
                                });
                                AutomationProperties.SetName(component, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key));
                                exp.Content = component;
                            }
                        };

                        if (options.ExpanderOptions.UnloadContentWhenCollapsed)
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
                            var component = itemControlDef.GetComponent(item, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key), additionalInfo, options, customConverters, customParameterComponents, (value, adInfo) => 
                            {
                                var valueListType = typeof(List<>).MakeGenericType(new[] { value.GetType() });
                                var itemValues = (IList)parameter.GetValue(valueListType, customConverters);
                                var keyAsInt = int.Parse(item.Key) - 1;
                                if (itemValues.Count > keyAsInt) {
                                    itemValues[keyAsInt] = value;
                                    updateParameterValue(itemValues, adInfo);
                                }
                            });
                            AutomationProperties.SetName(component, string.Format(options.IEnumerableSingleItemName, parameterName, item.Key));
                            expander.Content = component;
                        }
                    }
                    componentArea.Children.Add(expander);
                }
                if (!options.ReadOnly)
                {
                    var deleteButton = new Button
                    {
                        Content = options.DeleteEntryFromListText
                    };
                    deleteButton.Click += (sender, e) => 
                    {
                        var itemValues = (IList)parameter.GetValue(ToType(parameter.Type), customConverters);
                        var keyAsInt = int.Parse(item.Key) - 1;
                        if (itemValues.Count > keyAsInt) {
                            itemValues.RemoveAt(keyAsInt);
                            updateParameterValue(itemValues, null);
                            UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
                        }
                    };
                    if (parameter.CanBeConvertedTo(typeof(string))) {
                        AutomationProperties.SetHelpText(deleteButton, string.Format(options.DeleteEntryFromListAriaDescription, item.Key, parameterName, item.GetValue<string>(customConverters)));
                    }
                    else
                    {
                        AutomationProperties.SetHelpText(deleteButton, string.Format(options.DeleteEntryFromListAriaDescription, item.Key, parameterName, options.ValueCanNotBeConvertedToStringText));
                    }
                    componentArea.Children.Add(deleteButton);
                }
                stackPanel.Children.Add(componentArea);
            }
            if (!options.ReadOnly)
            {
                var addButton = new Button 
                {
                    Content = options.AddEntryToListText,
                };
                addButton.Click += (sender, e) =>
                {
                    var itemValues = (IList)parameter.GetValue(ToType(parameter.Type), customConverters);
                    if (itemValues == null)
                    {
                        itemValues = (IList)Activator.CreateInstance(ToType(parameter.Type));
                    }
                    itemValues.Add(GetDefaultValue(parameter, additionalInfo, options, customConverters));
                    updateParameterValue(itemValues, null);
                    UpdateControl(stackPanel, parameter, parameterName, additionalInfo, options, customConverters, customParameterComponents, updateParameterValue);
                };
                AutomationProperties.SetHelpText(addButton, string.Format(options.AddEntryToListAriaDescription, parameterName));
                stackPanel.Children.Add(addButton);
            }
        }

        private static object GetDefaultValue(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            if (parameter.Type == ParameterType.String_IEnumerable || parameter.Type == ParameterType.String_Multiline_IEnumerable)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(string), customConverters))
                {
                    return additionalInfo.GetByKey<string>("defaultValue", customConverters);
                }
                else if (options.IsNullable)
                {
                    return null;
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (parameter.Type == ParameterType.Int_IEnumerable)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(long), customConverters))
                {
                    return additionalInfo.GetByKey<long>("defaultValue", customConverters);
                }
                else if (options.IsNullable)
                {
                    return null;
                }
                else
                {
                    return default(long);
                }
            }
            else if (parameter.Type == ParameterType.Decimal_IEnumerable)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(decimal), customConverters))
                {
                    return additionalInfo.GetByKey<decimal>("defaultValue", customConverters);
                }
                else if (options.IsNullable)
                {
                    return null;
                }
                else
                {
                    return default(decimal);
                }
            }
            else if (parameter.Type == ParameterType.Bool_IEnumerable)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(bool), customConverters))
                {
                    return additionalInfo.GetByKey<bool>("defaultValue", customConverters);
                }
                else if (options.IsNullable)
                {
                    return null;
                }
                else
                {
                    return false;
                }
            }
            else if (parameter.Type == ParameterType.DateTime_IEnumerable || parameter.Type == ParameterType.Date_IEnumerable)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(DateTime), customConverters))
                {
                    return additionalInfo.GetByKey<DateTime>("defaultValue", customConverters);
                }
                else if (options.IsNullable)
                {
                    return null;
                }
                else
                {
                    return DateTime.Now;
                }
            }
            else if (parameter.Type == ParameterType.ParameterCollection_IEnumerable)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(ParameterCollection), customConverters))
                {
                    return additionalInfo.GetByKey<ParameterCollection>("defaultValue", customConverters);
                }
                else
                {
                    return new ParameterCollection();
                }
            }
            
            throw new NotSupportedException("We don't support " + parameter.Type + " as an IEnumerable.");
        }

        private static IEnumerable<Parameter> GetListItemsAsParameters(Parameter parameter, ParameterCollection additionalInfo, IParameterValueConverter[] customConverters)
        {
            var result = new List<Parameter>();
            if (parameter.Type == ParameterType.String_IEnumerable) 
            {
                var items = parameter.GetValue<string[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<string>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.String, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.String_Multiline_IEnumerable) 
            {
                var items = parameter.GetValue<string[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<string>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.String_Multiline, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.Int_IEnumerable) 
            {
                var items = parameter.GetValue<long?[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<long?>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.Int, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.Decimal_IEnumerable) 
            {
                var items = parameter.GetValue<decimal?[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<decimal?>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.Decimal, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.Bool_IEnumerable) 
            {
                var items = parameter.GetValue<bool?[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<bool?>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.Bool, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.DateTime_IEnumerable) 
            {
                var items = parameter.GetValue<DateTime?[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<DateTime?>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.DateTime, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.Date_IEnumerable) 
            {
                var items = parameter.GetValue<DateTime?[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<DateTime?>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.Date, additionalInfo, null, customConverters));
                }
            }
            else if (parameter.Type == ParameterType.ParameterCollection_IEnumerable) 
            {
                var items = parameter.GetValue<ParameterCollection[]>(customConverters);
                if (items == null)
                {
                    items = Array.Empty<ParameterCollection>();
                }
                for(var i = 0; i < items.Length; i++)
                {
                    result.Add(new Parameter((i+1).ToString(), items[i], ParameterType.ParameterCollection, additionalInfo, null, customConverters));
                }
            }

            return result;
        }

        private static IParameterComponentDefinition[] GetComponentList(IParameterComponentDefinition[] customParameterComponents)
        {
            if (customParameterComponents != null)
            {
                return customParameterComponents.Concat(Extensions.DefaultParameterComponents).ToArray();
            }
            else 
            {
                return Extensions.DefaultParameterComponents;
            }
        }

        private static Type ToType(ParameterType parameterType)
        {
            switch (parameterType)
            {
                case ParameterType.String_IEnumerable:
                    return typeof(List<string>);
                case ParameterType.String_Multiline_IEnumerable:
                    return typeof(List<string>);
                case ParameterType.Int_IEnumerable:
                    return typeof(List<long>);
                case ParameterType.Decimal_IEnumerable:
                    return typeof(List<decimal>);
                case ParameterType.Bool_IEnumerable:
                    return typeof(List<bool>);
                case ParameterType.DateTime_IEnumerable:
                    return typeof(List<DateTime>);
                case ParameterType.Date_IEnumerable:
                    return typeof(List<DateTime>);
                case ParameterType.ParameterCollection_IEnumerable:
                    return typeof(List<ParameterCollection>);
                default:
                    throw new NotSupportedException("We don't support " + parameterType + " as an IEnumerable.");
            }
        }
    }
}
