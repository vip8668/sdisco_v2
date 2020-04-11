using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Categories
{
    public class CreateOrEditCategoryModalViewModel
    {
       public CreateOrEditCategoryDto Category { get; set; }

	   
	   public bool IsEditMode => Category.Id.HasValue;
    }
}