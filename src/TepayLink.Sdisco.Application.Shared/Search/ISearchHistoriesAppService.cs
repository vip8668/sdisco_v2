using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Search.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Search
{
    public interface ISearchHistoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSearchHistoryForViewDto>> GetAll(GetAllSearchHistoriesInput input);

        Task<GetSearchHistoryForViewDto> GetSearchHistoryForView(long id);

		Task<GetSearchHistoryForEditOutput> GetSearchHistoryForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditSearchHistoryDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetSearchHistoriesToExcel(GetAllSearchHistoriesForExcelInput input);

		
    }
}