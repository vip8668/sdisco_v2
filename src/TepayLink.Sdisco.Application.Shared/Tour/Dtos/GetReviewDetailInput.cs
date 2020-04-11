using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Tour.Dtos
{
  public  class GetReviewDetailInput: PagedInputDto
    {
        public long ItemId { get; set; }
    }
}
