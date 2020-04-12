using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Cashout
{
    public interface IUserDefaultCashoutMethodTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserDefaultCashoutMethodTypeForViewDto>> GetAll(GetAllUserDefaultCashoutMethodTypesInput input);

        Task<GetUserDefaultCashoutMethodTypeForViewDto> GetUserDefaultCashoutMethodTypeForView(long id);

		Task<GetUserDefaultCashoutMethodTypeForEditOutput> GetUserDefaultCashoutMethodTypeForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditUserDefaultCashoutMethodTypeDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetUserDefaultCashoutMethodTypesToExcel(GetAllUserDefaultCashoutMethodTypesForExcelInput input);

		
		Task<PagedResultDto<UserDefaultCashoutMethodTypeCashoutMethodTypeLookupTableDto>> GetAllCashoutMethodTypeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserDefaultCashoutMethodTypeUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}