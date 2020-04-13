using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class GetCommentInputDto : PagedInputDto
    {
        public long BlogId { get; set; }
    }
    public class GetRepyInputDto : PagedInputDto
    {
        public long BlogId { get; set; }
        public long CommentId { get; set; }
    }
}
