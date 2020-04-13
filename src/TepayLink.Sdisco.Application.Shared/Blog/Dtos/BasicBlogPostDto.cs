using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Utils;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class BasicBlogPostDto
    {
        public long Id { get; set; }
        public string Slug => Title.GenerateSlug();
        public string Title { get; set; }
        public string ShortDesciption { get; set; }
        public DateTime PublishDate { get; set; }
        public string ThumbImage { get; set; }
        public int TotalComment { get; set; }
    }
}
