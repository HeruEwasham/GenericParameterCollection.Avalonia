using System;
using Avalonia.Controls;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class StringParameterComponent : IParameterComponentDefinition
    {
        private const char PASSWORD_CHAR = '*';
        public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            var textBox = new TextBox 
            {
                Text = parameter.GetValue<string>(customConverters),
                IsReadOnly = options.ReadOnly,
                Watermark = options.PlaceholderText
            };
            textBox.TextChanged += (sender, e) => 
            {
                updateParameterValue(((TextBox)sender).Text, null);
            };
            if (options.MaxNumberOfCharacters.HasValue) 
            {
                textBox.MaxLength = options.MaxNumberOfCharacters.Value;
            }
            if (options.IsPassword)
            {
                textBox.PasswordChar = PASSWORD_CHAR;
            }
            
            if (parameter.Type == ParameterType.String_Multiline)
            {
                textBox.AcceptsReturn = true;
                textBox.Height = options.TextAreaHeight;
                if (options.TextAreaWidth.HasValue) 
                {
                    textBox.Width = options.TextAreaWidth.Value;
                }
            }

            return textBox;
        }

        public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return ComponentParentType.Border;
        }

        public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return parameter.Type == ParameterType.String || parameter.Type == ParameterType.String_Multiline;
        }
    }
}
