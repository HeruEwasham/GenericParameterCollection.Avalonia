using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    public static class Extensions
    {
        internal static IParameterComponentDefinition[] DefaultParameterComponents = new IParameterComponentDefinition[] 
        {
            
        };
        
		public static ParameterCollection DeepCopyJson(this ParameterCollection parameters)
		{
			return ParameterCollection.FromJson(parameters.ToJson());
		}

        public static string HumanReadable(this string text)
        {
            return text.FirstCharToUpper();
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
