using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Entities;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Utils;

namespace TepayLink.Sdisco.Tour.Dtos
{
    /// <summary>
    /// Base class
    /// </summary>
    public class BasicTourItemDto:Entity<long>
    {
        public string Name { get; set; }
        
        public string Slug
        {
            get { return this.Name.GenerateSlug(); }
        }
        
        public ReviewSummaryDto Review { get; set; }
        //todo test Thumb image
        public  List<PhotoDto> ThumbImages { get; set; }
        public  int BookCount { get; set; }
        public BasicPriceDto Price { get; set; }
        
        public BasicLocationDto Location { get; set; }
        public  int Order { get; set; }
        public  string Language { get; set; }
        
        public bool IsLove { get; set; }

        public List<AvaiableTimeDto> AvaiableTimes { get; set; }
        public ProductTypeEnum Type { get; set; }
        public  bool InstantBook { get; set; }
        
    }
}