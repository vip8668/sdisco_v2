using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IBookingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input);

        Task<GetBookingForViewDto> GetBookingForView(int id);

		Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBookingDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBookingsToExcel(GetAllBookingsForExcelInput input);

		
		Task<PagedResultDto<BookingProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}