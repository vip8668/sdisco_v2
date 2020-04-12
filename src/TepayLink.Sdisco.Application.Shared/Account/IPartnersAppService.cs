using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IPartnersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPartnerForViewDto>> GetAll(GetAllPartnersInput input);

        Task<GetPartnerForViewDto> GetPartnerForView(long id);

		Task<GetPartnerForEditOutput> GetPartnerForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditPartnerDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetPartnersToExcel(GetAllPartnersForExcelInput input);

		
		Task<PagedResultDto<PartnerUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<PartnerDetinationLookupTableDto>> GetAllDetinationForLookupTable(GetAllForLookupTableInput input);
		
    }
}