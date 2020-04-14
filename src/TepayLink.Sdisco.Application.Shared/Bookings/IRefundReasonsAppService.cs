using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IRefundReasonsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRefundReasonForViewDto>> GetAll(GetAllRefundReasonsInput input);

        Task<GetRefundReasonForViewDto> GetRefundReasonForView(int id);

		Task<GetRefundReasonForEditOutput> GetRefundReasonForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRefundReasonDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRefundReasonsToExcel(GetAllRefundReasonsForExcelInput input);

		
    }
}