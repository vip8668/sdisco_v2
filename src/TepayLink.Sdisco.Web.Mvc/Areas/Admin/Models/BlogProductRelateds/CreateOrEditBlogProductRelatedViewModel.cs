using TepayLink.Sdisco.Blog.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BlogProductRelateds
{
    public class CreateOrEditBlogProductRelatedModalViewModel
    {
       public CreateOrEditBlogProductRelatedDto BlogProductRelated { get; set; }

	   		public string BlogPostTitle { get; set;}

		public string ProductName { get; set;}


	   public bool IsEditMode => BlogProductRelated.Id.HasValue;
    }
}