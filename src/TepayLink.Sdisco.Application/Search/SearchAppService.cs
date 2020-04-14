using Abp.Application.Services.Dto;

using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Localization;
using Microsoft.EntityFrameworkCore.Internal;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Authorization.Users.Profile.Dto;
using TepayLink.Sdisco.Blog;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Search.Dto;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Search
{
    public class SearchAppService : SdiscoAppServiceBase, ISearchAppService
    {


        private readonly IRepository<SearchHistory, long> _searchHistoryRepository;

        private static List<SearchCategoryDto> _searchCategories;

        private IRepository<Category> _category;
  
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<UserReview, long> _userReviewRepository;
        private readonly IRepository<UserReviewDetail, long> _userReviewDetailRepository;
        private readonly IRepository<BlogComment, long> _blogCommentRepository;
        private IRepository<Place, long> _placeRepository;
        private IRepository<Product, long> _tourRepository;
        private IRepository<ApplicationLanguage> _langRepository;
        private IRepository<ProductSchedule, long> _tourSchedule;
  
        private IRepository<ProductReview, long> _tourReviewRepository;
        private IRepository<User, long> _userRepository;

        private IRepository<BlogPost, long> _blogPostRepository;

        private ICommonAppService _commonAppService;


        public SearchAppService(IRepository<SearchHistory, long> searchHistoryRepository, IRepository<Category> category, IRepository<Country> countryRepository, IRepository<UserReview, long> userReviewRepository, IRepository<UserReviewDetail, long> userReviewDetailRepository, IRepository<BlogComment, long> blogCommentRepository, IRepository<Place, long> placeRepository, IRepository<Product, long> tourRepository, IRepository<ApplicationLanguage> langRepository, IRepository<ProductSchedule, long> tourSchedule, IRepository<ProductReview, long> tourReviewRepository, IRepository<User, long> userRepository, IRepository<BlogPost, long> blogPostRepository, ICommonAppService commonAppService)
        {
            _searchHistoryRepository = searchHistoryRepository;
            _category = category;
            _countryRepository = countryRepository;
            _userReviewRepository = userReviewRepository;
            _userReviewDetailRepository = userReviewDetailRepository;
            _blogCommentRepository = blogCommentRepository;
            _placeRepository = placeRepository;
            _tourRepository = tourRepository;
            _langRepository = langRepository;
            _tourSchedule = tourSchedule;
            _tourReviewRepository = tourReviewRepository;
            _userRepository = userRepository;
            _blogPostRepository = blogPostRepository;
            _commonAppService = commonAppService;
            _searchCategories = SearchCategoryDto.InitList();
        }

        public async Task<List<SearchCategoryDto>> GetSearchCategory()
        {
            return _searchCategories.Take(4).ToList();
            // throw new System.NotImplementedException();
        }

        public async Task<List<SearchHistoryOutputDto>> GetSearchHistory()
        {
            var history = _searchHistoryRepository.GetAll()
                .Where(p => p.CreatorUserId == AbpSession.UserId)
                .OrderByDescending(p => p.CreationTime).Take(5).ToList();
            List<SearchHistoryOutputDto> historyItem = new List<SearchHistoryOutputDto>();
            foreach (var item in history)
            {
                var category =
                    _searchCategories.FirstOrDefault(p => p.Type == item.Type);

                historyItem.Add(new SearchHistoryOutputDto
                {
                    Category = category,
                    KeyWord = item.Keyword
                });
            }

            return historyItem;
        }

        public async Task ClearSearchHistory()
        {
            _searchHistoryRepository.Delete(p => p.CreatorUserId == AbpSession.UserId);
        }


        public async Task<List<SearchHistoryOutputDto>> GetSuggestSearch(string keyword)
        {
            var itemList = new List<SearchHistoryOutputDto>();

            for (int i = 0; i < _searchCategories.Count; i++)
            {
                itemList.Add(new SearchHistoryOutputDto
                {
                    Category = _searchCategories[i],
                    KeyWord = keyword
                });
            }

            return itemList;
        }

        public async Task<object> SearchDestination(DestinationSearchDto input)
        {
            InsertSearchHistory(new SearchHistory
            {
                Keyword = input.KeyWord,
                Type = "Destinations",
            });

            List<long> placeIds =
                _placeRepository.GetAll()
                    .Where(p => p.Name.Contains(input.KeyWord) || p.DisplayAddress.Contains(input.KeyWord))
                    .Select(p => p.Id)
                    .ToList();


            if (string.IsNullOrEmpty(input.Type))
            {
                return new
                {
                    Tour = await SearchTour(input, placeIds, ProductTypeEnum.Tour),
                    TripPlan = await SearchTour(input, placeIds, ProductTypeEnum.TripPlan),
                    Activity = await SearchActivity(input, placeIds),

                };
            }


            if (input.Type.ToLower() == DestinationSearchDto.TYPE_TOUR.ToLower())
            {
                return await SearchTour(input, placeIds, ProductTypeEnum.Tour);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_TRIPPLAN.ToLower())
            {
                return await SearchTour(input, placeIds, ProductTypeEnum.TripPlan);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_ACTIVITY.ToLower())
            {
                return await SearchActivity(input, placeIds);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_TRAVEL_GUIDE.ToLower())
            {
                return await SearchTravelGuide(input, placeIds);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_LOCAL_PARTNER.ToLower())
            {
                return await SearhPartner(input, placeIds);
            }
            else
            {
                return await SearhItem(input, placeIds);
            }

            //            if (new[]
            //            {
            //                DestinationSearchDto.TYPE_Showticket.ToLower(),
            //                DestinationSearchDto.TYPE_BikeRentail.ToLower(),
            //                DestinationSearchDto.TYPE_CurrencyExchange.ToLower(),
            //                DestinationSearchDto.TYPE_MOBILE_DATA.ToLower(),
            //                DestinationSearchDto.TYPE_LocalGuideBook.ToLower(),
            //            }.Contains(input.Type.ToLower()))
            //            {
            //                return await SearhItem(input, placeIds);
            //            }
            //
            //            return null;
        }

        public async Task<object> Search(DestinationSearchDto input)
        {
            InsertSearchHistory(new SearchHistory
            {
                Keyword = input.KeyWord,
                Type = input.Type,
            });

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_TOUR.ToLower())
            {
                return await SearchTour(input, ProductTypeEnum.Tour);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_TRIPPLAN.ToLower())
            {
                return await SearchTour(input, ProductTypeEnum.TripPlan);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_ACTIVITY.ToLower())
            {
                return await SearchActivity(input);
            }

            if (input.Type.ToLower() == DestinationSearchDto.TYPE_THING_TO_BUY.ToLower())
            {
                return await SearhItem(input);
            }

            throw new System.NotImplementedException();
        }

        private async Task<object> SearchTour(DestinationSearchDto input, List<long> placeIds, ProductTypeEnum type)
        {
            var query =
                (from t in
                        _tourRepository.GetAll()
                 join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                 join c in _category.GetAll() on t.CategoryId equals c.Id
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                 //todo leftjoin chỗ này
                 join s in _tourSchedule.GetAll() on t.Id equals s.ProductId into g
                 from f in g.DefaultIfEmpty()
                 where t.Type == type
                       && t.Status == ProductStatusEnum.Publish
                       && placeIds.Contains(t.PlaceId??0)
                       && (input.TourCategories == null || !input.TourCategories.Any()
                                                        || input.TourCategories.Contains(t.CategoryId))
                       && (input.TripLength == 0 || t.TripLengh == input.TripLength)
                       //Neeus la tripplan khog loc theo schedule
                       && (type == ProductTypeEnum.TripPlan ||
                        ((input.FromPrice == 0 || (f != null && f.Price >= input.FromPrice))
                          && (input.ToPrice == 0 || (f != null && f.Price < input.ToPrice))
                          && (input.Guest == 0 || (f != null && (f.TotalSlot - f.TotalBook) >= input.Guest))
                          && (input.FromDate == null || (f != null && f.StartDate >= input.FromDate))
                          && (input.ToDate == null || (f != null && f.StartDate <= input.ToDate))))
                 select new BasicTourDto
                 {
                     Id = t.Id,
                     CategoryId = c.Id,
                     CategoryName = c.Name,
                     PlaceId = p.Id,
                     PlaceName = p.Name,
                     OfferLanguageId = t.LanguageId??0,
                     Title = t.Name,
                     LanguageOffer = l.DisplayName,
                     SoldCount = t.BookingCount,
                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                 }).Distinct();
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);


            var dicReview = await _commonAppService.GetTourReviewSummarys(itemIds);
            var dicThumbImages = await _commonAppService.GetTourThumbPhotos(itemIds);
            var dicAvaiableTimes = await _commonAppService.GetAvaiableTimeOfTours(itemIds);

            foreach (var item in list)
            {
                var reviewItem = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.Review = reviewItem;
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return new PagedResultDto<BasicTourDto>()
            {
                Items = list,
                TotalCount = total
            };
        }

        private async Task<object> SearchTour(DestinationSearchDto input, ProductTypeEnum type)
        {
            List<long> dealids = _tourRepository.GetAll()
                .Where(p => p.Type == type && p.Status == ProductStatusEnum.Publish &&
                            (p.Name.Contains(input.KeyWord) || p.Description.Contains(input.KeyWord)))
                .Select(p => p.Id).ToList();


            var query =
                (from t in
                        _tourRepository.GetAll()
                 join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                 join c in _category.GetAll() on t.CategoryId equals c.Id
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id

                 //todo leftjoin chỗ này
                 join s in _tourSchedule.GetAll() on t.Id equals s.ProductId into g
                 from f in g.DefaultIfEmpty()
                 where t.Type == type
                 && dealids.Contains(t.Id)
                       && t.Status == ProductStatusEnum.Publish

                       && (input.TourCategories == null || !input.TourCategories.Any()
                                                        || input.TourCategories.Contains(t.CategoryId))
                       && (input.TripLength == 0 || t.TripLengh == input.TripLength)
                       //Neeus la tripplan khog loc theo schedule
                       && (type == ProductTypeEnum.TripPlan ||
                        ((input.FromPrice == 0 || (f != null && f.Price >= input.FromPrice))
                          && (input.ToPrice == 0 || (f != null && f.Price < input.ToPrice))
                          && (input.Guest == 0 || (f != null && (f.TotalSlot - f.TotalBook) >= input.Guest))
                          && (input.FromDate == null || (f != null && f.StartDate >= input.FromDate))
                          && (input.ToDate == null || (f != null && f.StartDate <= input.ToDate))))
                 select new BasicTourDto
                 {
                     Id = t.Id,
                     CategoryId = c.Id,
                     CategoryName = c.Name,
                     PlaceId = p.Id,
                     PlaceName = p.Name,
                     OfferLanguageId = t.LanguageId??0,
                     Title = t.Name,
                     LanguageOffer = l.DisplayName,
                     SoldCount = t.BookingCount,
                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                 }).Distinct();
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);


            var dicReview = await _commonAppService.GetTourReviewSummarys(itemIds);
            var dicThumbImages = await _commonAppService.GetTourThumbPhotos(itemIds);
            var dicAvaiableTimes = await _commonAppService.GetAvaiableTimeOfTours(itemIds);

            foreach (var item in list)
            {
                var reviewItem = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.Review = reviewItem;
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return new PagedResultDto<BasicTourDto>()
            {
                Items = list,
                TotalCount = total
            };
        }

        private async Task<object> SearchActivity(DestinationSearchDto input)
        {
            List<long> dealids = _tourRepository.GetAll()
                .Where(p => p.Type == ProductTypeEnum.Activity && p.Status == ProductStatusEnum.Publish &&
                            (p.Name.Contains(input.KeyWord) || p.Description.Contains(input.KeyWord)))
                .Select(p => p.Id).ToList();


            //  List<long> ids = new List<long>();
            var query = (from a in _tourRepository.GetAll()
                         join l in _langRepository.GetAll() on a.LanguageId equals l.Id
                         join p in _placeRepository.GetAll() on a.PlaceId equals p.Id

                         //todo leftjoin chỗ này

                         join s in _tourSchedule.GetAll() on a.Id equals s.ProductId into g
                         from f in g.DefaultIfEmpty()
                         where a.Type == ProductTypeEnum.Activity
                               && a.Status == ProductStatusEnum.Publish
                               && dealids.Contains(a.Id)
                               && (input.FromPrice == 0 || (f != null && f.Price >= input.FromPrice))
                               && (input.ToPrice == 0 || (f != null && f.Price < input.ToPrice))
                               && (input.Guest == 0 || (f != null && ((f.TotalSlot - f.TotalBook) > input.Guest)))
                               && (input.FromDate == null || (f != null && f.StartDate >= input.FromDate))
                               && (input.ToDate == null || (f != null && f.StartDate <= input.ToDate))
                         select new BasicActivityDto
                         {
                             Id = a.Id,
                             Name = a.Name,
                           //  Order = a.Order,
                             Location = new BasicLocationDto
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Addess = p.DisplayAddress
                             },
                             BookCount = a.BookingCount,
                             Language = l.DisplayName,
                         }).Distinct();
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);
            var dicReview = await _commonAppService.GetTourItemReviewSummarys(itemIds);
            var dicThumbImages = await _commonAppService.GetTourItemThumbPhotos(itemIds);
            var dicAvaiableTimes = await _commonAppService.GetAvaiableTimeOfTourItems(itemIds);
            foreach (var item in list)
            {
                var reviewItem = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.Review = reviewItem;
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return new PagedResultDto<BasicActivityDto>
            {
                Items = list,
                TotalCount = total
            };
        }


        private async Task<object> SearchActivity(DestinationSearchDto input, List<long> placeIds)
        {
            var query = (from a in _tourRepository.GetAll()
                         join l in _langRepository.GetAll() on a.LanguageId equals l.Id
                         join p in _placeRepository.GetAll() on a.PlaceId equals p.Id

                         join s in _tourSchedule.GetAll() on a.Id equals s.ProductId into g
                         from f in g.DefaultIfEmpty()
                         where a.Type == ProductTypeEnum.Activity
                               && a.Status == ProductStatusEnum.Publish
                              && placeIds.Contains(p.Id)
                               && (input.FromPrice == 0 || (f != null && f.Price >= input.FromPrice))
                               && (input.ToPrice == 0 || (f != null && f.Price < input.ToPrice))
                               && (input.Guest == 0 || (f != null && ((f.TotalSlot - f.TotalBook) > input.Guest)))
                               && (input.FromDate == null || (f != null && f.StartDate >= input.FromDate))
                               && (input.ToDate == null || (f != null && f.StartDate <= input.ToDate))
                         select new BasicActivityDto
                         {
                             Id = a.Id,
                             Name = a.Name,
                            // Order = a.Order,
                             Location = new BasicLocationDto
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Addess = p.DisplayAddress
                             },
                             BookCount = a.BookingCount,
                             Language = l.DisplayName,
                         }).Distinct();
            var total = query.Count();
            var list = query.OrderBy(p => p.Id).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);
            var dicReview = await _commonAppService.GetTourItemReviewSummarys(itemIds);
            var dicThumbImages = await _commonAppService.GetTourItemThumbPhotos(itemIds);
            var dicAvaiableTimes = await _commonAppService.GetAvaiableTimeOfTourItems(itemIds);
            foreach (var item in list)
            {
                var reviewItem = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.Review = reviewItem;
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return new PagedResultDto<BasicActivityDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        private async Task<object> SearhItem(DestinationSearchDto input, List<long> placeIds)
        {

            ProductTypeEnum type = (ProductTypeEnum)Enum.Parse(typeof(ProductTypeEnum), input.Type);

            var query = (from a in _tourRepository.GetAll()
                         join l in _langRepository.GetAll() on a.LanguageId equals l.Id
                         join p in _placeRepository.GetAll() on a.PlaceId equals p.Id
                         //  join s in _itemSchedule.GetAll() on a.Id equals s.ItemId
                         where a.Type == type
                               && a.Status == ProductStatusEnum.Publish
                               && placeIds.Contains(a.PlaceId??0)
                         select new BasicActivityDto
                         {
                             Id = a.Id,
                             Name = a.Name,
                           //  Order = a.Order,
                             Location = new BasicLocationDto
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Addess = p.DisplayAddress
                             },
                             BookCount = a.BookingCount,
                             Language = l.DisplayName,
                         }).Distinct();
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);
            var dicReview = await _commonAppService.GetTourItemReviewSummarys(itemIds);
            var dicThumbImages = await _commonAppService.GetTourItemThumbPhotos(itemIds);
            var dicAvaiableTimes = await _commonAppService.GetAvaiableTimeOfTourItems(itemIds);
            foreach (var item in list)
            {
                var reviewItem = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.Review = reviewItem;
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return new PagedResultDto<BasicActivityDto>
            {
                Items = list,
                TotalCount = total
            };
        }


        private async Task<object> SearhItem(DestinationSearchDto input)
        {
            List<ProductTypeEnum> types = new List<ProductTypeEnum>
            {
                
                 ProductTypeEnum.Activity,
                  ProductTypeEnum.Hotel,
                  ProductTypeEnum.Restaurant,
                  ProductTypeEnum.TicketShow,
                  ProductTypeEnum.TicketPlace,
            };
            List<long> ids = _tourRepository.GetAll()
                .Where(p => types.Contains(p.Type) && p.Status == ProductStatusEnum.Publish &&
                            (p.Name.Contains(input.KeyWord) || p.Description.Contains(input.KeyWord)))
                .Select(p => p.Id).ToList();

            var query = (from a in _tourRepository.GetAll()
                         join l in _langRepository.GetAll() on a.LanguageId equals l.Id
                         join p in _placeRepository.GetAll() on a.PlaceId equals p.Id
                         //  join s in _itemSchedule.GetAll() on a.Id equals s.ItemId

                         where
                             a.Status == ProductStatusEnum.Publish
                             && ids.Contains(a.Id)
                         select new BasicActivityDto
                         {
                             Type = a.Type,
                             Id = a.Id,
                             Name = a.Name,
                          //   Order = a.Order,
                             Location = new BasicLocationDto
                             {
                                 Id = p.Id,
                                 Name = p.Name,
                                 Addess = p.DisplayAddress
                             },
                             BookCount = a.BookingCount,
                             Language = l.DisplayName,
                         }).Distinct();
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);
            var dicReview = await _commonAppService.GetTourItemReviewSummarys(itemIds);
            var dicThumbImages = await _commonAppService.GetTourItemThumbPhotos(itemIds);
            var dicAvaiableTimes = await _commonAppService.GetAvaiableTimeOfTourItems(itemIds);
            foreach (var item in list)
            {
                var reviewItem = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.Review = reviewItem;
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return new PagedResultDto<BasicActivityDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        private async Task<object> SearhPartner(DestinationSearchDto input, List<long> placeIds)
        {
            var query =
                from user in
                    _userRepository.GetAll()
                join country in _countryRepository.GetAll() on user.ContryId equals country.Id into _g
                from c in _g.DefaultIfEmpty()
                where placeIds.Contains(user.CityId ?? 0) || placeIds.Contains(user.DistrictId ?? 0) ||
                      placeIds.Contains(user.PrecintId ?? 0)
                select new GetProfileViewDto
                {
                    UserId = user.Id,
                    AboutMe = user.AboutMe,
                    FullName = user.FullName,
                    VerifyEmail = user.IsEmailConfirmed,
                    VerifyPhone = user.IsPhoneNumberConfirmed,
                    Ranking = user.Ranking,
                    Rating = user.Rating ?? 0,
                    VerifyGovermentId = false,
                    VerifySocialMedia = false,
                    Work = user.Work,
                    UserType = user.UserType,
                    Avatar = user.Avatar,
                    Languages = user.LanguageSpeak,
                    LiveIn = c != null ? c.DisplayName : ""
                };

            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var languageIds = list.Select(p => p.Languages).ToList();
            var listLanguageIds = new List<int>();
            foreach (var item in languageIds)
            {


                var arr = (item ?? "").Split(',');
                foreach (var langid in arr)
                {
                    try
                    {

                        listLanguageIds.Add(int.Parse(langid));
                    }
                    catch (Exception e)
                    {

                    }

                }



            }

            var languages =
                _langRepository.GetAll().Where(p => listLanguageIds.Contains(p.Id)).ToList();


            var userIds = list.Select(p => p.UserId).ToList();
            var reviewSummarys = _userReviewRepository.GetAll().Where(p => userIds.Contains(p.UserId))
                .Select(userreview => new UserReviewDto
                {
                    UserId = userreview.UserId,
                    Food = userreview.Food,
                    GuideTour = userreview.GuideTour,
                    Itineraty = userreview.Itineraty,
                    Rating = userreview.Rating,
                    ReviewCount = userreview.ReviewCount,
                    Service = userreview.Service,
                    Transport = userreview.Transport
                });
            ;
            var userReivewDetailIds = _userReviewDetailRepository.GetAll().Where(p => userIds.Contains(p.UserId))
                .GroupBy(p => p.UserId).Select(p => p.Max(x => x.Id)).ToList();

            var reviewDetails = (from p in _userReviewDetailRepository.GetAll()
                                 join user in _userRepository.GetAll() on p.CreatorUserId equals user.Id
                                 where userReivewDetailIds.Contains(p.Id)
                                 select new UserReviewDetailDto
                                 {
                                     Avatar = user.Avatar,
                                     Comment = p.Comment,
                                     Food = p.Food,
                                     Reviewer = user.FullName,
                                     ReviewDate = p.CreationTime,
                                     GuideTour = p.GuideTour,
                                     Itineraty = p.Itineraty,
                                     Rating = p.Rating,
                                     Service = p.Service,
                                     Title = p.Title,
                                     Transport = p.Transport,
                                     UserId = p.UserId
                                 }).ToList();
            foreach (var item in list)
            {
                var reviewSummary = reviewSummarys.FirstOrDefault(p => p.UserId == item.UserId);
                var reiveDetail = reviewDetails.FirstOrDefault((p => p.UserId == item.UserId));
                item.Review = new Review
                {
                    Reviews = reviewSummary,
                    TopReview = reiveDetail
                };
                try
                {
                    var langIds = item.Languages.Split(',').Select(p => int.Parse(p)).ToList();
                    item.Languages = languages.Where(p => langIds.Contains(p.Id)).Select(p => p.DisplayName).Join(",");
                }
                catch (Exception e)
                {
                    item.Languages = "";
                }
            }


            return new PagedResultDto<GetProfileViewDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        private async Task<object> SearchTravelGuide(DestinationSearchDto input, List<long> placeIds)
        {
            var query = _blogPostRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.KeyWord), p =>
                (p.Title.Contains(input.KeyWord) || p.Content.Contains(input.KeyWord)) &&
                p.Status == BlogStatusEnum.Publish).OrderByDescending(p => p.CreationTime);
            var total = query.Count();
            var itemList = query.Select(p => new BasicBlogPostDto
            {
                Id = p.Id,
                Title = p.Title,
                ThumbImage = p.ThumbImage,
                PublishDate = p.PublishDate,
                ShortDesciption = p.ShortDescription,
            }).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var blogIds = itemList.Select(p => p.Id);
            var comments = _blogCommentRepository.GetAll().Where(p => blogIds.Contains(p.BlogPostId)).GroupBy(p => p.Id)
                .Select(p => new
                {
                    Id = p.Key,
                    Total = p.Count()
                }).ToList();

            foreach (var item in itemList)
            {
                var comment = comments.FirstOrDefault(p => p.Id == item.Id);
                item.TotalComment = comment != null ? comment.Total : 0;
            }

            return new PagedResultDto<BasicBlogPostDto>
            {
                TotalCount = total,
                Items = itemList
            };
        }

        private async Task InsertSearchHistory(SearchHistory searchHistory)
        {
            if ((AbpSession.UserId ?? 0) == 0)
            {
                return;
            }

            var item = _searchHistoryRepository.FirstOrDefault(p =>
                p.Type == searchHistory.Type && p.Keyword == searchHistory.Keyword &&
                p.CreatorUserId == AbpSession.UserId);
            if (item == null)
            {
                _searchHistoryRepository.Insert(searchHistory);
            }
            else
            {
                item.CreationTime = DateTime.Now;
                _searchHistoryRepository.Update(item);
            }
        }

    }
}
