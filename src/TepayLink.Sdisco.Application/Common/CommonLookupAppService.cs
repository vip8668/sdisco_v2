using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Microsoft.EntityFrameworkCore;
using SDisco.Home.Dto;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Clients;
using TepayLink.Sdisco.Common.Dto;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.Editions.Dto;
using TepayLink.Sdisco.Localization.Dto;
using TepayLink.Sdisco.Products;

namespace TepayLink.Sdisco.Common
{

    public class CommonLookupAppService : SdiscoAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        private readonly IApplicationLanguageManager _applicationLanguageManager;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<Place, long> _placeRepository;
        private readonly IRepository<ClientSetting, long> _clientSettingRepository;


        public CommonLookupAppService(EditionManager editionManager, IApplicationLanguageManager applicationLanguageManager, IRepository<Country> countryRepository, IRepository<Currency> currencyRepository, IRepository<Place, long> placeRepository, IRepository<ClientSetting, long> clientSettingRepository)
        {
            _editionManager = editionManager;
            _applicationLanguageManager = applicationLanguageManager;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
            _placeRepository = placeRepository;
            _clientSettingRepository = clientSettingRepository;
        }
        [AbpAuthorize]
        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }
        [AbpAuthorize]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    );

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }
        [AbpAuthorize]
        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }
        /// <summary>
        /// Danh sách quốc gia
        /// </summary>
        /// <returns></returns>
        public async Task<List<CountryOutputDto>> GetCountries()
        {
            List<Country> countries = _countryRepository.GetAll().ToList();
            List<CountryOutputDto> outpustdto = countries.Select(p => new CountryOutputDto
            {
                DisplayName = p.DisplayName,
                Icon = p.Icon,
                Id = p.Id,
                Name = p.Name,
                IsDisabled = p.IsDisabled

            }).ToList();
            return outpustdto;
        }
        /// <summary>
        /// Danh sách tiền tệ
        /// </summary>
        /// <returns></returns>
        public async Task<List<CurrencyOutputDto>> GetCurrencies()
        {
            var currency = _currencyRepository.GetAll().ToList();
            var outpustdto = currency.Select(p => new CurrencyOutputDto
            {
                DisplayName = p.DisplayName,

                Id = p.Id,
                Name = p.Name,

                CurrencySign = p.CurrencySign

            }).ToList();
            return outpustdto;
        }
        /// <summary>
        /// Danh sách ngôn ngữ
        /// </summary>
        /// <returns></returns>
        public async Task<GetLanguagesOutput> GetLanguages()
        {
            var languages = (await _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId)).OrderBy(l => l.DisplayName);
            var defaultLanguage = await _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId);

            return new GetLanguagesOutput(
                ObjectMapper.Map<List<ApplicationLanguageListDto>>(languages),
                defaultLanguage?.Name
            );
        }
        /// <summary>
        /// danh sách tỉnh thành phố
        /// </summary>
        /// <returns></returns>
        public async Task<List<BasicPlaceDto>> GetCities()
        {
            //todo chỗ này xem lại
            return _placeRepository.GetAll().Select(p => new BasicPlaceDto
            {
                Id = p.Id,
                PlaceName = p.Name
            }).ToList();
        }
        /// <summary>
        /// lấy giá trị client setting
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> GetClientSetting(GetClientSettingDto input)
        {
            var clientSetting = _clientSettingRepository.FirstOrDefault(p => p.Key == input.Key.ToLower());
            if (clientSetting != null)
            {
                return clientSetting.Value;
            }

            return null;
        }

        /// <summary>
        /// Thêm mới hoặc update setting nếu có rồi thì update, nếu chưa có insert mới
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateClientSetting(CreateOrUpdateClientSettingDto input)
        {
            var clientSetting = _clientSettingRepository.FirstOrDefault(p => p.Key == input.Key.ToLower());
            if (clientSetting != null)
            {
                clientSetting.Value = input.Value;
                _clientSettingRepository.Update(clientSetting);
            }
            else
            {
                _clientSettingRepository.Insert(new ClientSetting
                {
                    Key = input.Key.ToLower(),
                    Value = input.Value

                });
            }


        }
    }
}
