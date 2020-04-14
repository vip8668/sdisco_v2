using System.Collections.Generic;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public interface IBookingRefundsExcelExporter
    {
        FileDto ExportToFile(List<GetBookingRefundForViewDto> bookingRefunds);
    }
}