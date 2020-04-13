using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetBlogPostInputDto : PagedInputDto
    {
        public string Keyword { get; set; }
    }
}
