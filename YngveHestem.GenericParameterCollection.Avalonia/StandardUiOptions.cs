using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    [AttributeConvertible]
    public class StandardUiOptions
    {
        /// <summary>
        /// Specifies how the background should look. Set to null for no specific background.
        /// </summary>
        [ParameterProperty("background")]
        [AdditionalInfo("tooltip", "Specifies how the background should look. Set to null for no specific background.")]
        public IBrush Background = null;
        
        /// <summary>
        /// Specifies the color on the border.
        /// </summary>
        [ParameterProperty("borderBrush")]
        [AdditionalInfo("tooltip", "Specifies the color on the border.")]
        public IBrush BorderBrush = new ImmutableSolidColorBrush(Colors.Black);

        /// <summary>
        /// Specifies the thickness of the border.
        /// </summary>
        [ParameterProperty("borderThickness")]
        [AdditionalInfo("tooltip", "Specifies the thickness of the border.")]
        public Thickness BorderThickness = new Thickness(1);

        /// <summary>
        /// Specifies the corner radius.
        /// </summary>
        [ParameterProperty("cornerRadius")]
        [AdditionalInfo("tooltip", "Specifies the corner radius.")]
        public CornerRadius CornerRadius = new CornerRadius(0);

        /// <summary>
        /// Specifies the margin of the border.
        /// </summary>
        [ParameterProperty("margin")]
        [AdditionalInfo("tooltp", "Specifies the margin of the border.")]
        public Thickness Margin = new Thickness(5);

        /// <summary>
        /// Specifies the padding of the border.
        /// </summary>
        [ParameterProperty("padding")]
        [AdditionalInfo("tooltip", "Specifies the padding of the border.")]
        public Thickness Padding = new Thickness(5);

        public virtual void UpdateFromParameterCollection(ParameterCollection parameters)
        {
            if (parameters.HasKeyAndCanConvertTo("background", typeof(IBrush), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                Background = parameters.GetByKey<IBrush>("background", ParameterCollectionViewOptions.OptionsParameterConverters);
            }

            if (parameters.HasKeyAndCanConvertTo("borderBrush", typeof(IBrush), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                BorderBrush = parameters.GetByKey<IBrush>("borderBrush", ParameterCollectionViewOptions.OptionsParameterConverters);
            }

            if (parameters.HasKeyAndCanConvertTo("borderThickness", typeof(Thickness), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                BorderThickness = parameters.GetByKey<Thickness>("borderThickness", ParameterCollectionViewOptions.OptionsParameterConverters);
            }

            if (parameters.HasKeyAndCanConvertTo("cornerRadius", typeof(CornerRadius), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                CornerRadius = parameters.GetByKey<CornerRadius>("cornerRadius", ParameterCollectionViewOptions.OptionsParameterConverters);
            }

            if (parameters.HasKeyAndCanConvertTo("margin", typeof(Thickness), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                Margin = parameters.GetByKey<Thickness>("margin", ParameterCollectionViewOptions.OptionsParameterConverters);
            }

            if (parameters.HasKeyAndCanConvertTo("padding", typeof(Thickness), ParameterCollectionViewOptions.OptionsParameterConverters)) 
            {
                Padding = parameters.GetByKey<Thickness>("padding", ParameterCollectionViewOptions.OptionsParameterConverters);
            }
        }
    }
}
