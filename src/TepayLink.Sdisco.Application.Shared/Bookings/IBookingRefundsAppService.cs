using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IBookingRefundsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBookingRefundForViewDto>> GetAll(GetAllBookingRefundsInput input);

        Task<GetBookingRefundForViewDto> GetBookingRefundForView(long id);

		Task<GetBookingRefundForEditOutput> GetBookingRefundForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBookingRefundDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBookingRefundsToExcel(GetAllBookingRefundsForExcelInput input);

		
    }
}