using System;
using System.Collections.Generic;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    public class BoxShadowsConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return targetType == typeof(BoxShadows) && sourceType == ParameterType.String;
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return sourceType == typeof(BoxShadows) && targetType == ParameterType.String;
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (rawValue == null || rawValue.Type == JTokenType.Null) 
            {
                return null;
            }
            try 
            {
                return BoxShadows.Parse(rawValue.ToObject<string>(jsonSerializer));
            }
            catch 
            {
                throw new ArgumentException("The values was not supported to be converted by " + nameof(BoxShadowsConverter));
            }
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (value == null)
            {
                return null;
            }
            return JToken.FromObject(((BoxShadows)value).ToString(), jsonSerializer);
        }
    }
}
