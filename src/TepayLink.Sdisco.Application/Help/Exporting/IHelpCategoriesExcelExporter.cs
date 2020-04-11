using System.Collections.Generic;
using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Help.Exporting
{
    public interface IHelpCategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetHelpCategoryForViewDto> helpCategories);
    }
}