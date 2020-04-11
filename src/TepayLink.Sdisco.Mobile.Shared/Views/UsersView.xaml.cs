using TepayLink.Sdisco.Models.Users;
using TepayLink.Sdisco.ViewModels;
using Xamarin.Forms;

namespace TepayLink.Sdisco.Views
{
    public partial class UsersView : ContentPage, IXamarinView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((UsersViewModel) BindingContext).LoadMoreUserIfNeedsAsync(e.Item as UserListModel);
        }
    }
}