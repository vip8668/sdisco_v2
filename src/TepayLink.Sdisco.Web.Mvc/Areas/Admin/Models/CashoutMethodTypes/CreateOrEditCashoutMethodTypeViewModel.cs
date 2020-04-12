using TepayLink.Sdisco.Cashout.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.CashoutMethodTypes
{
    public class CreateOrEditCashoutMethodTypeModalViewModel
    {
       public CreateOrEditCashoutMethodTypeDto CashoutMethodType { get; set; }

	   
	   public bool IsEditMode => CashoutMethodType.Id.HasValue;
    }
}