﻿using System.Collections.Generic;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog.Exporting
{
    public interface IBlogProductRelatedsExcelExporter
    {
        FileDto ExportToFile(List<GetBlogProductRelatedForViewDto> blogProductRelateds);
    }
}