using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IWalletsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWalletForViewDto>> GetAll(GetAllWalletsInput input);

        Task<GetWalletForViewDto> GetWalletForView(long id);

		Task<GetWalletForEditOutput> GetWalletForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditWalletDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetWalletsToExcel(GetAllWalletsForExcelInput input);

		
		Task<PagedResultDto<WalletUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}