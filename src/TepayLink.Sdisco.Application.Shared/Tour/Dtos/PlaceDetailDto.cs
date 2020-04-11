using System.Collections.Generic;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class PlaceDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public BasicLocationDto Location { get; set; }
        public  string OverView { get; set; }
        public  string WhatExpect { get; set; }
        public  ReviewSummaryDto Review { get; set; }
        public List<PhotoDto> Images { get; set; }
        //public  decimal Price { get; set; }
        //public  decimal ServiceFee { get; set; }
        //public  decimal Total { get; set; }
        public BasicPriceDto Price { get; set; }
        public  BasicItemDto Category { get; set; }
        /// <summary>
        /// Instant book : có book được ngay hay phải chờ xác nhận của Admin
        /// </summary>
        public bool InstantBook { get; set; }
        public  bool IsLove { get; set; }
        public  List<BasicLocationDto> NearbyPlaces { get; set; }
    }
}