using Avalonia.Controls;
using Avalonia.Media;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    public class ParameterCollectionView : UserControl
    {
        public ParameterCollection ParameterCollection 
        {
            get
            {
                return _currentParameterCollection;
            }
            set
            {
                _currentParameterCollection = value.DeepCopyJson();
                InitializeForm(_currentParameterCollection);
            }
        }

        public ParameterCollectionViewOptions Options {get; set;}

        /// <summary>
        /// Do you have any special components to handle a parameter instead of the default ones? Here you can define them.
        /// </summary>
        public IParameterComponentDefinition[] CustomParameterComponents 
        {
            get 
            {
                return _customParameterComponents;
            } 
            set 
            {
                _customParameterComponents = value;
                if (value != null)
                {
                    _parameterComponents = value.Concat(Extensions.DefaultParameterComponents).ToArray();
                }
                else 
                {
                    _parameterComponents = Extensions.DefaultParameterComponents;
                }
            }
        }

        /// <summary>
        /// Do you have any custom converters you want to use to convert the values? Here you can define them.
        /// </summary>
        public IParameterValueConverter[] CustomConverters { get; set; }


        private ParameterCollection _currentParameterCollection;

        private IParameterComponentDefinition[] _customParameterComponents;
        private IParameterComponentDefinition[] _parameterComponents = Extensions.DefaultParameterComponents;

        private StackPanel parameterComponentList;

        private void InitializeForm(ParameterCollection parameters) 
        {
            Options.MakeValid();
            if (parameters != null)
            {
                foreach (var parameter in parameters) 
                {
                    var additionalInfo = parameter.GetAdditionalInfo();
                    var localOptions = Options;
                    if (Options.AdditionalInfoWillOverride && additionalInfo != null)
                    {
                        localOptions = ParameterCollectionViewOptions.CreateFromParameterCollection(additionalInfo, Options);
                        localOptions.MakeValid();
                    }

                    if (additionalInfo == null)
                    {
                        additionalInfo = new ParameterCollection();
                    }

                    var parameterText = parameter.Key;
                    if (additionalInfo.HasKeyAndCanConvertTo(localOptions.ReadableParameterTextKey, typeof(string)))
                    {
                        parameterText = additionalInfo.GetByKey<string>(localOptions.ReadableParameterTextKey);
                    }
                    else if (localOptions.ShowParameterNameAsHumanReadable)
                    {
                        parameterText = parameterText.HumanReadable();
                    }

                    if (_parameterComponents.Any(p => p.ShouldComponentBeUsed(parameter, additionalInfo, localOptions, CustomConverters)))
                    {
                        var def = _parameterComponents.First(p => p.ShouldComponentBeUsed(parameter, additionalInfo, localOptions, CustomConverters));
                        var parentType = def.GetHowParameterNameIsShown(parameter, additionalInfo, localOptions, CustomConverters);
                        var component = def.GetComponent(parameter, parameterText, additionalInfo, localOptions, CustomConverters, CustomParameterComponents, (value, additionalInfo) => ChangeParameter(parameter.Key, value, additionalInfo));
                        if (parentType == ComponentParentType.None)
                        {
                            parameterComponentList.Children.Add(component);
                        }
                        else if (parentType == ComponentParentType.Border || parentType == ComponentParentType.BorderWithoutName)
                        {
                            var border = new Border();
                            border.Background = localOptions.BorderOptions.Background;
                            border.BorderBrush = localOptions.BorderOptions.BorderBrush;
                            border.BorderThickness = localOptions.BorderOptions.BorderThickness;
                            border.CornerRadius = localOptions.BorderOptions.CornerRadius;
                            border.BoxShadow = localOptions.BorderOptions.BoxShadow;

                            if (parentType == ComponentParentType.Border)
                            {
                                var stackPanel = new StackPanel();
                                stackPanel.Children.Add(new TextBlock {
                                    Text = parameterText,
                                });
                                stackPanel.Children.Add(component);
                                border.Child = stackPanel;
                            }
                            else 
                            {
                                border.Child = component;
                            }
                        }
                        else if (parentType == ComponentParentType.Expander)
                        {
                            <RadzenFieldset Text="@parameterText" MouseEnter="@(additionalInfo.HasKeyAndCanConvertTo(localOptions.TooltipParameterTextKey, typeof(string)) ? (args) => {tooltipService.Open(args, additionalInfo.GetByKey<string>(localOptions.TooltipParameterTextKey, CustomConverters), localOptions.TooltipOptions);} : null)">
                                <DynamicComponent Type="@compType" Parameters="@compParameters"></DynamicComponent>
                            </RadzenFieldset>
                        }
                    }
                }
            }
        }

        private ParameterCollection GetParameterCollectionFromForm() 
        {
            return new ParameterCollection();
        }
    }
}
