using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface ISaveItemsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSaveItemForViewDto>> GetAll(GetAllSaveItemsInput input);

        Task<GetSaveItemForViewDto> GetSaveItemForView(long id);

		Task<GetSaveItemForEditOutput> GetSaveItemForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditSaveItemDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetSaveItemsToExcel(GetAllSaveItemsForExcelInput input);

		
		Task<PagedResultDto<SaveItemProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);
		
    }
}