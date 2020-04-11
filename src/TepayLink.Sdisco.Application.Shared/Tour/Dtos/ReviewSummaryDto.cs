using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class ReviewSummaryDto
    {
        public float RatingAvg { get; set; }
        public float Intineraty { get; set; }
        public float Service { get; set; }
        public float Transport { get; set; }
        public float GuideTour { get; set; }
        public float Food { get; set; }
        public int ReviewCount { get; set; }
    }
}
