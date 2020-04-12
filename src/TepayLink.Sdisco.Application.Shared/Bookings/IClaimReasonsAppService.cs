using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Bookings
{
    public interface IClaimReasonsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetClaimReasonForViewDto>> GetAll(GetAllClaimReasonsInput input);

        Task<GetClaimReasonForViewDto> GetClaimReasonForView(int id);

		Task<GetClaimReasonForEditOutput> GetClaimReasonForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditClaimReasonDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetClaimReasonsToExcel(GetAllClaimReasonsForExcelInput input);

		
    }
}