using System.Collections.Generic;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.Security;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Tenants
{
    public class CreateTenantViewModel
    {
        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateTenantViewModel(IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            EditionItems = editionItems;
        }
    }
}