using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Tour.Dtos;


namespace SDisco.Home.Dto
{
    public class BasicActivityDto : BasicTourItemDto
    {
        public string WhatWeDo { get; set; }
        public string ShouldKnow { get; set; }
        
        
    }
}
