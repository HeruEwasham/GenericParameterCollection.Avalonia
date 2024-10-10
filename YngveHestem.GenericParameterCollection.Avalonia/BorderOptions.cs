using Avalonia.Media;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    [AttributeConvertible]
    public class BorderOptions : StandardUiOptions
    {
        /// <summary>
        /// Specifies if and how the box shadow will be shown.
        /// </summary>
        [ParameterProperty("boxShadow")]
        [AdditionalInfo("tooltip", "Specifies if and how the box shadow will be shown.")]
        public BoxShadows BoxShadow = new BoxShadows(); // Will be no shadow according to parse-method by 2024-09-14.

        public override void UpdateFromParameterCollection(ParameterCollection parameters)
        {
            base.UpdateFromParameterCollection(parameters);
            
            if (parameters.HasKeyAndCanConvertTo("boxShadow", typeof(BoxShadows), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                BoxShadow = parameters.GetByKey<BoxShadows>("boxShadow", ParameterCollectionViewOptions.OptionsParameterConverters);
            }
        }
    }
}
