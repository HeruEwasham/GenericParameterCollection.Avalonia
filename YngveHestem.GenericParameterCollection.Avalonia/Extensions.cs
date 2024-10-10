using System;
using System.Collections.Generic;
using System.Globalization;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    internal static class Extensions
    {
        internal static IParameterComponentDefinition[] DefaultParameterComponents = new IParameterComponentDefinition[] 
        {
            new StringParameterComponent(),
            new IntDecimalParameterComponent(),
            new BytesParameterComponent(),
            new BoolParameterComponent(),
            new DateTimeParameterComponent(),
            new ParameterCollectionViewParameterComponent(),
            new EnumSelectOneParameterComponent(),
            new SelectManyParameterComponent(),
            new IEnumerablesParameterComponent()
        };
        
		public static ParameterCollection DeepCopyJson(this ParameterCollection parameters)
		{
			return ParameterCollection.FromJson(parameters.ToJson());
		}

        public static string HumanReadable(this string text)
        {
            return text.FirstCharToUpper();
        }

        public static string GetSizeInMemory(this int bytesize)
        {
            return GetSizeInMemory((ulong)bytesize);
        }

        public static string GetSizeInMemory(this ulong bytesize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Convert.ToDouble(bytesize);
            int order = 0;
            while (len >= 1024D && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", len, sizes[order]);
        }

        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
        }
    }
}
