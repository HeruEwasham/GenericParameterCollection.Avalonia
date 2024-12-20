﻿using System;
using YngveHestem.FileTypeInfo;
using YngveHestem.GenericParameterCollection.Avalonia.ParameterConverters;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    [AttributeConvertible]
    	public class ParameterCollectionViewOptions : ICloneable
    	{
            public static readonly IParameterValueConverter[] OptionsParameterConverters = new IParameterValueConverter[]
            {
                new IBrushConverter(),
                new ThicknessConverter(),
                new CornerRadiusConverter(),
                new BoxShadowsConverter(),
                new FileTypeConverter()
            };


            /// <summary>
            /// Will a parameters additionalInfo override the given parameters if correct parameters are given?
            /// </summary>
            [ParameterProperty("additionalInfoWillOverride")]
            [AdditionalInfo("tooltip", "Will a parameters additionalInfo override the given parameters if correct parameters are given?")]
            public bool AdditionalInfoWillOverride = true;

            /// <summary>
            /// Convert the parameter name to a human readable format. If set to false, the name will be shown as written, while if set to true, it will be altered to be read easily by a human, like setting first character to an upper character. Mark that this will only have an effect if it is the parameter key that is used. If another name is given, that will always be written as is.
            /// </summary>
            [ParameterProperty("humanReadable")]
            [AdditionalInfo("tooltip", "Convert the parameter name to a human readable format. If set to false, the name will be shown as written, while if set to true, it will be altered to be read easily by a human, like setting first character to an upper character. Mark that this will only have an effect if it is the parameter key that is used. If another name is given, that will always be written as is.")]
            public bool ShowParameterNameAsHumanReadable = true;

            /// <summary>
            /// Specifies if the controls should be readOnly.
            /// </summary>
            [ParameterProperty("readOnly")]
            [AdditionalInfo("tooltip", "Specifies if the controls should be readOnly.")]
            public bool ReadOnly = false;

            /// <summary>
            /// Specifies a parameter key to a parameter in a parameters additional info to use instead of the parameters key to show to the user. If the specified parameter can not be found (or converted to string), the given parameter's key is instead used.
            /// </summary>
            [ParameterProperty("readableParameterTextKey")]
            [AdditionalInfo("tooltip", "Specifies a parameter key to a parameter in a parameters additional info to use instead of the parameters key to show to the user. If the specified parameter can not be found (or converted to string), the given parameter's key is instead used.")]
            public string ReadableParameterTextKey = "name";

            /// <summary>
            /// Specifies a parameter key to a parameter in a parameters additional info to use for a tooltip.
            /// </summary>
            [ParameterProperty("tooltipParameterTextKey")]
            [AdditionalInfo("tooltip", "Specifies a parameter key to a parameter in a parameters additional info to use for a tooltip.")]
            public string TooltipParameterTextKey = "tooltip";

            /// <summary>
            /// Specifies what the lowest number that can be entered in a number-field is.
            /// </summary>
            [ParameterProperty("minNumber")]
            [AdditionalInfo("tooltip", "Specifies what the lowest number that can be entered in a number-field is.")]
            public decimal MinNumber = decimal.MinValue;

            /// <summary>
            /// Specifies what the highest number that can be entered in a number-field is.
            /// </summary>
            [ParameterProperty("maxNumber")]
            [AdditionalInfo("tooltip", "Specifies what the highest number that can be entered in a number-field is.")]
            public decimal MaxNumber = decimal.MaxValue;

            /// <summary>
            /// Specifies how much the gui should increment the value on an int value if the step-button is pressed.
            /// </summary>
            [ParameterProperty("stepInt")]
            [AdditionalInfo("tooltip", "Specifies how much the gui should increment the value on an int value if the step-button is pressed.")]
            public int StepInteger = 1;

            /// <summary>
            /// Specifies how much the gui should increment the value on a decimal value if the step-button is pressed.
            /// </summary>
            [ParameterProperty("stepDecimal")]
            [AdditionalInfo("tooltip", "Specifies how much the gui should increment the value on a decimal value if the step-button is pressed.")]
            public decimal StepDecimal = 0.1m;

            /// <summary>
            /// Specifies how the int numbers should be shown.
            /// </summary>
            [ParameterProperty("numberFormatInt")]
            [AdditionalInfo("tooltip", "Specifies how the int numbers should be shown.")]
            public string NumberFormatInt = "0";

            /// <summary>
            /// Specifies how the decimal numbers should be shown.
            /// </summary>
            [ParameterProperty("numberFormatDecimal")]
            [AdditionalInfo("tooltip", "Specifies how the decimal numbers should be shown.")]
            public string NumberFormatDecimal = "0.00";

            /// <summary>
            /// Specifies the placeholder text on controls that support that.
            /// </summary>
            [ParameterProperty("placeholder")]
            [AdditionalInfo("tooltip", "Specifies the placeholder text on controls that support that.")]
            public string PlaceholderText = string.Empty;

            /// <summary>
            /// Specifies if strings should be shown as password (without readable characters).
            /// </summary>
            [ParameterProperty("isPassword")]
            [AdditionalInfo("tooltip", "Specifies if strings should be shown as password (without readable characters).")]
            public bool IsPassword = false;

            /// <summary>
            /// Specifies max number of characters that can be given in a text.
            /// </summary>
            [ParameterProperty("maxChars")]
            [AdditionalInfo("isNullable", true)]
            [AdditionalInfo("tooltip", "Specifies max number of characters that can be given in a text.")]
            public int? MaxNumberOfCharacters = null;

            /// <summary>
            /// Specifies the height of a textarea.
            /// </summary>
            [ParameterProperty("textareaHeight")]
            [AdditionalInfo("tooltip", "Specifies the height of a textarea.")]
            public double TextAreaHeight = 100;

            /// <summary>
            /// Specifies the width of a textarea. Null means that the default length will be used.
            /// </summary>
            [ParameterProperty("textareaWidth")]
            [AdditionalInfo("isNullable", true)]
            [AdditionalInfo("tooltip", "Specifies the width of a textarea. Null means that the default length will be used.")]
            public double? TextAreaWidth = null;

            /// <summary>
            /// Defines what types of file extensions is supported when selecting files for ParameterType.Bytes. All must have a leading .
            /// Empty string[] means all types supported/no filter added.
            /// </summary>
            [ParameterProperty("supportedExtensions")]
            [AdditionalInfo("tooltip", "Defines what types of file extensions is supported when selecting files for ParameterType.Bytes. All must have a leading .\nEmpty means all types supported/no filter added.")]
            public string[] SupportedFileExtensions = null;

            /// <summary>
            /// List of different file types and mappings between extensions, UTType (UTI) and mime-types. All file extensions in SupportedFileExtensions must be defined here to be supported. Default value is all the values that is defined in the library used. Check for yourself if you need to add your own values.
            /// </summary>
            [ParameterProperty("fileTypeMappings")]
            [AdditionalInfo("tooltip", "List of different file types and mappings between extensions, UTType (UTI) and mime-types. All file extensions in SupportedFileExtensions must be defined here to be supported. Default value is all the values that is defined in the library used. Check for yourself if you need to add your own values.")]
            [AdditionalInfo("expanderOptions.isExpanded", false, KeyIsPath = true)]
            [AdditionalInfo("expanderOptions.loadContentOnlyWhenExpanding", true, KeyIsPath = true)]
            public FileType[] FileTypeMappings = FileTypes.Types;

            /// <summary>
            /// Specifies the text to show on the Choose file button.
            /// </summary>
            [ParameterProperty("chooseFileText")]
            [AdditionalInfo("tooltip", "Specifies the text to show on the Choose file button.")]
            public string ChooseFileText = "Choose file";

            /// <summary>
            /// Specifies the text to show on the Delete file button.
            /// </summary>
            [ParameterProperty("deleteFileText")]
            [AdditionalInfo("tooltip", "Specifies the text to show on the Delete file button.")]
            public string DeleteFileText = "Delete";

            /// <summary>
            /// Specifies the max file size allowed.
            /// </summary>
            [ParameterProperty("maxFileSize")]
            [AdditionalInfo("tooltip", "Specifies the max file size allowed.")]
            public int MaxFileSize = 5 * 1024 * 1024;

            /// <summary>
            /// Specifies the height of the file preview.
            /// </summary>
            [ParameterProperty("previewHeight")]
            [AdditionalInfo("tooltip", "Specifies the height of the file preview.")]
            public int FilePreviewHeight = 300;

            /// <summary>
            /// Specifies the width of the file preview.
            /// </summary>
            [ParameterProperty("previewWidth")]
            [AdditionalInfo("tooltip", "Specifies the width of the file preview.")]
            public int FilePreviewWidth = 500;

            /// <summary>
            /// Specifies how the format of the day is on Date and DateTime-parameters.
            /// </summary>
            [ParameterProperty("dateFormatDay")]
            [AdditionalInfo("tooltip", "Specifies how the format of the day is on Date and DateTime-parameters.")]
            public string DateFormatDay = "dd";

            /// <summary>
            /// Specifies how the format of the month is on Date and DateTime-parameters.
            /// </summary>
            [ParameterProperty("dateFormatMonth")]
            [AdditionalInfo("tooltip", "Specifies how the format of the month is on Date and DateTime-parameters.")]
            public string DateFormatMonth = "MMMM";

            /// <summary>
            /// Specifies how the format of the year is on Date and DateTime-parameters.
            /// </summary>
            [ParameterProperty("dateFormatYear")]
            [AdditionalInfo("tooltip", "Specifies how the format of the year is on Date and DateTime-parameters.")]
            public string DateFormatYear = "yyyy";

            /// <summary>
            /// Specifies how much the gui should increment the minute when the minute step is clicked.
            /// </summary>
            [ParameterProperty("minutesStep")]
            [AdditionalInfo("tooltip", "Specifies how much the gui should increment the minute when the minute step is clicked.")]
            public int MinutesStep = 1;

            /// <summary>
            /// Specifies if the timer in a DateTime should be 12 or 24 hours. It need to either be "12HourClock" or "24HourClock".
            /// </summary>
            [ParameterProperty("clockIdentifier")]
            [AdditionalInfo("tooltip", "Specifies if the timer in a DateTime should be 12 or 24 hours. It need to either be \"12HourClock\" or \"24HourClock\".")]
            public string ClockIdentifier = "24HourClock";

            /// <summary>
            /// The earliest date that is possible to pick.
            /// </summary>
            [ParameterProperty("minDate", ParameterType.Date)]
            [AdditionalInfo("tooltip", "The earliest date that is possible to pick.")]
            public DateTime MinDate = DateTime.MinValue.Add(TimeSpan.FromHours(12)); // DateTime.MinValue will crash on converting to DateTimeOffset if not given some hours extra.

            /// <summary>
            /// The latest date that is possible to pick.
            /// </summary>
            [ParameterProperty("maxDate", ParameterType.Date)]
            [AdditionalInfo("tooltip", "The latest date that is possible to pick.")]
            public DateTime MaxDate = DateTime.MaxValue.Add(TimeSpan.FromHours(-12)); // DateTime.MaxValue will crash on converting to DateTimeOffset if not given some hours less.

            /// <summary>
            /// Specifies the text on the button to add a new object to the list.
            /// </summary>
            [ParameterProperty("addEntryToListText")]
            [AdditionalInfo("tooltip", "Specifies the text on the button to add a new object to the list.")]
            public string AddEntryToListText = "Add";

            /// <summary>
            /// Specifies the text on the button to delete the given object from the list.
            /// </summary>
            [ParameterProperty("deleteEntryFromListText")]
            [AdditionalInfo("tooltip", "Specifies the text on the button to delete the given object from the list.")]
            public string DeleteEntryFromListText = "Delete";

            /// <summary>
            /// Specifies the text used to describe the delete button on the given entry in a list for screen readers. You can use {0} to get the current number the entry are in the list, use {1} to get the parameters viewable name and {2} to get the current value.
            /// </summary>
            [ParameterProperty("deleteEntryFromListAriaDescription")]
            [AdditionalInfo("tooltip", "Specifies the text used to describe the delete button on the given entry in a list for screen readers. You can use {0} to get the current number the entry are in the list, use {1} to get the parameters viewable name and {2} to get the current value.")]
            public string DeleteEntryFromListAriaDescription = "Delete entry number {0} from the list in parameter \"{1}\". The entry has the value \"{2}\".";

            /// <summary>
            /// Specifies the text used to describe the add button to add a new entry in a list for screen readers. You can use {0} to get the parameters viewable name.
            /// </summary>
            [ParameterProperty("addEntryToListAriaDescription")]
            [AdditionalInfo("tooltip", "Specifies the text used to describe the add button to add a new entry in a list for screen readers. You can use {0} to get the parameters viewable name.")]
            public string AddEntryToListAriaDescription = "Add a new entry to the list in parameter \"{0}\".";

            /// <summary>
            /// Defines how any parent component is shown when using "extra parameters" like "parametersIf:true" and "parametersIf:false".
            /// </summary>
            [ParameterProperty("parentTypeWhenHavingExtraParameters")]
            [AdditionalInfo("tooltip", "Defines how any parent component is shown when using \"extra parameters\" like \"parametersIf:true\" and \"parametersIf:false\".")]
            public ExtraParametersParentType ParentTypeWhenHavingExtraParameters = ExtraParametersParentType.None;

            /// <summary>
            /// Specifies the text used on the the collection of extra parameters (not all ParentTypeWhenHavingExtraParameters-options use it). You can use {0} to get the parameterName of the main parameter and use {1} to get the value of the main parameter.
            /// </summary>
            [ParameterProperty("extraParametersName")]
            [AdditionalInfo("tooltip", "Specifies the text used on the the collection of extra parameters (not all ParentTypeWhenHavingExtraParameters-options use it). You can use {0} to get the parameterName of the main parameter and use {1} to get the value of the main parameter.")]
            public string ExtraParametersName = "Extra parameters when {0} is {1}";

            /// <summary>
            /// Should each extra parameter-collection (one for each value that has one), get it's own visible parent.
            /// </summary>
            [ParameterProperty("selectManyExtraParametersGetOwnParent")]
            [AdditionalInfo("tooltip", "Should each extra parameter-collection (one for each value that has one), get it's own visible parent.")]
            public bool SelectManyExtraParametersGetOwnParent = true;

            /// <summary>
            /// Specifies the text used on the the total collection of extra parameters for SelectMany-parameters (not all ParentTypeWhenHavingExtraParameters-options use it). You can use {0} to get the parameterName of the main parameter.
            /// </summary>
            [ParameterProperty("selectManyExtraParametersName")]
            [AdditionalInfo("tooltip", "Specifies the text used on the the total collection of extra parameters for SelectMany-parameters (not all ParentTypeWhenHavingExtraParameters-options use it). You can use {0} to get the parameterName of the main parameter.")]
            public string SelectManyExtraParametersName = "Extra parameters when {0} has theese values";

            /// <summary>
            /// Should the controls that support it allow setting a value to null or not.
            /// </summary>
            [ParameterProperty("isNullable")]
            [AdditionalInfo("tooltip", "Should the controls that support it allow setting a value to null or not.")]
            public bool IsNullable = false;

            /// <summary>
            /// The options for the border.
            /// </summary>
            [ParameterProperty("borderOptions")]
            [AdditionalInfo("tooltip", "The options for the border.")]
            public BorderOptions BorderOptions = new BorderOptions();

            /// <summary>
            /// The options for the expander.
            /// </summary>
            [ParameterProperty("expanderOptions")]
            [AdditionalInfo("tooltip", "The options for the expander.")]
            public ExpanderOptions ExpanderOptions = new ExpanderOptions();

            /// <summary>
            /// What should the text around where the number of bytes in selected file be. {0} inserts the bytes in readdable size.
            /// </summary>
            [ParameterProperty("byteSizeText")]
            [AdditionalInfo("tooltip", "What should the text around where the number of bytes in selected file be. {0} inserts the bytes in readdable size.")]
            public string ByteSizeText = "Selected item has size: {0}";

            /// <summary>
            /// What should the text around the filename be. {0} inserts the filename.
            /// </summary>
            [ParameterProperty("filenameText")]
            [AdditionalInfo("tooltip", "What should the text around the filename be. {0} inserts the filename.")]
            public string FilenameText = "Filename: {0}";

            /// <summary>
            /// What should the text to display when preview of byte-content is not available be.
            /// </summary>
            [ParameterProperty("previewContentNotAvailableText")]
            [AdditionalInfo("tooltip", "What should the text to display when preview of byte-content is not available be.")]
            public string PreviewOfThisContentNotAvailableText = "Preview of this content not available.";

            /// <summary>
            /// What should the text be when no file are selected.
            /// </summary>
            [ParameterProperty("noBytesSelectedText")]
            [AdditionalInfo("tooltip", "What should the text be when no file are selected.")]
            public string NoBytesSelectedText = "No file selected.";

            /// <summary>
            /// What should the text be when the file to be selected was bigger than MaxFileSize. {0} inserts the filename. {1} inserts the size of the file formatted in a readable size. {2} inserts the MaxFileSize in a readdable size.
            /// </summary>
            [ParameterProperty("maxFileSizeErrorText")]
            [AdditionalInfo("tooltip", "What should the text be when the file to be selected was bigger than MaxFileSize. {0} inserts the filename. {1} inserts the size of the file formatted in a readable size. {2} inserts the MaxFileSize in a readdable size.")]
            public string MaxFileSizeErrorText = "File \"{0}\" has size {1}. But we only allows files up to {2}.";

            /// <summary>
            /// The text to display on the button to set a value to null. The button is used on some controls, when isNullable is set, that do not handle setting a value to null another way.
            /// </summary>
            [ParameterProperty("setToNullButtonText")]
            [AdditionalInfo("tooltip", "The text to display on the button to set a value to null. The button is used on some controls, when isNullable is set, that do not handle setting a value to null another way.")]
            public string SetToNullButtonText = "Set to null";

            /// <summary>
            /// The text to use as a replacement for value, if it is not possible to convert the value to string. This is for instance used on the aria-helptext if the given value can not be converted to string.
            /// </summary>
            [ParameterProperty("valueCanNotBeConvertedToStringText")]
            [AdditionalInfo("tooltip", "The text to use as a replacement for value, if it is not possible to convert the value to string. This is for instance used on the aria-helptext if the given value can not be converted to string.")]
            public string ValueCanNotBeConvertedToStringText = "Value is not easy to read as a single string.";

            /// <summary>
            /// What will the name used on each item shown on an IEnumerable. Mark that based on other parameters, the name will not always be shown, but it will also be used as Aria-labels to identify each item. Use {0} to get the parametername on the IEnumerable, and {0} to get the item number.
            /// </summary>
            [ParameterProperty("iEnumerableSingleItemName")]
            [AdditionalInfo("tooltip", "What will the name used on each item shown on an IEnumerable. Mark that based on other parameters, the name will not always be shown, but it will also be used as Aria-labels to identify each item. Use {0} to get the parametername on the IEnumerable, and {0} to get the item number.")]
            public string IEnumerableSingleItemName = "{0} {1}";

            private ParameterCollectionViewOptions CreateCopy()
            {
                return new ParameterCollectionViewOptions
                {
                    AdditionalInfoWillOverride = AdditionalInfoWillOverride,
                    ShowParameterNameAsHumanReadable = ShowParameterNameAsHumanReadable,
                    ReadOnly = ReadOnly,
                    ReadableParameterTextKey = ReadableParameterTextKey,
                    TooltipParameterTextKey = TooltipParameterTextKey,
                    MinNumber = MinNumber,
                    MaxNumber = MaxNumber,
                    StepInteger = StepInteger,
                    StepDecimal = StepDecimal,
                    NumberFormatInt = NumberFormatInt,
                    NumberFormatDecimal = NumberFormatDecimal,
                    PlaceholderText = PlaceholderText,
                    IsPassword = IsPassword,
                    MaxNumberOfCharacters = MaxNumberOfCharacters,
                    TextAreaHeight = TextAreaHeight,
                    TextAreaWidth = TextAreaWidth,
                    SupportedFileExtensions = SupportedFileExtensions,
                    FileTypeMappings = FileTypeMappings,
                    ChooseFileText = ChooseFileText,
                    DeleteFileText = DeleteFileText,
                    MaxFileSize = MaxFileSize,
                    FilePreviewHeight = FilePreviewHeight,
                    FilePreviewWidth = FilePreviewWidth,
                    DateFormatDay = DateFormatDay,
                    DateFormatMonth = DateFormatMonth,
                    DateFormatYear = DateFormatYear,
                    MinutesStep = MinutesStep,
                    ClockIdentifier = ClockIdentifier,
                    MinDate = MinDate,
                    MaxDate = MaxDate,
                    AddEntryToListText = AddEntryToListText,
                    DeleteEntryFromListText = DeleteEntryFromListText,
                    DeleteEntryFromListAriaDescription = DeleteEntryFromListAriaDescription,
                    AddEntryToListAriaDescription = AddEntryToListAriaDescription,
                    ParentTypeWhenHavingExtraParameters = ParentTypeWhenHavingExtraParameters,
                    ExtraParametersName = ExtraParametersName,
                    SelectManyExtraParametersGetOwnParent = SelectManyExtraParametersGetOwnParent,
                    SelectManyExtraParametersName = SelectManyExtraParametersName,
                    IsNullable = IsNullable,
                    BorderOptions = (BorderOptions)BorderOptions.Clone(),
                    ExpanderOptions = (ExpanderOptions)ExpanderOptions.Clone(),
                    ByteSizeText = ByteSizeText,
                    FilenameText = FilenameText,
                    PreviewOfThisContentNotAvailableText = PreviewOfThisContentNotAvailableText,
                    NoBytesSelectedText = NoBytesSelectedText,
                    MaxFileSizeErrorText = MaxFileSizeErrorText,
                    SetToNullButtonText = SetToNullButtonText,
                    ValueCanNotBeConvertedToStringText = ValueCanNotBeConvertedToStringText,
                    IEnumerableSingleItemName = IEnumerableSingleItemName
                };
            }

            public void MakeValid()
            {
                if (BorderOptions == null)
                {
                    BorderOptions = new BorderOptions();
                }
            }

            /// <summary>
            /// Creates a copy from a list of parameters.
            /// </summary>
            /// <param name="parameters">The parameters.</param>
            /// <param name="defaultOptions">A list of options to use if correct parameter is not found. if this is null, the default parameter is used.</param>
            /// <returns></returns>
            public static ParameterCollectionViewOptions CreateFromParameterCollection(ParameterCollection parameters, ParameterCollectionViewOptions defaultOptions = null)
            {
                var options = new ParameterCollectionViewOptions();

                if (defaultOptions != null)
                {
                    options = defaultOptions.CreateCopy();
                }

                if (parameters == null)
                {
                    return options;
                }

                if (parameters.HasKeyAndCanConvertTo("additionalInfoWillOverride", typeof(bool)))
                {
                    options.AdditionalInfoWillOverride = parameters.GetByKey<bool>("additionalInfoWillOverride");
                }

                if (parameters.HasKeyAndCanConvertTo("humanReadable", typeof(bool)))
                {
                    options.ShowParameterNameAsHumanReadable = parameters.GetByKey<bool>("humanReadable");
                }

                if (parameters.HasKeyAndCanConvertTo("readOnly", typeof(bool)))
                {
                    options.ReadOnly = parameters.GetByKey<bool>("readOnly");
                }

                if (parameters.HasKeyAndCanConvertTo("readableParameterTextKey", typeof(string)))
                {
                    options.ReadableParameterTextKey = parameters.GetByKey<string>("readableParameterTextKey");
                }

                if (parameters.HasKeyAndCanConvertTo("tooltipParameterTextKey", typeof(string)))
                {
                    options.TooltipParameterTextKey = parameters.GetByKey<string>("tooltipParameterTextKey");
                }

                if (parameters.HasKeyAndCanConvertTo("minNumber", typeof(decimal)))
                {
                    options.MinNumber = parameters.GetByKey<decimal>("minNumber");
                }

                if (parameters.HasKeyAndCanConvertTo("maxNumber", typeof(decimal)))
                {
                    options.MaxNumber = parameters.GetByKey<decimal>("maxNumber");
                }

                if (parameters.HasKeyAndCanConvertTo("step", typeof(decimal)))
                {
                    var inc = parameters.GetByKey<decimal>("step");
                    if (inc < 1)
                    {
                        options.StepInteger = 1;
                    }
                    else
                    {
                        options.StepInteger = (int)inc;
                    }
                    options.StepDecimal = inc;
                }

                if (parameters.HasKeyAndCanConvertTo("stepInt", typeof(int)))
                {
                    options.StepInteger = parameters.GetByKey<int>("stepInt");
                }

                if (parameters.HasKeyAndCanConvertTo("stepDecimal", typeof(decimal)))
                {
                    options.StepDecimal = parameters.GetByKey<decimal>("stepDecimal");
                }

                if (parameters.HasKeyAndCanConvertTo("numberFormat", typeof(string)))
                {
                    var format = parameters.GetByKey<string>("numberFormat");
                    options.NumberFormatInt = format;
                    options.NumberFormatDecimal = format;
                }

                if (parameters.HasKeyAndCanConvertTo("numberFormatInt", typeof(string)))
                {
                    options.NumberFormatInt = parameters.GetByKey<string>("numberFormatInt");
                }

                if (parameters.HasKeyAndCanConvertTo("numberFormatDecimal", typeof(string)))
                {
                    options.NumberFormatDecimal = parameters.GetByKey<string>("numberFormatDecimal");
                }

                if (parameters.HasKeyAndCanConvertTo("placeholder", typeof(string)))
                {
                    options.PlaceholderText = parameters.GetByKey<string>("placeholder");
                }

                if (parameters.HasKeyAndCanConvertTo("isPassword", typeof(bool)))
                {
                    options.IsPassword = parameters.GetByKey<bool>("isPassword");
                }

                if (parameters.HasKeyAndCanConvertTo("maxChars", typeof(int?)))
                {
                    var v = parameters.GetByKey<int?>("maxChars");
                    if (v < 0)
                    {
                        options.MaxNumberOfCharacters = null;
                    }
                    else
                    {
                        options.MaxNumberOfCharacters = v;
                    }
                }

                if (parameters.HasKeyAndCanConvertTo("textareaHeight", typeof(double)))
                {
                    options.TextAreaHeight = parameters.GetByKey<double>("textareaHeight");
                }

                if (parameters.HasKeyAndCanConvertTo("textareaWidth", typeof(double?)))
                {
                    options.TextAreaWidth = parameters.GetByKey<double?>("textareaWidth");
                }

                if (parameters.HasKeyAndCanConvertTo("supportedExtensions", typeof(string[])))
                {
                    options.SupportedFileExtensions = parameters.GetByKey<string[]>("supportedExtensions");
                }

                if (parameters.HasKeyAndCanConvertTo("fileTypeMappings", typeof(FileType[]), OptionsParameterConverters))
                {
                    options.FileTypeMappings = parameters.GetByKey<FileType[]>("fileTypeMappings", OptionsParameterConverters);
                }

                if (parameters.HasKeyAndCanConvertTo("chooseFileText", typeof(string)))
                {
                    options.ChooseFileText = parameters.GetByKey<string>("chooseFileText");
                }

                if (parameters.HasKeyAndCanConvertTo("deleteFileText", typeof(string)))
                {
                    options.DeleteFileText = parameters.GetByKey<string>("deleteFileText");
                }

                if (parameters.HasKeyAndCanConvertTo("maxFileSize", typeof(int)))
                {
                    options.MaxFileSize = parameters.GetByKey<int>("maxFileSize");
                }

                if (parameters.HasKeyAndCanConvertTo("previewHeight", typeof(int)))
                {
                    options.FilePreviewHeight = parameters.GetByKey<int>("previewHeight");
                }

                if (parameters.HasKeyAndCanConvertTo("previewWidth", typeof(int)))
                {
                    options.FilePreviewWidth = parameters.GetByKey<int>("previewWidth");
                }

                if (parameters.HasKeyAndCanConvertTo("dateFormatDay", typeof(string)))
                {
                    options.DateFormatDay = parameters.GetByKey<string>("dateFormatDay");
                }

                if (parameters.HasKeyAndCanConvertTo("dateFormatMonth", typeof(string)))
                {
                    options.DateFormatMonth = parameters.GetByKey<string>("dateFormatMonth");
                }

                if (parameters.HasKeyAndCanConvertTo("dateFormatYear", typeof(string)))
                {
                    options.DateFormatYear = parameters.GetByKey<string>("dateFormatYear");
                }

                if (parameters.HasKeyAndCanConvertTo("minutesStep", typeof(int)))
                {
                    options.MinutesStep = parameters.GetByKey<int>("minutesStep");
                }

                if (parameters.HasKeyAndCanConvertTo("clockIdentifier", typeof(string)))
                {
                    options.ClockIdentifier = parameters.GetByKey<string>("clockIdentifier");
                }

                if (parameters.HasKeyAndCanConvertTo("minDate", typeof(DateTime)))
                {
                    options.MinDate = parameters.GetByKey<DateTime>("minDate");
                }

                if (parameters.HasKeyAndCanConvertTo("maxDate", typeof(DateTime)))
                {
                    options.MaxDate = parameters.GetByKey<DateTime>("maxDate");
                }

                if (parameters.HasKeyAndCanConvertTo("addEntryToListText", typeof(string)))
                {
                    options.AddEntryToListText = parameters.GetByKey<string>("addEntryToListText");
                }

                if (parameters.HasKeyAndCanConvertTo("deleteEntryFromListText", typeof(string)))
                {
                    options.DeleteEntryFromListText = parameters.GetByKey<string>("deleteEntryFromListText");
                }

                if (parameters.HasKeyAndCanConvertTo("deleteEntryFromListAriaDescription", typeof(string)))
                {
                    options.DeleteEntryFromListAriaDescription = parameters.GetByKey<string>("deleteEntryFromListAriaDescription");
                }

                if (parameters.HasKeyAndCanConvertTo("addEntryToListAriaDescription", typeof(string)))
                {
                    options.AddEntryToListAriaDescription = parameters.GetByKey<string>("addEntryToListAriaDescription");
                }

                if (parameters.HasKeyAndCanConvertTo("parentTypeWhenHavingExtraParameters", typeof(ExtraParametersParentType)))
                {
                    options.ParentTypeWhenHavingExtraParameters = parameters.GetByKey<ExtraParametersParentType>("parentTypeWhenHavingExtraParameters");
                }

                if (parameters.HasKeyAndCanConvertTo("extraParametersName", typeof(string)))
                {
                    options.ExtraParametersName = parameters.GetByKey<string>("extraParametersName");
                }

                if (parameters.HasKeyAndCanConvertTo("selectManyExtraParametersGetOwnParent", typeof(bool)))
                {
                    options.SelectManyExtraParametersGetOwnParent = parameters.GetByKey<bool>("selectManyExtraParametersGetOwnParent");
                }

                if (parameters.HasKeyAndCanConvertTo("selectManyExtraParametersName", typeof(string)))
                {
                    options.SelectManyExtraParametersName = parameters.GetByKey<string>("selectManyExtraParametersName");
                }

                if (parameters.HasKeyAndCanConvertTo("isNullable", typeof(bool)))
                {
                    options.IsNullable = parameters.GetByKey<bool>("isNullable");
                }

                if (parameters.HasKeyAndCanConvertTo("borderOptions", typeof(BorderOptions), OptionsParameterConverters))
                {
                    options.BorderOptions.UpdateFromParameterCollection(parameters.GetByKey<ParameterCollection>("borderOptions", OptionsParameterConverters));
                }

                if (parameters.HasKeyAndCanConvertTo("expanderOptions", typeof(ExpanderOptions), OptionsParameterConverters))
                {
                    options.ExpanderOptions.UpdateFromParameterCollection(parameters.GetByKey<ParameterCollection>("expanderOptions", OptionsParameterConverters));
                }

                if (parameters.HasKeyAndCanConvertTo("byteSizeText", typeof(string)))
                {
                    options.ByteSizeText = parameters.GetByKey<string>("byteSizeText");
                }

                if (parameters.HasKeyAndCanConvertTo("filenameText", typeof(string)))
                {
                    options.FilenameText = parameters.GetByKey<string>("filenameText");
                }

                if (parameters.HasKeyAndCanConvertTo("previewContentNotAvailableText", typeof(string)))
                {
                    options.PreviewOfThisContentNotAvailableText = parameters.GetByKey<string>("previewContentNotAvailableText");
                }

                if (parameters.HasKeyAndCanConvertTo("noBytesSelectedText", typeof(string)))
                {
                    options.NoBytesSelectedText = parameters.GetByKey<string>("noBytesSelectedText");
                }

                if (parameters.HasKeyAndCanConvertTo("maxFileSizeErrorText", typeof(string)))
                {
                    options.MaxFileSizeErrorText = parameters.GetByKey<string>("maxFileSizeErrorText");
                }

                if (parameters.HasKeyAndCanConvertTo("setToNullButtonText", typeof(string)))
                {
                    options.SetToNullButtonText = parameters.GetByKey<string>("setToNullButtonText");
                }

                if (parameters.HasKeyAndCanConvertTo("valueCanNotBeConvertedToStringText", typeof(string)))
                {
                    options.ValueCanNotBeConvertedToStringText = parameters.GetByKey<string>("valueCanNotBeConvertedToStringText");
                }

                if (parameters.HasKeyAndCanConvertTo("iEnumerableSingleItemName", typeof(string)))
                {
                    options.IEnumerableSingleItemName = parameters.GetByKey<string>("iEnumerableSingleItemName");
                }

                return options;
            }

        public object Clone()
        {
            return CreateCopy();
        }
    }
}
