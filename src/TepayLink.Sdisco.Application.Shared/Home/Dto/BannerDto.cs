
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Home.Dto
{
  public  class BannerDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PhotoDto> Images { get; set; }
    }
}
