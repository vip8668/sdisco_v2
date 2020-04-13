using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Blog.Dtos
{
    public class PartnerShipDto : Entity
    {
        public string Logo { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int Order { get; set; }
    }
}
