using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.WithDrawRequests
{
    public class CreateOrEditWithDrawRequestModalViewModel
    {
       public CreateOrEditWithDrawRequestDto WithDrawRequest { get; set; }

	   
	   public bool IsEditMode => WithDrawRequest.Id.HasValue;
    }
}