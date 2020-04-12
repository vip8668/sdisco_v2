using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Partners
{
    public class CreateOrEditPartnerModalViewModel
    {
       public CreateOrEditPartnerDto Partner { get; set; }

	   		public string UserName { get; set;}

		public string DetinationName { get; set;}


	   public bool IsEditMode => Partner.Id.HasValue;
    }
}