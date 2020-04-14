using TepayLink.Sdisco.Clients.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ClientSettings
{
    public class CreateOrEditClientSettingModalViewModel
    {
       public CreateOrEditClientSettingDto ClientSetting { get; set; }

	   
	   public bool IsEditMode => ClientSetting.Id.HasValue;
    }
}