using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Configuration.Tenants.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}