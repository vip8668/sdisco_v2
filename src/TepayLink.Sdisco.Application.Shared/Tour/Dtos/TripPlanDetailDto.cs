using System;
using System.Collections.Generic;
using System.Text;


namespace TepayLink.Sdisco.Tour.Dtos
{
    public class TripPlanDetailDto
    {
        
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description{get;set;}
        
        public bool OutOfTour { get; set; }
        
        /// <summary>
        /// Ảnh đại diện của ngày
        /// </summary>
        public string ThumbImage { get; set; }
        public List<BasicLocationDto> Locations { get; set; }
        public List<BasicItemDto> Activities { get; set; }
        public BasicItemDto Accomodation { get; set; }
        public List<BasicItemDto> Transport { get; set; }
        public List< BasicItemDto> Restaurant { get; set; }
        public int Order { get; set; }
    }
}
