using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Banks
{
    public class CreateOrEditBankModalViewModel
    {
       public CreateOrEditBankDto Bank { get; set; }

	   
	   public bool IsEditMode => Bank.Id.HasValue;
    }
}