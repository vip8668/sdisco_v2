using Abp.Application.Navigation;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Layout
{
    public class MenuViewModel
    {
        public UserMenu Menu { get; set; }

        public string CurrentPageName { get; set; }
    }
}