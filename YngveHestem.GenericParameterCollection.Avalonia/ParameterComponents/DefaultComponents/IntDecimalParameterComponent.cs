using System;
using Avalonia.Controls;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class IntDecimalParameterComponent : IParameterComponentDefinition
    {
        public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            var control = new NumericUpDown 
            {
                Minimum = options.MinNumber,
                Maximum = options.MaxNumber,
                IsReadOnly = options.ReadOnly,
                Watermark = options.PlaceholderText
            };

            if (parameter.Type == ParameterType.Int)
            {
                control.Increment = options.StepInteger;
                control.FormatString = options.NumberFormatInt;
                control.ParsingNumberStyle = System.Globalization.NumberStyles.Integer;

                if (options.IsNullable)
                {
                    control.Value = parameter.GetValue<int?>(customConverters);
                    control.ValueChanged += (sender, e) => updateParameterValue((int?)e.NewValue, null);
                }
                else
                {
                    if (parameter.HasValue()) 
                    {
                        control.Value = parameter.GetValue<int>(customConverters);
                    }
                    else 
                    {
                        control.Value = null;
                    }

                    control.ValueChanged += (sender, e) => 
                    {
                        if (e.NewValue.HasValue)
                        {
                            updateParameterValue((int)e.NewValue.Value, null);
                        }
                    };
                }
            }
            else if (parameter.Type == ParameterType.Decimal)
            {
                control.Increment = options.StepDecimal;
                control.FormatString = options.NumberFormatDecimal;
                control.ParsingNumberStyle = System.Globalization.NumberStyles.Number;

                if (options.IsNullable)
                {
                    control.Value = parameter.GetValue<decimal?>(customConverters);
                    control.ValueChanged += (sender, e) => updateParameterValue(e.NewValue, null);
                }
                else
                {
                    if (parameter.HasValue()) 
                    {
                        control.Value = parameter.GetValue<decimal>(customConverters);
                    }
                    else 
                    {
                        control.Value = null;
                    }

                    control.ValueChanged += (sender, e) => 
                    {
                        if (e.NewValue.HasValue)
                        {
                            updateParameterValue(e.NewValue.Value, null);
                        }
                    };
                }
            }

            return control;
        }

        public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return ComponentParentType.Border;
        }

        public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return parameter.Type == ParameterType.Int || parameter.Type == ParameterType.Decimal;
        }
    }
}
