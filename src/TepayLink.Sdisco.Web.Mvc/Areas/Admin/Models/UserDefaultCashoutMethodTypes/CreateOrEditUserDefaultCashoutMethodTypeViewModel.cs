using TepayLink.Sdisco.Cashout.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.UserDefaultCashoutMethodTypes
{
    public class CreateOrEditUserDefaultCashoutMethodTypeModalViewModel
    {
       public CreateOrEditUserDefaultCashoutMethodTypeDto UserDefaultCashoutMethodType { get; set; }

	   		public string CashoutMethodTypeTitle { get; set;}

		public string UserName { get; set;}


	   public bool IsEditMode => UserDefaultCashoutMethodType.Id.HasValue;
    }
}