using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using SDisco.Utils;

namespace SDisco.Home.Dto
{
   public class BasicPlaceDto:Entity<long>
    {
        public string PlaceName { get; set; }
        public string Slug
        {
            get { return this.PlaceName.GenerateSlug(); }
        }
        public string ThumbImage { get; set; }
        public int TotalBooked { get; set; }
    }
}
