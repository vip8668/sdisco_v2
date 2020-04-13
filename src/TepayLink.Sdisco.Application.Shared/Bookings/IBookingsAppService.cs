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

        Task<GetBookingForViewDto> GetBookingForView(long id);

		Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBookingDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBookingsToExcel(GetAllBookingsForExcelInput input);

		
		Task<PagedResultDto<BookingProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}