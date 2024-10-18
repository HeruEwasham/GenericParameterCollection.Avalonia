using System;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterConverters
{
    public class IBrushConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return targetType == typeof(IBrush) && sourceType == ParameterType.String;
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return ((sourceType == typeof(IBrush) && value == null) || typeof(ISolidColorBrush).IsAssignableFrom(sourceType)) && targetType == ParameterType.String;
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (rawValue == null || rawValue.Type == JTokenType.Null) 
            {
                return null;
            }
            var value = rawValue.ToObject<string>(jsonSerializer);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            if (!Color.TryParse(value, out var color))
            {
                return new ImmutableSolidColorBrush(Colors.Black); // If could not parse, be gentle and return the default color black.
            }

            return new ImmutableSolidColorBrush(color);
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (value == null)
            {
                return null;
            }
            return JToken.FromObject(((ISolidColorBrush)value).Color.ToString(), jsonSerializer);
        }
    }
}
