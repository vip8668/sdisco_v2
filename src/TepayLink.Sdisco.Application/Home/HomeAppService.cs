using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Home.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Home
{
    public class HomeAppService : SdiscoAppServiceBase, IHomeAppService
    {
        private IRepository<Category> _category;

        private IRepository<Place, long> _placeRepository;
        private IRepository<Product, long> _productRepository;
        private IRepository<ApplicationLanguage> _langRepository;
        private IRepository<Detination,long> _destinationRepository;

        private ICommonAppService _commonAppService;


        /// <summary>
        /// Lấy banner quảng cáo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BannerDto> GetBanner(GetBannerDto input)
        {
            if (input.BannerOrder == 1)
            {
                return new BannerDto
                {
                    Images = new List<PhotoDto>
                    {
                        new PhotoDto
                        {
                            Url = "http://123.31.29.208:22742/images/banner1.jpg"
                        }
                    }
                };
            }

            return new BannerDto
            {
                Title = "Exclusively for sDisco",
                Description =
                    "We are partnering with various airlines across the globe to get you to wherever you need to be.",
                Images = new List<PhotoDto>
                {
                    new PhotoDto
                    {
                        Url = "http://123.31.29.208:22742/images/discount1.jpg",
                    },
                    new PhotoDto
                    {
                        Url = "http://123.31.29.208:22742/images/discount2.jpg",
                    }
                }
            };
        }

        /// <summary>
        /// Danh sách Category sản phẩm ( 3G, Vé, show diễn,....)
        /// </summary>
        /// <returns></returns>
        public async Task<List<BasicProductCategoryDto>> GetListProductCategory()
        {
            var listCategories = _category.GetAll()
                .Select(p => new BasicProductCategoryDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.ProductType,

                    ThumbImages = new List<PhotoDto>
                    {
                        new PhotoDto
                        {
                            Url = p.Image
                        }
                    }
                }).ToList();

            return listCategories;
        }

        /// <summary>
        /// Danh mục Tour
        /// </summary>
        /// <returns></returns>
        public async Task<List<BasicTourCategoryDto>> GetListTourCatgory()
        {
            var listCategories = _category.GetAll()
                .Where(p => p.ProductType == ProductTypeEnum.Tour).Select(p =>
                    new BasicTourCategoryDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        SearchType = ProductTypeEnum.Tour.ToString("G"),
                        ThumbImages = new List<PhotoDto>
                        {
                            new PhotoDto
                            {
                                Url = p.Image
                            }
                        }
                    }).ToList();

            return listCategories;
        }

        /// <summary>
        /// Danh Mục Trip
        /// </summary>
        /// <returns></returns>
        public async Task<List<BasicTourCategoryDto>> GetListTripCatgory()
        {
            var listCategories = _category.GetAll()
                .Where(p => p.ProductType == ProductTypeEnum.Trip).Select(p =>
                    new BasicTourCategoryDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        SearchType = ProductTypeEnum.Tour.ToString("G"),
                        ThumbImages = new List<PhotoDto>
                        {
                            new PhotoDto
                            {
                                Url = p.Image
                            }
                        }
                    }).ToList();

            return listCategories;
        }

        /// <summary>
        /// Hoạt động hàng đầu
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<BasicActivityDto>> GetTopActivity(PagedInputDto input)
        {
            
            var query = (from a in _productRepository.GetAll()
                join l in _langRepository.GetAll() on a.LanguageId equals l.Id
                join p in _placeRepository.GetAll() on a.PlaceId equals p.Id
                where a.IsTop && a.Type == ProductTypeEnum.Activity
                              && a.Status == ProductStatusEnum.Publish
                select new BasicActivityDto
                {
                    Id = a.Id,
                    Name = a.Name,

                    Location = new BasicLocationDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Addess = p.DisplayAddress
                    },
                    BookCount = a.BookingCount,
                    Language = l.DisplayName,
                });
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds, ItemTypeEnum.TourItem);
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


        /// <summary>
        /// Điểm đến hàng đầu
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<BasicPlaceDto>> GetTopPlaces(PagedInputDto inputDto)
        {
            var query = _de.GetAll().Where(p => p.IsTop && p.PlaceType == PlaceTypeEnum.Destination);
            var total = query.Count();
            var places = query.Skip(inputDto.SkipCount).Take(inputDto.MaxResultCount).ToList();
            var list = ObjectMapper.Map<List<BasicPlaceDto>>(places);
            return new PagedResultDto<BasicPlaceDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        /// <summary>
        /// tour hàng đầu
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<BasicTourDto>> GetTopTours(PagedInputDto inputDto)
        {
            var query =
                (from t in
                        _tourRepository.GetAll()
                    join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                    join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                    join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                    //     join r in _tourReviewRepository.GetAll() on t.Id equals r.TourId into rGroup
                    where t.Type == TourTypeEnum.Tour && t.IsTop // && r.ReviewType == ReviewTypeEnum.Tour
                                                      && t.Status == TourStatusEnum.Publish
                    select new BasicTourDto
                    {
                        Id = t.Id,
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        PlaceId = p.Id,
                        PlaceName = p.PlaceName,
                        OfferLanguageId = t.LanguageId,
                        Title = t.Name,
                        LanguageOffer = l.DisplayName,
                        SoldCount = t.BookingCount,
                        IsHotDeal = t.IsHotDeal,
                        BestSaller = t.IsBestSeller,
                    });
            var total = query.Count();
            var list = query.Skip(inputDto.SkipCount).Take(inputDto.MaxResultCount).ToList();

            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds, ItemTypeEnum.Tour);


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

        //  private void FillData(List<>)

        /// <summary>
        ///Chuyến đi hàng đầu
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<BasicTourDto>> GetTopTrips(PagedInputDto inputDto)
        {
            var query =
                (from t in
                        _tourRepository.GetAll()
                    join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                    join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                    join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                    //  join r in _tourReviewRepository.GetAll() on t.Id equals r.TourId
                    where t.Type == TourTypeEnum.TripPlan && t.IsTop &&
                          t.IsTop // && r.ReviewType == ReviewTypeEnum.Tour
                          && t.Status == TourStatusEnum.Publish
                    select new BasicTourDto
                    {
                        Id = t.Id,
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        PlaceId = p.Id,
                        PlaceName = p.PlaceName,
                        OfferLanguageId = t.LanguageId,
                        //  Rating = r.RatingAvg,
                        Title = t.Name,

                        LanguageOffer = l.DisplayName,

                        // ReviewCount = r.ReviewCount,
                        SoldCount = t.BookingCount,
                        TripLength = t.TripLengh,

                        IsHotDeal = t.IsHotDeal,
                        BestSaller = t.IsBestSeller,
                    });
            var total = query.Count();
            var list = query.Skip(inputDto.SkipCount).Take(inputDto.MaxResultCount).ToList();
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await _commonAppService.GetSaveItem(itemIds, ItemTypeEnum.Tour);
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
    }
}