using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Affiliate.Dto
{
    public class GetTripCreated : PagedInputDto
    {

        public long? UserId { get; set; }
    }
}
