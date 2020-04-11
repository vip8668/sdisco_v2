using TepayLink.Sdisco.Blog.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.BlogComments
{
    public class CreateOrEditBlogCommentModalViewModel
    {
       public CreateOrEditBlogCommentDto BlogComment { get; set; }

	   		public string BlogPostTitle { get; set;}


	   public bool IsEditMode => BlogComment.Id.HasValue;
    }
}