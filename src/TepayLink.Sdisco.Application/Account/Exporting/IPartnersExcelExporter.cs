using System.Collections.Generic;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account.Exporting
{
    public interface IPartnersExcelExporter
    {
        FileDto ExportToFile(List<GetPartnerForViewDto> partners);
    }
}