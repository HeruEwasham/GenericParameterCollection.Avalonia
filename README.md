# GenericParameterCollection.Avalonia

This provides controls for using [GenericParameterCollection](https://github.com/HeruEwasham/GenericParameterCollection) in the UI-toolkit [Avalonia](https://avaloniaui.net).

## How to use this package

The easiest way to use the package is to download it from nuget.

## Main features/controls

### ParameterCollectionView

This is the main control that handles editing a given ParameterCollection object.

#### Properties and events

Here is a list of properties and events of the ParameterCollectionView-component.

##### ParameterCollection

Set this to the ParameterCollection the component should render.

##### Options

This property let you set customized Options for how the ParameterCollectionView should look and behave.

##### OnChange-event

This event is triggered each time the user do a change in the component.

When the event is triggered you will get an object that will contain both the new ParameterCollection, and the key to the parameter that was changed.

##### CustomParameterComponents

Here you can define a list of custom component definitions.

This can be good to use if you for instance want a color picker for picking some colors, or have your own component you want to use for some parameter.

##### CustomConverters

Do you have some custom converters you need to use for converting some custom value, or want to change how some value is converted in the ParameterCollectionView? Then you can define a list of theese here.

#### Example code

TODO

### Options

The controls let you provide a ParameterCollectionViewOptions. Here you can define some customisation of how the control looks and works. Most are both self explanatory and well documented in code. Most of theese options can also be set for a specific parameter if the option AdditionalInfoWillOverride is set to true (default is true). Then one or more of the given parameters below can be given in a parameters additionalInfo.

#### Different options

Here is a list of parameters that can either be defined in ParameterCollectionViewOptions or as a ParameterCollection (some can only be given in one, while many can be given both ways).

If you define this in a ParameterCollection-ParameterType, the changes will affect all parameters in that ParameterCollection.

| Variable name in option-class | Parameter key | Type | Description | Default value in option-class |
| ----------- | ----------- | ----------- | ----------- | ----------- |
| AdditionalInfoWillOverride | additionalInfoWillOverride | bool | Can parameters from a ParameterCollection, like AdditionalInfo from a parameter, override the values defined in this options-object. Mark that if this is set to false in the Options-object that it uses as a base, this will have no effect. | true |
| ShowParameterNameAsHumanReadable | humanReadable | bool | Convert the parameter name to a human readable format. If set to false, the name will be shown as written, while if set to true, it will be altered to be read easily by a human, like setting first character to an upper character. Mark that this will only have an effect if it is the parameter key that is used. If another name is given, that will always be written as is. | True |
| ReadOnly | readOnly | bool | If true, the control that shows the parameters value should be read only/disabled | False |
| ReadableParameterTextKey | readableParameterTextKey | string | Specifies a parameter key to a parameter in a parameters additional info to use instead of the parameters key to show to the user. If the specified parameter can not be found (or converted to string), the given parameter's key is instead used. | name |
| TooltipParameterTextKey | tooltipParameterTextKey | string | Specifies a parameter key to a parameter in a parameters additional info to use for a tooltip. | tooltip |
| MinNumber | minNumber | decimal | Specifies what the lowest number that can be entered in a number-field is. | decimal.MinValue |
| MaxNumber | maxNumber | decimal | Specifies what the highest number that can be entered in a number-field is. | decimal.MaxValue |
| StepInteger and StepDecimal | step | decimal | Defines how much a number (both integers and decimals) should increment/decrement with if the increment/decrement buttons are used | if int, 1, if a decimal-number, 0.1 |
| StepInteger | stepInt | int | Specifies how much the gui should increment the value on an int value if the step-button is pressed. | 1 |
| StepDecimal | stepDecimal | decimal | Specifies how much the gui should increment the value on a decimal value if the step-button is pressed. | 0.1 |
| PlaceholderText | placeholder | string | Specifies the placeholder text on controls that support that. | string.Empty |
| IsPassword | isPassword | bool | Specifies if strings should be shown as password (without readable characters). | false |
| MaxNumberOfCharacters | maxChars | long? | Specifies max number of characters that can be given in a text. | null |
| NumberOfRowsInTextArea | textareaRows | int | Specifies the number of rows a textarea will show. | 5 |
| NumberOfColumnsInTextArea | textareaColumns | int | Specifies the number of columns (characters) a textarea will show horizontally. | 100 |
| AcceptedMimeTypes | acceptedMimeTypes | string[] | Specifies what mime types are accepted when selecting file. | null |
| ChooseFileText | chooseFileText | string | Specifies the text to show on the Choose file button. | Choose file |
| DeleteFileText | deleteFileText | string | Specifies the text to show on the Delete file button. | Delete |
| MaxFileSize | maxFileSize | int | Specifies the max file size allowed. | 5 * 1024 * 1024 |
| FilePreviewHeight | previewHeight | int | Specifies the height of the file preview. | 300 |
| FilePreviewWidth | previewWidth | int | Specifies the width of the file preview. | 500 |
| MinDate | minDate | DateTime | The earliest date that is possible to pick. | DateTime.Today.AddYears(-1000) |
| MaxDate | maxDate | DateTime | The latest date that is possible to pick. | DateTime.Today.AddYears(1000) |
| DateTimeFormat | dateTimeFormat | string | Specifies the time format to be shown in DateTime-parameters. | g |
| DateFormat | dateFormat | string | Specifies the time format to be shown in Date-parameters. | d |
| HoursStep | hoursStep | decimal | Specifies how much the gui should increment the hour when the hour step is clicked. | 1.0 |
| MinutesStep | minutesStep | decimal | Specifies how much the gui should increment the minute when the minute step is clicked. | 1.0 |
| SecondsStep | secondsStep | decimal | Specifies how much the gui should increment the second when the second step is clicked. | 1.0 |
| AddEntryToListText | addEntryToListText | string | Specifies the text on the button to add a new object to the list. | Add |
| DeleteEntryFromListText | deleteEntryFromListText | string | Specifies the text on the button to delete the given object from the list. | Delete |
| DeleteEntryFromListAriaDescription | deleteEntryFromListAriaDescription | string | Specifies the text used to describe the delete button on the given entry in a list for screen readers. You can use {0} to get the current number the entry are in the list, use {1} to get the parameters viewable name and {2} to get the current value. | Delete entry number {0} from the list in parameter "{1}". The entry has the value "{2}". |
| AddEntryToListAriaDescription | addEntryToListAriaDescription | string | Specifies the text used to describe the add button to add a new entry in a list for screen readers. You can use {0} to get the parameters viewable name. | Add a new entry to the list in parameter "{0}". |
| | defaultValue | TValue (Generic baseed on value (IEnumerable)) | This is used on IEnumerable-types to define their Default-value (which is their initial state when adding new value). | If not defined, this will either be default(TValue) or String.Empty if TValue is string or DateTime.Now if TValue is DateTime. |
| | parametersIf:true | ParameterCollection | Any parameters specified here in a bool-parameter will be shown and be editable (if not set to readonly), if the value is set to true. If set to false (or null), this will not be shown. | |
| | parametersIf:false | ParameterCollection | Any parameters specified here in a bool-parameter will be shown and be editable (if not set to readonly), if the value is set to false. If set to true (or null), this will not be shown. | |
| | parametersIf:null | ParameterCollection | Any parameters specified here in a bool-parameter will be shown and be editable (if not set to readonly), if the value is set to null. If set to true or false, this will not be shown. | |
| ParentTypeWhenHavingExtraParameters | parentTypeWhenHavingExtraParameters | ComponentParentType | Defines how any parent component is shown when using "extra parameters" like "parametersIf:true" and "parametersIf:false". Possible valuees are None, Border, BorderWithoutName, Expander | None |
| ExtraParametersName | extraParametersName | string | Specifies the text used on the the collection of extra parameters (not all ParentTypeWhenHavingExtraParameters-options use it). You can use {0} to get the parameterName of the main parameter and use {1} to get the value of the main parameter. | Extra parameters when {0} is {1} |
| | parametersIf:true:name | string | Specifies the text used on the the collection of extra parameters if value is true (not all ParentTypeWhenHavingExtraParameters-options use it). If set, this will be used instead of ExtraParametersName. | |
| | parametersIf:false:name | string | Specifies the text used on the the collection of extra parameters if value is false (not all ParentTypeWhenHavingExtraParameters-options use it). If set, this will be used instead of ExtraParametersName. | |
| | parametersIf:null:name | string | Specifies the text used on the the collection of extra parameters if value is null (not all ParentTypeWhenHavingExtraParameters-options use it). If set, this will be used instead of ExtraParametersName. | |
| | parametersIf:true:options | ParameterCollection | Any parameters specified here that corresponds to the given parameter name for an option in the option-class, will alter the options inputted to the shown ParameterCollection (the same way that adding parameter to a parameters AdditionalInfo). | |
| | parametersIf:false:options | ParameterCollection | Any parameters specified here that corresponds to the given parameter name for an option in the option-class, will alter the options inputted to the shown ParameterCollection (the same way that adding parameter to a parameters AdditionalInfo). | |
| | parametersIf:null:options | ParameterCollection | Any parameters specified here that corresponds to the given parameter name for an option in the option-class, will alter the options inputted to the shown ParameterCollection (the same way that adding parameter to a parameters AdditionalInfo). | |
| | parametersIf:VALUE | ParameterCollection | Any parameters specified here in an Enum-, SelectOne or SelectMany-parameter will be shown and be editable (if not set to readonly), if the value is set to the value specified as VALUE. Remember that VALUE-part of the name need to be exactly as the value (including uppercase/lowercase, etc.) | |
| | parametersIf:VALUE:name | string | Specifies the text used on the the collection of extra parameters if value is VALUE (not all ParentTypeWhenHavingExtraParameters-options use it). If set, this will be used instead of ExtraParametersName. | |
| | parametersIf:VALUE:options | ParameterCollection | Any parameters specified here that corresponds to the given parameter name for an option in the option-class, will alter the options inputted to the shown ParameterCollection (the same way that adding parameter to a parameters AdditionalInfo). | |
| SelectManyExtraParametersGetOwnParent | selectManyExtraParametersGetOwnParent | bool | Should each extra parameter-collection (one for each value that has one), get it's own visible parent (RadzenFieldset). ExtraParametersParentType will here decide if all the given collections should have one too or not. | true |
| SelectManyExtraParametersName | selectManyExtraParametersName | string | Specifies the text used on the the total collection of extra parameters for SelectMany-parameters (not all ParentTypeWhenHavingExtraParameters-options use it). You can use {0} to get the parameterName of the main parameter. | Extra parameters when {0} has theese values |
| | prettyValues | ParameterCollection | Let you change the values shown to the user on Enum-, SelectOne- and SelectMany-parameter to something other. The value gotten back and sent in must be the correct value. This can be useful if you want to support multiple languages or if you just want to show prettier enum-values to the user. | |
| IsNullable | isNullable | bool | Should the controls that support it allow setting a value to null or not. | false |