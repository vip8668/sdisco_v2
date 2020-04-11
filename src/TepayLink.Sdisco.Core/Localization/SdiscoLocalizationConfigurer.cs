using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace TepayLink.Sdisco.Localization
{
    public static class SdiscoLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    SdiscoConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(SdiscoLocalizationConfigurer).GetAssembly(),
                        "TepayLink.Sdisco.Localization.Sdisco"
                    )
                )
            );
        }
    }
}