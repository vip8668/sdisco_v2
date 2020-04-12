using TepayLink.Sdisco.Account.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.SaveItems
{
    public class CreateOrEditSaveItemModalViewModel
    {
       public CreateOrEditSaveItemDto SaveItem { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => SaveItem.Id.HasValue;
    }
}