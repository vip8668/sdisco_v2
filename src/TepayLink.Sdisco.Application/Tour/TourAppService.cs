using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public class TourAppService : SdiscoAppServiceBase, ITourAppService
    {

        private readonly IRepository<Product, long> _tourRepository;      
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;
        // private readonly IRepository<ItemSchedule, long> _itemScheduleRepository;
        private readonly IRepository<ProductDetail, long> _tourDetailRepository;

        private readonly IRepository<Place, long> _placeRepository;

        private readonly IRepository<Category, int> _tourCategoryRepository;
        private readonly IRepository<SimilarProduct, long> _similarTourRepisitory;
        private readonly IRepository<SuggestedProduct, long> _suggestTourRepository;
        private readonly ICommonAppService _commonAppService;
        private readonly IRepository<ProductSchedule, long> _tourScheduleRepository;
        private readonly IRepository<ProductDetailCombo, long> _tourDetailItemRepository;

        public TourAppService(IRepository<Product, long> tourRepository, IRepository<User, long> userRepository, IRepository<ApplicationLanguage> langRepository, IRepository<ProductDetail, long> tourDetailRepository, IRepository<Place, long> placeRepository, IRepository<Category, int> tourCategoryRepository, IRepository<SimilarProduct, long> similarTourRepisitory, IRepository<SuggestedProduct, long> suggestTourRepository, ICommonAppService commonAppService, IRepository<ProductSchedule, long> tourScheduleRepository, IRepository<ProductDetailCombo, long> tourDetailItemRepository)
        {
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _langRepository = langRepository;
            _tourDetailRepository = tourDetailRepository;
            _placeRepository = placeRepository;
            _tourCategoryRepository = tourCategoryRepository;
            _similarTourRepisitory = similarTourRepisitory;
            _suggestTourRepository = suggestTourRepository;
            _commonAppService = commonAppService;
            _tourScheduleRepository = tourScheduleRepository;
            _tourDetailItemRepository = tourDetailItemRepository;
        }

        public async Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input)
        {
            return await _commonAppService.GetTourGuestPhoto(input);
        }

        public async Task<BasicTripPlanDto> GetTripPlan(GetTripPlanInput input)
        {
            return await GetTripPlan(input.TourId, input.HotelStar, input.SharedType);
        }

        public async Task<long> ChangeInfo(GetTripPlanInput input)
        {
            throw new NotImplementedException();
            //var tour = _tourRepository.FirstOrDefault(p => p.Id == input.TourId);
            //var newTour = _tourRepository.GetAll().OrderBy(p => p.ShardType).OrderBy(p => p.Star).FirstOrDefault(p =>
            //    p.ParentId == tour.ParentId && (p.Star == input.HotelStar) && ((byte)p.ShardType == input.SharedType));

            //if (newTour == null)
            //{
            //    throw new Abp.UI.UserFriendlyException("Tour không tồn tại");
            //}

            //return newTour.Id;
        }

        public async Task<List<BasicTourDto>> GetSimilarTour(long tourId)
        {
            var list =
            (
                from t in
                    _tourRepository.GetAll()
                join sm in _similarTourRepisitory.GetAll() on t.Id equals sm.SimilarProductId
                join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                where t.Status == ProductStatusEnum.Publish
                      && sm.ProductId == tourId && t.Type == ProductTypeEnum.Tour
                select new BasicTourDto
                {
                    Id = t.Id,
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    PlaceId = p.Id,
                    PlaceName = p.Name,
                    OfferLanguageId = t.LanguageId ?? 0,
                    Title = t.Name,
                    LanguageOffer = l.DisplayName,
                    SoldCount = t.BookingCount,
                    IsHotDeal = t.IsHotDeal,
                    BestSaller = t.IsBestSeller,
                }).OrderByDescending(p => p.Id).Take(8).ToList();
            if (list.Count == 0)
            {
                list =
            (
                from t in
                    _tourRepository.GetAll()
                    // join sm in _similarTourRepisitory.GetAll() on t.Id equals sm.SimilarTourId
                join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                where t.Type == ProductTypeEnum.Tour
                      && t.Status == ProductStatusEnum.Publish
                //   && sm.TourId == tourId && sm.Type == TourTypeEnum.Tour
                select new BasicTourDto
                {
                    Id = t.Id,
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    PlaceId = p.Id,
                    PlaceName = p.Name,
                    OfferLanguageId = t.LanguageId ?? 0,
                    Title = t.Name,
                    LanguageOffer = l.DisplayName,
                    SoldCount = t.BookingCount,
                    IsHotDeal = t.IsHotDeal,
                    BestSaller = t.IsBestSeller,
                }).OrderByDescending(p => p.Id).Take(8).ToList();

            }
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);
            foreach (var item in list)
            {
                var reviewItem = await _commonAppService.GetTourReviewSummary(item.Id);
                item.Review = reviewItem;
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = await _commonAppService.GetAvaiableTimeOfTour(item.Id);
            }
            return list;
        }

        public async Task<List<BasicTourDto>> GetSuggestTripPlan(long tourId)
        {
            var list =
            (
                from t in
                    _tourRepository.GetAll()
                join sm in _suggestTourRepository.GetAll() on t.Id equals sm.SuggestedProductId
                join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                where t.Type == ProductTypeEnum.TripPlan
                      && t.Status == ProductStatusEnum.Publish
                      && sm.ProductId == tourId
                select new BasicTourDto
                {
                    Id = t.Id,
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    PlaceId = p.Id,
                    PlaceName = p.Name,
                    OfferLanguageId = t.LanguageId ?? 0,

                    Title = t.Name,

                    LanguageOffer = l.DisplayName,

                    SoldCount = t.BookingCount,


                    IsHotDeal = t.IsHotDeal,
                    BestSaller = t.IsBestSeller,
                }).OrderByDescending(p => p.Id).Take(8).ToList();

            if (list.Count == 0)
            {
                list =
         (
             from t in
                 _tourRepository.GetAll()
                 // join sm in _suggestTourRepository.GetAll() on t.Id equals sm.SuggestedTourId
             join l in _langRepository.GetAll() on t.LanguageId equals l.Id
             join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
             join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
             where t.Type == ProductTypeEnum.TripPlan
                   && t.Status == ProductStatusEnum.Publish

             select new BasicTourDto
             {
                 Id = t.Id,
                 CategoryId = c.Id,
                 CategoryName = c.Name,
                 PlaceId = p.Id,
                 PlaceName = p.Name,
                 OfferLanguageId = t.LanguageId ?? 0,

                 Title = t.Name,

                 LanguageOffer = l.DisplayName,

                 SoldCount = t.BookingCount,


                 IsHotDeal = t.IsHotDeal,
                 BestSaller = t.IsBestSeller,
             }).OrderByDescending(p => p.Id).Take(8).ToList();
            }

            var itemIds = list.Select((p => p.Id)).ToList();



            var listSaveItem = await _commonAppService.GetSaveItem(itemIds);

            foreach (var item in list)
            {
                var reviewItem = await _commonAppService.GetTourReviewSummary(item.Id);
                item.Review = reviewItem;
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = await _commonAppService.GetAvaiableTimeOfTour(item.Id);
            }

            return list;
        }

        public async Task<List<BasicTourCategoryDto>> GetCategory()
        {
            var listCategories = _tourCategoryRepository.GetAll()
                .Where(p => p.ProductType == ProductTypeEnum.Tour).OrderBy(p => p.Order).ToList().Select(p=> new BasicTourCategoryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    
                    SearchType = p.ProductType.ToString("G"),
                    
                }).ToList();
            return listCategories;
        }

        public async Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input)
        {
            return await _commonAppService.GetTourReviewDetail(input);
        }

        public async Task<TourDetailDto> GetTourDetail(long tourId)
        {
            var tour = (from p in _tourRepository.GetAll()
                        join l in _langRepository.GetAll() on p.LanguageId equals l.Id
                        join u in _userRepository.GetAll() on p.HostUserId equals u.Id
                        where p.Id == tourId
                        select new TourDetailDto
                        {
                            Overview = p.Description,
                            Policies = p.Policies,

                            Language = l.DisplayName,
                            Title = p.Name,
                            HostUserInfo = new BasicHostUserInfo
                            {
                                Ranking = u.Ranking,
                                Ratting = u.Rating ?? 0,
                                FullName = u.FullName,
                                UserId = u.Id,
                                Avarta = u.Avatar
                            },
                            InstallBook = p.InstantBook
                        }).FirstOrDefault();

            if (tour == null)
                return null;

            var tourDetails = _tourDetailRepository.GetAll().Where(p => p.ProductId == tourId).ToList();
            var listTourDetailIds = tourDetails.Select(p => p.Id).ToList();


            ;
            tour.TripPlan = await GetTripPlan(tourId);
            var listActivityName = tour.TripPlan.TripPlanDetailDtos.Select(p => p.Activities.Select(x => x.Name).ToList()).Distinct().ToList();
            var activitynames = new List<string>();
            foreach (var activity in listActivityName)
            {
                activitynames.AddRange(activity);
            }

            activitynames = activitynames.Distinct().ToList();
            tour.Activities = activitynames;
            tour.Images = await _commonAppService.GetTourPhoto(tourId);

            tour.IsLove = await _commonAppService.IsSave(tourId);
            tour.Review = await _commonAppService.GetTourReviewSummary(tourId);
            return tour;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tourId"></param>
        /// <param name="star"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<BasicTripPlanDto> GetTripPlan(long tourId, int star = 0, byte type = 0)
        {
            var tour = _tourRepository.FirstOrDefault(p => p.Id == tourId);

            var item = _tourScheduleRepository.GetAll().Where(p => p.ProductId == tourId).OrderBy(p => p.Price)
                .FirstOrDefault();
            var tourDetails = _tourDetailRepository.GetAll().Where(p => p.ProductId == tourId).ToList();




            var tourItems = (from x in _tourRepository.GetAll()
                             join q in _tourDetailItemRepository.GetAll()
                                 on x.Id equals q.ItemId
                             join place in _placeRepository.GetAll() on x.PlaceId equals place.Id
                             where q.ProductId == tourId
                             select new
                             {
                                 x.Name,
                                 Star = 0,
                                 x.Id,
                                 TourDetailId = q.ProductDetailId,
                                 x.Price,
                                 x.Type,
                                 x.PlaceId,
                                 place.DisplayAddress,
                                 PlaceName = place.Name,
                                 place.Lat,
                                 place.Long,
                             }).ToList();



            var tripPlanDetailDtos = new List<TripPlanDetailDto>();

            foreach (var tourDetail in tourDetails)
            {
                var tripPlanDto = new TripPlanDetailDto
                {
                    Name = tourDetail.Title,
                    Order = tourDetail.Order,
                    Description = tourDetail.Description,
                    ThumbImage = tourDetail.ThumbImage,

                    Activities = tourItems
                        .Where(p => p.TourDetailId == tourDetail.Id && p.Type == ProductTypeEnum.Activity).Select(p =>
                            new BasicItemDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                ItemType = ProductTypeEnum.Activity
                            }).ToList(),
                    Restaurant = tourItems
                        .Where(p => p.TourDetailId == tourDetail.Id && p.Type == ProductTypeEnum.Restaurant).Select(
                            p => new BasicItemDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                ItemType = ProductTypeEnum.Restaurant
                            }).ToList(),
                    Transport = tourItems
                        .Where(p => p.TourDetailId == tourDetail.Id && p.Type == ProductTypeEnum.Transport).Select(p =>
                            new BasicItemDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                ItemType = ProductTypeEnum.Transport
                            }).ToList(),
                    Locations = tourItems
                        .Where(p => p.TourDetailId == tourDetail.Id /*&& p.Type == TourItemTypeEnum.place*/).Select(p =>
                            new BasicLocationDto()
                            {
                                Id = p.PlaceId??0,
                                Name = p.PlaceName,
                                Addess = p.DisplayAddress,
                                Lat = p.Lat,
                                Long = p.Long
                            }).Distinct().ToList(),
                    Accomodation = tourItems
                        .Where(p => p.TourDetailId == tourDetail.Id && p.Type == ProductTypeEnum.Hotel)
                        .OrderBy(p => p.Star).Select(p => new BasicItemDto { Name = p.Name, Id = p.Id }).FirstOrDefault()
                };


                tripPlanDetailDtos.Add(tripPlanDto);
            }

            var result = new BasicTripPlanDto
            {
              //  HotelStar = 0,tour.Star,
               // ShareType = tour.ShardType,
                TourId = tourId,
                TripPlanDetailDtos = tripPlanDetailDtos,
                TripLength = tripPlanDetailDtos.Count(),
                Price =
                    new BasicPriceDto
                    {
                        Price = item != null ? item.Price : 0,
                        Hotel = item != null ? item.HotelPrice : 0,
                        Ticket = item != null ? item.TicketPrice : 0,
                        ServiceFee = 0,//item != null ? item.ServiceFee : 0,
                        OldPrice = 0,//item != null ? item.OldPrice : 0,
                    },
            };
            return result;
        }
    }
}
