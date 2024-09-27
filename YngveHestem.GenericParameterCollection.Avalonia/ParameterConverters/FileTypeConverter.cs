using System.Collections.Generic;
using YngveHestem.FileTypeInfo;
using YngveHestem.GenericParameterCollection.ParameterValueConverters;

namespace YngveHestem.GenericParameterCollection.Avalonia.ParameterConverters
{
    public class FileTypeConverter : ParameterCollectionParameterConverter<FileType>
    {
        protected override bool CanConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return value.HasKeyAndCanConvertTo("descriptiveText", typeof(string), customConverters)
                && value.HasKeyAndCanConvertTo("extensions", typeof(string[]), customConverters)
                && value.HasKeyAndCanConvertTo("mimeTypes", typeof(string[]), customConverters)
                && value.HasKeyAndCanConvertTo("uTTypes", typeof(string[]), customConverters);
        }

        protected override bool CanConvertToParameterCollection(FileType value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return true;
        }

        protected override FileType ConvertFromParameterCollection(ParameterCollection value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new FileType(value.GetByKey<string>("descriptiveText", customConverters),
                value.GetByKey<string[]>("extensions", customConverters),
                value.GetByKey<string[]>("mimeTypes", customConverters),
                value.GetByKey<string[]>("uTTypes", customConverters));
        }

        protected override ParameterCollection ConvertToParameterCollection(FileType value, IEnumerable<IParameterValueConverter> customConverters)
        {
            return new ParameterCollection 
            {
                { "descriptiveText", value.DescriptiveText, ParameterType.String, null, customConverters },
                { "extensions", value.Extensions, ParameterType.String_IEnumerable, null, customConverters },
                { "mimeTypes", value.MimeTypes, ParameterType.String_IEnumerable, null, customConverters },
                { "uTTypes", value.UTTypes, ParameterType.String_IEnumerable, null, customConverters }
            };
        }
    }
}
