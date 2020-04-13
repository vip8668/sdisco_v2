using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class CommentInputDto
    {
        public long BlogId { get; set; }
        public int Ratting { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Comment { get; set; }
        public string Website { get; set; }
    }
    public class ReplyCommentInputDto
    {
        public long BlogId { get; set; }
        public long CommentId { get; set; }
        public int Ratting { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Comment { get; set; }
        public string Website { get; set; }
    }
}
