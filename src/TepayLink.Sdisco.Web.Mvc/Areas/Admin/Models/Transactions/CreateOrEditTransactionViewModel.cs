using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Transactions
{
    public class CreateOrEditTransactionModalViewModel
    {
       public CreateOrEditTransactionDto Transaction { get; set; }

	   
	   public bool IsEditMode => Transaction.Id.HasValue;
    }
}