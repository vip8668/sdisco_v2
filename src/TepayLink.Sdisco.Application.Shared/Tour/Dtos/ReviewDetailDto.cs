using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class ReviewDetailDto
    {
        public  long Id { get; set; }
        public  long UserId { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public double Ratting { get; set; }
        public string Avatar { get; set; }
        public DateTime ReviewDate { get; set; }
        
        public int ReplyCount { get; set; }
    }
}
