using System;
using Avalonia.Controls;
using Avalonia.Layout;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class DateTimeParameterComponent : IParameterComponentDefinition
    {
        public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            var currentValue = parameter.GetValue<DateTime?>(customConverters);
            var dateControl = new DatePicker
            {
                IsEnabled = !options.ReadOnly,
                MinYear = new DateTimeOffset(options.MinDate),
                MaxYear = new DateTimeOffset(options.MaxDate),
                DayFormat = options.DateFormatDay,
                MonthFormat = options.DateFormatMonth,
                YearFormat = options.DateFormatYear
            };
            if (currentValue.HasValue) 
            {
                dateControl.SelectedDate = new DateTimeOffset(currentValue.Value);
            }
            if (parameter.Type == ParameterType.DateTime)
            {
                var timeControl = new TimePicker 
                {
                    IsEnabled = !options.ReadOnly,
                    MinuteIncrement = options.MinutesStep,
                    ClockIdentifier = options.ClockIdentifier
                };
                if (currentValue.HasValue) 
                {
                    timeControl.SelectedTime = currentValue.Value.TimeOfDay;
                }
                dateControl.SelectedDateChanged += (sender, e) =>
                {
                    if (e.NewDate.HasValue)
                    {
                        if (e.NewDate.Value.Date >= options.MinDate && e.NewDate.Value.Date <= options.MaxDate)
                        {
                            if (timeControl.SelectedTime.HasValue)
                            {
                                updateParameterValue(e.NewDate.Value.Date.Add(timeControl.SelectedTime.Value), null);
                            }
                            else
                            {
                                timeControl.SelectedTime = TimeSpan.Zero;
                                updateParameterValue(e.NewDate.Value.Date, null);
                            }
                        }
                        else if (e.NewDate.Value.Date < options.MinDate)
                        {
                            dateControl.SelectedDate = options.MinDate;
                            if (timeControl.SelectedTime.HasValue)
                            {
                                updateParameterValue(options.MinDate.Add(timeControl.SelectedTime.Value), null);
                            }
                            else
                            {
                                timeControl.SelectedTime = TimeSpan.Zero;
                                updateParameterValue(options.MinDate, null);
                            }
                        }
                        else if (e.NewDate.Value.Date > options.MaxDate)
                        {
                            dateControl.SelectedDate = options.MaxDate;
                            if (timeControl.SelectedTime.HasValue)
                            {
                                updateParameterValue(options.MaxDate.Add(timeControl.SelectedTime.Value), null);
                            }
                            else 
                            {
                                timeControl.SelectedTime = TimeSpan.Zero;
                                updateParameterValue(options.MaxDate, null);
                            }
                        }
                    }
                    else if (options.IsNullable)
                    {
                        updateParameterValue(null, null);
                    }
                };

                timeControl.SelectedTimeChanged += (sender, e) =>
                {
                    if (e.NewTime.HasValue)
                    {
                        if (dateControl.SelectedDate.HasValue)
                        {
                            updateParameterValue(dateControl.SelectedDate.Value.Date.Add(e.NewTime.Value), null);
                        }
                        else
                        {
                            updateParameterValue(null, null);
                        }
                    }
                    else if (options.IsNullable)
                    {
                        if (dateControl.SelectedDate.HasValue)
                        {
                            updateParameterValue(dateControl.SelectedDate.Value.Date, null);
                        }
                        else
                        {
                            updateParameterValue(null, null);
                        }
                    }
                };
                
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                stackPanel.Children.Add(dateControl);
                stackPanel.Children.Add(timeControl);

                if (options.IsNullable) 
                {
                    var setNullableButton = new Button
                    {
                        Content = options.SetToNullButtonText
                    };
                    setNullableButton.Click += (sender, e) =>
                    {
                        dateControl.SelectedDate = null;
                        timeControl.SelectedTime = null;
                    };
                    stackPanel.Children.Add(setNullableButton);
                }

                return stackPanel;
            }
            else if (parameter.Type == ParameterType.Date)
            {
                dateControl.SelectedDateChanged += (sender, e) =>
                {
                    if (e.NewDate.HasValue)
                    {
                        if (e.NewDate.Value.Date >= options.MinDate && e.NewDate.Value.Date <= options.MaxDate)
                        {
                            updateParameterValue(e.NewDate.Value.Date, null);
                        }
                        else if (e.NewDate.Value.Date < options.MinDate)
                        {
                            dateControl.SelectedDate = options.MinDate;
                            updateParameterValue(options.MinDate, null);
                        }
                        else if (e.NewDate.Value.Date > options.MaxDate)
                        {
                            dateControl.SelectedDate = options.MaxDate;
                            updateParameterValue(options.MaxDate, null);
                        }
                    }
                    else if (options.IsNullable)
                    {
                        updateParameterValue(null, null);
                    }
                };
            }

            if (options.IsNullable) 
            {
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                stackPanel.Children.Add(dateControl);
                var setNullableButton = new Button
                {
                    Content = options.SetToNullButtonText
                };
                setNullableButton.Click += (sender, e) =>
                {
                    dateControl.SelectedDate = null;
                };
                stackPanel.Children.Add(setNullableButton);
                return stackPanel;
            }
            return dateControl;
        }

        public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return ComponentParentType.Border;
        }

        public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return parameter.Type == ParameterType.DateTime || parameter.Type == ParameterType.Date;
        }
    }
}
