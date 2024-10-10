using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Layout;
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
            List<object> itemValues;
            foreach(var item in GetListItemsAsParameters(parameter, additionalInfo, customConverters, out itemValues))
            {
                var itemControlDef = componentsList.First(p => p.ShouldComponentBeUsed(item, additionalInfo, options, customConverters));
                var parentType = itemControlDef.GetHowParameterNameIsShown(parameter, additionalInfo, options, customConverters);
                var component = itemControlDef.GetComponent(item, parameterName + " " + item.Key, additionalInfo, options, customConverters, customParameterComponents, (value, adInfo) => 
                {
                    var keyAsInt = int.Parse(item.Key);
                    if (itemValues.Count > keyAsInt) {
                        itemValues[keyAsInt] = value;
                        updateParameterValue(itemValues, adInfo); // TODO: Rewrite so itemValues are the correct type all the time (no conversions each time it is an update)
                    }
                });
                var componentArea = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                componentArea.Children.Add(component);
                if (!options.ReadOnly)
                {
                    var deleteButton = new Button
                    {
                        Content = options.DeleteEntryFromListText
                    };
                    deleteButton.Click += (sender, e) => 
                    {
                        var keyAsInt = int.Parse(item.Key);
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
            else if (parameter.Type == ParameterType.ParameterCollection)
            {
                if (additionalInfo.HasKeyAndCanConvertTo("defaultValue", typeof(ParameterCollection), customConverters))
                {
                    return additionalInfo.GetByKey<ParameterCollection>("defaultValue", customConverters);
                }
                else
                {
                    return null;
                }
            }
            
            throw new NotSupportedException("We don't support " + parameter.Type + " as an IEnumerable.");
        }

        private static IEnumerable<Parameter> GetListItemsAsParameters(Parameter parameter, ParameterCollection additionalInfo, IParameterValueConverter[] customConverters, out List<object> itemValues)
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.String, additionalInfo, null, customConverters));
                }
                itemValues = items.ToList<dynamic>();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.String_Multiline, additionalInfo, null, customConverters));
                }
                itemValues = items.ToList<dynamic>();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.Int, additionalInfo, null, customConverters));
                }
                itemValues = items.Select(x => (dynamic)x).ToList();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.Decimal, additionalInfo, null, customConverters));
                }
                itemValues = items.Select(x => (object)x).ToList();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.Bool, additionalInfo, null, customConverters));
                }
                itemValues = items.Select(x => (object)x).ToList();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.DateTime, additionalInfo, null, customConverters));
                }
                itemValues = items.Select(x => (object)x).ToList();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.Date, additionalInfo, null, customConverters));
                }
                itemValues = items.Select(x => (object)x).ToList();
                return result;
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
                    result.Add(new Parameter(i.ToString(), items[i], ParameterType.ParameterCollection, additionalInfo, null, customConverters));
                }
                itemValues = items.ToList<object>();
                return result;
            }

            throw new NotSupportedException("We don't support " + parameter.Type + " as an IEnumerable.");
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
    }
}
