using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using NUglify.JavaScript.Syntax;

using TepayLink.Sdisco.Tour.Dtos;

namespace SDisco.Home.Dto
{
    public class BasicProductCategoryDto : BasicTourItemDto
    {
        public string SearchType =>
            this.Type.ToString("G");
    }
}