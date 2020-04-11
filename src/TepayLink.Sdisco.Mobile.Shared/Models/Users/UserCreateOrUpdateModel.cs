using System.ComponentModel;
using Abp.AutoMapper;
using TepayLink.Sdisco.Authorization.Users.Dto;

namespace TepayLink.Sdisco.Models.Users
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}