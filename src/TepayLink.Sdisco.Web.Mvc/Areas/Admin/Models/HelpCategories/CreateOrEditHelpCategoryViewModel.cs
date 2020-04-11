using TepayLink.Sdisco.Help.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.HelpCategories
{
    public class CreateOrEditHelpCategoryModalViewModel
    {
       public CreateOrEditHelpCategoryDto HelpCategory { get; set; }

	   
	   public bool IsEditMode => HelpCategory.Id.HasValue;
    }
}