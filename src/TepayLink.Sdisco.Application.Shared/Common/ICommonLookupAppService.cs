using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Common.Dto;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.Localization.Dto;

namespace TepayLink.Sdisco.Common
{
    public interface ICommonLookupAppService : IApplicationService
    {
        Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false);

        Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input);

        GetDefaultEditionNameOutput GetDefaultEditionName();

        Task<List<CountryOutputDto>> GetCountries();
        Task<List<CurrencyOutputDto>> GetCurrencies();
        Task<GetLanguagesOutput> GetLanguages();

        Task<List<BasicPlaceDto>> GetCities();
        Task<string> GetClientSetting(GetClientSettingDto input);
        Task CreateOrUpdateClientSetting(CreateOrUpdateClientSettingDto input);
    }
}