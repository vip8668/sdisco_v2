using TepayLink.Sdisco.Help.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.HelpContents
{
    public class CreateOrEditHelpContentModalViewModel
    {
       public CreateOrEditHelpContentDto HelpContent { get; set; }

	   		public string HelpCategoryCategoryName { get; set;}


	   public bool IsEditMode => HelpContent.Id.HasValue;
    }
}