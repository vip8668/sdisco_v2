using TepayLink.Sdisco.Blog.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BlogPosts
{
    public class CreateOrEditBlogPostModalViewModel
    {
       public CreateOrEditBlogPostDto BlogPost { get; set; }

	   
	   public bool IsEditMode => BlogPost.Id.HasValue;
    }
}