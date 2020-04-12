using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.UserSubcribers
{
    public class CreateOrEditUserSubcriberModalViewModel
    {
       public CreateOrEditUserSubcriberDto UserSubcriber { get; set; }

	   
	   public bool IsEditMode => UserSubcriber.Id.HasValue;
    }
}