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

        /// <summary>
        /// If set to true, the content of the expander will first load when expanded, and not if it is collapsed.
        /// </summary>
        [ParameterProperty("loadContentOnlyWhenExpanding")]
        [AdditionalInfo("tooltip", "If set to true, the content of the expander will first load when expanded, and not if it is collapsed.")]
        public bool LoadContentOnlyWhenExpanding = false;

        /// <summary>
        /// If set to true, the content of the expander will unload when it has collapsed. Mark that it will only work if LoadContentOnlyWhenExpanding is also set to true.
        /// </summary>
        [ParameterProperty("unloadContentWhenCollapsed")]
        [AdditionalInfo("tooltip", "If set to true, the content of the expander will unload when it has collapsed. Mark that it will only work if LoadContentOnlyWhenExpanding is also set to true.")]
        public bool UnloadContentWhenCollapsed = false;

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

            if (parameters.HasKeyAndCanConvertTo("loadContentOnlyWhenExpanding", typeof(bool), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                LoadContentOnlyWhenExpanding = parameters.GetByKey<bool>("loadContentOnlyWhenExpanding", ParameterCollectionViewOptions.OptionsParameterConverters);
            }

            if (parameters.HasKeyAndCanConvertTo("unloadContentWhenCollapsed", typeof(bool), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                UnloadContentWhenCollapsed = parameters.GetByKey<bool>("unloadContentWhenCollapsed", ParameterCollectionViewOptions.OptionsParameterConverters);
            }
        }
    }
}
