using System;
using Avalonia.Controls;
using Avalonia.Media;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.Avalonia;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace Testproject.Desktop;

public class ColorPickerComponent : IParameterComponentDefinition
{
    public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
    {
        var control = new ColorPicker
        {
            Color = parameter.GetValue<Color>(customConverters)
        };
        control.ColorChanged += (sender, e) => updateParameterValue(e.NewColor, null);
        return control;
    }

    public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
    {
        return ComponentParentType.Border;
    }

    public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
    {
        return additionalInfo.HasKeyAndCanConvertTo("type", typeof(string), customConverters) 
            && additionalInfo.GetByKey<string>("type", customConverters) == "color"
            && parameter.CanBeConvertedTo(typeof(Color), customConverters);
    }
}
