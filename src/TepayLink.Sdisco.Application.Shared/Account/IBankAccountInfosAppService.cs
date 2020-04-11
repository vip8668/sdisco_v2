using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IBankAccountInfosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBankAccountInfoForViewDto>> GetAll(GetAllBankAccountInfosInput input);

        Task<GetBankAccountInfoForViewDto> GetBankAccountInfoForView(long id);

		Task<GetBankAccountInfoForEditOutput> GetBankAccountInfoForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditBankAccountInfoDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetBankAccountInfosToExcel(GetAllBankAccountInfosForExcelInput input);

		
		Task<PagedResultDto<BankAccountInfoBankLookupTableDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BankAccountInfoBankBranchLookupTableDto>> GetAllBankBranchForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BankAccountInfoUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}