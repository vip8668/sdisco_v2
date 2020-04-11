using System.Collections.Generic;
using Abp.Localization;
using TepayLink.Sdisco.Install.Dto;

namespace TepayLink.Sdisco.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
