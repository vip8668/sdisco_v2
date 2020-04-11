using TepayLink.Sdisco.AdminConfig.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BankBranchs
{
    public class CreateOrEditBankBranchModalViewModel
    {
       public CreateOrEditBankBranchDto BankBranch { get; set; }

	   		public string BankBankName { get; set;}


	   public bool IsEditMode => BankBranch.Id.HasValue;
    }
}