using TepayLink.Sdisco.KOL.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ShareTransactions
{
    public class CreateOrEditShareTransactionModalViewModel
    {
       public CreateOrEditShareTransactionDto ShareTransaction { get; set; }

	   
	   public bool IsEditMode => ShareTransaction.Id.HasValue;
    }
}