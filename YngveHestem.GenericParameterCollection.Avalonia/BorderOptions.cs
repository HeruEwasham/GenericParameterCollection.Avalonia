using Avalonia;
using Avalonia.Media;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    [AttributeConvertible]
    public class BorderOptions
    {
        [ParameterProperty("background")]
        public IBrush Background = null;
        
        [ParameterProperty("borderBrush")]
        public IBrush BorderBrush = new SolidColorBrush(Colors.Black);

        [ParameterProperty("borderThickness")]
        public Thickness BorderThickness = new Thickness(1);

        [ParameterProperty("cornerRadius")]
        public CornerRadius CornerRadius = new CornerRadius(0);

        [ParameterProperty("boxShadow")]
        public BoxShadows BoxShadow = new BoxShadows(); // Will be no shadow according to parse-method by 2024-09-14.
    }
}
