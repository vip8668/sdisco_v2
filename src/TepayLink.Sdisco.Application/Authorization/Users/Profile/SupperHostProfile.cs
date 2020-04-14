using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Authorization.Users.Profile.Dto;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Reports;
using TepayLink.Sdisco.Tour;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Authorization.Users.Profile
{
    [AbpAuthorize]
    public class SupperHostProfile : SdiscoAppServiceBase, ISupperHostProfile
    {
        private readonly IRepository<RefundReason> _refundReasonRepository;

        //  private readonly IRepository<DB.Booking, long> _bookingRepository;
        private readonly IRepository<BookingRefund, long> _bookingRefundRepository;
        private readonly IRepository<ProductReviewDetail, long> _tourReviewDetailRepository;
        private readonly ICommonAppService _commonAppService;
        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<ProductSchedule, long> _tourScheduleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Wallet, long> _walletRepository;

        private readonly IRepository<Transaction, long> _transactionRepository;

        private readonly IRepository<BookingDetail, long> _bookingDetailRepository;
        private readonly IRepository<RevenueByMonth, long> _revenueByMonthRepository;
        private readonly IRepository<WithDrawRequest, long> _withDrawRequestRepository;

        private readonly IRepository<Place, long> _placeRepository;

     



        /// <summary>
        /// My Booking
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MyBookingDto>> GetMyBooking(MyBookingInputDto inputDto)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var fromDate = new DateTime(inputDto.Year, inputDto.Month, inputDto.Day);
            var todate = fromDate.AddDays(1).AddSeconds(-1);
            var query = from p in _bookingDetailRepository.GetAll()
                            //  join q in _tourRepository.GetAll() on p.ItemId equals q.Id
                            // join x in _tourScheduleRepository.GetAll() on p.ItemScheduleId equals x.Id
                        where
                            p.HostUserId == hostUserId
                            && p.StartDate >= fromDate && p.StartDate <= todate
                             && p.Status != BookingStatusEnum.Deleted && p.Status != BookingStatusEnum.Hide

                        select new MyBookingDto
                        {
                            ItemId = p.ProductId??0,
                            Price = new BasicPriceDto
                            {
                                Price = p.Amount,
                            },
                            Status = p.Status,
                            Quantity = p.Quantity,
                            TotalAmount = p.Fee + p.Amount,
                            // Title = q.Name,
                            // ItemId = q.Id,
                         //   ItemType = p.ItemType,
                            BookingId = p.BookingId,
                            ItemScheduleId = p.ProductScheduleId,
                            BookingDetailId = p.Id,
                            //   TotalBook = x.TotalBook,
                            //  TotalSeat = x.TotalSlot,
                            //  TripLength = q.TripLengh,
                        };
            var total = query.Count();
            var list = query.Skip(inputDto.SkipCount).Take(inputDto.MaxResultCount).ToList();

            var itemId1s = list.Select(p => p.ItemId).Distinct().ToList();
          //  var itemId2s = list.Where(p => p.ItemType == ItemTypeEnum.TourItem).Select(p => p.ItemId).Distinct().ToList();
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
                             //  TotalBook = x.TotalBook,
                             // TotalSeat = x.TotalSlot,
                             Quantity = p.Quantity,
                             TotalAmount = p.TotalAmount,
                             TripLength = q.TripLengh,
                         }).ToList();

          
           

            var thumbImages1 = await _commonAppService.GetTourThumbPhotos(itemId1s);
            var reviews1 = await _commonAppService.GetTourReviewSummarys(itemId1s);
            foreach (var item in list1)
            {
                item.ThumbImages = thumbImages1.ContainsKey(item.ItemId) ? thumbImages1[item.ItemId] : null;// await _commonAppService.GetTourThumbPhoto(item.ItemId);
                item.Review = reviews1.ContainsKey(item.ItemId) ? reviews1[item.ItemId] : null;
            }

          
            return new PagedResultDto<MyBookingDto>()
            {
                Items = list1,
                TotalCount = total
            };
        }

        /// <summary>pa
        /// Lý do hoàn tiền
        /// </summary>
        /// <returns></returns>
        public async Task<List<RefundReasonDto>> GetRefundReason()
        {
            return _refundReasonRepository.GetAll().Select(p => new RefundReasonDto
            {
                Id = p.Id,
                ReasonText = p.ReasonText
            }).ToList();
        }

        //todo chỗ này xem lại nghiệp vụ hoàn tiền này có cần duyệt không
        /// <summary>
        /// Hoàn tiền
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        [UnitOfWork]
        public async Task Refund(RefundDto input)
        {
            var booking = _bookingDetailRepository.FirstOrDefault(p => p.Id == input.BookingDetailId);
            if (booking == null)
            {
                throw new UserFriendlyException("Booking không tồn tại");
            }

            if (booking.Status == BookingStatusEnum.Refunded)
            {
                throw new UserFriendlyException("Booking đã được refund");
            }


            if (booking.Status == BookingStatusEnum.Cancel)
            {
                throw new UserFriendlyException("Booking đã được hủy");
            }

            var refund = new BookingRefund
            {
                Description = "",
                BookingDetailId = booking.Id,
                Status = 0,
                RefundMethodId = input.RefundMethodId,
            };
            if (booking.Status == BookingStatusEnum.WaitConfirm)
            {
                //todo hoàn tiền cho Khách hàng.
            }

            booking.Status = BookingStatusEnum.Refunded;
            _bookingDetailRepository.Update(booking);

            _bookingRefundRepository.Insert(refund);
        }

        /// <summary>
        /// Trip Created
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TourOfHost>> GetMyTour(PagedInputDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var query =
                (from t in
                        _tourRepository.GetAll()
                 where t.HostUserId == hostUserId
                       && t.Status == ProductStatusEnum.Publish
                 select new TourOfHost
                 {
                     Id = t.Id,
                     Title = t.Name,
                     SoldCount = t.BookingCount,
                     TripLength = t.TripLengh,
                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                     ShareCount = t.ShareCount,
                     CoppyCount = t.CoppyCount,
                     ViewCount = t.ViewCount,
                     Revenue = 0//t.TotalRevenew
                 });

            var total = query.Count();
            var list = query.OrderByDescending(p => p.Id).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
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

            return new PagedResultDto<TourOfHost>()
            {
                Items = list,
                TotalCount = total
            };
        }

        /// <summary>
        /// Danh sách review
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ReviewDetailDto>> GetReview(GetSupperHostProfileReview input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var query = from p in _tourReviewDetailRepository.GetAll()
                        join q in _tourRepository.GetAll() on p.ProductId equals q.Id
                        join u in _userRepository.GetAll() on p.CreatorUserId equals u.Id
                        where q.HostUserId == hostUserId
                                                          && (p.Read == false || input.ReadStatus == 1)
                                                          && p.ReplyId == null
                        select new ReviewDetailDto
                        {
                            Id = p.Id,
                            UserId = p.Id,
                            Title = p.Title,
                            Reviewer = u.FullName,
                            Avatar = u.Avatar,
                            Comment = p.Comment,
                            Ratting = p.RatingAvg,
                            ReviewDate = p.CreationTime,
                        };
            var total = query.Count();


            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var ids = query.Select(p => p.Id).ToList();

            var repliesList = _tourReviewDetailRepository
                .GetAll().Where(p => ids.Contains(p.ReplyId ?? 0))
                .GroupBy(p => p.ReplyId)
                .Select(p => new
                {
                    p.Key,
                    Total = p.Count()
                });
            foreach (var item in list)
            {
                var c = repliesList.FirstOrDefault(p => p.Key == item.Id);
                item.ReplyCount = c?.Total ?? 0;
            }


            return new PagedResultDto<ReviewDetailDto>
            {
                Items = list,
                TotalCount = total
            };
        }


        public async Task<List<ReviewDetailDto>> GetReply(long reviewId)
        {
            var query = from p in _tourReviewDetailRepository.GetAll()
                        join u in _userRepository.GetAll() on p.CreatorUserId equals u.Id
                        where p.ReplyId == reviewId
                        select new ReviewDetailDto
                        {
                            Id = p.Id,
                            UserId = p.Id,
                            Title = p.Title,
                            Reviewer = u.FullName,
                            Avatar = u.Avatar,
                            Comment = p.Comment,
                            Ratting = p.RatingAvg,
                            ReviewDate = p.CreationTime,
                        };
            var total = query.Count();


            var list = query.ToList();

            var ids = query.Select(p => p.Id).ToList();

            var repliesList = _tourReviewDetailRepository
                .GetAll().Where(p => ids.Contains(p.ReplyId ?? 0))
                .GroupBy(p => p.ReplyId)
                .Select(p => new
                {
                    p.Key,
                    Total = p.Count()
                });
            foreach (var item in list)
            {
                var c = repliesList.FirstOrDefault(p => p.Key == item.Id);
                item.ReplyCount = c?.Total ?? 0;
            }

            return list;
        }


        public async Task ReadReview(long reviewId)
        {
            var review = _tourReviewDetailRepository.FirstOrDefault(p => p.Id == reviewId);

            review.Read = true;
            _tourReviewDetailRepository.Update(review);
        }

        /// <summary>
        /// reply review
        /// </summary>
        /// <param name="reviewDto"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task ReplyReview(ReplyReviewDto reviewDto)
        {
            var review = _tourReviewDetailRepository.FirstOrDefault(p => p.Id == reviewDto.ReviewId);

            var newReview = new ProductReviewDetail
            {
                ReplyId = reviewDto.ReviewId,
                Comment = reviewDto.Comment,
               
                BookingId = review.BookingId,
                ProductId = review.ProductId,
            };
            _tourReviewDetailRepository.Insert(newReview);
        }

        /// <summary>
        /// Lịch sử ví
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<BalanceHistoryDto>> GetBalanceHistory(PagedInputDto input)
        {
            string policy = "Money paid according to service quantity. Penaty polyci.... ";
            var query = _transactionRepository.GetAll()
                .Where(p => p.UserId == AbpSession.UserId && p.WalletType == WalletTypeEnum.Money);

            var total = query.Count();
            query = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);


            var bookingDetail = query.Where(p => p.BookingDetailId != null).Select(p => p.BookingDetailId).ToList();

            var bookingList = _bookingDetailRepository.GetAll().Where(p => bookingDetail.Contains(p.Id)).Select(
                p => new
                {
                    p.Id,
                  //  p.ItemType,
                    p.ProductId
                }).ToList();
            var tourIds = bookingList.Select((p => p.Id)).ToList();

            var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).Select(q => new { q.Id, q.Name });
           

          
            var listdata = new List<BalanceHistoryDto>();
            foreach (var item in query)
            {
                var historyItem = new BalanceHistoryDto
                {
                    Amount = item.Amount * (item.Side == 1 ? 1 : -1),
                    Date = item.CreationTime,
                    Description = item.Descrition,
                    Side = item.Side,
                    Policy = policy
                };


                var booking = bookingList.FirstOrDefault((p => p.Id == item.BookingDetailId));
                if (booking != null)
                {
                    var bookingItem =
                        tours.FirstOrDefault((p =>
                            p.Id == booking.ProductId));
                    historyItem.ItemTitle = bookingItem?.Name;
                    historyItem.Description = bookingItem?.Name;
                    historyItem.Note = "You have successful paid for trip " + bookingItem?.Name;
                    historyItem.ItemId = bookingItem?.Id ?? 0;
                }


                listdata.Add(historyItem);
            }


            return new PagedResultDto<BalanceHistoryDto>
            {
                TotalCount = total,
                Items = listdata
            };
        }

        /// <summary>
        /// Get paid/unpaid amount
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GetPaidAmountOutputDto>> GetPaidAmount(GetPaidAmountInputDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            //todo fake data
            var query = _bookingDetailRepository.GetAll()
                .Where(p => p.IsDone == false && p.HostUserId == hostUserId &&
                            (new[]
                            {
                                BookingStatusEnum.Accepted, BookingStatusEnum.WaitConfirm, BookingStatusEnum.Accepted
                            })
                            .Contains(p.Status));

            if (input.Type == 1)
            {
                query = query.Where(p => p.HostPaymentStatus == 1);
            }
            else if (input.Type == 0)
            {
                query = query.Where(p => p.HostPaymentStatus == 0);
            }

            var total = query.Count();

            var list = query.ToList();

            var tourIds = list.Select(p => p.ProductId);
            

            var listTour = from p in _tourRepository.GetAll()
                           join q in _placeRepository.GetAll() on p.PlaceId equals q.Id
                           where tourIds.Contains(p.Id)
                           select new
                           {
                               p.Id,

                               p.Name,
                               PlaceName=   q.Name
                           };


            var data = new List<GetPaidAmountOutputDto>();
            foreach (var item in list)
            {
                var paid = new GetPaidAmountOutputDto
                {
                    BookingDetailId = item.Id,
                    ItemId = item.ProductId??0,
                   
                    Amount = item.Amount - item.Fee,
                };
                var abc = listTour.FirstOrDefault(p => p.Id == item.ProductId);
                paid.Place = abc != null ? abc.PlaceName : "";
                paid.Title = abc != null ? abc.Name : "";


                paid.ThumbImages = await
                     _commonAppService.GetTourThumbPhoto(item.ProductId??0);
                paid.Price = await
                     _commonAppService.GetPriceOfTour(item.ProductId??0);


                paid.Review = await
                     _commonAppService.GetTourReviewSummary(item.ProductId ?? 0);
                 

                if (input.Type == 0)
                {
                    paid.DayLeft = (int)(item.EndDate - DateTime.Now).TotalDays;
                    if (paid.DayLeft < 0)
                        paid.DayLeft = 0;
                }

                data.Add(paid);
            }


            return new PagedResultDto<GetPaidAmountOutputDto>()
            {
                Items = data,
                TotalCount = total
            };


            // if()
            // throw new System.NotImplementedException();
        }

        /// <summary>
        /// rút tiền
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ///
        [UnitOfWork]
        public async Task<long> WithDrawMoney(WithDrawDto input)
        {
            if (input.Amount <= 0)
            {
                throw new UserFriendlyException("Số tiển rút phải lớn hơn 0");
            }

            if (input.BankAccountId <= 0)
            {
                throw new UserFriendlyException("Tài khoản rút tiền không hợp lệ");
            }

            var wallet =
                _walletRepository.FirstOrDefault(
                    p => p.UserId == (AbpSession.UserId ?? 0) && p.Type == WalletTypeEnum.Money);
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    Balance = 0,
                    UserId = AbpSession.UserId ?? 0,
                    Type = WalletTypeEnum.Money
                };
                _walletRepository.Insert(wallet);
                throw new UserFriendlyException("Tài khoản không đủ tiền để rút");
            }

            if (wallet.Balance < input.Amount)
            {
                throw new UserFriendlyException("Tài khoản không đủ tiền để rút");
            }

            var withdrawRequest = new WithDrawRequest
            {
                Amount = input.Amount,
                BankAccountId = input.BankAccountId,
                Status = WithDrawRequestStatus.Init,
                UserId = AbpSession.UserId ?? 0,
            };
            withdrawRequest.Id = _withDrawRequestRepository.InsertAndGetId(withdrawRequest);

            var tran = new Transaction
            {
                Amount = input.Amount,
                Side = 2,
                Descrition = "Withdraw",
                TransType = TransactionType.WithDraw,
                UserId = AbpSession.UserId ?? 0,
                WalletType = WalletTypeEnum.Money,
                RefId = withdrawRequest.Id
            };

            wallet.Balance = wallet.Balance - input.Amount;
            _walletRepository.Update(wallet);
            tran.Id = _transactionRepository.InsertAndGetId(tran);


            return withdrawRequest.Id;
        }

        [UnitOfWork]
        public async Task CancelDrawMoney(long id)
        {
            var request = _withDrawRequestRepository.FirstOrDefault(p => p.UserId == AbpSession.UserId && p.Id == id);
            if (request != null)
            {
                request.Status = WithDrawRequestStatus.Cancel;
                _withDrawRequestRepository.Update(request);
                var tran = new Transaction
                {
                    Amount = request.Amount,
                    Side = 1,
                    Descrition = "Cancel withdraw",
                    TransType = TransactionType.CancelWithDraw,
                    UserId = AbpSession.UserId ?? 0,
                    WalletType = WalletTypeEnum.Money,
                    RefId = id,
                };
                var wallet =
                    _walletRepository.FirstOrDefault(
                        p => p.UserId == (AbpSession.UserId ?? 0) && p.Type == WalletTypeEnum.Money);
                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        Balance = 0,
                        UserId = AbpSession.UserId ?? 0,
                        Type = WalletTypeEnum.Money
                    };
                    _walletRepository.Insert(wallet);
                    wallet.Balance = wallet.Balance + request.Amount;
                    _walletRepository.Update(wallet);
                    tran.Id = _transactionRepository.InsertAndGetId(tran);
                }
            }
        }

        /// <summary>
        /// Lấy số dư
        /// </summary>
        /// <returns></returns>
        public async Task<BalanceOutputDto> GetBalance()
        {
            var wallet =
                _walletRepository.FirstOrDefault(p => p.UserId == AbpSession.UserId && p.Type == WalletTypeEnum.Money);
            return new BalanceOutputDto
            {
                Balance = wallet != null ? wallet.Balance : 0
            };
            // throw new NotImplementedException();
        }

        /// <summary>
        /// lấy data cho biểu đồ
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ChartBalanceDto>> GetChartBalance(GetChartBalanceDto input)
        {
            DateTime fromDate;
            DateTime toDate;

            if (input.Type == 1)
            {
                fromDate = new DateTime(DateTime.Now.Year, 1, 1);
                toDate = new DateTime(DateTime.Now.Year + 1, 1, 1).AddSeconds(-1);
            }
            else if (input.Type == 3)
            {
                fromDate = DateTime.Today.AddDays(-7);
                toDate = DateTime.Today.AddDays(1);
            }
            else if (input.Type == 4)
            {
                fromDate = DateTime.Today.AddDays(-30);
                toDate = DateTime.Today.AddDays(1);
            }
            else
            {
                fromDate = new DateTime(2017, 1, 1);
                toDate = new DateTime(DateTime.Now.Year + 1, 1, 1).AddSeconds(-1);
            }

            var query = _revenueByMonthRepository.GetAll().Where(p =>
                p.UserId == AbpSession.UserId && p.Date > fromDate && p.Date < toDate);


            if (input.Type == 1)
            {
                var chartData = query.GroupBy(p => p.Date.Month).Select(p => new ChartBalanceDto
                {
                    Key = p.Key,
                    Value = p.Sum(q => q.Revenue),
                    KeyDate = new DateTime(fromDate.Year, p.Key, 1)
                }).ToList();


                for (var i = fromDate.Month; i <= toDate.Month; i++)
                {
                    if (chartData.All(p => p.Key != i))
                    {
                        chartData.Add(new ChartBalanceDto
                        {
                            Key = i,
                            Value = 0,
                            KeyDate = new DateTime(fromDate.Year, i, 1)
                        });
                    }
                }

                chartData = chartData.OrderBy(p => p.Key).ToList();
                return chartData;
            }

            if (input.Type == 2)
            {
                var chartData = query.GroupBy(p => p.Date.Year).Select(p => new ChartBalanceDto
                {
                    Key = p.Key,
                    Value = p.Sum(q => q.Revenue),
                    KeyDate = new DateTime(p.Key, 1, 1)
                }).ToList();

                for (var i = fromDate.Year; i <= toDate.Year; i++)
                {
                    if (chartData.All(p => p.Key != i))
                    {
                        chartData.Add(new ChartBalanceDto
                        {
                            Key = i,
                            Value = 0,
                            KeyDate = new DateTime(i, 1, 1)
                        });
                    }
                }

                chartData = chartData.OrderBy(p => p.Key).ToList();
                return chartData;
            }

            if (input.Type == 3 || input.Type == 4)
            {
                var list = query.ToList();
                var data = list.GroupBy(p => new DateTime(p.Date.Year, p.Date.Month, p.Date.Day)).Select(p =>
                    new ChartBalanceDto
                    {
                        KeyDate = p.Key,
                        Value = p.Sum(q => q.Revenue)
                    }).ToList();

                for (int i = 0; i < (input.Type == 3 ? 7 : 30); i++)
                {
                    if (!data.Any(p => p.KeyDate == fromDate.AddDays(i)))
                    {
                        data.Add(new ChartBalanceDto
                        {
                            KeyDate = fromDate.AddDays(i),
                            Value = 0
                        });
                    }
                }

                data = data.OrderBy(p => p.KeyDate).ToList();
                return data;
            }


            return null;
        }


        /// <summary>
        /// Lấy doanh thu / số chuyến đi
        /// </summary>
        /// <returns></returns>
        public async Task<RevenueSummaryDto> GetRevenueSummary()
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var totalTour = _tourRepository.GetAll()
                .Where(p => p.Status == ProductStatusEnum.Publish && p.HostUserId == hostUserId &&
                            p.Type == ProductTypeEnum.Tour).ToList();
            var totaBbook = totalTour.Count(p => p.BookingCount > 0);


            return new RevenueSummaryDto
            {
                Percent = (totaBbook / totalTour.Count()) * 100,
                TotalRevenue = 0,//totalTour.Sum(p => p.TotalRevenew),
                TotalTrip = totalTour.Count()
            };
        }

        /// <summary>
        /// List refund method
        /// </summary>
        /// <returns></returns>
        public async Task<List<RefundMethodDto>> GetRefundMethod()
        {
            //todo fake data
            return new List<RefundMethodDto>()
            {
                new RefundMethodDto
                {
                    Id = 1,
                    Name = "VISA"
                },
                new RefundMethodDto
                {
                    Id = 2,
                    Name = "ATM"
                }
            };
            //  throw new NotImplementedException();
        }


        /// <summary>
        /// Danh sách ngày có tour book trong tháng
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<int>> GetDayBookingInMonth(BasicDateDto inputDto)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var fromDate = new DateTime(inputDto.Year, inputDto.Month, 1);
            var todate = fromDate.AddMonths(1).AddSeconds(-1);
            var query = (from p in _bookingDetailRepository.GetAll()
                         where
                             p.HostUserId == hostUserId && p.StartDate >= fromDate && p.StartDate <= todate
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
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
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


            return list;
        }

        public async Task DonePayment(long bookingDetailId)
        {
            var item = _bookingDetailRepository.FirstOrDefault(p => p.Id == bookingDetailId);
            if (item != null)
            {
                item.IsDone = true;
                _bookingDetailRepository.Update(item);
            }
        }

        public async Task<PagedResultDto<WithdrawOutputDto>> GetPendingWithdraw(PagedInputDto input)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var query = _withDrawRequestRepository.GetAll()
                .Where(p => p.CreatorUserId == AbpSession.UserId && p.Status == WithDrawRequestStatus.Init).Select(p =>
                    new WithdrawOutputDto
                    {
                        Id = p.Id,
                        Amount = p.Amount,
                        Description = "Withdraw",
                        CreatedDate = p.CreationTime
                    });
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return new PagedResultDto<WithdrawOutputDto>
            {
                Items = list,
                TotalCount = total
            };
        }


        /// <summary>
        /// 1:
        /// </summary>
        /// <param name="bookingDetailId"></param>
        /// <param name="status"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task UpdateBookingStatus(long bookingDetailId, int status, string note)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p =>
                p.Id == bookingDetailId && p.HostUserId == hostUserId);
            if (bookingDetail == null)
                throw new UserFriendlyException("Booking không tồn tại");
            if (bookingDetail.Status != BookingStatusEnum.WaitConfirm)
            {
                throw new UserFriendlyException("Trạng thái booking không hợp lệ");
            }

            if (status == (int)BookingStatusEnum.Accepted || status == (int)BookingStatusEnum.Refused)
            {
                if (status == (int)BookingStatusEnum.Accepted)
                    bookingDetail.Status = BookingStatusEnum.Accepted;
                else
                    bookingDetail.Status = BookingStatusEnum.Cancel;
                bookingDetail.Note = note;
                //todo chỗ này xem lại cần hoàn tiền cho khách hàng
                _bookingDetailRepository.Update(bookingDetail);
            }
            else
            {
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            }
        }

        public async Task ApproveBooking(long bookingDetailId, string note)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p =>
                p.Id == bookingDetailId && p.HostUserId == hostUserId);
            if (bookingDetail == null)
                throw new UserFriendlyException("Booking không tồn tại");

            if (bookingDetail.Status != BookingStatusEnum.WaitConfirm)
            {
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            }

            bookingDetail.Status = BookingStatusEnum.Accepted;
            bookingDetail.Note = note;
            _bookingDetailRepository.Update(bookingDetail);
        }

        public async Task RefuseBooking(long bookingDetailId, string note)
        {
            long hostUserId = _commonAppService.GetHostUserId(AbpSession.UserId ?? 0);
            var bookingDetail = _bookingDetailRepository.FirstOrDefault(p =>
                p.Id == bookingDetailId && p.HostUserId == hostUserId);
            if (bookingDetail == null)
                throw new UserFriendlyException("Booking không tồn tại");

            if (bookingDetail.Status != BookingStatusEnum.WaitConfirm)
            {
                throw new UserFriendlyException("Trạng thái không hợp lệ");
            }

            bookingDetail.Note = note;
            bookingDetail.Status = BookingStatusEnum.Cancel;
            //todo chỗ này xem lại cần hoàn tiền cho khách hàng
            _bookingDetailRepository.Update(bookingDetail);
        }
    }
}
