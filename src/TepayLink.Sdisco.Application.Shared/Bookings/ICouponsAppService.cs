using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface ICouponsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCouponForViewDto>> GetAll(GetAllCouponsInput input);

        Task<GetCouponForViewDto> GetCouponForView(long id);

		Task<GetCouponForEditOutput> GetCouponForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditCouponDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetCouponsToExcel(GetAllCouponsForExcelInput input);

		
    }
}