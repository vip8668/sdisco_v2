using TepayLink.Sdisco.Products.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Utilities
{
    public class CreateOrEditUtilityModalViewModel
    {
       public CreateOrEditUtilityDto Utility { get; set; }

	   
	   public bool IsEditMode => Utility.Id.HasValue;
    }
}