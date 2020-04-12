using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.TransPortdetails
{
    public class CreateOrEditTransPortdetailModalViewModel
    {
       public CreateOrEditTransPortdetailDto TransPortdetail { get; set; }

	   		public string ProductName { get; set;}


	   public bool IsEditMode => TransPortdetail.Id.HasValue;
    }
}