using System;
using System.Collections.Generic;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace Testproject.Desktop;

public class ColorConverter : IParameterValueConverter
{
    public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
    {
        return targetType == typeof(Color) && sourceType == ParameterType.String && Color.TryParse(rawValue.ToObject<string>(), out _);
    }

    public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
    {
        return sourceType == typeof(Color) && targetType == ParameterType.String;
    }

    public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
    {
        var value = rawValue.ToObject<string>();
        if (value == null)
        {
            return null;
        }
        return Color.Parse(value);
    }

    public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
    {
        if (value == null)
        {
            return null;
        }
        return value.ToString();
    }
}
