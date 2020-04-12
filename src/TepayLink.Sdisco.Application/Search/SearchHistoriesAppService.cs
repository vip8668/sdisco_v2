

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Search.Exporting;
using TepayLink.Sdisco.Search.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Search
{
	[AbpAuthorize(AppPermissions.Pages_SearchHistories)]
    public class SearchHistoriesAppService : SdiscoAppServiceBase, ISearchHistoriesAppService
    {
		 private readonly IRepository<SearchHistory, long> _searchHistoryRepository;
		 private readonly ISearchHistoriesExcelExporter _searchHistoriesExcelExporter;
		 

		  public SearchHistoriesAppService(IRepository<SearchHistory, long> searchHistoryRepository, ISearchHistoriesExcelExporter searchHistoriesExcelExporter ) 
		  {
			_searchHistoryRepository = searchHistoryRepository;
			_searchHistoriesExcelExporter = searchHistoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetSearchHistoryForViewDto>> GetAll(GetAllSearchHistoriesInput input)
         {
			
			var filteredSearchHistories = _searchHistoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Keyword.Contains(input.Filter) || e.Type.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeywordFilter),  e => e.Keyword == input.KeywordFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter),  e => e.Type == input.TypeFilter);

			var pagedAndFilteredSearchHistories = filteredSearchHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var searchHistories = from o in pagedAndFilteredSearchHistories
                         select new GetSearchHistoryForViewDto() {
							SearchHistory = new SearchHistoryDto
							{
                                UserId = o.UserId,
                                Keyword = o.Keyword,
                                Type = o.Type,
                                Id = o.Id
							}
						};

            var totalCount = await filteredSearchHistories.CountAsync();

            return new PagedResultDto<GetSearchHistoryForViewDto>(
                totalCount,
                await searchHistories.ToListAsync()
            );
         }
		 
		 public async Task<GetSearchHistoryForViewDto> GetSearchHistoryForView(long id)
         {
            var searchHistory = await _searchHistoryRepository.GetAsync(id);

            var output = new GetSearchHistoryForViewDto { SearchHistory = ObjectMapper.Map<SearchHistoryDto>(searchHistory) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SearchHistories_Edit)]
		 public async Task<GetSearchHistoryForEditOutput> GetSearchHistoryForEdit(EntityDto<long> input)
         {
            var searchHistory = await _searchHistoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSearchHistoryForEditOutput {SearchHistory = ObjectMapper.Map<CreateOrEditSearchHistoryDto>(searchHistory)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSearchHistoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_SearchHistories_Create)]
		 protected virtual async Task Create(CreateOrEditSearchHistoryDto input)
         {
            var searchHistory = ObjectMapper.Map<SearchHistory>(input);

			
			if (AbpSession.TenantId != null)
			{
				searchHistory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _searchHistoryRepository.InsertAsync(searchHistory);
         }

		 [AbpAuthorize(AppPermissions.Pages_SearchHistories_Edit)]
		 protected virtual async Task Update(CreateOrEditSearchHistoryDto input)
         {
            var searchHistory = await _searchHistoryRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, searchHistory);
         }

		 [AbpAuthorize(AppPermissions.Pages_SearchHistories_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _searchHistoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSearchHistoriesToExcel(GetAllSearchHistoriesForExcelInput input)
         {
			
			var filteredSearchHistories = _searchHistoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Keyword.Contains(input.Filter) || e.Type.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.KeywordFilter),  e => e.Keyword == input.KeywordFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter),  e => e.Type == input.TypeFilter);

			var query = (from o in filteredSearchHistories
                         select new GetSearchHistoryForViewDto() { 
							SearchHistory = new SearchHistoryDto
							{
                                UserId = o.UserId,
                                Keyword = o.Keyword,
                                Type = o.Type,
                                Id = o.Id
							}
						 });


            var searchHistoryListDtos = await query.ToListAsync();

            return _searchHistoriesExcelExporter.ExportToFile(searchHistoryListDtos);
         }


    }
}