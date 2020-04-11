using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Configuration.Host.Dto;
using TepayLink.Sdisco.Editions.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}