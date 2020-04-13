using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Utils;

namespace TepayLink.Sdisco.Help.Dto
{
    public class HelpCategoryDto : Entity
    {
        public string CategoryName { get; set; }
        public string Slug => CategoryName.GenerateSlug();
    }
    public class HelpContentDto : Entity<long>
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    public class HelpContentSearchInputDto : PagedInputDto
    {
        public string Keyword { get; set; }
        public int CategoryId { get; set; }

    }
}
