using System.Threading.Tasks;
using TepayLink.Sdisco.Views;
using Xamarin.Forms;

namespace TepayLink.Sdisco.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
