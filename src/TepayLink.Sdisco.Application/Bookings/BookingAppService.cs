using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SDisco.Booking.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Payment;
using TepayLink.Sdisco.Payment.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Bookings
{

    [AbpAuthorize]
    public class BookingAppService : SdiscoAppServiceBase, IBookingAppService
    {
        private readonly IRepository<ProductSchedule, long> _tourScheduleRepository;



        private readonly IRepository<Product, long> _tourRepository;
        private readonly ICommonAppService _commonAppService;
        private readonly IRepository<Booking, long> _bookingRepository;

        private readonly IRepository<ProductDetail, long> _tourDetailRepository;

        private readonly IRepository<ProductDetailCombo, long> _tourDetailItemRepository;

        private readonly IRepository<ProductImage, long> _imageRepository;




        private readonly IRepository<BookingDetail, long> _bookingDetailRepository;
        private readonly IRepository<Coupon, long> _couponRepository;
        private readonly IPaymentAppService _paymentAppService;
        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<ProductReviewDetail, long> _tourReviewDetailRepository;
        private readonly IRepository<ProductReview, long> _tourReviewRepository;
        private readonly IRepository<ClaimReason, int> _claimRepository;
        private readonly IRepository<BookingClaim, long> _bookingClaimRepository;

        public BookingAppService(IRepository<ProductSchedule, long> tourScheduleRepository, IRepository<Product, long> tourRepository, ICommonAppService commonAppService, IRepository<Booking, long> bookingRepository, IRepository<ProductDetail, long> tourDetailRepository, IRepository<ProductDetailCombo, long> tourDetailItemRepository, IRepository<ProductImage, long> imageRepository, IRepository<BookingDetail, long> bookingDetailRepository, IRepository<Coupon, long> couponRepository, IPaymentAppService paymentAppService, IOrderAppService orderAppService, IRepository<ProductReviewDetail, long> tourReviewDetailRepository, IRepository<ProductReview, long> tourReviewRepository, IRepository<ClaimReason, int> claimRepository, IRepository<BookingClaim, long> bookingClaimRepository)
        {
            _tourScheduleRepository = tourScheduleRepository;
            _tourRepository = tourRepository;
            _commonAppService = commonAppService;
            _bookingRepository = bookingRepository;
            _tourDetailRepository = tourDetailRepository;
            _tourDetailItemRepository = tourDetailItemRepository;
            _imageRepository = imageRepository;
            _bookingDetailRepository = bookingDetailRepository;
            _couponRepository = couponRepository;
            _paymentAppService = paymentAppService;
            _orderAppService = orderAppService;
            _tourReviewDetailRepository = tourReviewDetailRepository;
            _tourReviewRepository = tourReviewRepository;
            _claimRepository = claimRepository;
            _bookingClaimRepository = bookingClaimRepository;
        }


        private async Task<List<AvaiableTimeOfTourDto>> GetAvaiableTimeOfTourBySelectedDate(
            GetAvaiableTimeOfTourInput input)
        {
            var item = _tourRepository.FirstOrDefault(p => p.Id == input.TourId);
            var avaiableList = _tourScheduleRepository.GetAll().Where(p =>
                    p.ProductId == input.TourId && p.StartDate >= input.FromDate && p.StartDate < input.ToDate &&
                    p.StartDate >= DateTime.Today).Select(p =>
                    new AvaiableTimeOfTourDto
                    {
                        TotalBook = p.TotalBook,
                        TourId = p.ProductId,
                        TourScheduleId = p.Id,
                        StartTime = p.StartDate,
                        ToTime = p.EndDate,
                        ItemType = input.Type,
                        TotalSlot = p.TotalSlot,
                        InstallBook = item.InstantBook,
                        Price = new BasicPriceDto
                        {
                            Price = p.Price,
                            // OldPrice = p.OldPrice,
                            Hotel = p.HotelPrice,
                            // ServiceFee = p.ServiceFee,
                            Ticket = p.TicketPrice,
                        }
                    }).ToList();
            // Tính price
            return avaiableList;


        }


        /// <summary>
        /// Lấy thời gian có thể đặt chuyến/ mua vé, Item....
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<AvaiableTourDto> GetAvaiableTimeOfTour(GetAvaiableTimeOfTourDto input)
        {
            var tour = _tourRepository.FirstOrDefault(p => p.Id == input.TourId);
            var user = UserManager.Users.FirstOrDefault(p => p.Id == tour.HostUserId);

            var hostUser = new List<BasicHostUserInfo>();

            var avaiable = new AvaiableTourDto
            {
                Current = await GetAvaiableTimeOfTourBySelectedDate(new GetAvaiableTimeOfTourInput
                {
                    TourId = input.TourId,
                    FromDate = new DateTime(input.SelectedDate.Year, input.SelectedDate.Month, input.SelectedDate.Day),
                    ToDate = new DateTime(input.SelectedDate.Year, input.SelectedDate.Month, input.SelectedDate.Day)
                        .AddDays(1),
                }),
                NextAvaiabled = await GetAvaiableTimeOfTourBySelectedDate(new GetAvaiableTimeOfTourInput
                {
                    TourId = input.TourId,
                    FromDate = new DateTime(input.SelectedDate.Year, input.SelectedDate.Month, input.SelectedDate.Day)
                        .AddDays(1),
                    ToDate = new DateTime(input.SelectedDate.Year, input.SelectedDate.Month, 1).AddMonths(1)
                }),

                HostUsers = hostUser
            };
            return avaiable;
        }


        /// <summary>
        /// Lấy thông tin tour
        /// </summary>
        /// <param name="tourId"></param>
        /// <returns></returns>
        public async Task<TourDetailDto> GetTourDetail(GetTourDetailInputDto input)
        {
            var tour = _tourRepository.GetAll().FirstOrDefault(p => p.Id == input.ItemId);


            var basicIten = new TourDetailDto()
            {
                Title = tour?.Name,
            };
            basicIten.Images = await _commonAppService.GetTourThumbPhoto(input.ItemId);

            var hostUser = UserManager.Users.FirstOrDefault(p => p.Id == tour.HostUserId);

            basicIten.HostUserInfo = new BasicHostUserInfo
            {
                Avarta = hostUser?.Avatar,
                UserId = hostUser?.Id ?? 0
            };
            return basicIten;

        }


        /// <summary>
        ///Đặt tiếp tour
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<NapasOutputDto> BookingPedingTour(PayPendingBooking input)
        {
            var booking = _bookingRepository.FirstOrDefault(p => p.Id == input.BookingId);
            if (booking == null)
                throw new UserFriendlyException("Booking không tồn tại");
            if (booking.Status != BookingStatusEnum.WaitPayment)
                throw new UserFriendlyException("Booking không tồn tại");
            decimal amount = 0;
            decimal serviceFee = 0;
            Coupon couponCode = null;
            //Check couponcode
            if (!string.IsNullOrEmpty(input.CouponCode))
            {
                couponCode = _couponRepository.GetAll().FirstOrDefault(p => p.Code == input.CouponCode);
                if (couponCode == null)
                {
                    throw new UserFriendlyException("Mã ưu đãi không tồn tại");
                }

                if (couponCode.Status == CouponStatusEnum.Used)
                {
                    throw new UserFriendlyException("Mã ưu đãi đã sư dụng");
                }
            }


            booking.BonusAmount = 0;
            booking.CouponCode = "";
            booking.CouponId = 0;

            if (couponCode != null)
            {
                booking.BonusAmount = couponCode?.Amount ?? 0;
                booking.CouponCode = input.CouponCode;
                booking.CouponId = couponCode?.Id ?? 0;
                couponCode.Status = CouponStatusEnum.Used;
                _couponRepository.Update(couponCode);
            }

            _bookingRepository.Update(booking);


            var order = await _orderAppService.CreateOrder(new CreateOrderInputDto
            {
                Amount = booking.Fee + booking.Amount - booking.BonusAmount,
                Note = "Đặt tour",
                OrderType = OrderTypeEnum.Booking,
                UserId = AbpSession.UserId ?? 0,
                Currency = "USD",
                BookingId = booking.Id,
            });
            return await _paymentAppService.PaymentOrder(new PaymentInputDto
            {
                CardType = input.Payment.CardType,
                CardId = input.Payment.CardId,
                OrderId = order.Id,
                SuccessUrl = input.Payment.SuccessUrl,
                FailUrl = input.Payment.FailUrl
            });
        }

        /// <summary>
        /// My Booking Traveler
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        ///
        ///
        [Audited]
        public async Task<PagedResultDto<MyBookingDto>> GetMyBooking(MyBookingInputDto inputDto)
        {
            var startTime = DateTime.Now;
            var fromDate = new DateTime(inputDto.Year, inputDto.Month, inputDto.Day);
            var todate = fromDate.AddDays(1).AddSeconds(-1);
            var query = from p in _bookingDetailRepository.GetAll().Include(p => p.ProductFk)
                        where
                            p.BookingUserId == AbpSession.UserId && p.StartDate >= fromDate && p.StartDate <= todate
                            && p.Status != BookingStatusEnum.Deleted && p.Status != BookingStatusEnum.Hide
                        select new MyBookingDto
                        {
                            ItemId = p.ProductId??0,
                            Price = new BasicPriceDto
                            {
                                Price = p.Amount,
                            },
                            Status = p.Status,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,

                            Quantity = p.Quantity,
                            TotalAmount = p.Amount + p.Fee,

                           // ItemType = p.ProductFk.Type,
                            BookingId = p.BookingId,
                            ItemScheduleId = p.ProductScheduleId,
                            BookingDetailId = p.Id,
                        };
            var total = query.Count();

            //  Console.WriteLine("Count : "+( DateTime.Now-startTime).TotalMilliseconds);
            var list = query.Skip(inputDto.SkipCount).Take(inputDto.MaxResultCount).ToList();
            //  Console.WriteLine("query : "+( DateTime.Now-startTime).TotalMilliseconds);

            var itemId1s = list.Select(p => p.ItemId).Distinct().ToList();

            var tours = _tourRepository.GetAll().Where(p => itemId1s.Contains(p.Id)).ToList();

            var list1 = (from p in list
                         join q in tours on p.ItemId equals q.Id
                         // join x in _tourScheduleRepository.GetAll() on p.ItemScheduleId equals x.Id
                         //  where p.ItemType == ItemTypeEnum.Tour
                         select new MyBookingDto
                         {
                             Price = p.Price,
                             Status = p.Status,
                             Title = q.Name,
                             ItemId = q.Id,
                            // ItemType = p.ItemType,
                             BookingId = p.BookingId,
                             ItemScheduleId = p.ItemScheduleId,
                             BookingDetailId = p.BookingDetailId,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             Quantity = p.Quantity,
                             TotalAmount = p.TotalAmount,
                             TripLength = q.TripLengh,
                         }).ToList();



            //  Console.WriteLine("join list2 : "+( DateTime.Now-startTime).TotalMilliseconds);

            var thumbImages1 = await _commonAppService.GetTourThumbPhotos(itemId1s);
            var reviews1 = await _commonAppService.GetTourReviewSummarys(itemId1s);
            foreach (var item in list1)
            {
                item.ThumbImages =
                    thumbImages1.ContainsKey(item.ItemId)
                        ? thumbImages1[item.ItemId]
                        : null; // await _commonAppService.GetTourThumbPhoto(item.ItemId);
                item.Review = reviews1.ContainsKey(item.ItemId) ? reviews1[item.ItemId] : null;
            }
            return new PagedResultDto<MyBookingDto>()
            {
                Items = list1,
                TotalCount = total
            };
        }

        /// <summary>
        /// Hủy bỏ trip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        ///
        [UnitOfWork]
        public async Task UpdateBookingStatus(UpdateBookingStatusDto input)
        {
            //todo check điều kiện nào được hủy chuyến
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p => p.Id == input.BookingDetailId);
            if (bookingDetail == null)
                throw new UserFriendlyException("Booking không tồn tại");

            if (input.Status == BookingStatusEnum.Deleted)
            {
                if (bookingDetail.Status != BookingStatusEnum.WaitPayment)
                {
                    throw new UserFriendlyException("Trạng thái không hợp lệ");
                }

                bookingDetail.Status = input.Status;
                bookingDetail.Note = input.Note;
            }
            else if (input.Status == BookingStatusEnum.Cancel)
            {
                if (bookingDetail.Status != BookingStatusEnum.WaitConfirm &&
                    bookingDetail.Status != BookingStatusEnum.Accepted)
                {
                    throw new UserFriendlyException("Trạng thái không hợp lệ");
                }

                //todo chỗ này đưa vào config
                // int x1 = 30;
                // int x2 = 7;
                decimal refundAmount = 0;
                if ((DateTime.Now - bookingDetail.StartDate).TotalDays >= AppConsts.X_DAY_1)
                {
                    refundAmount = bookingDetail.Amount * AppConsts.REFUND_PERCENT_1 / 100;
                }
                else if ((DateTime.Now - bookingDetail.StartDate).TotalDays >= AppConsts.X_DAY_2)
                {
                    refundAmount = bookingDetail.Amount * AppConsts.REFUND_PERCENT_2 / 100;
                }

                bookingDetail.RefundAmount = refundAmount;
                bookingDetail.Status = input.Status;
                bookingDetail.Note = input.Note;

                var tour = _tourRepository.GetAll().FirstOrDefault(p => p.Id == bookingDetail.ProductId);
                tour.BookingCount = tour.BookingCount - bookingDetail.Quantity;
                //  tour.TotalRevenew = tour.TotalRevenew - bookingDetail.Amount;

                _tourRepository.Update(tour);

                var tourSchedule =
                    _tourScheduleRepository.FirstOrDefault(p => p.Id == bookingDetail.ProductScheduleId);
                tourSchedule.TotalBook = tourSchedule.TotalBook - bookingDetail.Quantity;
                _tourScheduleRepository.Update(tourSchedule);
                _bookingDetailRepository.Update(bookingDetail);

            }

            if (input.Status == BookingStatusEnum.Hide)
            {
                if (bookingDetail.Status != BookingStatusEnum.Complete)
                {
                    throw new UserFriendlyException("Trạng thái không hợp lệ");
                }

                bookingDetail.Status = input.Status;
            }

            if (input.Status == BookingStatusEnum.Complete)
            {
                if (bookingDetail.Status != BookingStatusEnum.Accepted && bookingDetail.EndDate > DateTime.Now)
                {
                    throw new UserFriendlyException("Trạng thái không hợp lệ");
                }

                bookingDetail.Status = input.Status;
            }


            _bookingDetailRepository.Update(bookingDetail);
        }

        /// <summary>
        /// Report tour
        /// </summary>
        /// <param name="claimInput"></param>
        /// <returns></returns>
        public async Task Report(ClaimInputDto claimInput)
        {
            var bookingClaim = new BookingClaim
            {
                BookingDetailId = claimInput.BookingId,
                ClaimReasonId = claimInput.ClaimId,
                //  Type = ItemTypeEnum.Tour,
                // ItemId = claimInput.ItemId,
            };
            _bookingClaimRepository.Insert(bookingClaim);
        }

        /// <summary>
        /// Vết đánh giá Tour
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [UnitOfWork]
        public async Task WriteReview(WriteReviewInputDto input)
        {
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p => p.Id == input.BookingId);
            if (bookingDetail == null)
            {
                throw new UserFriendlyException("Booking not exist");
            }

            if (bookingDetail.Status != BookingStatusEnum.Complete)
            {
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            }

            var photos = input.Photos.Select(p => new ProductImage
            {
                ImageType = ImageType.GuestImage,
                ProductId = bookingDetail.ProductId,

                Url = p,
            });
            foreach (var photo in photos)
                _imageRepository.Insert(photo);

            var reviewDetail = new ProductReviewDetail
            {
                Comment = input.Comment,
                Title = input.Title,
                Transport = input.Transport,
                BookingId = input.BookingId,

                ProductId = input.ItemId,
                Food = input.Food,
                Intineraty = input.Itineraty,
                Service = input.Service,
                GuideTour = input.GuidTour,
                RatingAvg = (float)input.AvgRatting(),
            };
            _tourReviewDetailRepository.Insert(reviewDetail);
            var tourReview =
                _tourReviewRepository.FirstOrDefault(p => p.ProductId == input.ItemId);
            if (tourReview == null)
            {
                tourReview = new ProductReview
                {
                    Food = input.Food,
                    Intineraty = input.Itineraty,
                    Service = input.Service,
                    Transport = input.Transport,
                    GuideTour = input.GuidTour,

                    RatingAvg = (float)input.AvgRatting(),
                    ReviewCount = 1
                };
                _tourReviewRepository.Insert(tourReview);
            }
            else
            {
                var review = (from p in _tourReviewDetailRepository.GetAll()
                              where p.ProductId == input.ItemId
                              group p by p.ProductId
                    into g
                              select new
                              {
                                  Food = g.Sum(p => p.Food),
                                  Service = g.Sum(p => p.Service),
                                  Transport = g.Sum(p => p.Transport),
                                  GuideTour = g.Sum(p => p.GuideTour),
                                  Intineraty = g.Sum(p => p.Intineraty),
                                  Count = g.Count()
                              }).FirstOrDefault();

                tourReview.Food = (float)Math.Round(review.Food / review.Count, 2);
                tourReview.Service = (float)Math.Round(review.Service / review.Count, 2);
                tourReview.Transport = (float)Math.Round(review.Transport / review.Count, 2);
                tourReview.GuideTour = (float)Math.Round(review.GuideTour / review.Count, 2);
                tourReview.Intineraty = (float)Math.Round(review.Intineraty / review.Count, 2);
                tourReview.ReviewCount = review.Count;
                _tourReviewRepository.Update(tourReview);
            }
        }

        /// <summary>
        /// Danh sách lý do report
        /// </summary>
        /// <returns></returns>
        public async Task<List<ClaimDto>> GetClaim()
        {
            var list = _claimRepository.GetAll().Select(p => new ClaimDto
            {
                ClaimId = p.Id,
                ClaimTitle = p.Title
            }).ToList();
            return list;
        }

        /// <summary>
        /// Check mã giảm giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        
        
        //todo chỗ này xem lại
        [AbpAllowAnonymous]
        public async Task<CheckCouponOutputDto> CheckCouponCode(CheckCouponInputDto input)
        {
            var couponcode = _couponRepository.FirstOrDefault(p => p.Code == input.CouponCode);
            if (couponcode == null || couponcode.Status == CouponStatusEnum.Used)
            {
                throw new UserFriendlyException("Mã giảm giá không tồn tại");
            }

            return new CheckCouponOutputDto
            {
                Amount = couponcode.Amount
            };
        }

        public async Task<List<AvaiableTimeDto>> GetAvaiableTimesInMonthOfTour(DateTime month, long tourId,
            int itemType)
        {
            var fromDate = new DateTime(month.Year, month.Month, 1);
            var todate = fromDate.AddMonths(1);


            var tour = _tourRepository.GetAll().Where(p => p.Id == tourId).FirstOrDefault();
            if (tour.Type != ProductTypeEnum.TripPlan)
            {
                var items = _tourScheduleRepository.GetAll()
                    .Where(p => p.ProductId == tourId && p.StartDate >= fromDate && p.StartDate < todate &&
                                p.StartDate >= DateTime.Today && (p.TotalSlot - p.LockedSlot - p.TotalBook)>0)
                    .Select(p => new AvaiableTimeDto
                    {
                        Price = new BasicPriceDto
                        {
                            Price = p.Price,
                        },
                        FromDate = p.StartDate,
                        DepartureTime = p.DepartureTime,
                        ToDate = p.EndDate,
                        ScheduleId = p.Id,
                        TotalBook = p.TotalBook,
                        TotalSlot = p.TotalSlot,
                        AvaiableSlot = p.Avaiable
                    }).ToList();
                var feeconfig = _commonAppService.GetFeeConfigs(new List<long> { tour.HostUserId ?? 0 }).Result
                    .FirstOrDefault();
                if (feeconfig != null)
                    foreach (var item in items)
                    {
                        item.Price.ServiceFee = feeconfig.ServiceFee(item.Price.Price);
                    }


                return items;
            }
            else
            {
                var tourDetails = _tourDetailRepository.GetAll().Where(p => p.ProductId == tourId);
                var tourdetailItem = _tourDetailItemRepository.GetAll().Where(p => p.ProductId == tourId).ToList();
                var itemIds = tourdetailItem.Select(p => p.ItemId).ToList();
                var itemScheduleList = _tourScheduleRepository.GetAll().Where(p =>
                        p.StartDate >= fromDate && p.EndDate < todate &&
                        p.StartDate >= DateTime.Today && p.Avaiable > 0 && p.AllowBook && itemIds.Contains(p.ProductId))
                    .ToList();
                var items = new List<AvaiableTimeDto>();
                var tourItems = _tourRepository.GetAll().Where(p => itemIds.Contains(p.Id));
                var hostUserIds = tourItems.Select(p => p.HostUserId ?? 0).ToList();
                var feeConfigs = await _commonAppService.GetFeeConfigs(hostUserIds);
                //check tung ngay trong thang
                for (int i = DateTime.Today.Day; i <= todate.AddDays(-1).Day; i++)
                {
                    var slot = int.MaxValue;
                    decimal price = 0;
                    decimal sercviceFee = 0;
                    int j = 0;
                    foreach (var tourdetail in tourDetails)
                    {
                        fromDate = new DateTime(month.Year, month.Month, i).AddDays(j);
                        todate = fromDate.AddDays(j + 1);
                        var itemIdsInTourDetail = tourdetailItem.Where(p => p.ProductDetailId == tourdetail.Id)
                            .Select(p => p.ItemId).ToList();
                        var tourItemScheduleInDays = itemScheduleList.Where(p =>
                            p.StartDate >= fromDate && p.EndDate < todate &&
                            p.StartDate >= DateTime.Today && p.Avaiable > 0 && p.AllowBook &&
                            itemIdsInTourDetail.Contains(p.ProductId)).ToList();
                        if (tourItemScheduleInDays.Any())
                        {
                            price += tourItemScheduleInDays.Sum(p => p.Price);
                            slot = Math.Min(tourItemScheduleInDays.Min(p => p.TotalSlot), slot);
                            foreach (var item in tourItemScheduleInDays)
                            {
                                var tourItem = tourItems.FirstOrDefault(p => p.Id == item.ProductId);
                                if (tourItem != null)
                                {
                                    var feeConfig =
                                        feeConfigs.FirstOrDefault(p => p.HostUserId == tourItem.HostUserId);
                                    if (feeConfig != null)
                                    {
                                        sercviceFee += feeConfig.ServiceFee(item.Price);
                                    }
                                }
                            }
                        }

                        j++;
                    }

                    if (price > 0)
                    {
                        items.Add(new AvaiableTimeDto
                        {
                            AvaiableSlot = slot,
                            FromDate = new DateTime(month.Year, month.Month, i),
                            ToDate = new DateTime(month.Year, month.Month, j + 1),
                            Price = new BasicPriceDto
                            {
                                Price = price,
                                ServiceFee = sercviceFee
                            },
                            TotalBook = tour.BookingCount,
                            TotalSlot = slot,
                        });
                    }
                }

                return items;
            }



        }

        /// <summary>
        /// Book Tour và thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        ///
        [UnitOfWork]
        public async Task<NapasOutputDto> BookingAndPay(BookingAndPayTourDto input)
        {
            //if (input.ItemType == ItemTypeEnum.TourItem)
            //{
            //    return await BookingTourItem(input);
            //}
            //else
            //{
            return await BookingTour(input);
            // }
        }

        /// <summary>
        /// Book tour Item
        /// </summary>
        /// <param name="input"></param>
        private async Task<NapasOutputDto> BookingTourItem(BookingAndPayTourDto input)
        {
            return null;
            //decimal amount = 0;
            //decimal serviceFee = 0;
            //Coupon couponCode = null;
            ////Check couponcode
            //if (!string.IsNullOrEmpty(input.CouponCode))
            //{
            //    couponCode = _couponRepository.GetAll().FirstOrDefault(p => p.Code == input.CouponCode);
            //    if (couponCode == null)
            //    {
            //        throw new UserFriendlyException("Mã ưu đãi không tồn tại");
            //    }

            //    if (couponCode.Status == CouponStatusEnum.Used)
            //    {
            //        throw new UserFriendlyException("Mã ưu đãi đã sư dụng");
            //    }
            //}

            //var tourSchedule = _itemScheduleRepository.FirstOrDefault(p => p.Id == input.TourScheduleId);
            //if (tourSchedule.Avaiable < input.NumberOfGuest)
            //    throw new UserFriendlyException("Tour đã được book hết");
            //tourSchedule.LockedSlot = tourSchedule.LockedSlot + input.NumberOfGuest;
            //_itemScheduleRepository.Update(tourSchedule);
            //amount = tourSchedule.Price * input.NumberOfGuest;
            //serviceFee = (decimal)((double)amount * SDiscoConsts.FeePercent);
            //var tour = _tourRepository.FirstOrDefault(p => p.Id == input.ItemId);

            //if (tour != null)
            //{
            //    tour.BookingCount = tour.BookingCount + 1;
            //    _tourRepository.Update(tour);
            //}

            //var feeConfig = _commonAppService.GetFeeConfigs(new List<long> { tour.HostUserId }).Result.FirstOrDefault();
            //if (feeConfig != null)
            //    serviceFee = feeConfig.ServiceFee(tourSchedule.Price) * input.NumberOfGuest;
            //var bookingDetail = new BookingDetail
            //{
            //    StartDate = tourSchedule.FromTime,
            //    EndDate = tourSchedule.ToTime,
            //    ItemId = input.ItemId,
            //    ItemType = ItemTypeEnum.TourItem,
            //    Amount = amount,
            //    Fee = serviceFee,
            //    TripLength = tour?.TripLengh ?? 0,
            //    HostUserId = tour?.HostUserId ?? 0,
            //    BookingUserId = AbpSession.UserId ?? 0,
            //    Status = BookingStatusEnum.WaitPayment,
            //    Quantity = input.NumberOfGuest,
            //    HostPaymentStatus = 0,
            //    ItemScheduleId = input.TourScheduleId,
            //};
            //List<BookingDetail> bookingDetails = new List<BookingDetail>();
            //bookingDetails = new List<BookingDetail>
            //{
            //    bookingDetail
            //};

            //var booking = new DB.Booking
            //{
            //    ItemId = input.ItemId,
            //    Amount = amount,
            //    Status = BookingStatusEnum.WaitPayment,
            //    ItemScheduleId = input.TourScheduleId,
            //    Note = input.Note,
            //    GuestInfo = JsonConvert.SerializeObject(input.Guests),
            //    Contact = input.Contact != null ? JsonConvert.SerializeObject(input.Contact) : "",
            //    StartDate = tourSchedule.FromTime,
            //    EndDate = tourSchedule.ToTime,
            //    Quantity = input.NumberOfGuest,
            //    TripLength = tourSchedule.TripLength,
            //    Fee = serviceFee,
            //    BookingType = ItemTypeEnum.TourItem,
            //    CouponCode = input.CouponCode,
            //    BonusAmount = couponCode?.Amount ?? 0,
            //    CouponId = couponCode?.Id ?? 0
            //};
            //if (couponCode != null)
            //{
            //    couponCode.Status = 1;
            //    _couponRepository.Update(couponCode);
            //}

            //booking.Id = _bookingRepository.InsertAndGetId(booking);
            //foreach (var bookingDetail1 in (bookingDetails))
            //{
            //    bookingDetail1.BookingId = booking.Id;
            //    _bookingDetailRepository.Insert(bookingDetail1);
            //}

            //var order = await _orderAppService.CreateOrder(new CreateOrderInputDto
            //{
            //    Amount = serviceFee + amount - (couponCode?.Amount ?? 0),
            //    Note = "Đặt tour",
            //    OrderType = OrderTypeEnum.Booking,
            //    UserId = AbpSession.UserId ?? 0,
            //    Currency = "USD",
            //    BookingId = booking.Id,
            //});
            //return await _paymentAppService.PaymentOrder(new PaymentInputDto
            //{
            //    CardType = input.Payment.CardType,
            //    CardId = input.Payment.CardId,
            //    OrderId = order.Id,
            //    SuccessUrl = input.Payment.SuccessUrl,
            //    FailUrl = input.Payment.FailUrl
            //});
        }

        [UnitOfWork]
        private async Task<NapasOutputDto> BookingTour(BookingAndPayTourDto input)
        {
            decimal bonusamount = 0;
            long couponCodeId = 0;
            //Check couponcode
            if (!string.IsNullOrEmpty(input.CouponCode))
            {
                var couponCode = _couponRepository.GetAll().FirstOrDefault(p => p.Code == input.CouponCode);
                if (couponCode == null)
                {
                    throw new UserFriendlyException("Mã ưu đãi không tồn tại");
                }

                if (couponCode.Status == CouponStatusEnum.Used)
                {
                    throw new UserFriendlyException("Mã ưu đãi đã sư dụng");
                }

                couponCode.Status = CouponStatusEnum.Used;
                _couponRepository.Update(couponCode);
                bonusamount = couponCode.Amount;
                couponCodeId = couponCode.Id;
            }

            var item = _tourRepository.FirstOrDefault(p => p.Id == input.ItemId);
            List<BookingDetail> bookingDetails = new List<BookingDetail>();
            var booking = new Booking();
            decimal amount = 0;
            decimal serviceFee = 0;
            //  item.Type = TourTypeEnum.Tour;
            if (item.Type == ProductTypeEnum.TripPlan)
            {
                //Tinh lai gia cua item.

                var tourDetails = _tourDetailRepository.GetAll().Where(p => p.ProductId == item.Id).OrderBy(p => p.Order)
                    .ToList();
                var tourDetailIds = tourDetails.Select(p => p.Id).ToList();
                var tourDetailItemsItems = _tourDetailItemRepository.GetAll()
                    .Where(p => tourDetailIds.Contains(p.ProductDetailId ?? 0))
                    .ToList();
                var tourItemIds = tourDetailItemsItems.Select(p => p.ItemId).Distinct().ToList();

                var tourItems = _tourRepository.GetAll().Where(p => tourItemIds.Contains(p.Id));

                var hostUserId = tourItems.Select(p => p.HostUserId ?? 0).ToList();
                var feeConfigs = _commonAppService.GetFeeConfigs(hostUserId).Result;

                int i = 0;

                foreach (var day in tourDetails)
                {
                    var itemsInday = tourDetailItemsItems.Where(p => p.ProductDetailId == day.Id);
                    var itemIds = itemsInday.Select(p => p.ItemId).ToList();
                    var listItemAvaiable = _tourScheduleRepository.GetAll().Where(p => itemIds.Contains(p.ProductId) &&
                                                                                       p.AllowBook &&
                                                                                       p.Avaiable >=
                                                                                       input.NumberOfGuest &&
                                                                                       p.StartDate >=
                                                                                       input.BookingDate.Value.AddDays(
                                                                                           i) &&
                                                                                       p.StartDate <
                                                                                       input.BookingDate.Value.AddDays(
                                                                                           i + 1)).ToList();

                    amount = amount + listItemAvaiable.Sum(p => p.Price) * input.NumberOfGuest;
                    foreach (var scheduleItem in listItemAvaiable)
                    {
                        var tour = tourItems.FirstOrDefault(p => p.Id == scheduleItem.ProductId);
                        var itemInday = itemsInday.FirstOrDefault(p => p.ItemId == scheduleItem.ProductId);
                        scheduleItem.LockedSlot = scheduleItem.LockedSlot + input.NumberOfGuest;
                        _tourScheduleRepository.Update(scheduleItem);
                        var feeConfig = feeConfigs.FirstOrDefault(p => p.HostUserId == tour.HostUserId);
                        decimal fee = 0;
                        if (feeConfig != null)
                        {
                            fee = feeConfig.ServiceFee(scheduleItem.Price);
                            serviceFee += fee * (decimal)input.NumberOfGuest;
                        }

                        var bookingDetail = new BookingDetail
                        {
                            StartDate = input.BookingDate.Value.AddDays(i),
                            EndDate = input.BookingDate.Value.AddDays(i + 1).AddSeconds(-1),
                            ProductId = scheduleItem.ProductId,
                            //ItemType = ItemTypeEnum.TourItem,
                            Amount = scheduleItem.Price * (decimal)input.NumberOfGuest,
                            Fee = fee * (decimal)input.NumberOfGuest,
                            TripLength = tour.TripLengh,
                            ProductScheduleId = scheduleItem.Id,
                            HostUserId = tour.HostUserId ?? 0,
                            BookingUserId = AbpSession.UserId ?? 0,
                            Status = BookingStatusEnum.WaitPayment,
                            Quantity = input.NumberOfGuest,
                            HostPaymentStatus = 0,
                            AffiliateUserId = itemInday.CreatorUserId ?? 0,
                            RoomId = itemInday.RoomId ?? 0,
                            ProductDetailComboId = itemInday?.Id
                        };

                        bookingDetails.Add(bookingDetail);
                    }

                    i++;
                }

                booking = new Booking
                {
                    ProductId = input.ItemId,
                    Contact = input.Contact != null ? JsonConvert.SerializeObject(input.Contact) : "",
                    Amount = amount,
                    Status = BookingStatusEnum.WaitPayment,
                    ProductScheduleId = input.TourScheduleId,
                    Note = input.Note,
                    GuestInfo = JsonConvert.SerializeObject(input.Guests),
                    StartDate = input.BookingDate.Value,
                    EndDate = input.BookingDate.Value.AddDays(item.TripLengh).AddSeconds(-1),
                    Quantity = input.NumberOfGuest,
                    TripLength = item.TripLengh,
                    Fee = serviceFee,
                    //   BookingType = ItemTypeEnum.Tour,
                    CouponCode = input.CouponCode,
                    CouponId = couponCodeId,
                    BonusAmount = bonusamount,
                    TotalAmount = amount + serviceFee - bonusamount

                };
            }
            else
            {
                var tourSchedule = _tourScheduleRepository.FirstOrDefault(p => p.Id == input.TourScheduleId);

                tourSchedule.LockedSlot = tourSchedule.LockedSlot + input.NumberOfGuest;

                _tourScheduleRepository.Update(tourSchedule);

                amount = tourSchedule.Price * input.NumberOfGuest;

                var tour = _tourRepository.FirstOrDefault(p => p.Id == input.ItemId);
                if (tour != null)
                {
                    tour.BookingCount = tour.BookingCount + 1;
                    _tourRepository.Update(tour);
                }

                var feeConfig = _commonAppService.GetFeeConfigs(new List<long> { tour.HostUserId ?? 0 }).Result
                    .FirstOrDefault();
                if (feeConfig != null)
                    serviceFee = feeConfig.ServiceFee(tourSchedule.Price) * input.NumberOfGuest;

                var bookingDetail = new BookingDetail
                {
                    StartDate = tourSchedule.StartDate,
                    EndDate = tourSchedule.EndDate,
                    ProductId = input.ItemId,
                    //ItemType = ItemTypeEnum.Tour,
                    Amount = amount,
                    Fee = serviceFee,
                    TripLength = tour.TripLengh,
                    HostUserId = tour.HostUserId ?? 0,
                    BookingUserId = AbpSession.UserId ?? 0,
                    Status = BookingStatusEnum.WaitPayment,
                    Quantity = input.NumberOfGuest,
                    HostPaymentStatus = 0,
                    ProductScheduleId = input.TourScheduleId,
                };
                bookingDetails = new List<BookingDetail>
                {
                    bookingDetail
                };
                booking = new Booking
                {
                    ProductId = input.ItemId,
                    Contact = input.Contact != null ? JsonConvert.SerializeObject(input.Contact) : "",
                    Amount = amount,
                    Status = BookingStatusEnum.WaitPayment,

                    ProductScheduleId = input.TourScheduleId,
                    Note = input.Note,
                    GuestInfo = JsonConvert.SerializeObject(input.Guests),
                    StartDate = tourSchedule.StartDate,
                    EndDate = tourSchedule.EndDate,
                    Quantity = input.NumberOfGuest,
                    TripLength = tourSchedule.TripLength,
                    Fee = serviceFee,

                    CouponCode = input.CouponCode,
                    BonusAmount = bonusamount,
                    CouponId = couponCodeId,
                    TotalAmount = amount + serviceFee - bonusamount
                };
            }

            booking.Id = _bookingRepository.InsertAndGetId(booking);
            foreach (var bookingDetail in (bookingDetails))
            {
                bookingDetail.BookingId = booking.Id;
                _bookingDetailRepository.Insert(bookingDetail);
            }

            var order = await _orderAppService.CreateOrder(new CreateOrderInputDto
            {
                Amount = serviceFee + amount - bonusamount,
                Note = "Đặt tour",
                OrderType = OrderTypeEnum.Booking,
                UserId = AbpSession.UserId ?? 0,
                Currency = "USD",
                BookingId = booking.Id,
            });
            return await _paymentAppService.PaymentOrder(new PaymentInputDto
            {
                CardType = input.Payment.CardType,
                CardId = input.Payment.CardId,
                OrderId = order.Id,
                SuccessUrl = input.Payment.SuccessUrl,
                FailUrl = input.Payment.FailUrl,
            });
        }

        /// <summary>
        /// Thông tin booking
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public async Task<TepayLink.Sdisco.Bookings.Dto.BookingDetailDtoV1> GetBookingDetail(long bookingId)
        {
            var booking = _bookingDetailRepository.FirstOrDefault(p => p.Id == bookingId);
            if (booking == null)
                return null;
            var item = new TepayLink.Sdisco.Bookings.Dto.BookingDetailDtoV1
            {
                Amount = booking.Amount,
                FromDate = booking.StartDate,
                ToDate = booking.EndDate,
                NumberOfGuest = booking.Quantity,
                Status = booking.Status,
                TourScheduleId = booking.ProductScheduleId,
                //  Type = booking.ItemType,
                TourDetail = await GetTourDetail(new GetTourDetailInputDto
                { ItemId = booking.ProductId ?? 0 })
            };
            return item;
        }

        public async Task<TepayLink.Sdisco.Bookings.Dto.BookingDetailDtoV1> GetPendingBooking(long bookingId)
        {
            var booking = _bookingRepository.FirstOrDefault(p => p.Id == bookingId);
            if (booking == null)
                return null;
            if (booking.Status != BookingStatusEnum.WaitPayment)
                return null;
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p => p.BookingId == bookingId);
            var item = new TepayLink.Sdisco.Bookings.Dto.BookingDetailDtoV1
            {
                Amount = booking.Amount,
                FromDate = booking.StartDate,
                ToDate = booking.EndDate,
                NumberOfGuest = booking.Quantity,
                Status = booking.Status,
                TourScheduleId = booking.ProductScheduleId,
                Fee = booking.Fee,


                TourDetail = await GetTourDetail(new GetTourDetailInputDto
                { ItemId = booking.ProductId ?? 0 })
            };
            return item;
        }

        public async Task<TepayLink.Sdisco.Bookings.Dto.BookingDetailDtoV1> BookingInfo(long bookingId)
        {
            var booking = _bookingRepository.FirstOrDefault(p => p.Id == bookingId);
            if (booking == null)
                return null;
            //            if (booking.Status != BookingStatusEnum.WaitPayment)
            //                return null;
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p => p.BookingId == bookingId);
            var item = new TepayLink.Sdisco.Bookings.Dto.BookingDetailDtoV1
            {
                Amount = booking.Amount,
                FromDate = booking.StartDate,
                ToDate = booking.EndDate,
                NumberOfGuest = booking.Quantity,
                Status = booking.Status,
                TourScheduleId = booking.ProductScheduleId,
                Fee = booking.Fee,

                TourDetail = await GetTourDetail(new GetTourDetailInputDto
                { ItemId = booking.ProductId ?? 0 })
            };
            return item;
        }


        /// <summary>
        /// Danh sách ngày có tour book trong tháng
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<int>> GetDayBookingInMonth(BasicDateDto inputDto)
        {
            var fromDate = new DateTime(inputDto.Year, inputDto.Month, 1);
            var todate = fromDate.AddMonths(1).AddSeconds(-1);

            var query = (from p in _bookingDetailRepository.GetAll()
                         where
                             p.BookingUserId == AbpSession.UserId && p.StartDate >= fromDate && p.StartDate <= todate
                         select p.StartDate.Day).Distinct().ToList();

            return query;
        }

        /// <summary>
        /// Lấy danh sách booking theo list ngày
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MyBookingGroupByDayDto>> MyBooking(MyBookingInput1Dto inputDto)
        {
            var list = new List<MyBookingGroupByDayDto>();
            foreach (var day in inputDto.Days)
            {
                var result = GetMyBooking(new MyBookingInputDto
                {
                    Day = day,
                    MaxResultCount = inputDto.MaxResultCount,
                    Month = inputDto.Month,
                    SkipCount = inputDto.SkipCount,
                    Year = inputDto.Year
                }).Result;
                var item = new MyBookingGroupByDayDto
                {
                    Data = result,
                    Date = new DateTime(inputDto.Year, inputDto.Month, day)
                };
                list.Add(item);
            }

            list = list.OrderBy(p => p.Date).ToList();


            return list;
        }

    }
}
