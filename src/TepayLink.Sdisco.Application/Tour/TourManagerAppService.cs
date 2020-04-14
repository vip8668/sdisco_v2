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
using System.Linq;
using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.TripPlanManager.Dto;
using Abp.Domain.Uow;
using Abp.UI;
using TepayLink.Sdisco.Products.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public class TourManagerAppService : SdiscoAppServiceBase, ITourManagerAppService
    {
        private readonly IRepository<ProductReview, long> _tourReviewRepository;
        private readonly IRepository<ProductReviewDetail, long> _tourReviewDetailRepository;
        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;

        private readonly IRepository<ProductDetail, long> _tourDetailRepository;

        private readonly IRepository<Place, long> _placeRepository;
        //private readonly IRepository<TourDetailSchedule, long> _tourDetailScheduleRepository;
        private readonly IRepository<Category> _tourItemCategoryRepository;
        private readonly IRepository<ProductSchedule, long> _tourScheduleRepository;
        private readonly IRepository<TransPortdetail, long> _transportDetail;
        private readonly ICommonAppService _commonAppService;
        private readonly IRepository<Utility> _utilityRepository;
        private readonly IRepository<ProductDetailCombo, long> _tourDetailItemRepository;
        private readonly IRepository<ProductUtility, long> _tourUtilityRepository;

        public TourManagerAppService(IRepository<ProductReview, long> tourReviewRepository, IRepository<ProductReviewDetail, long> tourReviewDetailRepository, IRepository<Product, long> tourRepository, IRepository<User, long> userRepository, IRepository<ApplicationLanguage> langRepository, IRepository<ProductDetail, long> tourDetailRepository, IRepository<Place, long> placeRepository, IRepository<Category> tourItemCategoryRepository, IRepository<ProductSchedule, long> tourScheduleRepository, IRepository<TransPortdetail, long> transportDetail, ICommonAppService commonAppService, IRepository<Utility> utilityRepository, IRepository<ProductDetailCombo, long> tourDetailItemRepository, IRepository<ProductUtility, long> tourUtilityRepository)
        {
            _tourReviewRepository = tourReviewRepository;
            _tourReviewDetailRepository = tourReviewDetailRepository;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _langRepository = langRepository;
            _tourDetailRepository = tourDetailRepository;
            _placeRepository = placeRepository;
            _tourItemCategoryRepository = tourItemCategoryRepository;
            _tourScheduleRepository = tourScheduleRepository;
            _transportDetail = transportDetail;
            _commonAppService = commonAppService;
            _utilityRepository = utilityRepository;
            _tourDetailItemRepository = tourDetailItemRepository;
            _tourUtilityRepository = tourUtilityRepository;
        }

        public async Task<PagedResultDto<BasicTourDto>> GetDraftTour(PagedInputDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var query =
                (from t in
                        _tourRepository.GetAll()
                 where t.Type == ProductTypeEnum.Tour // && r.ReviewType == ReviewTypeEnum.Tour
                       && t.Status == ProductStatusEnum.Draft
                       && t.HostUserId == hostUserId
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

        public async Task<PagedResultDto<BasicTourDto>> GetPublicTour(PagedInputDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var query =
                (from t in
                        _tourRepository.GetAll()
                 where t.Type == ProductTypeEnum.Tour // && r.ReviewType == ReviewTypeEnum.Tour
                       && t.Status == ProductStatusEnum.Publish
                       && t.HostUserId == hostUserId
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
                item.Review = await _commonAppService.GetTourReviewSummary(item.Id);

                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
            }

            return new PagedResultDto<BasicTourDto>
            {
                Items = list,
                TotalCount = totalItem
            };
        }

        /// <summary>
        /// Danh mục category để search
        /// </summary>
        /// <returns></returns>
        public async Task<List<BasicItemDto>> GetSearchCategory()
        {
            return new List<BasicItemDto>()
            {
                //new BasicItemDto
                //{
                //    Id = (byte) TourItemTypeEnum.Place,
                //    Name = TourItemTypeEnum.Place.ToString("G")
                //},
                //new BasicItemDto
                //{
                //    Id = (byte) TourItemTypeEnum.Tour,
                //    Name = TourItemTypeEnum.Tour.ToString("G")
                //},
                new BasicItemDto
                {
                    Id = (byte) ProductTypeEnum.Activity,
                    Name = ProductTypeEnum.Activity.ToString("G")
                },

                new BasicItemDto
                {
                    Id = (byte) ProductTypeEnum.TicketShow,
                    Name = ProductTypeEnum.TicketShow.ToString("G")
                }
            };
        }

        /// <summary>
        /// Tìm kiếm Tour
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SearchItemOutputDto>> SearchItem(SearhTourItemInputDto input)
        {
            return await _commonAppService.SearchItem(input, true);
        }

        [UnitOfWork]
        public async Task DeleteTour(long tourId)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var existTripPlan =
                _tourRepository.FirstOrDefault(p => p.Id == tourId && p.HostUserId == hostUserId);
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
                _commonAppService.DeleteImageOfTour(existTripPlan.Id, new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
                var allDetail = _tourDetailRepository.GetAll().Where(p => p.ProductId == tourId).ToList();
                foreach (var itemDetail in allDetail)
                {


                    _tourDetailRepository.Delete(itemDetail);
                }
            }
        }

        /// <summary>
        /// Tìm kiếm Hotel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SearchHotelOutputDto>> SearchHotel(SearhTourItemInputDto input)
        {
            return await _commonAppService.SearchHotel(input, true);
        }

        /// <summary>
        /// Tìm kiếm Transport
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SearchTransportOutputDto>> SearchTransport(SearchTransportInputDto input)
        {
            return await _commonAppService.SearchTransport(input, true);
        }


        private Place GetPlace(string address)
        {
            var place = _placeRepository.FirstOrDefault(p => p.DisplayAddress == address);

            if (place == null)
            {
                place = new Place
                {
                    DisplayAddress = address,
                    Name = address,
                };
                place.Id = _placeRepository.InsertAndGetId(place);
            }

            return place;
        }

        /// <summary>
        /// Thêm mới Hotel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<SearchHotelOutputDto> CreateHotel(CreateHotelDto input)
        {
            return await _commonAppService.CreateHotel(input, true);
        }
        /// <summary>
        /// Thêm mới Transport
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<SearchTransportOutputDto> CreateTransport(CreateTransportDto input)
        {
            return await _commonAppService.CreateTransport(input, true);
        }
        [UnitOfWork]
        public async Task<SearchItemOutputDto> SaveActivity(CreateActivityDto input)
        {
            return await _commonAppService.SaveActivity(input, true);
        }
        /// <summary>
        /// Lưu Chuyến đi
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Save(CreateTripPlanInputDto input)
        {
            if (input.SaveDraft == false)
            {
                if (input.StartDate == null)
                {
                    throw new UserFriendlyException("Start Date is null");
                }
            }

            bool tourIsExist;
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);


            Product existTour = null;

            if (input.Id != 0)
            {
                existTour = _tourRepository.FirstOrDefault(p =>
                    p.HostUserId == hostUserId && p.Id == input.Id && p.Type == ProductTypeEnum.Tour);
                if (existTour == null)
                {
                    throw new UserFriendlyException("Tour not exist");
                }
            }

            tourIsExist = existTour != null;

            if (existTour == null)
                existTour = new Product
                {
                    Name = input.Title,
                    Description = input.Description,
                    Status = ProductStatusEnum.Draft,
                    LanguageId = input.LanguageId == 0 ? 1 : input.LanguageId,
                    Type = ProductTypeEnum.Tour,

                    TotalSlot = input.TotalSlot,

                    BookingCount = 0,
                    TripLengh = input.Plans.Count,
                    HostUserId = hostUserId,
                    Policies = input.Policy,
                    InstantBook = input.InstallBook,
                    PlaceId = 1,
                    Overview = input.Overview,

                };
            else
            {
                existTour.Name = input.Title;
                existTour.Description = input.Description;
                existTour.InstantBook = input.InstallBook;
                existTour.LanguageId = input.LanguageId;
                existTour.Policies = input.Policy;
                existTour.Overview = input.Overview;
                existTour.TotalSlot = input.TotalSlot;
                existTour.TripLengh = input.Plans.Count;
            }

            if (existTour.Id > 0)
            {
                _tourRepository.Update(existTour);
            }
            else
            {
                existTour.Id = _tourRepository.InsertAndGetId(existTour);
            }
            if (!input.AddSchedule)
            {
                _commonAppService.DeleteImageOfTour(existTour.Id, new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
                _commonAppService.InsertImages(input.Photos, existTour.Id, ImageType.TourImage);
                _commonAppService.InsertImages(input.Photos, existTour.Id, ImageType.ThumbImage);
                var listTouDetailItemIds = new List<long>();
                foreach (var day in input.Plans)
                {
                    listTouDetailItemIds.AddRange(day.Hotels.Select(p => p.TourDetailItemId));
                    listTouDetailItemIds.AddRange(day.Transport.Select(p => p.TourDetailItemId));
                    listTouDetailItemIds.AddRange(day.Tours.Select(p => p.TourDetailItemId));
                }
                var listTouDetailItemIdExist = _tourDetailItemRepository.GetAll().Where(p => p.ProductId == existTour.Id)
                    .Select(
                        p => new
                        {
                            p.Id,
                            TourDetailId = p.ProductDetailId
                        }).ToList();
                var deleteTourDetailIds = listTouDetailItemIdExist
                    .Where(p => !listTouDetailItemIds.Contains(p.Id)).Select(p => p.Id).ToList();
                var listTourDetailIds = listTouDetailItemIdExist.Select(p => p.TourDetailId).Distinct().ToList();
                var listTourItemDetails = _tourDetailItemRepository.GetAll()
                    .Where(p => listTouDetailItemIdExist.Select(x => x.Id).Contains(p.Id));
                var listTourDetails = _tourDetailRepository.GetAll().Where(p => listTourDetailIds.Contains(p.Id));
                //Nếu là draft hoặc tạo mới và publish luôn --> delete lịch trình draft
                if (existTour.Status == ProductStatusEnum.Draft || !tourIsExist)
                {
                    _tourDetailRepository.Delete(p => p.ProductId == existTour.Id && !listTourDetailIds.Contains(p.Id));
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
                                ProductId = existTour.Id,
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
                                    ProductId = existTour.Id,
                                    Description = item.Description
                                };
                                _tourDetailItemRepository.Insert(tourDetailItem);
                            }
                        }

                        foreach (var hotel in day.Hotels)
                        {
                            var tourDetailItem = listTourItemDetails.FirstOrDefault(p => p.Id == hotel.TourDetailItemId);
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
                                    ProductId = existTour.Id,
                                    RoomId = hotel.RoomId,
                                    Description = hotel.Description
                                };
                                _tourDetailItemRepository.Insert(tourDetailItem);
                            }

                        }

                        foreach (var transport in day.Transport)
                        {
                            var tourDetailItem = listTourItemDetails.FirstOrDefault(p => p.Id == transport.TourDetailItemId);
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
                                    ProductId = existTour.Id,
                                    Description = transport.Description
                                };
                                _tourDetailItemRepository.Insert(tourDetailItem);
                            }
                        }
                    }
                }
            }
            //Nếu publish Tour
            if (input.SaveDraft == false && input.StartDate != null)
            {
                // Tính giá
                var totalPrice = input.Plans.Sum(p =>
                    p.Tours.Sum(q => q.Price) + p.Hotels.Sum(q => q.Price) + p.Transport.Sum(q => q.Price));
                var costPrice = input.Plans.Sum(p =>
                    p.Tours.Sum(q => q.CostPrice) + p.Hotels.Sum(q => q.CostPrice) + p.Transport.Sum(q => q.CostPrice));

                var tourSchedule = new ProductSchedule
                {
                    TotalBook = 0,
                    TotalSlot = input.TotalSlot,
                    ProductId = existTour.Id,
                    Price = totalPrice,
                    CostPrice = costPrice,
                    StartDate = input.StartDate.Value,
                    EndDate = input.StartDate.Value.AddDays(input.Plans.Count).AddSeconds(-1),
                    TripLength = input.Plans.Count,
                };
                tourSchedule.Id = _tourScheduleRepository.InsertAndGetId(tourSchedule);
                //insert ItemSchedule

                var listTourDetail = _tourDetailRepository.GetAll().Where(p => p.ProductId == existTour.Id)
                    .OrderBy(p => p.Order).ToList();

                //int i = 0;
                //foreach (var plan in input.Plans)
                //{
                //    foreach (var item in plan.Tours)
                //    {
                //        var itemSchedule = new ProductSchedule
                //        {
                //            Price = item.Price,
                //            CostPrice = item.CostPrice,
                //            TotalSlot = input.TotalSlot,
                //            AllowBook = false,
                //            StartDate = input.StartDate.Value.AddDays(i),
                //            EndDate = input.StartDate.Value.AddDays(i + 1).AddSeconds(-1),
                //            TripLength = 1,
                //            ProductId = item.ItemId,
                //            TotalBook = 0,
                //            //   Avaiable = input.TotalSlot,
                //            TourScheduleId = tourSchedule.Id,
                //            TourDetailId = listTourDetail[i].Id
                //        };
                //        _itemScheduleRepository.Insert(itemSchedule);
                //    }

                //    foreach (var item in plan.Hotels)
                //    {
                //        var itemSchedule = new ItemSchedule
                //        {
                //            Price = item.Price,
                //            CostPrice = item.CostPrice,
                //            TotalSlot = input.TotalSlot,
                //            AllowBook = false,
                //            FromTime = input.StartDate.Value.AddDays(i),
                //            ToTime = input.StartDate.Value.AddDays(i + 1).AddSeconds(-1),
                //            TripLength = 1,
                //            ItemId = item.ItemId,
                //            RoomId = item.RoomId,
                //            TotalBook = 0,
                //            //  Avaiable = input.TotalSlot,
                //            TourScheduleId = tourSchedule.Id,
                //            TourDetailId = listTourDetail[i].Id
                //        };
                //        _itemScheduleRepository.Insert(itemSchedule);
                //    }

                //    foreach (var item in plan.Transport)
                //    {
                //        var itemSchedule = new ItemSchedule
                //        {
                //            Price = item.Price,
                //            CostPrice = item.CostPrice,
                //            TotalSlot = input.TotalSlot,
                //            AllowBook = false,
                //            FromTime = input.StartDate.Value.AddDays(i),
                //            ToTime = input.StartDate.Value.AddDays(i + 1).AddSeconds(-1),
                //            TripLength = 1,
                //            ItemId = item.ItemId,
                //            TotalBook = 0,
                //            // Avaiable = input.TotalSlot,
                //            TourScheduleId = tourSchedule.Id,
                //            TourDetailId = listTourDetail[i].Id
                //        };
                //        _itemScheduleRepository.Insert(itemSchedule);
                //    }

                //    i++;
                //}

                existTour.Status = ProductStatusEnum.Publish;
                _tourRepository.Update(existTour);
            }
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
        /// <summary>
        /// UPdate Hotel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task UpdateHotel(CreateHotelDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var hotel = _tourRepository.FirstOrDefault(p => p.Id == input.HotelId && p.HostUserId == hostUserId);
            if (hotel == null)
                throw new UserFriendlyException("Hotel not exist");

            Place place = GetPlace(input.Address);
            hotel.PlaceId = place.Id;
            hotel.Name = input.Name;
            hotel.InstantBook = input.InstantBook;
            //hotel.Address = input.Address;
            hotel.Star = input.Star;
            hotel.LanguageId = input.Language;
            hotel.IncludeTourGuide = input.IncludeTourGuide;
            var listRoom = _tourRepository.GetAll()
                .Where(p => p.ParentId == input.HotelId && p.Type == ProductTypeEnum.HotelRoom);
            //list delete room
            //todo chỗ này nên dùng linQ left join
            var listDeleteRoomIds = listRoom.Where(p => input.Rooms.FirstOrDefault(q => q.RoomId == p.Id) == null)
                .Select(p => p.Id);
            _tourRepository.Delete(p => listDeleteRoomIds.Contains(p.Id));
            foreach (var room in input.Rooms)
            {
                if (room.RoomId == 0)
                {
                    var itemRoom = new Product
                    {
                        Name = room.RoomName,
                        Price = room.Price,
                        CostPrice = room.CostPrice,
                        ParentId = hotel.Id,
                        Type = ProductTypeEnum.HotelRoom,
                        LanguageId = 1,
                    };
                    itemRoom.Id = _tourRepository.InsertAndGetId(itemRoom);
                }
                else
                {
                    var item = listRoom.FirstOrDefault(p => p.Id == room.RoomId);
                    if (item != null)
                    {
                        item.Name = room.RoomName;
                        item.Price = room.Price;
                        item.CostPrice = room.CostPrice;
                        _tourRepository.Update(item);
                    }
                }
            }

            //todo chỗ này dùng leftjoin để xóa
            _tourUtilityRepository.Delete(p => p.ProductId == hotel.Id);
            foreach (var utilityId in input.UtilitiesId)
            {
                var tourUtility = new ProductUtility
                {
                    ProductId = hotel.Id,

                    UtilityId = utilityId
                };
                _tourUtilityRepository.Insert(tourUtility);
            }

            _commonAppService.DeleteImageOfTour(input.HotelId, new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
            _commonAppService.InsertImages(input.Photos, hotel.Id, ImageType.TourImage);
            if (input.ThumbImages == null || input.ThumbImages.Count == 0)
                input.ThumbImages = input.Photos;
            _commonAppService.InsertImages(input.ThumbImages, hotel.Id, ImageType.ThumbImage);
        }
        /// <summary>
        /// update transport
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task UpdateTransport(CreateTransportDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var transport =
                _tourRepository.FirstOrDefault(p => p.Id == input.TransportId && p.HostUserId == hostUserId);
            if (transport == null)
                throw new UserFriendlyException("Transport not exist");
            transport.Name = input.Name;
            transport.Price = input.Price;
            transport.CostPrice = input.CostPrice;
            transport.Description = input.Description;
            transport.InstantBook = input.InstantBook;
            var transportDetail = _transportDetail.FirstOrDefault(p => p.ProductId == input.TransportId);
            if (transportDetail == null)
                throw new UserFriendlyException("Transport not exist");
            _tourRepository.Update(transport);
            _transportDetail.Update(transportDetail);
            //todo chỗ này dùng leftjoin để xóa
            _tourUtilityRepository.Delete(p =>  p.ProductId == transport.Id);
            foreach (var utilityId in input.UtilitiesId)
            {
                var tourUtility = new ProductUtility
                {
                    ProductId = transport.Id,
                   
                    UtilityId = utilityId
                };
                _tourUtilityRepository.Insert(tourUtility);
            }
            _commonAppService.DeleteImageOfTour(input.TransportId,  new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
            _commonAppService.InsertImages(input.Photos, transport.Id, ImageType.TourImage);
            if (input.ThumbImages == null || input.ThumbImages.Count == 0)
                input.ThumbImages = input.Photos;
            _commonAppService.InsertImages(input.ThumbImages, transport.Id, ImageType.ThumbImage);
        }
        public async Task UpdateActivity(CreateActivityDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var activity =
                _tourRepository.FirstOrDefault(p => p.Id == input.ActivityId && p.HostUserId == hostUserId);
            if (activity == null)
                throw new UserFriendlyException("Activity not exist");
            var place = GetPlace(input.Address);


            activity.Name = input.Name;
            activity.Description = input.Description;
            activity.PlaceId = place.Id;
            activity.Price = input.Price;
            activity.CostPrice = input.CostPrice;
            //activity.Address = input.Address;
            activity.StartTime = input.StartTime;
            activity.Duration = input.Duration;
            activity.InstantBook = input.InstantBook;
            activity.IncludeTourGuide = input.IncludeTourGuide;
            _tourRepository.Update(activity);

            _tourUtilityRepository.Delete(p => p.ProductId == activity.Id);
            foreach (var utilityId in input.UtilitiesId)
            {
                var tourUtility = new ProductUtility
                {
                    ProductId = activity.Id,
                   
                    UtilityId = utilityId
                };
                _tourUtilityRepository.Insert(tourUtility);
            }
            _commonAppService.DeleteImageOfTour(input.ActivityId,  new List<ImageType> { ImageType.ThumbImage, ImageType.TourImage });
            _commonAppService.InsertImages(input.Photos, activity.Id, ImageType.TourImage);
            if (input.ThumbImages == null || input.ThumbImages.Count == 0)
                input.ThumbImages = input.Photos;
            _commonAppService.InsertImages(input.ThumbImages, activity.Id, ImageType.ThumbImage);

        }

        /// <summary>
        /// Lấy thông tin tour để chỉnh sửa ( truyền vào Id của tour)
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public async Task<CustomizeTripOutputDto> GetTourForEdit(long tripId)
        {
            return await GetTripForEditOrCustomize(tripId);
        }
        /// <summary>
        /// Lấy thông tin trip để chỉnh sửa
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        private async Task<CustomizeTripOutputDto> GetTripForEditOrCustomize(long tourId)
        {
            return await _commonAppService.GetTripForEditOrCustomize(tourId);
        }





    }
}
