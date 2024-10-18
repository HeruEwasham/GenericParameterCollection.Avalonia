using System;
using Avalonia.Controls;
using Avalonia.Rendering.Composition;
using YngveHestem.GenericParameterCollection;
using YngveHestem.GenericParameterCollection.Avalonia;

namespace Testproject.Desktop;

public partial class MainWindow : Window
{
    private ParameterCollectionView _parameterCollectionView;
    private TextBox _currentParameterCollectionAsText;
    private TextBox _debugWindow;
    private ParameterCollection _parameters = new ParameterCollection
{
        { "TestString", "This is a test" },
        { "TestString Multiline", "This is a test.\n\nTest123.", true },
        { "TestString Password", string.Empty, new ParameterCollection
            {
                { "isPassword", true}
            }
        },
        { "TestInt", 15 },
        { "TestFloat", 15.01f },
        { "testFloat with desc", 15.05f, new ParameterCollection
            {
                { "name", "Testing Float with tooltip" },
                { "tooltip", "This is a tooltip." }
            }
        },
        { "Some options", new ParameterCollection
            {
                { "string 1", "This is a string." },
                { "Select an option", "Option 5", new string[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6" } }
            }
        },
        /*{ "Example color with custom component", "rgb(0, 255, 0)",
            new ParameterCollection
            {
                { "type", "color" }
            }
        },*/
        { "Select some bytes", new byte[] {1, 3, 5} },
        { "Are this any good?", true },
        { "What blend mode do you like best?", CompositionBlendMode.ColorBurn },
        { "Select a date and a time", DateTime.Now },
        { "Select a date", DateTime.Now, true },
        { "Select a date between today and 10 days from today", DateTime.Now, true, new ParameterCollection
        {
                { "minDate", DateTime.Today },
                { "maxDate", DateTime.Today.AddDays(10) }
            }
        },
        { "A list of strings", new string[] { "Object 1", "Object 2", "Object 3" } },
        { "Select one or more options (or none)", new string[] {}, new string[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6" } },
        { "A list of int's", new int[] { 1, 2, 3 } },
        { "A list of float's", new float[] { 1.0f, 2.5f, 3.45f } },
        { "A list of bool's", new bool[] { true, true, false } },
        { "A list of date and time's", new DateTime[] { DateTime.Now, DateTime.UnixEpoch, DateTime.UtcNow } },
        { "A list of only date's", new DateTime[] { DateTime.Now, DateTime.UnixEpoch, DateTime.UtcNow }, true },
        { "A list of ParameterCollection with a few parameters",
            new ParameterCollection[] {
                new ParameterCollection
                {
                    { "Name", "Sherlock Holmes" },
                    { "Work", "Detective" },
                    { "Is fictional", true }
                },
                new ParameterCollection
                {
                    { "Name", "Albert Einstein" },
                    { "Work", "Theoretical physicist" },
                    { "Is fictional", false }
                }
            }, new ParameterCollection
            {
                { "defaultValue",
                    new ParameterCollection
                {
                        { "Name", string.Empty },
                        { "Work", string.Empty },
                        { "Is fictional", false }
                    }
                }
            }
        },

        // Bools with extra parameters:
        { "Bool with extra parameters if true", false, new ParameterCollection 
            {
                { "parametersIf:true", new ParameterCollection 
                    {
                        { "Do you like this?", true },
                        { "Comment", string.Empty }
                    }
                }
            }
        },
        { "Bool with extra parameters if both true and false", false, new ParameterCollection 
            {
                { "parametersIf:true", new ParameterCollection 
                    {
                        { "Do you like this?", true },
                        { "Comment", string.Empty }
                    }
                },
                { "parametersIf:false", new ParameterCollection 
                    {
                        { "Why is this not set to true?", string.Empty }
                    }
                },
            }
        },
        { "Bool with extra parameters if true with name on extra parameters (and a small additional question if not)", false, new ParameterCollection 
            {
                { "parametersIf:true", new ParameterCollection 
                    {
                        { "Do you like this?", true },
                        { "Comment", string.Empty }
                    }
                },
                { "parametersIf:false", new ParameterCollection 
                    {
                        { "Why is this not set to true?", string.Empty }
                    }
                },
                { "parametersIf:true:name", "Then we need some more input" },
                { "parametersIf:false:name", "Ok, can we ask one additional question?" },
                { "parentTypeWhenHavingExtraParameters", ExtraParametersParentType.ExpanderOnOnlyCollection }
            }
        },

        // Enum and SelectOne-types with extra parameters:
        { "Select which border-form do you like best", ComponentParentType.Border, new ParameterCollection 
            {
                { "parametersIf:None", new ParameterCollection 
                    {
                        { "Do you like it because it is old-school?", true },
                        { "Comment", string.Empty }
                    }
                }
            }
        },
        { "Select which border-form you like best with more extra parameters", ComponentParentType.Border, new ParameterCollection 
            {
                { "parametersIf:None", new ParameterCollection 
                    {
                        { "Do you like it because it is old-school?", true },
                        { "Comment", string.Empty }
                    }
                },
                { "parametersIf:Border", new ParameterCollection 
                    {
                        { "When was you born?", DateTime.Now, true },
                        { "Comment", string.Empty }
                    }
                },
                { "parametersIf:Border:name", "I do wonder..." },
                { "parentTypeWhenHavingExtraParameters", ExtraParametersParentType.ExpanderOnOnlyCollection },
                { "prettyValues", new ParameterCollection 
                    {
                        { "Border", "The nice border" }
                    }
                }
            }
        },
        { "Which of theese artists do you like best", "Elvis", new string[] {"Rihanna", "Elvis", "Elton John", "AC/DC", "ABBA"}, new ParameterCollection 
            {
                { "parametersIf:Elvis", new ParameterCollection 
                    {
                        { "You know he is dead?", true },
                    }
                },
                { "parametersIf:AC/DC", new ParameterCollection 
                    {
                        { "Do you like Thunderstruck", true },
                    }
                },
                { "parametersIf:ABBA", new ParameterCollection 
                    {
                        { "Which country are they from?", string.Empty },
                    }
                },
                { "parentTypeWhenHavingExtraParameters", ExtraParametersParentType.ExpanderOverWholeParameter }
            }
        },
        { "Select all the artists you like", Array.Empty<string>(), new string[] {"Rihanna", "Elvis", "Elton John", "AC/DC", "ABBA"}, new ParameterCollection 
            {
                { "parametersIf:Elvis", new ParameterCollection 
                    {
                        { "You know he is dead?", true },
                    }
                },
                { "parametersIf:AC/DC", new ParameterCollection 
                    {
                        { "Do you like Thunderstruck", true },
                    }
                },
                { "parametersIf:ABBA", new ParameterCollection 
                    {
                        { "Which country are they from?", string.Empty },
                    }
                },
                { "prettyValues", new ParameterCollection 
                    {
                        { "Elvis", "Elvis Presley" },
                        { "Rihanna", "Robyn Rihanna Fenty" }
                    }
                }
            }
        },
        { "Bool with null value", null, typeof(bool?) },
        { "Bool wth null value and allow settng to null", null, typeof(bool?), 
            new ParameterCollection 
            {
                { "isNullable", true }
            }
        },
        { "Int starting as null, but not nullable", null, typeof(int?) },
        { "Int nullable", 456, 
            new ParameterCollection 
            {
                { "isNullable", true }
            }
        },
        { "Int nullable and starting as null", null, typeof(int?), 
            new ParameterCollection 
            {
                { "isNullable", true }
            }
        },
        { "Decimal starting as null, but not nullable", null, typeof(double?) },
        { "Decimal nullable", 456.56, 
            new ParameterCollection 
            {
                { "isNullable", true }
            }
        },
        { "Date starting as null, but not nullable", (DateTime?)null, true },
        { "Date nullable", DateTime.Now, true, 
            new ParameterCollection 
            {
                { "isNullable", true }
            }
        },
        { "DateTime nullable and start as null", (DateTime?)null, 
            new ParameterCollection 
            {
                { "isNullable", true }
            }
        },
        { "DateTime starting as null, but not nullable, with custom defaultValue", (DateTime?)null, 
            new ParameterCollection 
            {
                { "defaultValue", DateTime.Now.AddDays(5) },
                { "isNullable", false}
            }
        },
        { "DateTime starting as null, but not specifically say nullable", (DateTime?)null},
        { "enum with null/empty value", null, typeof(ParameterType?) },
        { "int with null and min max over 0", null, typeof(int?),
            new ParameterCollection
            {
                { "minNumber", 12 },
                { "maxNumber", 16 }
            }
        },
        { "Select which bool-control you like best (with readonly under-options)", ComponentParentType.None, new ParameterCollection 
            {
                { "parametersIf:None", new ParameterCollection 
                    {
                        { "Do you like it because it is old-school?", true },
                        { "Comment", "It seems you can not change me, but I are multiline", true }
                    }
                },
                { "parametersIf:None:options", new ParameterCollection 
                    {
                        { "readOnly", true }
                    }
                }
            }
        },
    };

    private ParameterCollectionViewOptions _options = new ParameterCollectionViewOptions
    {
        PlaceholderText = "This is a placeholder text."
    };

    public MainWindow()
    {
        Title = "Testproject for GenericParameterCollection.Avalonia";
        _currentParameterCollectionAsText = new TextBox 
        { 
            AcceptsReturn = true,
            IsReadOnly = true,
            Text = _parameters.ToJson(Newtonsoft.Json.Formatting.Indented),
            Height = 300
        };
        _debugWindow = new TextBox 
        { 
            AcceptsReturn = true,
            IsReadOnly = true,
            Text = "Debug window ready",
            Height = 100
        };
        _parameterCollectionView = new ParameterCollectionView(_parameters, _options)
        {
            Height = 500
        };
        _parameterCollectionView.ValueChanged += _parameterCollectionView_ValueChanged;
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(new TextBlock { Text = "Parameters"});
        stackPanel.Children.Add(_parameterCollectionView);
        stackPanel.Children.Add(_currentParameterCollectionAsText);
        stackPanel.Children.Add(_debugWindow);

        var optionsArea = new ParameterCollectionView(ParameterCollection.FromObject(_options, ParameterCollectionViewOptions.OptionsParameterConverters));
        optionsArea.ValueChanged += (sender, e) =>
        {
            _parameterCollectionView.Options = e.NewParameterCollection.ToObject<ParameterCollectionViewOptions>(ParameterCollectionViewOptions.OptionsParameterConverters);
        };
        var splitView = new SplitView
        {
            IsPaneOpen = true,
            DisplayMode = SplitViewDisplayMode.Inline,
            Pane = optionsArea,
            Content = stackPanel
        };
        
        Content = splitView;
    }

    private void _parameterCollectionView_ValueChanged(object? sender, ParameterCollectionViewOnChangeEventArgs e)
    {
        _currentParameterCollectionAsText.Text = e.NewParameterCollection.ToJson(Newtonsoft.Json.Formatting.Indented);
        _debugWindow.Text += Environment.NewLine + "New event: " + e.ParameterKey + " changed.";
    }
}