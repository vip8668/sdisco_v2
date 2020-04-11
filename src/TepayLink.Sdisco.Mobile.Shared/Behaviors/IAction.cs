using Xamarin.Forms.Internals;

namespace TepayLink.Sdisco.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}