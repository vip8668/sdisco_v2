using System.Collections.Generic;
using MvvmHelpers;
using TepayLink.Sdisco.Models.NavigationMenu;

namespace TepayLink.Sdisco.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}