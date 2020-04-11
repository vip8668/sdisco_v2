using TepayLink.Sdisco.Models.Tenants;
using TepayLink.Sdisco.ViewModels;
using Xamarin.Forms;

namespace TepayLink.Sdisco.Views
{
    public partial class TenantsView : ContentPage, IXamarinView
    {
        public TenantsView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((TenantsViewModel)BindingContext).LoadMoreTenantsIfNeedsAsync(e.Item as TenantListModel);
        }
    }
}