using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Affiliate.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Affiliate
{
    public interface IShortLinksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShortLinkForViewDto>> GetAll(GetAllShortLinksInput input);

        Task<GetShortLinkForViewDto> GetShortLinkForView(long id);

		Task<GetShortLinkForEditOutput> GetShortLinkForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditShortLinkDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetShortLinksToExcel(GetAllShortLinksForExcelInput input);

		
    }
}