using System;
using System.Collections.Generic;
using Avalonia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterConverters
{
    public class ThicknessConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return targetType == typeof(Thickness) && (sourceType == ParameterType.String || sourceType == ParameterType.Int || sourceType == ParameterType.Decimal);
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return sourceType == typeof(Thickness) && (targetType == ParameterType.String || ((Thickness)value).IsUniform && (targetType == ParameterType.Int || targetType == ParameterType.Decimal));
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (rawValue == null || rawValue.Type == JTokenType.Null) 
            {
                return null;
            }
            if (sourceType == ParameterType.String)
            {
                return Thickness.Parse(rawValue.ToObject<string>(jsonSerializer));
            }
            else if (sourceType == ParameterType.Int || sourceType == ParameterType.Decimal) 
            {
                return new Thickness(rawValue.ToObject<double>(jsonSerializer));
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(ThicknessConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (value == null)
            {
                return null;
            }
            if (targetType == ParameterType.String)
            {
                return JToken.FromObject(((Thickness)value).ToString(), jsonSerializer);
            }
            else if (((Thickness)value).IsUniform && (targetType == ParameterType.Int || targetType == ParameterType.Decimal)) 
            {
                return JToken.FromObject(((Thickness)value).Bottom, jsonSerializer);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(ThicknessConverter));
        }
    }
}
