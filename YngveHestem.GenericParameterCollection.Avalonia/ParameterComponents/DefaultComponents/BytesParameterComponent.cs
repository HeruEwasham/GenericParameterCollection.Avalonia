using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using YngveHestem.FileTypeInfo;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterComponents.DefaultComponents
{
    public class BytesParameterComponent : IParameterComponentDefinition
    {
        public Control GetComponent(Parameter parameter, string parameterName, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters, IParameterComponentDefinition[] customParameterComponents, Action<object, ParameterCollection> updateParameterValue)
        {
            var stackPanel = new StackPanel();
            var infoText = new TextBlock();

            if (parameter.HasValue()) 
            {
                var byteData = parameter.GetValue<byte[]>(customConverters);
                infoText.Text = string.Format(options.ByteSizeText, byteData.Length.GetSizeInMemory());
                if (additionalInfo.HasKeyAndCanConvertTo("filename", typeof(string))) 
                {
                    infoText.Text += Environment.NewLine + string.Format(options.FilenameText, additionalInfo.GetByKey<string>("filename"));
                }
            }
            else
            {
                infoText.Text = options.NoBytesSelectedText;
            }
            stackPanel.Children.Add(infoText);

            if (!options.ReadOnly) 
            {
                var buttonArea = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                };
                var selectBytesButton = new Button 
                {
                    Content = options.ChooseFileText
                };
                var deleteBytesButton = new Button 
                {
                    Content = options.DeleteFileText
                };
                selectBytesButton.Click += async (s, e) => 
                {
                    // Get top level from the current control. Alternatively, you can use Window reference instead.
                    var topLevel = TopLevel.GetTopLevel(stackPanel);

                    // Start async operation to open the dialog.
                    var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                    {
                        Title = options.ChooseFileText,
                        AllowMultiple = false,
                        FileTypeFilter = GetFileTypeFilter(options.SupportedFileExtensions, options.FileTypeMappings)
                    });

                    if (files.Count >= 1)
                    {
                        var checkedFileSize = false;
                        var properties = await files[0].GetBasicPropertiesAsync();
                        if (properties.Size != null)
                        {
                            if (properties.Size.Value > (ulong)options.MaxFileSize)
                            {
                                var box = MessageBoxManager
                                .GetMessageBoxStandard(parameterName, 
                                    string.Format(options.MaxFileSizeErrorText,
                                        files[0].Name,
                                        properties.Size.Value.GetSizeInMemory(),
                                        options.MaxFileSize.GetSizeInMemory()
                                    ), 
                                    ButtonEnum.Ok, 
                                    Icon.Error
                                );
                                await box.ShowAsync();
                                return;
                            }
                            checkedFileSize = true;
                        }
                        // Open reading stream from the first file.
                        using (var stream = await files[0].OpenReadAsync())
                        {
                            if (!checkedFileSize && stream.Length > options.MaxFileSize)
                            {
                                var box = MessageBoxManager
                                .GetMessageBoxStandard(parameterName, 
                                    string.Format(options.MaxFileSizeErrorText,
                                        files[0].Name,
                                        properties.Size.Value.GetSizeInMemory(),
                                        options.MaxFileSize.GetSizeInMemory()
                                    ), 
                                    ButtonEnum.Ok, 
                                    Icon.Error
                                );
                                await box.ShowAsync();
                                return;
                            }
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                // Reads all the content of file as a text.
                                var fileContent = memoryStream.ToArray();;
                                
                                if (additionalInfo.HasKeyAndCanConvertTo("filename", typeof(string), customConverters))
                                {
                                    additionalInfo.GetParameterByKey("filename").SetValue(files[0].Name, customConverters);
                                }
                                else 
                                {
                                    additionalInfo.Add("filename", files[0].Name, ParameterType.String, null, customConverters);
                                }

                                if (additionalInfo.HasKeyAndCanConvertTo("extension", typeof(string), customConverters))
                                {
                                    additionalInfo.GetParameterByKey("extension").SetValue(Path.GetExtension(files[0].Name), customConverters);
                                }
                                else 
                                {
                                    additionalInfo.Add("extension", Path.GetExtension(files[0].Name), ParameterType.String, null, customConverters);
                                }

                                updateParameterValue(fileContent, additionalInfo);
                                infoText.Text = string.Format(options.ByteSizeText, fileContent.Length.GetSizeInMemory()) 
                                    + Environment.NewLine + string.Format(options.FilenameText, files[0].Name);
                                deleteBytesButton.IsVisible = true;
                                deleteBytesButton.IsEnabled = true;
                            }
                        }
                    }
                };
                
                deleteBytesButton.Click += (s, e) =>
                {
                    if (parameter.HasValue()) 
                    {
                        if (additionalInfo.HasKeyAndCanConvertTo("filename", typeof(string), customConverters))
                        {
                            additionalInfo.GetParameterByKey("filename").SetValue(null, customConverters);
                        }
                        else 
                        {
                            additionalInfo.Add("filename", null, ParameterType.String, null, customConverters);
                        }

                        if (additionalInfo.HasKeyAndCanConvertTo("extension", typeof(string), customConverters))
                        {
                            additionalInfo.GetParameterByKey("extension").SetValue(null, customConverters);
                        }
                        else 
                        {
                            additionalInfo.Add("extension", null, ParameterType.String, null, customConverters);
                        }
                        updateParameterValue(null, additionalInfo);
                        infoText.Text = options.NoBytesSelectedText;
                        deleteBytesButton.IsEnabled = false;
                        deleteBytesButton.IsVisible = false;
                    }
                };

                buttonArea.Children.Add(selectBytesButton);
                buttonArea.Children.Add(deleteBytesButton);
                stackPanel.Children.Add(buttonArea);
            }


            // TODO: Add preview functionality
            return stackPanel;
        }

        private IReadOnlyList<FilePickerFileType> GetFileTypeFilter(string[] supportedFileExtensions, FileType[] fileTypeMappings)
        {
            if (supportedFileExtensions == null || supportedFileExtensions.Length == 0) {
                return new[] { FilePickerFileTypes.All };
            }
            
            var fileTypes = fileTypeMappings.GetByExtension(supportedFileExtensions);

            var list = new List<FilePickerFileType>();
            foreach (var fileType in fileTypes)
            {
                list.Add(new FilePickerFileType(fileType.DescriptiveText) 
                {
                    Patterns = fileType.Extensions.Select(x => "*" + x).ToList(),
                    MimeTypes = fileType.MimeTypes,
                    AppleUniformTypeIdentifiers = fileType.UTTypes
                });
            }

            return list;
        }

        public ComponentParentType GetHowParameterNameIsShown(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return ComponentParentType.Border;
        }

        public bool ShouldComponentBeUsed(Parameter parameter, ParameterCollection additionalInfo, ParameterCollectionViewOptions options, IParameterValueConverter[] customConverters)
        {
            return parameter.Type == ParameterType.Bytes;
        }
    }
}
