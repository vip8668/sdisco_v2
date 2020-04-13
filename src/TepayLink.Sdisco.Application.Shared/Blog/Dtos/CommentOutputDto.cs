using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class CommentOutputDto
    {
        public long Id { get; set; }
        public long BlogId { get; set; }
        public long UserId { get; set; }
        public double? Ratting { get; set; }

        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Comment { get; set; }

        public int ReplyCount { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
