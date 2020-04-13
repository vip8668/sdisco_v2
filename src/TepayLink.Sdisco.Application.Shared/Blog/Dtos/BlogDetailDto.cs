using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class BlogDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ThumbImage { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int TotalComment { get; set; }
    }
}
