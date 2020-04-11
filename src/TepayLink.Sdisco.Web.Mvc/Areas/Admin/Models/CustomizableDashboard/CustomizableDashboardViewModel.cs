using TepayLink.Sdisco.DashboardCustomization;
using TepayLink.Sdisco.DashboardCustomization.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.CustomizableDashboard
{
    public class CustomizableDashboardViewModel
    {
        public DashboardOutput DashboardOutput { get; }

        public Dashboard UserDashboard { get; }

        public CustomizableDashboardViewModel(
            DashboardOutput dashboardOutput,
            Dashboard userDashboard)
        {
            DashboardOutput = dashboardOutput;
            UserDashboard = userDashboard;
        }
    }
}