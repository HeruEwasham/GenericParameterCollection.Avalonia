using Avalonia.Controls;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    [AttributeConvertible]
    public class ExpanderOptions : StandardUiOptions
    {
        /// <summary>
        /// Which direction the expanding should be done at.
        /// </summary>
        [ParameterProperty("expandDirection")]
        [AdditionalInfo("tooltip", "Which direction the expanding should be done at.")]
        public ExpandDirection ExpandDirection = ExpandDirection.Down;

        /// <summary>
        /// Should the expandable be expanded or not when rendering.
        /// </summary>
        [ParameterProperty("isExpanded")]
        [AdditionalInfo("tooltip", "Should the expandable be expanded or not when rendering.")]
        public bool IsExpanded = true;

        public override void UpdateFromParameterCollection(ParameterCollection parameters)
        {
            base.UpdateFromParameterCollection(parameters);
            
            if (parameters.HasKeyAndCanConvertTo("expandDirection", typeof(ExpandDirection), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                ExpandDirection = parameters.GetByKey<ExpandDirection>("expandDirection", ParameterCollectionViewOptions.OptionsParameterConverters);
            }
            
            if (parameters.HasKeyAndCanConvertTo("isExpanded", typeof(bool), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                IsExpanded = parameters.GetByKey<bool>("isExpanded", ParameterCollectionViewOptions.OptionsParameterConverters);
            }
        }
    }
}
