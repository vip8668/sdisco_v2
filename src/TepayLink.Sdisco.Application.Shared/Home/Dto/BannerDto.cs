using SDisco.Tour.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDisco.Home.Dto
{
  public  class BannerDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PhotoDto> Images { get; set; }
    }
}
