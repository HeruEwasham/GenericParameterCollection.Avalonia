﻿using System;
using System.Collections.Generic;
using Avalonia;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterConverters
{
    public class CornerRadiusConverter : IParameterValueConverter
    {
        public bool CanConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            return targetType == typeof(CornerRadius) && (sourceType == ParameterType.String || sourceType == ParameterType.Int || sourceType == ParameterType.Decimal);
        }

        public bool CanConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters)
        {
            return sourceType == typeof(CornerRadius) && (targetType == ParameterType.String || ((CornerRadius)value).IsUniform && (targetType == ParameterType.Int || targetType == ParameterType.Decimal));
        }

        public object ConvertFromParameter(ParameterType sourceType, Type targetType, JToken rawValue, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (rawValue == null || rawValue.Type == JTokenType.Null) 
            {
                return null;
            }
            if (sourceType == ParameterType.String)
            {
                return CornerRadius.Parse(rawValue.ToObject<string>(jsonSerializer));
            }
            else if (sourceType == ParameterType.Int || sourceType == ParameterType.Decimal) 
            {
                return new CornerRadius(rawValue.ToObject<double>(jsonSerializer));
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(CornerRadiusConverter));
        }

        public JToken ConvertFromValue(ParameterType targetType, Type sourceType, object value, ParameterCollection additionalInfo, IEnumerable<IParameterValueConverter> customConverters, JsonSerializer jsonSerializer)
        {
            if (value == null)
            {
                return null;
            }
            if (targetType == ParameterType.String)
            {
                return JToken.FromObject(((CornerRadius)value).ToString(), jsonSerializer);
            }
            else if (((CornerRadius)value).IsUniform && (targetType == ParameterType.Int || targetType == ParameterType.Decimal)) 
            {
                return JToken.FromObject(((CornerRadius)value).BottomLeft, jsonSerializer);
            }

            throw new ArgumentException("The values was not supported to be converted by " + nameof(CornerRadiusConverter));
        }
    }
}
