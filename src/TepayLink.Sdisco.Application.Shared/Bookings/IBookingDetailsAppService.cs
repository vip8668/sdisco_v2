using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IBookingDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBookingDetailForViewDto>> GetAll(GetAllBookingDetailsInput input);

        Task<GetBookingDetailForViewDto> GetBookingDetailForView(long id);

		Task<GetBookingDetailForEditOutput> GetBookingDetailForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBookingDetailDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBookingDetailsToExcel(GetAllBookingDetailsForExcelInput input);

		
		Task<PagedResultDto<BookingDetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}