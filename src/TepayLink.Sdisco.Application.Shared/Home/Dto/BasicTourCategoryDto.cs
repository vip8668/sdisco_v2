using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using SDisco.Tour.Dto;

namespace SDisco.Home.Dto
{
   public class BasicTourCategoryDto:BasicTourItemDto
    {
        public string SearchType { get; set; }
       
    }
}
