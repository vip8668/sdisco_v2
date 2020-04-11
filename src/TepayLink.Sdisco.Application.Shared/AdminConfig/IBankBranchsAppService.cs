using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.AdminConfig
{
    public interface IBankBranchsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankBranchForViewDto>> GetAll(GetAllBankBranchsInput input);

        Task<GetBankBranchForViewDto> GetBankBranchForView(int id);

		Task<GetBankBranchForEditOutput> GetBankBranchForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBankBranchDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBankBranchsToExcel(GetAllBankBranchsForExcelInput input);

		
		Task<PagedResultDto<BankBranchBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input);
		
    }
}