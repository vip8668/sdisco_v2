using System.Collections.Generic;
using TepayLink.Sdisco.Editions.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}