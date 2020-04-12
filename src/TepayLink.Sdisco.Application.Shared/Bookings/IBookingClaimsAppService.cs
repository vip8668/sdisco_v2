using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IBookingClaimsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBookingClaimForViewDto>> GetAll(GetAllBookingClaimsInput input);

        Task<GetBookingClaimForViewDto> GetBookingClaimForView(long id);

		Task<GetBookingClaimForEditOutput> GetBookingClaimForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBookingClaimDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBookingClaimsToExcel(GetAllBookingClaimsForExcelInput input);

		
		Task<PagedResultDto<BookingClaimClaimReasonLookupTableDto>> GetAllClaimReasonForLookupTable(GetAllForLookupTableInput input);
		
    }
}