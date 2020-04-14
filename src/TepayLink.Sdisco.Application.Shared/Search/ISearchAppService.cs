using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Search.Dto;

namespace TepayLink.Sdisco.Search
{
  public  interface ISearchAppService
    {
        Task<List<SearchCategoryDto>> GetSearchCategory();
        Task<List<SearchHistoryOutputDto>> GetSearchHistory();
        Task ClearSearchHistory();
        Task<List<SearchHistoryOutputDto>> GetSuggestSearch(string keyword);
        Task<object> SearchDestination(DestinationSearchDto input);

        Task<object> Search(DestinationSearchDto input);
    }
}
