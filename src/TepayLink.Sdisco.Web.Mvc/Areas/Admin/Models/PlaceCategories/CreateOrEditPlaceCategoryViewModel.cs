using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.PlaceCategories
{
    public class CreateOrEditPlaceCategoryModalViewModel
    {
       public CreateOrEditPlaceCategoryDto PlaceCategory { get; set; }

	   
	   public bool IsEditMode => PlaceCategory.Id.HasValue;
    }
}