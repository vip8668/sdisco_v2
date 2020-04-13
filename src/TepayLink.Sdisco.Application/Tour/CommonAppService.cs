using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Http;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tours
{
    public class CommonAppService : SdiscoAppServiceBase, ICommonAppService
    {
        private readonly IRepository<ProductReview, long> _tourReviewRepository;
        private readonly IRepository<ProductReviewDetail, long> _tourReviewDetailRepository;
        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<ProductImage, long> _imageRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;
        private readonly IRepository<ProductSchedule, long> _itemScheduleRepository;
        private readonly IRepository<ProductDetail, long> _tourDetailRepository;
        private readonly IRepository<Place, long> _placeRepository;
       // private readonly IRepository<TourDetailSchedule, long> _tourDetailScheduleRepository;
        private readonly IRepository<Category> _tourCategoryRepository;
        private readonly IRepository<SimilarProduct, long> _similarTourItemRepository;
        private readonly IRepository<RelatedProduct, long> _relatedTourWithTourItemRepository;
        private readonly IRepository<Utility, int> _utilityRepository;
        private readonly IRepository<ProductUtility, long> _tourUtilityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<SaveItem, long> _saveItemRepository;
        private readonly IRepository<Wallet, long> _walletRepository;
        private readonly IRepository<PartnerRevenue, long> _partnerRevenueRepository;
        private readonly IRepository<ShareTransaction, long> _shareTransactionRepository;
        private readonly IRepository<TransPortdetail, long> _transportDetailRepository;
        private readonly IRepository<ProductDetailCombo, long> _tourDetailItemRepository;
        private readonly IRepository<FeeConfig, long> _feeConfigRepository;


        public async Task<PagedResultDto<ReviewDetailDto>> GetTourItemReviewDetail(GetReviewDetailInput input)
        {
            var query = (from p in _tourReviewDetailRepository.GetAll()
                         join q in _userRepository.GetAll() on p.CreatorUserId equals q.Id
                         where  p.ProductId == input.ItemId
                         select new ReviewDetailDto
                         {
                             Avatar = q.Avatar,
                             Comment = p.Comment,
                             Ratting = p.RatingAvg,
                             ReviewDate = p.CreationTime,
                             Reviewer = q.FullName,
                             Title = p.Title
                         });

            int total = query.Count();
            var reviewDetail = query.OrderByDescending(p => p.ReviewDate).Skip(input.SkipCount)
                .Take(input.MaxResultCount).ToList();
            return new PagedResultDto<ReviewDetailDto>
            {
                TotalCount = total,
                Items = reviewDetail
            };
        }

        public async Task<GuestPhotoDto> GetTourItemGuestPhoto(GetGuestPhotoInput input)
        {
            var query = _imageRepository.GetAll()
                .Where(p => p.ProductId == input.ItemId && p.ImageType == ImageType.GuestImage)
                .Select(p => new PhotoDto
                {
                    Title = p.Title,
                    Tag = p.Tag,
                    Url = p.Url
                });

            var total = query.Count();
            var photos = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var tags = photos.GroupBy(p => p.Tag).Select(p => new PhotoTag
            {
                Tag = p.Key,
                PhotoCount = p.Count()
            }).ToList();
            return new
                GuestPhotoDto()
            {
                TotalItem = total,
                Photos = photos,
                Tags = tags
            };
        }

        public async Task<PagedResultDto<ReviewDetailDto>> GetTourReviewDetail(GetReviewDetailInput input)
        {
            var query = (from p in _tourReviewDetailRepository.GetAll()
                         join q in _userRepository.GetAll() on p.CreatorUserId equals q.Id
                         where p.ProductId == input.ItemId
                         select new ReviewDetailDto
                         {
                             Avatar = q.Avatar,
                             Comment = p.Comment,
                             Ratting = p.RatingAvg,
                             ReviewDate = p.CreationTime,
                             Reviewer = q.FullName,
                             Title = p.Title
                         });

            int total = query.Count();
            var reviewDetail = query.OrderByDescending(p => p.ReviewDate).Skip(input.SkipCount)
                .Take(input.MaxResultCount).ToList();
            return new PagedResultDto<ReviewDetailDto>
            {
                TotalCount = total,
                Items = reviewDetail
            };
        }

        public async Task<GuestPhotoDto> GetTourGuestPhoto(GetGuestPhotoInput input)
        {
            var query = _imageRepository.GetAll()
                .Where(p => p.ProductId == input.ItemId && p.ImageType == ImageType.GuestImage
                           )
                .Select(p => new PhotoDto
                {
                    Title = p.Title,
                    Tag = p.Tag,
                    Url = p.Url
                });

            var total = query.Count();
            var photos = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var tags = photos.GroupBy(p => p.Tag).Select(p => new PhotoTag
            {
                Tag = p.Key,
                PhotoCount = p.Count()
            }).ToList();
            return new
                GuestPhotoDto()
            {
                TotalItem = total,
                Photos = photos,
                Tags = tags
            };
        }

        public async Task<ReviewSummaryDto> GetTourItemReviewSummary(long tourId)
        {
            var item = _tourReviewRepository
                .GetAll().FirstOrDefault(p => p.ProductId == tourId);
            if (item != null)
            {
                return ObjectMapper.Map<ReviewSummaryDto>(item);
            }

            return null;
        }

        public async Task<Dictionary<long, List<UtilityDto>>> GetUtilities(List<long> itemIds)
        {
            var query = from p in _utilityRepository.GetAll()
                        join q in _tourUtilityRepository.GetAll() on p.Id equals q.UtilityId
                        where  itemIds.Contains(q.ProductId)
                        select new
                        {
                            ItemId = q.ProductId,
                            UtilityId = q.UtilityId,
                            p.Icon,
                            p.Name
                        };
            Dictionary<long, List<UtilityDto>> dic = new Dictionary<long, List<UtilityDto>>();
            foreach (var itemId in itemIds)
            {
                dic.Add(itemId, query.Where(p => p.ItemId == itemId).Select(p => new UtilityDto
                {
                    Icon = p.Icon,
                    Id = p.UtilityId,
                    Name = p.Name
                }).ToList());
            }

            return dic;
        }

        public async Task<List<UtilityDto>> GetUtilityByIds(List<int> utilitiesId)
        {
            return _utilityRepository.GetAll().Where(p => utilitiesId.Contains(p.Id)).Select(p => new UtilityDto
            {
                Icon = p.Icon,
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }

        public async Task<Dictionary<long, ReviewSummaryDto>> GetTourItemReviewSummarys(List<long> tourIds)
        {
            var dic = new Dictionary<long, ReviewSummaryDto>();
            if (tourIds.Count == 1)
            {
                var item = await GetTourItemReviewSummary(tourIds[0]);
                dic.Add(tourIds[0], item);
            }
            else
            {
                var query = _tourReviewRepository
                    .GetAll().Where(p =>  tourIds.Contains(p.ProductId));
                foreach (var item in query)
                {
                    dic.Add(item.ProductId, ObjectMapper.Map<ReviewSummaryDto>(item));
                }
            }

            return dic;
        }

        public async Task<ReviewSummaryDto> GetTourReviewSummary(long tourId)
        {
            var item = _tourReviewRepository
                .GetAll().FirstOrDefault(p =>  p.ProductId == tourId);
            if (item != null)
            {
                return ObjectMapper.Map<ReviewSummaryDto>(item);
            }

            return null;
        }

        public async Task<decimal> GetRevenueOfTour(long tourId)
        {
            //todo xem lai cho nay
            return 0;
           // return _tourRepository.GetAll().Where(p => p.Id == tourId).Select(p => p.TotalRevenew).FirstOrDefault();
        }

        public async Task<Dictionary<long, ReviewSummaryDto>> GetTourReviewSummarys(List<long> tourIds)
        {
            var dic = new Dictionary<long, ReviewSummaryDto>();
            if (tourIds.Count == 1)
            {
                var item = await GetTourReviewSummary(tourIds[0]);
                dic.Add(tourIds[0], item);
            }
            else
            {
                var query = _tourReviewRepository
                    .GetAll().Where(p => tourIds.Contains(p.ProductId));
                foreach (var item in query)
                {
                    dic.Add(item.ProductId, ObjectMapper.Map<ReviewSummaryDto>(item));
                }
            }

            return dic;
        }

        public async Task<List<AvaiableTimeDto>> GetAvaiableTimeOfTour(long tourId)
        {
            return _itemScheduleRepository.GetAll().Where(p => p.ProductId == tourId && p.StartDate >= DateTime.Today).Select(
                p => new AvaiableTimeDto
                {
                    ItemId = p.ProductId,
                    FromDate = p.StartDate,
                    DepartureTime = p.DepartureTime,
                    ToDate = p.EndDate,
                    Price = new BasicPriceDto
                    {
                        Price = p.Price,
                    },
                    ScheduleId = p.Id
                }).ToList();
        }

        public async Task<Dictionary<long, List<AvaiableTimeDto>>> GetAvaiableTimeOfTours(List<long> tourIds)
        {
            var dic = new Dictionary<long, List<AvaiableTimeDto>>();
            if (tourIds.Count == 1)
            {
                var item = await GetAvaiableTimeOfTour(tourIds[0]);
                dic.Add(tourIds[0], item);
            }
            else
            {
                var query = _itemScheduleRepository.GetAll()
                    .Where(p => tourIds.Contains(p.ProductId) && p.StartDate >= DateTime.Today).Select(p =>
                        new AvaiableTimeDto
                        {
                            ItemId = p.ProductId,
                            FromDate = p.StartDate,
                            DepartureTime = p.DepartureTime,
                            ToDate = p.EndDate,
                            Price = new BasicPriceDto
                            {
                                Price = p.Price,
                            },
                            ScheduleId = p.Id,
                        }).ToList();

                var ids = query.Select(p => p.ItemId).Distinct().ToList();


                foreach (var id in ids)
                {
                    var list = query.Where(p => p.ItemId == id).ToList();
                    dic.Add(id, list);
                }
            }

            return dic;
        }


        public async Task<List<AvaiableTimeDto>> GetAvaiableTimeOfTourItem(long itemId)
        {
            return _itemScheduleRepository.GetAll().Where(p => p.ProductId == itemId && p.StartDate >= DateTime.Today)
                .Select(p => new AvaiableTimeDto
                {
                    ItemId = p.ProductId,
                    FromDate = p.StartDate,
                    DepartureTime = p.DepartureTime,
                    ToDate = p.EndDate,
                    Price = new BasicPriceDto
                    {
                        Price = p.Price,
                    },
                    ScheduleId = p.Id
                }).ToList();
        }

        public async Task<Dictionary<long, List<AvaiableTimeDto>>> GetAvaiableTimeOfTourItems(List<long> itemIds)
        {
            var dic = new Dictionary<long, List<AvaiableTimeDto>>();
            if (itemIds.Count == 1)
            {
                var item = await GetAvaiableTimeOfTourItem(itemIds[0]);
                dic.Add(itemIds[0], item);
            }
            else
            {
                var query = _itemScheduleRepository.GetAll()
                    .Where(p => itemIds.Contains(p.ProductId) && p.StartDate >= DateTime.Today).Select(p =>
                        new AvaiableTimeDto
                        {
                            ItemId = p.ProductId,
                            FromDate = p.StartDate,
                            DepartureTime = p.DepartureTime,
                            ToDate = p.EndDate,
                            Price = new BasicPriceDto
                            {
                                Price = p.Price,
                            },
                            ScheduleId = p.Id
                        }).ToList();

                var ids = query.Select(p => p.ItemId).Distinct().ToList();


                foreach (var id in ids)
                {
                    var list = query.Where(p => p.ItemId == id).ToList();
                    dic.Add(id, list);
                }
            }

            return dic;
        }

        public async Task<BasicPriceDto> GetPriceOfTour(long tourId)
        {
            return _itemScheduleRepository.GetAll().Where(p => p.ProductId == tourId).Select(p => new BasicPriceDto
            {
                Price = p.Price,
                Hotel = p.HotelPrice,
                Ticket = p.TicketPrice,
                OldPrice = 0,
                ServiceFee =0
            }
            ).OrderBy(p => p.Price).FirstOrDefault();
        }

        public async Task<Dictionary<long, BasicPriceDto>> GetPriceOfTours(List<long> tourIds)
        {
            var dic = new Dictionary<long, BasicPriceDto>();
            if (tourIds.Count == 1)
            {
                var item = await GetPriceOfTour(tourIds[0]);
                dic.Add(tourIds[0], item);
            }
            else
            {
                var query = _itemScheduleRepository.GetAll().Where(p => tourIds.Contains(p.ProductId)).Select(p =>
                    new BasicPriceDto
                    {
                        ItemId = p.ProductId,
                        Price = p.Price,
                        Hotel = p.HotelPrice,
                        Ticket = p.TicketPrice,
                        OldPrice = 0,
                        ServiceFee =0
                    }
                );


                var ids = query.Select(p => p.ItemId).Distinct().ToList();


                foreach (var id in ids)
                {
                    var item = query.Where(p => p.ItemId == id).OrderBy(p => p.Price).FirstOrDefault();
                    dic.Add(id, item);
                }
            }

            return dic;
        }

        public async Task<BasicPriceDto> GetPriceOfTourItem(long tourId)
        {
            return _itemScheduleRepository.GetAll().Where(p => p.ProductId == tourId).Select(p => new BasicPriceDto
            {
                Price = p.Price,
                OldPrice = 0
            }
            ).OrderBy(p => p.Price).FirstOrDefault();
        }

        public async Task<Dictionary<long, BasicPriceDto>> GetPriceOfTourItems(List<long> tourIds)
        {
            var dic = new Dictionary<long, BasicPriceDto>();
            if (tourIds.Count == 1)
            {
                var item = await GetPriceOfTourItem(tourIds[0]);
                dic.Add(tourIds[0], item);
            }

            else
            {
                var query = _itemScheduleRepository.GetAll().Where(p => tourIds.Contains(p.ProductId)).Select(p =>
                    new BasicPriceDto
                    {
                        ItemId = p.ProductId,
                        Price = p.Price,

                        OldPrice = 0,
                        ServiceFee =0
                    }
                );


                var ids = query.Select(p => p.ItemId).Distinct().ToList();


                foreach (var id in ids)
                {
                    var item = query.Where(p => p.ItemId == id).OrderBy(p => p.Price).FirstOrDefault();
                    dic.Add(id, item);
                }
            }

            return dic;
        }

        public async Task<List<PhotoDto>> GetTourItemPhoto(long itemId)
        {
            return _imageRepository.GetAll().Where(p =>
                    p.ImageType == ImageType.TourImage && p.ProductId == itemId )
                .Select(p =>
                    new PhotoDto
                    {
                        Url = p.Url,
                        Title = p.Title,
                        Tag = p.Tag
                    }).ToList();
        }

        public async Task<List<PhotoDto>> GetTourItemThumbPhoto(long itemId)
        {
            return _imageRepository.GetAll().Where(p =>
                    p.ImageType == ImageType.ThumbImage && p.ProductId == itemId )
                .Select(p =>
                    new PhotoDto
                    {
                        Url = p.Url,
                        Title = p.Title,
                        Tag = p.Tag
                    }).ToList();
        }

        public async Task<Dictionary<long, List<PhotoDto>>> GetTourItemThumbPhotos(List<long> itemIds)
        {
            var dic = new Dictionary<long, List<PhotoDto>>();
            if (itemIds.Count == 1)
            {
                var item = await GetTourItemThumbPhoto(itemIds[0]);
                dic.Add(itemIds[0], item);
            }

            else
            {
                var query = _imageRepository.GetAll().Where(p =>
                        p.ImageType == ImageType.ThumbImage && itemIds.Contains(p.ProductId ?? 0) )
                    .Select(p =>
                        new ProductImage
                        {
                            ProductId = p.ProductId,
                            Url = p.Url,
                            Title = p.Title,
                            Tag = p.Tag
                        });


                var ids = query.Select(p => (p.ProductId ?? 0)).Distinct().ToList();


                foreach (var id in ids)
                {
                    var item = query.Where(p => p.ProductId == id).Select(p =>
                        new PhotoDto
                        {
                            Url = p.Url,
                            Title = p.Title,
                            Tag = p.Tag
                        }).ToList();
                    dic.Add(id, item);
                }
            }

            return dic;
        }

        public async Task<List<PhotoDto>> GetTourPhoto(long itemId)
        {
            return _imageRepository.GetAll().Where(p =>
                p.ImageType == ImageType.TourImage && p.ProductId == itemId).Select(
                p =>
                    new PhotoDto
                    {
                        Url = p.Url,
                        Title = p.Title,
                        Tag = p.Tag
                    }).ToList();
        }

        public async Task<List<PhotoDto>> GetTourThumbPhoto(long itemId)
        {
            return _imageRepository.GetAll().Where(p =>
                    p.ImageType == ImageType.ThumbImage && p.ProductId == itemId)
                .Select(p =>
                    new PhotoDto
                    {
                        Url = p.Url,
                        Title = p.Title,
                        Tag = p.Tag
                    }).ToList();
        }

        public async Task<Dictionary<long, List<PhotoDto>>> GetTourThumbPhotos(List<long> itemIds)
        {
            var dic = new Dictionary<long, List<PhotoDto>>();
            if (itemIds.Count == 1)
            {
                var item = await GetTourThumbPhoto(itemIds[0]);
                dic.Add(itemIds[0], item);
            }

            else
            {
                var query = _imageRepository.GetAll().Where(p =>
                        p.ImageType == ImageType.ThumbImage && itemIds.Contains(p.ProductId ?? 0))
                    .Select(p =>
                        new ProductImage
                        {
                            ProductId = p.ProductId,
                            Url = p.Url,
                            Title = p.Title,
                            Tag = p.Tag
                        });


                var ids = query.Select(p => (p.ProductId ?? 0)).Distinct().ToList();


                foreach (var id in ids)
                {
                    var item = query.Where(p => p.ProductId == id).Select(p =>
                        new PhotoDto
                        {
                            Url = p.Url,
                            Title = p.Title,
                            Tag = p.Tag
                        }).ToList();
                    dic.Add(id, item);
                }
            }

            return dic;
        }

        public async Task<List<BasicTourDto>> GetRelateTour(long itemId)
        {
            var list =
                (from t in
                        _tourRepository.GetAll()
                 join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                 join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                 join re in _relatedTourWithTourItemRepository.GetAll() on t.Id equals re.RelatedProductId
                 where t.Type == ProductTypeEnum.Tour
                       && t.Status == ProductStatusEnum.Publish
                       && re.ProductId == itemId
                 select new BasicTourDto
                 {
                     Id = t.Id,
                     CategoryId = c.Id,
                     CategoryName = c.Name,
                     PlaceId = p.Id,
                     PlaceName = p.DisplayAddress,
                     OfferLanguageId = t.LanguageId??0,
                     Title = t.Name,

                     LanguageOffer = l.DisplayName,
                     SoldCount = t.BookingCount,
                     TripLength = t.TripLengh,

                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                 }).OrderByDescending(p => p.Id).Take(8).ToList();

            if (list.Count == 0)
                list =
                (from t in
                        _tourRepository.GetAll()
                 join l in _langRepository.GetAll() on t.LanguageId equals l.Id
                 join c in _tourCategoryRepository.GetAll() on t.CategoryId equals c.Id
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                 // join re in _relatedTourWithTourItemRepository.GetAll() on t.Id equals re.RelatedTourId
                 where t.Type == ProductTypeEnum.Tour
                       && t.Status == ProductStatusEnum.Publish
                 // && re.ItemId == itemId
                 select new BasicTourDto
                 {
                     Id = t.Id,
                     CategoryId = c.Id,
                     CategoryName = c.Name,
                     PlaceId = p.Id,
                     PlaceName = p.DisplayAddress,
                     OfferLanguageId = t.LanguageId??0,
                     Title = t.Name,

                     LanguageOffer = l.DisplayName,
                     SoldCount = t.BookingCount,
                     TripLength = t.TripLengh,

                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                 }).OrderByDescending(p => p.Id).Take(8).ToList();

            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await GetSaveItem(itemIds);
            var dicReview = await GetTourReviewSummarys(itemIds);
            var dicThumbImages = await GetTourThumbPhotos(itemIds);
            var dicAvaiableTimes = await GetAvaiableTimeOfTours(itemIds);
            foreach (var item in list)
            {
                item.Review = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return list;
        }

        public async Task<List<BasicTourDto>> GetTourByTourIds(List<long> tourIds)
        {
            var list =
                (from t in
                        _tourRepository.GetAll()
                 join lang in _langRepository.GetAll() on t.LanguageId equals lang.Id into _l
                 from l in _l.DefaultIfEmpty(new ApplicationLanguage())
                 join ca in _tourCategoryRepository.GetAll() on t.CategoryId equals ca.Id into _ca
                 from c in _ca.DefaultIfEmpty(new Category())
                 join place in _placeRepository.GetAll() on t.PlaceId equals place.Id into _place
                 from p in _place.DefaultIfEmpty(new Place())
                 where t.Type == ProductTypeEnum.Tour
                       && t.Status == ProductStatusEnum.Publish
                       && tourIds.Contains(t.Id)
                 select new BasicTourDto
                 {
                     Id = t.Id,
                     CategoryId = c.Id,
                     CategoryName = c.Name,
                     PlaceId = p.Id,
                     PlaceName = p.DisplayAddress,
                     OfferLanguageId = t.LanguageId??0,
                     Title = t.Name,
                     LanguageOffer = l.DisplayName,
                     SoldCount = t.BookingCount,
                     TripLength = t.TripLengh,
                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                 }).ToList();

            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await GetSaveItem(itemIds);

            var dicReview = await GetTourReviewSummarys(itemIds);
            var dicThumbImages = await GetTourThumbPhotos(itemIds);
            var dicAvaiableTimes = await GetAvaiableTimeOfTours(itemIds);

            foreach (var item in list)
            {
                item.Review = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
            }

            return list;
        }


        public async Task<List<SaveItemDto>> GetSaveItem(List<long> itemIds)
        {
            var items = _saveItemRepository.GetAll().Where(p =>
                    itemIds.Contains(p.ProductId)  && p.CreatorUserId == AbpSession.UserId)
                .Select(p => new SaveItemDto
                {
                    ItemId = p.ProductId
                }).ToList();
            return items;
        }

        public async Task<bool> IsSave(long itemId)
        {
            var isLove = _saveItemRepository
                             .FirstOrDefault(p =>
                                 itemId == (p.ProductId) &&
                                 p.CreatorUserId == AbpSession.UserId) != null;
            return isLove;
        }

        public async Task<List<BasicTourItemDto>> GetRelateTourItem(long itemId)
        {
            var list =
                (from t in
                        _tourRepository.GetAll()
                 join re in _similarTourItemRepository.GetAll() on t.Id equals re.SimilarProductId
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                 where re.ProductId == itemId
                 && t.Status == ProductStatusEnum.Publish
                 select new BasicTourItemDto
                 {
                     Id = t.Id,
                     Name = t.Name,
                     BookCount = t.BookingCount,
                     InstantBook = t.InstantBook,
                     Location = new BasicLocationDto
                     {
                         Id = p.Id,
                         Name = p.Name,
                         Addess = p.DisplayAddress
                     },
                 }).OrderByDescending(p => p.Id).Take(8).ToList();
            if (list.Count == 0)
            {


                list =
                (from t in
                        _tourRepository.GetAll()
                     //  join re in _similarTourItemRepository.GetAll() on t.Id equals re.SimilarItemId
                 join p in _placeRepository.GetAll() on t.PlaceId equals p.Id
                 where t.Status == ProductStatusEnum.Publish && t.Type == ProductTypeEnum.Activity
                 select new BasicTourItemDto
                 {
                     Id = t.Id,
                     Name = t.Name,
                     BookCount = t.BookingCount,
                     InstantBook = t.InstantBook,
                     Location = new BasicLocationDto
                     {
                         Id = p.Id,
                         Name = p.Name,
                         Addess = p.DisplayAddress
                     },
                 }).OrderByDescending(p => p.Id).Take(8).ToList();
            }
            var itemIds = list.Select((p => p.Id)).ToList();
            var listSaveItem = await GetSaveItem(itemIds);
            var dicReview = await GetTourItemReviewSummarys(itemIds);
            var dicThumbImages = await GetTourItemThumbPhotos(itemIds);
            var dicAvaiableTimes = await GetAvaiableTimeOfTourItems(itemIds);

            foreach (var item in list)
            {
                item.ThumbImages = dicThumbImages.ContainsKey(item.Id) ? dicThumbImages[item.Id] : new List<PhotoDto>();
                item.Review = dicReview.ContainsKey(item.Id) ? dicReview[item.Id] : new ReviewSummaryDto();
                item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.AvaiableTimes = dicAvaiableTimes.ContainsKey(item.Id)
                    ? dicAvaiableTimes[item.Id]
                    : new List<AvaiableTimeDto>();
                var avaiableTime = item.AvaiableTimes.OrderBy(p => p.Price).FirstOrDefault();
                item.Price = avaiableTime?.Price;
            }

            return list;
        }

        public long GetHostUserId(long userId)
        {
            var user = UserManager.Users.FirstOrDefault(p => p.Id == userId);
            return user != null ? ((user.HostUserId ?? 0L) != 0 ? (user.HostUserId ?? 0) : userId) : userId;
        }

        public void InsertImages(List<string> photos, long itemId, ImageType imageType)
        {
            foreach (var photo in photos)
            {
                _imageRepository.Insert(new ProductImage
                {
                    ImageType = imageType,
                    ProductId = itemId,
                    
                    Url = photo,
                });

            }
        }

        public void DeleteImageOfTour(long itemId, IList<ImageType> imageTypes)
        {
            _imageRepository.Delete(p => p.ProductId == itemId  && imageTypes.Contains(p.ImageType));

        }
        [UnitOfWork]
        public async Task ProcessBonus(long tripId, RevenueTypeEnum type)
        {
            //todo xem lai cho nay

            //var trip = _tourRepository.FirstOrDefault(p => p.Id == tripId && p.Type == TourTypeEnum.TripPlan);
            //if (trip != null)
            //{

            //    int point = 0;
            //    if (type == RevenueTypeEnum.Shared)
            //        point = AppConsts.SharePoint;
            //    if (type == RevenueTypeEnum.Coppy)
            //        point = AppConsts.CoppyPoint;
            //    if (type == RevenueTypeEnum.Booking)
            //        point = AppConsts.Booking;
            //    if (point == 0)
            //        return;
            //    var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            //    var existShare = _shareTransactionRepository.GetAll()
            //        .FirstOrDefault(p =>
            //            p.ItemId == tripId && p.ItemType == ItemTypeEnum.Tour && p.IP == remoteIpAddress && p.Type == type);
            //    if (existShare == null)
            //    {
            //        existShare = new ShareTransaction
            //        {
            //            Type = type,
            //            IP = remoteIpAddress,
            //            ItemId = tripId,
            //            Point = point,
            //            UserId = trip.CreatorUserId ?? 0,
            //            ItemType = ItemTypeEnum.Tour,
            //        };
            //        var revenue = new PartnerRevenue
            //        {
            //            ItemType = ItemTypeEnum.Tour,
            //            ItemId = tripId,

            //            Point = point,
            //            Money = 0,
            //            RevenueType = type,
            //            Status = RevenueStatusEnum.Approve,
            //            Userid = trip.CreatorUserId ?? 0,


            //        };
            //        _partnerRevenueRepository.Insert(revenue);
            //        _shareTransactionRepository.Insert(existShare);
            //        var wallet = _walletRepository.FirstOrDefault(p =>
            //            p.UserId == trip.CreatorUserId && p.Type == WalletType.Point);
            //        if (wallet == null)
            //        {
            //            wallet = new Wallet
            //            {
            //                Type = WalletType.Point,
            //                Balance = point,
            //                UserId = trip.CreatorUserId ?? 0,
            //            };
            //            _walletRepository.Insert(wallet);
            //        }
            //        else
            //        {
            //            wallet.Balance = wallet.Balance + point;
            //            _walletRepository.Update(wallet);
            //        }
            //    }
            //}
        }

        public async Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input, bool checkHostUser)
        {
            long hostUserId = GetHostUserId(AbpSession.UserId ?? 0);
            if (input.CategoryIds.Count == 0)
                input.CategoryIds = (await GetSearchCategory()).Select(p => int.Parse(p.Id.ToString())).ToList();

            var tempList =
                (from p in _tourItemRepository.GetAll()
                 join l in _langRepository.GetAll() on p.LanguageId equals l.Id
                 join place in _placeRepository.GetAll() on p.PlaceId equals place.Id
                 where p.Name.Contains(input.Keyword)
                       && (input.CategoryIds.Count == 0 || input.CategoryIds.Contains((byte)(p.Type)))
                       && (p.HostUserId == hostUserId || checkHostUser == false)
                 select new SearchItemOutputDto
                 {
                     Name = p.Name,
                     Id = p.Id,
                     IncludeTourGuide = p.IncludeTourGuide,
                     Description = p.Description,
                     Language = l.DisplayName,
                     Type = p.Type,
                     LanguageId = p.LanguageId,
                     StartTime = p.StartTime,
                     Duration = p.Duration,
                     InstantBook = p.InstantBook,
                     Location = new BasicLocationDto
                     {
                         Addess = place.Address,
                         Name = place.PlaceName,
                         Id = place.Id,
                         Lat = place.Lat,
                         Long = place.Long
                     },
                     Price = new BasicPriceDto
                     {
                         Price = p.Price,
                         CostPrice = p.CostPrice,
                     },
                 }).ToList();

            var itemList = new List<SearchItemOutputDto>();

            var utilites =
                await GetUtilities(tempList.Select(p => p.Id).ToList(), ItemTypeEnum.TourItem);

            foreach (var item in tempList)
            {
                item.Review = await GetTourItemReviewSummary(item.Id);
                item.ThumbImages = await GetTourItemThumbPhoto(item.Id);
                item.Utilities = utilites[item.Id];
                itemList.Add(item);
            };
            return itemList;

        }

        public async Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input, bool checkHostUser)
        {
            long hostUserId = GetHostUserId(AbpSession.UserId ?? 0);
            var itemList =
                (from p in _tourItemRepository.GetAll()
                 join place in _placeRepository.GetAll() on p.PlaceId equals place.Id

                 where p.Name.Contains(input.Keyword)
                       && (p.HostUserId == hostUserId || checkHostUser == false)
                 select new SearchHotelOutputDto
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Star = p.Star,
                     LanguageId = p.LanguageId,

                     Address = p.Address,
                     Location = new BasicLocationDto
                     {
                         Addess = place.Address,
                         Name = place.PlaceName
                     },

                     InstantBook = p.InstantBook,
                     Description = p.Description,
                 }).ToList();
            List<long> hotelIds = itemList.Select(p => p.Id).Distinct().ToList();

            List<TourItem> listRoom = new List<TourItem>();
            if (hotelIds.Any())
            {
                listRoom = _tourItemRepository.GetAll().Where(p => hotelIds.Contains(p.ParentId ?? 0)).ToList();
            }
            var utilites =
                await GetUtilities(hotelIds, ItemTypeEnum.TourItem);

            var langIds = itemList.Select(p => p.LanguageId).ToList().Distinct();
            var langs = _langRepository.GetAll().Where(p => langIds.Contains(p.Id));

            foreach (var item in itemList)
            {
                item.ThumbImages = await GetTourItemThumbPhoto(item.Id);
                item.Review = await GetTourItemReviewSummary(item.Id);
                item.Language = langs.FirstOrDefault(p => p.Id == item.LanguageId)?.DisplayName;
                List<RoomDto> rooms = listRoom.Where(p => p.ParentId == item.Id).Select(p => new RoomDto
                {
                    Id = p.Id,

                    RoomName = p.Name,
                    Price = new BasicPriceDto
                    {
                        Price = p.Price,
                        CostPrice = p.CostPrice
                    }
                }).ToList();
                item.Rooms = rooms;
                item.Utilities = utilites[item.Id];
            }
            ;
            return itemList;
        }

        public async Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input, bool checkHostUser)
        {
            long hostUserId = GetHostUserId(AbpSession.UserId ?? 0);
            var tempList =
                (from p in _tourItemRepository.GetAll()

                 join t in _transportDetailRepository.GetAll() on p.Id equals t.ItemId
                 where t.From.Contains(input.From) &&
                       t.To.Contains(input.To)
                       && (p.HostUserId == hostUserId || checkHostUser == false)
                 select new SearchTransportOutputDto
                 {
                     Id = p.Id,
                     LanguageId = p.LanguageId,

                     Description = p.Description,
                     From = t.From,
                     To = t.To,
                     TotalSeat = t.TotalSeat,
                     Name = p.Name,
                     Status = p.Status,
                     InstantBook = p.InstantBook,
                     Duration = p.Duration,
                     StartTime = p.StartTime,
                     IsTaxi = t.IsTaxi,
                     Price = new BasicPriceDto
                     {
                         Price = p.Price,
                         CostPrice = p.CostPrice
                     },
                 }).ToList();


            var itemList = new List<SearchTransportOutputDto>();

            var utilites =
                await GetUtilities(tempList.Select(p => p.Id).ToList(), ItemTypeEnum.TourItem);
            var langIds = itemList.Select(p => p.LanguageId).ToList().Distinct();
            var langs = _langRepository.GetAll().Where(p => langIds.Contains(p.Id));
            foreach (var item in tempList)
            {
                item.Language = langs.FirstOrDefault(p => p.Id == item.LanguageId)?.DisplayName;
                item.Review = await GetTourItemReviewSummary(item.Id);
                item.ThumbImages = await GetTourItemThumbPhoto(item.Id);
                item.Utilities = utilites[item.Id];
                itemList.Add(item);
            }

            ;

            return itemList;
        }

        private DB.Place GetPlace(string address)
        {
            var place = _placeRepository.FirstOrDefault(p => p.Address == address);

            if (place == null)
            {
                place = new DB.Place
                {
                    Address = address,
                    PlaceName = address,
                };
                place.Id = _placeRepository.InsertAndGetId(place);
            }

            return place;
        }

        public async Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input, bool isFromHost)
        {

            long hostUserId = GetHostUserId(AbpSession.UserId ?? 0);

            DB.Place place = GetPlace(input.Address);
            var hotel = new TourItem
            {
                Name = input.Name,
                Star = input.Star,
                Type = TourItemTypeEnum.Hotel,
                PlaceId = place.Id,
                Status = isFromHost ? StatusEnum.Active : StatusEnum.WaitApprove,
                LanguageId = input.Language,
                Address = input.Address,
                Description = input.Description,
                HostUserId = hostUserId,
                InstantBook = input.InstantBook,
                IncludeTourGuide = input.IncludeTourGuide,
                Policies = input.Policy,
            };
            //insert hotel
            hotel.Id = _tourItemRepository.InsertAndGetId(hotel);
            InsertImages(input.Photos, hotel.Id, ImageTypeEnum.TourImage, ItemTypeEnum.TourItem);
            if (input.ThumbImages == null || input.ThumbImages.Count == 0)
                input.ThumbImages = input.Photos;
            InsertImages(input.ThumbImages, hotel.Id, ImageTypeEnum.ThumbImage, ItemTypeEnum.TourItem);
            //insert photo 
            var rooms = new List<TourItem>();
            long selectedReoomId = 0;
            foreach (var room in input.Rooms)
            {
                var itemRoom = new TourItem
                {
                    Name = room.RoomName,
                    Price = room.Price,
                    CostPrice = room.CostPrice,
                    ParentId = hotel.Id,
                    Type = TourItemTypeEnum.HotelRoom,
                    LanguageId = input.Language,
                    HostUserId = hostUserId
                };
                itemRoom.Id = _tourItemRepository.InsertAndGetId(itemRoom);
                if (room.Selected)
                    selectedReoomId = itemRoom.Id;
                rooms.Add(itemRoom);
            }
            var language = _langRepository.FirstOrDefault(p => p.Id == input.Language);

            var item = new SearchHotelOutputDto
            {
                Name = hotel.Name,
                Id = hotel.Id,
                Description = input.Description,
                Status = hotel.Status,
                InstantBook = hotel.InstantBook,
                Language = language?.DisplayName,
                LanguageId = input.Language,
                Address = input.Address,

                Location = new BasicLocationDto
                {
                    Id = place.Id,
                    Name = place.PlaceName
                },
                Star = input.Star,
                Rooms = rooms.Select(p => new RoomDto
                {
                    Id = p.Id,
                    Price = new BasicPriceDto
                    {
                        Price = p.Price,
                        CostPrice = p.CostPrice
                    },
                    RoomName = p.Name,
                    IsSelected = p.Id == selectedReoomId
                }).ToList(),
                ThumbImages = input.ThumbImages.Select(p => new PhotoDto
                {
                    Url = p
                }).ToList(),
            };
            foreach (var utilityId in input.UtilitiesId)
            {
                var tourUtility = new TourUtility
                {
                    ItemId = hotel.Id,
                    Type = ItemTypeEnum.TourItem,
                    UtilityId = utilityId
                };
                _tourUtilityRepository.Insert(tourUtility);
            }
            item.Utilities = await GetUtilityByIds(input.UtilitiesId);
            return item;


        }

        public async Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input, bool isFromHost)
        {
            if (!Validate.ValidateStartTime(input.StartTime))
                throw new UserFriendlyException("Start time is invalid");
            long hostUserId = GetHostUserId(AbpSession.UserId ?? 0);
            var transport = new TourItem
            {
                Name = input.Name,
                Type = TourItemTypeEnum.Transport,
                Price = input.Price,
                Status = isFromHost ? StatusEnum.Active : StatusEnum.WaitApprove,
                StartTime = input.StartTime,
                Address = input.Address,
                CostPrice = input.CostPrice,
                InstantBook = input.InstantBook,
                Policies = input.Policy,
                Duration = input.Duration,
                LanguageId = input.Language,
                IncludeTourGuide = input.IncludeTourGuide,

                Description = input.Description,
                HostUserId = hostUserId
            };
            transport.Id = _tourItemRepository.InsertAndGetId(transport);


            InsertImages(input.Photos, transport.Id, ImageTypeEnum.TourImage, ItemTypeEnum.TourItem);
            if (input.ThumbImages == null || input.ThumbImages.Count == 0)
                input.ThumbImages = input.Photos;
            InsertImages(input.ThumbImages, transport.Id, ImageTypeEnum.ThumbImage, ItemTypeEnum.TourItem);


            //Detail transport

            _transportDetailRepository.Insert(new TransportDetail
            {
                From = input.From,
                To = input.To,
                ItemId = transport.Id,
                TotalSeat = input.TotalSeat,
                IsTaxi = input.IsTaxi
            });

            //Schedule


            var item = new SearchTransportOutputDto
            {
                Id = transport.Id,
                Name = input.Name,
                From = input.From,
                To = input.To,
                TotalSeat = input.TotalSeat,
                Description = input.Description,

                Address = input.Address,
                IsTaxi = false,
                Price = new BasicPriceDto
                {
                    Price = input.Price,
                    CostPrice = input.CostPrice
                },
                StartTime = input.StartTime,
                Duration = input.Duration,
                ThumbImages = input.ThumbImages.Select(p => new PhotoDto
                {
                    Url = p
                })
                    .ToList()
            };

            foreach (var utilityId in input.UtilitiesId)
            {
                var tourUtility = new TourUtility
                {
                    ItemId = transport.Id,
                    Type = ItemTypeEnum.TourItem,
                    UtilityId = utilityId
                };
                _tourUtilityRepository.Insert(tourUtility);
            }


            item.Utilities = await GetUtilityByIds(input.UtilitiesId);

            return item;
        }

        public async Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input, bool isFromHost)
        {
            if (!Validate.ValidateStartTime(input.StartTime))
                throw new UserFriendlyException("Start time invalid");
            long hostUserId = GetHostUserId(AbpSession.UserId ?? 0);
            var place = GetPlace(input.Address);
            if (place == null)
            {
                place = new DB.Place
                {
                    Address = input.Address,
                    PlaceName = input.Address,
                };
                place.Id = _placeRepository.InsertAndGetId(place);
            }

            var activity = new TourItem
            {
                Name = input.Name,
                Type = TourItemTypeEnum.Activity,
                Price = input.Price,
                Status = isFromHost ? StatusEnum.Active : StatusEnum.WaitApprove,
                PlaceId = place.Id,
                LanguageId = input.Language,
                Address = input.Address,
                IncludeTourGuide = input.IncludeTourGuide,
                Description = input.Description,
                HostUserId = hostUserId,
                CostPrice = input.CostPrice,
                Duration = input.Duration,
                StartTime = input.StartTime,
                Policies = input.Policy,
                InstantBook = input.InstantBook,
            };

            activity.Id = _tourItemRepository.InsertAndGetId(activity);



            InsertImages(input.Photos, activity.Id, ImageTypeEnum.TourImage, ItemTypeEnum.TourItem);
            if (input.ThumbImages == null || input.ThumbImages.Count == 0)
                input.ThumbImages = input.Photos;
            InsertImages(input.ThumbImages, activity.Id, ImageTypeEnum.ThumbImage, ItemTypeEnum.TourItem);

            var language = _langRepository.FirstOrDefault(p => p.Id == input.Language);
            var item = new SearchItemOutputDto
            {
                Id = activity.Id,
                InstantBook = input.InstantBook,
                Name = input.Name,
                Location = new BasicLocationDto
                {
                    Id = place.Id,
                    Name = place.PlaceName,
                },
                Language = language?.DisplayName,
                IncludeTourGuide = input.IncludeTourGuide,
                Description = input.Description,
                Type = TourItemTypeEnum.Activity,
                Price = new BasicPriceDto
                {
                    Price = input.Price,
                    CostPrice = input.CostPrice
                },
                StartTime = input.StartTime,
                Duration = input.Duration,
                ThumbImages = input.ThumbImages.Select(p => new PhotoDto
                {
                    Url = p
                })
                    .ToList()
            };

            foreach (var utilityId in input.UtilitiesId)
            {
                var tourUtility = new TourUtility
                {
                    ItemId = activity.Id,
                    Type = ItemTypeEnum.TourItem,
                    UtilityId = utilityId
                };
                _tourUtilityRepository.Insert(tourUtility);
            }


            item.Utilities = await GetUtilityByIds(input.UtilitiesId);

            return item;
        }

        public async Task<List<BasicItemDto>> GetSearchCategory()
        {
            return new List<BasicItemDto>()
            {
                new BasicItemDto
                {
                    Id = (byte) TourItemTypeEnum.Place,
                    Name = TourItemTypeEnum.Place.ToString("G")
                },
                new BasicItemDto
                {
                    Id = (byte) TourItemTypeEnum.Tour,
                    Name = TourItemTypeEnum.Tour.ToString("G")
                },
                new BasicItemDto
                {
                    Id = (byte) TourItemTypeEnum.Activity,
                    Name = TourItemTypeEnum.Activity.ToString("G")
                },

                new BasicItemDto
                {
                    Id = (byte) TourItemTypeEnum.ShowTicket,
                    Name = TourItemTypeEnum.ShowTicket.ToString("G")
                }
            };
        }

        public async Task<CustomizeTripOutputDto> GetTripForEditOrCustomize(long tourId)
        {
            var trip = _tourRepository.FirstOrDefault(p => p.Id == tourId);
            var user = trip.Type == TourTypeEnum.Tour ?
                _userRepository.FirstOrDefault(p => p.Id == trip.HostUserId) :
                _userRepository.FirstOrDefault(p => p.Id == trip.CreatorUserId);
            var tourSchedule = _tourScheduleRepository.FirstOrDefault(p => p.TourId == tourId);
            var customTrip = new CustomizeTripOutputDto
            {
                StartDate = tourSchedule != null ? tourSchedule.StartDate : (DateTime?)(null),
                Title = trip.Name,
                InstallBook = trip.InstantBook,
                LanguageId = trip.LanguageId,
                Overview = trip.Overview,
                Policy = trip.Policies,
                Description = trip.Description,
                Photos = await GetTourThumbPhoto(tourId),
                TotalSlot = trip.TotalSlot,
                HostUser = new BasicHostUserInfo
                {
                    Avarta = user?.Avatar,
                    Ranking = user?.Ranking,
                    Ratting = user?.Rating ?? 0,
                    FullName = user?.FullName,
                    UserId = user?.Id ?? 0
                }
            };
            var plan = new List<DayOfTripPlanCustomizeDto>();
            var tourDetails = _tourDetailRepository.GetAll().Where(p => p.TourId == tourId).ToList();
            var tourItemDetailSchedules = _tourDetailItemRepository.GetAll().Where(p => p.TourId == tourId);
            var tourItemIds = tourItemDetailSchedules.Select(p => p.ItemId).ToList().Distinct().ToList();
            var tourItems = _tourItemRepository.GetAll().Where(p => tourItemIds.Contains(p.Id));
            var languagesIds = tourItems.Select(p => p.LanguageId).Distinct().ToList();
            var placeIds = tourItems.Select(p => p.PlaceId).Distinct().ToList();
            var places = _placeRepository.GetAll().Where(p => placeIds.Contains(p.Id)).ToList();
            var hotelIds = tourItems.Where(p => p.Type == TourItemTypeEnum.Hotel).Select(p => p.Id).ToList();
            var transportIds = tourItems.Where(p => p.Type == TourItemTypeEnum.Transport).Select(p => p.Id).ToList();
            var languages = _langRepository.GetAll().Where(p => languagesIds.Contains(p.Id));
            var allRooms = _tourItemRepository.GetAll().Where(p => hotelIds.Contains(p.ParentId ?? 0)).ToList();
            var transportDetails = _transportDetailRepository.GetAll().Where(p => transportIds.Contains(p.ItemId)).ToList();
            var tourThumImagesDic = await GetTourItemThumbPhotos(tourItemIds);
            var tourReviewDic = await GetTourItemReviewSummarys(tourItemIds);
            var utilites =
                await GetUtilities(tourItemIds, ItemTypeEnum.TourItem);
            foreach (var item in tourDetails)
            {
                var hotels = new List<SearchHotelOutputDto>();
                var transports = new List<SearchTransportOutputDto>();
                var listItem = new List<SearchItemOutputDto>();
                var itemInDays = tourItemDetailSchedules.Where(p => p.TourDetailId == item.Id);
                foreach (var itemInDay in itemInDays)
                {
                    var tourItem = tourItems.FirstOrDefault(p => p.Id == itemInDay.ItemId);

                    if (tourItem == null)
                        continue;
                    var location = places.Where(p => p.Id == tourItem.PlaceId).Select((p =>
                        new BasicLocationDto
                        {
                            Id = p.Id,
                            Name = p.PlaceName,
                            Addess = p.Address,
                            Lat = p.Lat,
                            Long = p.Long
                        })).FirstOrDefault();

                    if (tourItem.Type == TourItemTypeEnum.Hotel)
                    {
                        var rooms = allRooms.Where(p => p.ParentId == tourItem.Id).Select(p =>
                            new RoomDto
                            {
                                Id = p.Id,
                                Price = new BasicPriceDto
                                {
                                    Price = p.Price,
                                    CostPrice = p.CostPrice
                                },
                                RoomName = p.Name,
                                IsSelected = itemInDay.RoomId == p.Id
                            }).ToList();

                        var hotelItem = new SearchHotelOutputDto
                        {
                            TourDetailItemId = itemInDay.Id,
                            Description = itemInDay.Description,
                            Name = tourItem.Name,
                            Location = location,
                            LanguageId = tourItem.LanguageId,
                            Language = languages.FirstOrDefault(p => p.Id == tourItem.LanguageId)?.DisplayName,
                            Star = tourItem.Star,
                            Status = tourItem.Status,
                            Address = tourItem.Address,

                            InstantBook = tourItem.InstantBook,
                            Review = tourReviewDic.ContainsKey(tourItem.Id) ? tourReviewDic[tourItem.Id] : null,
                            ThumbImages = tourThumImagesDic.ContainsKey(tourItem.Id)
                                ? tourThumImagesDic[tourItem.Id]
                                : null,
                            Rooms = rooms,
                            Id = tourItem.Id
                        };
                        if (utilites.ContainsKey(hotelItem.Id))
                        {
                            hotelItem.Utilities = utilites[hotelItem.Id];
                        }

                        hotels.Add(hotelItem);
                    }
                    else if (tourItem.Type == TourItemTypeEnum.Transport)
                    {
                        var transportDetail = transportDetails.FirstOrDefault(p => p.ItemId == tourItem.Id);

                        if (transportDetail == null)
                            continue;
                        var transportItem = new SearchTransportOutputDto
                        {
                            TourDetailItemId = itemInDay.Id,
                            Id = tourItem.Id,
                            Description = itemInDay.Description,
                            LanguageId = tourItem.LanguageId,
                            Language = languages.FirstOrDefault(p => p.Id == tourItem.LanguageId)?.DisplayName,
                            TotalSeat = transportDetail.TotalSeat,
                            Status = tourItem.Status,

                            Name = tourItem.Name,
                            Review = tourReviewDic.ContainsKey(tourItem.Id) ? tourReviewDic[tourItem.Id] : null,
                            ThumbImages = tourThumImagesDic.ContainsKey(tourItem.Id)
                                ? tourThumImagesDic[tourItem.Id]
                                : null,
                            Price = new BasicPriceDto
                            {
                                Price = tourItem.Price,
                                CostPrice = tourItem.CostPrice
                            },
                            StartTime = tourItem.StartTime,
                            Duration = tourItem.Duration,
                            InstantBook = tourItem.InstantBook,
                            From = transportDetail.From,
                            TotalSlot = transportDetail.TotalSeat,
                            To = transportDetail.To,
                        };
                        if (utilites.ContainsKey(transportItem.Id))
                        {
                            transportItem.Utilities = utilites[transportItem.Id];
                        }

                        transports.Add(transportItem);
                    }
                    else
                    {
                        var language = languages.FirstOrDefault(p => p.Id == tourItem.LanguageId);
                        var tourItemDto = new SearchItemOutputDto()
                        {
                            TourDetailItemId = itemInDay.Id,
                            Id = tourItem.Id,
                            Description = itemInDay.Description,
                            Name = tourItem.Name,
                            LanguageId = tourItem.LanguageId,

                            Review = tourReviewDic.ContainsKey(tourItem.Id) ? tourReviewDic[tourItem.Id] : null,
                            ThumbImages = tourThumImagesDic.ContainsKey(tourItem.Id)
                                ? tourThumImagesDic[tourItem.Id]
                                : null,
                            Type = tourItem.Type,
                            //   TypeText = tourItem.Type.ToString("G"),
                            IncludeTourGuide = tourItem.IncludeTourGuide,
                            Location = location,

                            Language = language != null ? language.DisplayName : "",
                            StartTime = tourItem.StartTime,
                            Duration = tourItem.Duration,
                            InstantBook = tourItem.InstantBook,
                            Price = new BasicPriceDto
                            {
                                Price = tourItem.Price,
                                CostPrice = tourItem.CostPrice
                            },
                        };
                        if (utilites.ContainsKey(tourItemDto.Id))
                        {
                            tourItemDto.Utilities = utilites[tourItemDto.Id];
                        }

                        listItem.Add(tourItemDto);
                    }
                }

                var dayOfTripPlanDto = new DayOfTripPlanCustomizeDto
                {
                    Title = item.Title,
                    Order = item.Order,
                    Hotels = hotels,
                    Transport = transports,
                    Tours = listItem
                };
                plan.Add(dayOfTripPlanDto);
            }
            customTrip.Plans = plan;
            return customTrip;
        }
        public async Task<decimal> GetFeeByHostUser(long userId, long price)
        {
            var fee = _feeConfigRepository.FirstOrDefault(p => p.HostUserId == userId);
            if (fee != null)
            {
                var feeAmount = fee.FeePercent * price / 100;

                return (decimal)Math.Round(feeAmount, 2);
            }
            return 0;
        }

        public async Task<List<FeeConfigDto>> GetFeeConfigs(List<long> userIds)
        {
            var fee = _feeConfigRepository.GetAll().Where(p => userIds.Contains(p.HostUserId)).Select(p => new FeeConfigDto
            {
                FeePercent = p.FeePercent,
                HostUserId = p.HostUserId,
                Id = p.Id
            });
            return fee.ToList();

        }

    }
}