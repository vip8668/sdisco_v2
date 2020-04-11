using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BankAccountInfos
{
    public class CreateOrEditBankAccountInfoModalViewModel
    {
       public CreateOrEditBankAccountInfoDto BankAccountInfo { get; set; }

	   		public string BankBankName { get; set;}

		public string BankBranchBranchName { get; set;}

		public string UserName { get; set;}


	   public bool IsEditMode => BankAccountInfo.Id.HasValue;
    }
}