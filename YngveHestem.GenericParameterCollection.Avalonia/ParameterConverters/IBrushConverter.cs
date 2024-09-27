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
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return targetType == typeof(IBrush) && sourceType == ParameterType.String;
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return typeof(ISolidColorBrush).IsAssignableFrom(sourceType) && targetType == ParameterType.String;
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (!Color.TryParse(rawValue.ToObject<string>(jsonSerializer), out var color))
            {
                throw new ArgumentException("The value in the parameter could not be converted to a color.");
            }

            return new ImmutableSolidColorBrush(color);
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return JToken.FromObject(((ISolidColorBrush)value).Color.ToString(), jsonSerializer);
        }
    }
}
