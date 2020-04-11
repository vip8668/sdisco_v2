using System.Collections.Generic;
using TepayLink.Sdisco.Caching.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}