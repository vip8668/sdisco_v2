using System.Collections.Generic;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.MultiTenancy.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Tenants
{
    public class EditTenantViewModel
    {
        public TenantEditDto Tenant { get; set; }

        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public EditTenantViewModel(TenantEditDto tenant, IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            Tenant = tenant;
            EditionItems = editionItems;
        }
    }
}