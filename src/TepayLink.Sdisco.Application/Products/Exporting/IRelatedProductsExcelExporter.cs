using System.Collections.Generic;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products.Exporting
{
    public interface IRelatedProductsExcelExporter
    {
        FileDto ExportToFile(List<GetRelatedProductForViewDto> relatedProducts);
    }
}