using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.KOL
{
    public interface IPartnerRevenuesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPartnerRevenueForViewDto>> GetAll(GetAllPartnerRevenuesInput input);

        Task<GetPartnerRevenueForViewDto> GetPartnerRevenueForView(long id);

		Task<GetPartnerRevenueForEditOutput> GetPartnerRevenueForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPartnerRevenueDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPartnerRevenuesToExcel(GetAllPartnerRevenuesForExcelInput input);

		
    }
}