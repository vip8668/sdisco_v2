using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;
using System.Linq;
using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.TripPlanManager.Dto;
using Abp.Domain.Uow;
using Abp.UI;
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.Products.Dtos;

namespace TepayLink.Sdisco.TripplanManager
{
    public class TripPlanManagerAppService:SdiscoAppServiceBase, ITripPlanManagerAppService
    {
        private readonly IRepository<ProductReview, long> _tourReviewRepository;
        private readonly IRepository<ProductReviewDetail, long> _tourReviewDetailRepository;

        private readonly IRepository<Product, long> _tourRepository;

        //  private readonly IRepository<DB.Image, long> _imageRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;
        private readonly IRepository<ProductDetail, long> _tourDetailRepository;
        private readonly IRepository<Place, long> _placeRepository;
        
        private readonly IRepository<Category> _tourItemCategoryRepository;
        
        private readonly IRepository<TransPortdetail, long> _transportDetail;
        private readonly IRepository<Utility> _utilityRepository;

        private readonly IRepository<ProductSchedule, long> _tourScheduleRepository;

        private readonly IRepository<ProductUtility, long> _tourUtilityRepository;


        private readonly ICommonAppService _commonAppService;

        private readonly IRepository<ProductDetailCombo, long> _tourDetailItemRepository;

        public TripPlanManagerAppService(IRepository<ProductReview, long> tourReviewRepository, IRepository<ProductReviewDetail, long> tourReviewDetailRepository, IRepository<Product, long> tourRepository, IRepository<User, long> userRepository, IRepository<ApplicationLanguage> langRepository, IRepository<ProductDetail, long> tourDetailRepository, IRepository<Place, long> placeRepository, IRepository<Category> tourItemCategoryRepository, IRepository<TransPortdetail, long> transportDetail, IRepository<Utility> utilityRepository, IRepository<ProductSchedule, long> tourScheduleRepository, IRepository<ProductUtility, long> tourUtilityRepository, ICommonAppService commonAppService, IRepository<ProductDetailCombo, long> tourDetailItemRepository)
        {
            _tourReviewRepository = tourReviewRepository;
            _tourReviewDetailRepository = tourReviewDetailRepository;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _langRepository = langRepository;
            _tourDetailRepository = tourDetailRepository;
            _placeRepository = placeRepository;
            _tourItemCategoryRepository = tourItemCategoryRepository;
            _transportDetail = transportDetail;
            _utilityRepository = utilityRepository;
            _tourScheduleRepository = tourScheduleRepository;
            _tourUtilityRepository = tourUtilityRepository;
            _commonAppService = commonAppService;
            _tourDetailItemRepository = tourDetailItemRepository;
        }

        public async Task<PagedResultDto<BasicTourDto>> GetDraftTrip(PagedInputDto input)
        {
            return await GetTrip(input, ProductStatusEnum.Draft);
        }


        public async Task<PagedResultDto<BasicTourDto>> GetWaitApproveTrip(PagedInputDto input)
        {
            return await GetTrip(input, ProductStatusEnum.WaitApprove);
        }


        /// <summary>
        /// Lấy danh sách public Trip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<BasicTourDto>> GetPublicTrip(PagedInputDto input)
        {
            return await GetTrip(input, ProductStatusEnum.Publish);
        }


        private async Task<PagedResultDto<BasicTourDto>> GetTrip(PagedInputDto input, ProductStatusEnum status)
        {
            var query =
                   (from t in
                           _tourRepository.GetAll()
                    where t.Type == ProductTypeEnum.TripPlan
                       && t.Status == status
                       && t.CreatorUserId == AbpSession.UserId
                    select new BasicTourDto
                    {
                        Id = t.Id,

                        Title = t.Name,
                        SoldCount = t.BookingCount,

                        IsHotDeal = t.IsHotDeal,
                        BestSaller = t.IsBestSeller,
                    });

            int totalItem = query.Count();
            var list = query.OrderByDescending(p => p.Id).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            foreach (var item in list)
            {
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
            }

            return new PagedResultDto<BasicTourDto>
            {
                Items = list,
                TotalCount = totalItem
            };

        }

        /// <summary>
        /// Danh sách Category để search
        /// </summary>
        /// <returns></returns>
        public async Task<List<BasicItemDto>> GetSearchCategory()
        {
            return await _commonAppService.GetSearchCategory();

        }

        /// <summary>
        /// Tìm kiếm Tour
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input)
        {
            return await _commonAppService.SearchItem(input, false);

        }

        /// <summary>
        /// Xóa Trip Plan (truyền vào Id cần xóa vào trường id)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task DeleteTripPlan(BasicItemDto item)
        {
            var existTripPlan = _tourRepository.FirstOrDefault(p => p.Id == item.Id);
            if (existTripPlan == null)
                throw new UserFriendlyException("Tripplan not exist");
            if (existTripPlan.Status == ProductStatusEnum.Publish)
            {
                existTripPlan.Status = ProductStatusEnum.Delete;
                _tourRepository.Update(existTripPlan);
            }
            else
            {
                _tourRepository.Delete(existTripPlan);
                _commonAppService.DeleteImageOfTour(existTripPlan.Id,
                    new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });


                var allDetail = _tourDetailRepository.GetAll().Where(p => p.ProductId == item.Id).ToList();
                foreach (var itemDetail in allDetail)
                {
                    _tourDetailRepository.Delete(itemDetail);
                }
            }
        }

        public async Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input)
        {
            return await _commonAppService.SearchHotel(input, false);

        }

        /// <summary>
        /// Tìm kiếm Transport
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input)
        {
            return await _commonAppService.SearchTransport(input, false);

        }

        /// <summary>
        /// Thêm mới Hotel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input)
        {
            return await _commonAppService.CreateHotel(input, false);

        }

        /// <summary>
        /// Thêm mới Transport
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input)
        {
            return await _commonAppService.CreateTransport(input, false);


        }


        public async Task<List<UtilityDto>> GetUtility()
        {
            return _utilityRepository.GetAll().Select(p => new UtilityDto
            {
                Icon = p.Icon,
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }


        [UnitOfWork]
        public async Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input)
        {
            return await _commonAppService.SaveActivity(input, false);
        }


        /// <summary>
        /// Tạo mới tripplan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task SaveTripPlan(CreateTripPlanInputDto input)
        {
            bool tourIsExist;

           Product tripPlan = null;

            if (input.Id != 0)
            {
                tripPlan = _tourRepository.FirstOrDefault(p =>
                    p.CreatorUserId == AbpSession.UserId && p.Id == input.Id && p.Type == ProductTypeEnum.TripPlan);
                if (tripPlan == null)
                {
                    throw new UserFriendlyException("Trip Plan not exist");
                }
            }

            tourIsExist = tripPlan != null;

            if (tripPlan == null)
                tripPlan = new Product
                {
                    Name = input.Title,
                    Description = input.Description,
                    Status = ProductStatusEnum.Draft,
                    LanguageId = input.LanguageId == 0 ? 1 : input.LanguageId,
                    Type = ProductTypeEnum.TripPlan,
                    //LikeCount = 0,
                    BookingCount = 0,
                    TripLengh = input.Plans.Count,
                    Policies = input.Policy,
                    InstantBook = input.InstallBook,
                    PlaceId = 1,
                    Overview = input.Overview
                };
            else
            {
                tripPlan.Name = input.Title;
                tripPlan.Description = input.Description;
                tripPlan.InstantBook = input.InstallBook;
                tripPlan.LanguageId = input.LanguageId;
                tripPlan.Policies = input.Policy;
                tripPlan.Overview = input.Overview;
                tripPlan.TripLengh = input.Plans.Count;
            }

            if (tripPlan.Id > 0)
            {
                _tourRepository.Update(tripPlan);
            }
            else
            {
                tripPlan.Id = _tourRepository.InsertAndGetId(tripPlan);
            }


            _commonAppService.DeleteImageOfTour(tripPlan.Id, 
                new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
            _commonAppService.InsertImages(input.Photos, tripPlan.Id, ImageType.TourImage);

            _commonAppService.InsertImages(input.Photos, tripPlan.Id, ImageType.ThumbImage);


            var listTouDetailItemIds = new List<long>();
            var listIds = new List<long>();
            foreach (var day in input.Plans)
            {
                listIds.AddRange(day.Hotels.Select(p => p.ItemId));

                listIds.AddRange(day.Transport.Select(p => p.ItemId));
                listIds.AddRange(day.Tours.Select(p => p.ItemId));
                listTouDetailItemIds.AddRange(day.Hotels.Select(p => p.TourDetailItemId));
                listTouDetailItemIds.AddRange(day.Transport.Select(p => p.TourDetailItemId));
                listTouDetailItemIds.AddRange(day.Tours.Select(p => p.TourDetailItemId));
            }

            var listTouDetailItemIdExist = _tourDetailItemRepository.GetAll().Where(p => p.ProductId == tripPlan.Id)
                .Select(
                    p => new
                    {
                        p.Id,
                        TourDetailId=   p.ProductDetailId
                    }).ToList();
            var deleteTourDetailIds = listTouDetailItemIdExist
                .Where(p => !listTouDetailItemIds.Contains(p.Id)).Select(p => p.Id).ToList();

            var listTourDetailIds = listTouDetailItemIdExist.Select(p => p.TourDetailId).Distinct().ToList();

            var listTourItemDetails = _tourDetailItemRepository.GetAll()
                .Where(p => listTouDetailItemIdExist.Select(x => x.Id).Contains(p.Id));

            var listTourDetails = _tourDetailRepository.GetAll().Where(p => listTourDetailIds.Contains(p.Id));

            //Nếu là draft hoặc tạo mới và publish luôn --> delete lịch trình draft
            if (tripPlan.Status == ProductStatusEnum.Draft || !tourIsExist)
            {
                _tourDetailRepository.Delete(p => p.ProductId == tripPlan.Id && !listTourDetailIds.Contains(p.Id));
                _tourDetailItemRepository.Delete(p => deleteTourDetailIds.Contains(p.Id));
                int i = 0;

                foreach (var day in input.Plans)
                {
                    i++;

                    var listTourDetailIdsInday =
                        day.Tours.Where(p => p.TourDetailItemId > 0).Select(p => p.TourDetailItemId).ToList();
                    listTourDetailIdsInday.AddRange(day.Hotels.Where(p => p.TourDetailItemId > 0)
                        .Select(p => p.TourDetailItemId).ToList());
                    listTourDetailIdsInday.AddRange(day.Transport.Where(p => p.TourDetailItemId > 0)
                        .Select(p => p.TourDetailItemId).ToList());
                    ProductDetail tourDetail = null;

                    if (listTourDetailIdsInday.Any())
                    {
                        var item = listTouDetailItemIdExist.FirstOrDefault(p =>
                            p.Id == listTourDetailIdsInday[0]);
                        if (item != null)
                        {
                            tourDetail = listTourDetails.FirstOrDefault(p => p.Id == item.TourDetailId);
                        }
                    }

                    if (tourDetail != null)
                    {
                        tourDetail.Order = i;
                        tourDetail.Title = day.Title;
                        _tourDetailRepository.Update(tourDetail);
                    }
                    else
                    {
                        tourDetail = new ProductDetail
                        {
                            Title = day.Title,
                            Order = i,
                            ProductId = tripPlan.Id,
                        };

                        tourDetail.Id = _tourDetailRepository.InsertAndGetId(tourDetail);
                    }


                    foreach (var item in day.Tours)
                    {
                        var tourDetailItem = listTourItemDetails.FirstOrDefault(p => p.Id == item.TourDetailItemId);
                        if (tourDetailItem != null)
                        {
                            tourDetailItem.Description = item.Description;
                            _tourDetailItemRepository.Update(tourDetailItem);
                        }
                        else
                        {
                            tourDetailItem = new ProductDetailCombo
                            {
                                ItemId = item.ItemId,
                                ProductDetailId = tourDetail.Id,
                                ProductId = tripPlan.Id,
                                Description = item.Description
                            };
                            _tourDetailItemRepository.Insert(tourDetailItem);
                        }
                    }

                    foreach (var hotel in day.Hotels)
                    {
                        var tourDetailItem =
                            listTourItemDetails.FirstOrDefault(p => p.Id == hotel.TourDetailItemId);
                        if (tourDetailItem != null)
                        {
                            tourDetailItem.Description = hotel.Description;
                            tourDetailItem.RoomId = hotel.RoomId;

                            _tourDetailItemRepository.Update(tourDetailItem);
                        }
                        else
                        {
                            tourDetailItem = new ProductDetailCombo
                            {
                                ItemId = hotel.ItemId,
                                ProductDetailId = tourDetail.Id,
                                ProductId = tripPlan.Id,
                                RoomId = hotel.RoomId,
                                Description = hotel.Description
                            };
                            _tourDetailItemRepository.Insert(tourDetailItem);
                        }
                    }

                    foreach (var transport in day.Transport)
                    {
                        var tourDetailItem =
                            listTourItemDetails.FirstOrDefault(p => p.Id == transport.TourDetailItemId);
                        if (tourDetailItem != null)
                        {
                            tourDetailItem.Description = transport.Description;
                            _tourDetailItemRepository.Update(tourDetailItem);
                        }
                        else
                        {
                            tourDetailItem = new ProductDetailCombo
                            {
                                ItemId = transport.ItemId,
                                ProductDetailId = tourDetail.Id,
                                ProductId = tripPlan.Id,
                                Description = transport.Description
                            };
                            _tourDetailItemRepository.Insert(tourDetailItem);
                        }
                    }
                }
            }


            //Nếu publish Tour
            if (input.SaveDraft == false && input.StartDate != null)
            {
                bool isWatting = _tourRepository.GetAll().Where(p => listIds.Contains(p.Id) && p.Status == ProductStatusEnum.WaitApprove).Any();
                tripPlan.Status = isWatting ? ProductStatusEnum.WaitApprove : ProductStatusEnum.Publish;
                _tourRepository.Update(tripPlan);
            }
        }


        /// <summary>
        /// Tạo mới tripplan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task SaveCustomizeTrip(CreateTripPlanInputDto input)
        {
            var trip = _tourRepository.GetAll()
                .FirstOrDefault(p => p.Id == input.Id && p.Type == ProductTypeEnum.TripPlan);
            if (trip == null)
            {
                throw new UserFriendlyException("Tripplan not exist");
            }

            if (trip.CreatorUserId == AbpSession.UserId)
            {
                input.Id = 0;
                await SaveTripPlan(input);
                return;
            }

            var tripPlan = new Product
            {
                Name = input.Title,
                Description = input.Description,
                Status = ProductStatusEnum.Draft,
                LanguageId = input.LanguageId == 0 ? 1 : input.LanguageId,
                Type = ProductTypeEnum.TripPlan,
                LikeCount = 0,
                BookingCount = 0,
                TripLengh = input.Plans.Count,
                Policies = input.Policy,
                InstantBook = input.InstallBook,
                PlaceId = 1,
                Overview = input.Overview,
                TripCoppyId = input.Id
            };
            tripPlan.Id = _tourRepository.InsertAndGetId(tripPlan);


            _commonAppService.InsertImages(input.Photos, tripPlan.Id, ImageType.TourImage);

            _commonAppService.InsertImages(input.Photos, tripPlan.Id, ImageType.ThumbImage);


            int i = 0;
            foreach (var day in input.Plans)
            {
                i++;
                var tourDetail = new ProductDetail
                {
                    Title = day.Title,
                    Order = i,
                    ProductId = tripPlan.Id,
                };
                tourDetail.Id = _tourDetailRepository.InsertAndGetId(tourDetail);

                var itemIds = day.Tours.Select(p => p.TourDetailItemId).ToList();
                itemIds.AddRange(day.Transport.Select(p => p.TourDetailItemId).ToList());
                itemIds.AddRange(day.Hotels.Select(p => p.TourDetailItemId).ToList());

                itemIds = itemIds.Distinct().ToList();
                var existTourDetailItems = _tourDetailItemRepository.GetAll().Where(p => itemIds.Contains(p.Id));

                foreach (var item in day.Tours)
                {
                    var tourDetailItem = new ProductDetailCombo
                    {
                        ItemId = item.ItemId,
                        ProductDetailId = tourDetail.Id,
                        ProductId = tripPlan.Id,
                        Description = item.Description
                    };
                    var existItem = existTourDetailItems.FirstOrDefault(p => p.ItemId == item.TourDetailItemId);
                    if (existItem != null)
                        tourDetailItem.CreatorUserId = existItem.CreatorUserId;
                    _tourDetailItemRepository.Insert(tourDetailItem);
                }

                foreach (var hotel in day.Hotels)
                {
                    var tourDetailItem = new ProductDetailCombo
                    {
                        ItemId = hotel.ItemId,
                        ProductDetailId = tourDetail.Id,
                        ProductId = tripPlan.Id,
                        RoomId = hotel.RoomId,
                        Description = hotel.Description
                    };
                    var existItem = existTourDetailItems.FirstOrDefault(p => p.ItemId == hotel.TourDetailItemId);
                    if (existItem != null)
                        tourDetailItem.CreatorUserId = existItem.CreatorUserId;
                    _tourDetailItemRepository.Insert(tourDetailItem);
                }

                foreach (var transport in day.Transport)
                {
                    var tourDetailItem = new ProductDetailCombo
                    {
                        ItemId = transport.ItemId,
                        ProductDetailId = tourDetail.Id,
                        ProductId = tripPlan.Id,
                        Description = transport.Description
                    };
                    var existItem = existTourDetailItems.FirstOrDefault(p => p.ItemId == transport.TourDetailItemId);
                    if (existItem != null)
                        tourDetailItem.CreatorUserId = existItem.CreatorUserId;
                    _tourDetailItemRepository.Insert(tourDetailItem);
                }
            }
            //Nếu publish Tour
            if (input.SaveDraft == false && input.StartDate != null)
            {
                tripPlan.Status = ProductStatusEnum.WaitApprove;
                _tourRepository.Update(tripPlan);
            }
            UnitOfWorkManager.Current.SaveChanges();
            _commonAppService.ProcessBonus(input.Id, RevenueTypeEnum.Coppy);
        }

        /// <summary>
        /// Lấy thông tin chuyến đi để chỉnh sửa ( truyền vào Id của Trip)
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public async Task<CustomizeTripOutputDto> GetTripForEdit(long tripId)
        {
            return await GetTripForEditOrCustomize(tripId);
        }

        /// <summary>
        /// Lấy thông tin chuyến đi để Customizer ( truyền vào Id của Trip)
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public async Task<CustomizeTripOutputDto> GetTripForCustomize(long tripId)
        {
            return await GetTripForEditOrCustomize(tripId);
        }

        [UnitOfWork]
        public async Task Save(CreateTripPlanInputDto input)
        {
            bool tourIsExist;

            Product tripPlan = null;

            if (input.Id != 0)
            {
                tripPlan = _tourRepository.FirstOrDefault(p =>
                    p.CreatorUserId == AbpSession.UserId && p.Id == input.Id && p.Type == ProductTypeEnum.TripPlan);
                if (tripPlan == null)
                {
                    throw new UserFriendlyException("Trip Plan not exist");
                }
            }

            tourIsExist = tripPlan != null;

            if (tripPlan == null)
                tripPlan = new Product
                {
                    Name = input.Title,
                    Description = input.Description,
                    Status = ProductStatusEnum.Draft,
                    LanguageId = input.LanguageId == 0 ? 1 : input.LanguageId,
                    Type = ProductTypeEnum.TripPlan,
                    LikeCount = 0,
                    BookingCount = 0,
                    TripLengh = input.Plans.Count,
                    Policies = input.Policy,
                    InstantBook = input.InstallBook,
                    PlaceId = 1,
                    Overview = input.Overview
                };
            else
            {
                tripPlan.Name = input.Title;
                tripPlan.Description = input.Description;
                tripPlan.InstantBook = input.InstallBook;
                tripPlan.LanguageId = input.LanguageId;
                tripPlan.Policies = input.Policy;
                tripPlan.Overview = input.Overview;
                tripPlan.TripLengh = input.Plans.Count;
            }

            if (tripPlan.Id > 0)
            {
                _tourRepository.Update(tripPlan);
            }
            else
            {
                tripPlan.Id = _tourRepository.InsertAndGetId(tripPlan);
            }
            var listTouDetailItemIds = new List<long>();
            var listIds = new List<long>();
            if (!input.AddSchedule)
            {
                _commonAppService.DeleteImageOfTour(tripPlan.Id,
                    new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
                _commonAppService.InsertImages(input.Photos, tripPlan.Id, ImageType.TourImage);

                _commonAppService.InsertImages(input.Photos, tripPlan.Id, ImageType.ThumbImage);



                foreach (var day in input.Plans)
                {
                    listIds.AddRange(day.Hotels.Select(p => p.ItemId));

                    listIds.AddRange(day.Transport.Select(p => p.ItemId));
                    listIds.AddRange(day.Tours.Select(p => p.ItemId));

                    listTouDetailItemIds.AddRange(day.Hotels.Select(p => p.TourDetailItemId));
                    listTouDetailItemIds.AddRange(day.Transport.Select(p => p.TourDetailItemId));
                    listTouDetailItemIds.AddRange(day.Tours.Select(p => p.TourDetailItemId));
                }

                var listTouDetailItemIdExist = _tourDetailItemRepository.GetAll().Where(p => p.ProductId == tripPlan.Id)
                    .Select(
                        p => new
                        {
                            p.Id,
                            TourDetailId=     p.ProductDetailId
                        }).ToList();
                var deleteTourDetailIds = listTouDetailItemIdExist
                    .Where(p => !listTouDetailItemIds.Contains(p.Id)).Select(p => p.Id).ToList();

                var listTourDetailIds = listTouDetailItemIdExist.Select(p => p.TourDetailId).Distinct().ToList();

                var listTourItemDetails = _tourDetailItemRepository.GetAll()
                    .Where(p => listTouDetailItemIdExist.Select(x => x.Id).Contains(p.Id));

                var listTourDetails = _tourDetailRepository.GetAll().Where(p => listTourDetailIds.Contains(p.Id));

                //Nếu là draft hoặc tạo mới và publish luôn --> delete lịch trình draft

                _tourDetailRepository.Delete(p => p.ProductId == tripPlan.Id && !listTourDetailIds.Contains(p.Id));
                _tourDetailItemRepository.Delete(p => deleteTourDetailIds.Contains(p.Id));
                int i = 0;

                foreach (var day in input.Plans)
                {
                    i++;

                    var listTourDetailIdsInday =
                        day.Tours.Where(p => p.TourDetailItemId > 0).Select(p => p.TourDetailItemId).ToList();
                    listTourDetailIdsInday.AddRange(day.Hotels.Where(p => p.TourDetailItemId > 0)
                        .Select(p => p.TourDetailItemId).ToList());
                    listTourDetailIdsInday.AddRange(day.Transport.Where(p => p.TourDetailItemId > 0)
                        .Select(p => p.TourDetailItemId).ToList());
                    ProductDetail tourDetail = null;

                    if (listTourDetailIdsInday.Any())
                    {
                        var item = listTouDetailItemIdExist.FirstOrDefault(p =>
                            p.Id == listTourDetailIdsInday[0]);
                        if (item != null)
                        {
                            tourDetail = listTourDetails.FirstOrDefault(p => p.Id == item.TourDetailId);
                        }
                    }

                    if (tourDetail != null)
                    {
                        tourDetail.Order = i;
                        tourDetail.Title = day.Title;
                        _tourDetailRepository.Update(tourDetail);
                    }
                    else
                    {
                        tourDetail = new ProductDetail
                        {
                            Title = day.Title,
                            Order = i,
                            ProductId = tripPlan.Id,
                        };

                        tourDetail.Id = _tourDetailRepository.InsertAndGetId(tourDetail);
                    }


                    foreach (var item in day.Tours)
                    {
                        var tourDetailItem = listTourItemDetails.FirstOrDefault(p => p.Id == item.TourDetailItemId);
                        if (tourDetailItem != null)
                        {
                            tourDetailItem.Description = item.Description;
                            _tourDetailItemRepository.Update(tourDetailItem);
                        }
                        else
                        {
                            tourDetailItem = new ProductDetailCombo
                            {
                                ItemId = item.ItemId,
                                ProductDetailId = tourDetail.Id,
                                ProductId = tripPlan.Id,
                                Description = item.Description
                            };
                            _tourDetailItemRepository.Insert(tourDetailItem);
                        }
                    }

                    foreach (var hotel in day.Hotels)
                    {
                        var tourDetailItem =
                            listTourItemDetails.FirstOrDefault(p => p.Id == hotel.TourDetailItemId);
                        if (tourDetailItem != null)
                        {
                            tourDetailItem.Description = hotel.Description;
                            tourDetailItem.RoomId = hotel.RoomId;

                            _tourDetailItemRepository.Update(tourDetailItem);
                        }
                        else
                        {
                            tourDetailItem = new ProductDetailCombo
                            {
                                ItemId = hotel.ItemId,
                                ProductDetailId = tourDetail.Id,
                                ProductId = tripPlan.Id,
                                RoomId = hotel.RoomId,
                                Description = hotel.Description
                            };
                            _tourDetailItemRepository.Insert(tourDetailItem);
                        }
                    }

                    foreach (var transport in day.Transport)
                    {
                        var tourDetailItem =
                            listTourItemDetails.FirstOrDefault(p => p.Id == transport.TourDetailItemId);
                        if (tourDetailItem != null)
                        {
                            tourDetailItem.Description = transport.Description;
                            _tourDetailItemRepository.Update(tourDetailItem);
                        }
                        else
                        {
                            tourDetailItem = new ProductDetailCombo
                            {
                                ItemId = transport.ItemId,
                                ProductDetailId = tourDetail.Id,
                                ProductId = tripPlan.Id,
                                Description = transport.Description
                            };
                            _tourDetailItemRepository.Insert(tourDetailItem);
                        }
                    }
                }
            }


            //Nếu publish Tour
            if (input.SaveDraft == false)
            {
                bool isWatting = _tourRepository.GetAll().Where(p => listIds.Contains(p.Id) && p.Status == ProductStatusEnum.WaitApprove).Any();
                tripPlan.Status = isWatting ? ProductStatusEnum.WaitApprove : ProductStatusEnum.Publish;

                _tourRepository.Update(tripPlan);
            }
        }


        /// <summary>
        /// Lấy thông tin trip để chỉnh sửa
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        private async Task<CustomizeTripOutputDto> GetTripForEditOrCustomize(long tourId)
        {
            return await _commonAppService.GetTripForEditOrCustomize(tourId);
        }

        /// <summary>
        /// UPdate Hotel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task UpdateHotel(CreateHotelDto input)
        {
            return;

        }

        /// <summary>
        /// update transport
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task UpdateTransport(CreateTransportDto input)
        {
            return;
        }

        /// <summary>
        /// Check Tour Avaiable
        /// 
        /// httpcode 200: Oke
        /// httpcode 500: Error ( show trường error.message ra)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CheckTourAvaiable(CheckTourItemAvaiableDto input)
        {
            //todo chỗ này check sau
        }
    }
}
