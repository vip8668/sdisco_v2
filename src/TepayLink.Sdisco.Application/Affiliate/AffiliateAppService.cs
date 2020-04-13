using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using SDisco.Affiliate.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Affiliate.Dto;
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour;
using System.Linq;
using TepayLink.Sdisco.Utils;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Affiliate
{
    [AbpAuthorize()]
    class AffiliateAppService : SdiscoAppServiceBase, IAffiliateAppService
    {
        private readonly IRepository<Partner, long> _partnerRepository;

        private readonly IRepository<Transaction, long> _transactionRepository;
        private readonly IRepository<Product, long> _tourRepository;

        private readonly IRepository<PartnerRevenue, long> _partnerRevenueRepository;
        private readonly IRepository<ShareTransaction, long> _shareTransactionRepository;
        private readonly ICommonAppService _commonAppService;

        private readonly IRepository<WithDrawRequest, long> _withDrawRequestRepository;
        private readonly IRepository<ShortLink, long> _shortLinkRepository;





        /// <summary>
        /// Đăng ký partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreatePartner(CreatePartnerInputDto input)
        {
            var existingPartner = _partnerRepository.FirstOrDefault(p => p.UserId == AbpSession.UserId);
            if (existingPartner != null)
            {
                throw new UserFriendlyException("Bạn đã là partner");
            }

            var partner = new Partner
            {
                Name = input.Name,
                Comment = input.Comment,
                DetinationId = input.PlaceId,
                Status = 0,
                UserId = AbpSession.UserId ?? 0,
                Languages = string.Join(",", input.Languages ?? new List<int>()),
                SkypeId = input.SkypeId,
                WebsiteUrl = input.WebsiteUrl,
                AffiliateKey = "",
                AlreadyBecomeSdiscoPartner = input.AlreadyBecomeSdiscoPartner,
                HasDriverLicense = input.HasDriverLicense
            };
            _partnerRepository.Insert(partner);
        }

        /// <summary>
        /// Lây danh sách chuyến đi đã tạo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AffiliateTourDTo>> GetTripCrated(GetTripCreated input)
        {
            var query =
                (from t in
                        _tourRepository.GetAll()
                 where t.Type == ProductTypeEnum.Trip
                       && t.CreatorUserId == AbpSession.UserId
                       && t.Status == ProductStatusEnum.Publish
                 select new AffiliateTourDTo
                 {
                     Id = t.Id,

                     OfferLanguageId = t.LanguageId ?? 0,
                     Title = t.Name,


                     SoldCount = t.BookingCount,
                     TripLength = t.TripLengh,

                     IsHotDeal = t.IsHotDeal,
                     BestSaller = t.IsBestSeller,
                     ShareCount = t.ShareCount,
                     CoppyCount = t.CoppyCount,
                     ViewCount = t.ViewCount,
                 });

            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            // var itemIds = list.Select((p => p.Id)).ToList();
            // var listSaveItem = await _commonAppService.GetSaveItem(itemIds, ItemTypeEnum.Tour);

            foreach (var item in list)
            {
                var reviewItem = await _commonAppService.GetTourReviewSummary(item.Id);
                item.Review = reviewItem;
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
                //   item.IsLove = listSaveItem.FirstOrDefault(p => p.ItemId == item.Id) != null;
                item.Revenue = await _commonAppService.GetRevenueOfTour(item.Id);
                //   item.AvaiableTimes = await _commonAppService.GetAvaiableTimeOfTour(item.Id);
                //  item.ReviewCount 
            }

            return new PagedResultDto<AffiliateTourDTo>()
            {
                Items = list,
                TotalCount = total
            };
        }

        /// <summary>
        /// Create SHortlink
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<CreateShortlinkOutput> CreateShortLink(CreateShortLinkInput inputDto)
        {
            var shortlink = new ShortLink
            {
                UserId = AbpSession.UserId ?? 0,
                FullLink = inputDto.Link,
            };
            var id = _shortLinkRepository.InsertAndGetId(shortlink);
            shortlink.ShortCode = new Hashids().Encode(new[] { (int)id });
            _shortLinkRepository.Update(shortlink);
            return new CreateShortlinkOutput
            {
                ShortCode = shortlink.ShortCode
            };
        }

        /// <summary>
        /// Lấy link từ Short link
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns> Trả ra link full của 1 short linl</returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<string> GetFullLink(CheckFullLinkInput input)
        {
            var ids = new Hashids().Decode(input.ShortCode.ToUpper());
            if (ids.Length == 0)
                throw new UserFriendlyException("Link không tồn tại");
            var shortLink = _shortLinkRepository.FirstOrDefault(p => p.Id == ids[0]);
            if (shortLink == null)
                throw new UserFriendlyException("Link không tồn tại");
            return shortLink.FullLink;
        }


        public async Task<PagedResultDto<PayoutHistoryDto>> GetPayoutHistory(PayOutInputDto input)
        {
            var query = _withDrawRequestRepository.GetAll().Where(p => p.UserId == AbpSession.UserId && p.CreationTime >= input.FromDate && p.CreationTime <= input.ToDate);

            var toal = query.Count();


            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).Select(p => new PayoutHistoryDto
            {
                Amount = p.Amount,
                Date = p.CreationTime,
                Reason = "Withdraw"
            }).ToList();
            return new PagedResultDto<PayoutHistoryDto>()
            {
                Items = list,
                TotalCount = toal
            };
        }

        //Todo FIx data chưa làm
        /// <summary>
        /// Payout list 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PayoutDto> GetPayoutList(PayOutInputDto input)
        {
            var query = _partnerRevenueRepository.GetAll()
                .Where(p => p.CreationTime >= input.FromDate && p.CreationTime <= input.ToDate && p.Userid == AbpSession.UserId)
                .GroupBy(p => new { p.ProductId, p.CreationTime.Month, p.CreationTime.Year }).Select(p => new { p.Key, Point = p.Sum(x => x.Point), Money = p.Sum(x => x.Money) });

            var totalCount = query.Count();
            var totalMoney = query.Sum(p => p.Money);
            var itemList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();






            var tourIds = itemList.Select(p => p.Key.ProductId).ToList();
            var list = new List<PayoutOutputDto>();

            var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).Select(p => new Product { Id = p.Id, Name = p.Name }).ToList();
            foreach (var item in itemList)
            {
                string tripTitle = "";

                tripTitle = tours.FirstOrDefault(p => p.Id == item.Key.ProductId)?.Name;

                var payoutDto = new PayoutOutputDto
                {
                    Date = new DateTime(item.Key.Year, item.Key.Month, 1),
                    Money = (decimal)item.Money,
                    Point = item.Point,
                    TripTitle = tripTitle
                };
                list.Add(payoutDto);
            }



            return new PayoutDto()
            {
                PayoutList = new PagedResultDto<PayoutOutputDto>()
                {
                    Items = list,
                    TotalCount = totalCount
                },
                TotalMoney = (decimal)totalMoney
            };
        }

        /// <summary>
        /// Danh sách hoa hồng mới nhất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<CommissionDto>> GetLastCommission(PagedInputDto input)
        {
            var query = _shareTransactionRepository.GetAll().Where(p => p.UserId == AbpSession.UserId);
            var total = query.Count();
            query = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
            var list = query.Select(p => new CommissionDto
            {
                ItemId = p.ProductId,

                Point = p.Point,

                RevenueType = p.Type,
            }).ToList();


            var tourIds = list.Select(p => p.ItemId).ToList();



            if (tourIds.Any())
            {
                var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).ToList();
                foreach (var item in list)
                {

                    var tourItem = tours.FirstOrDefault(p => p.Id == item.ItemId);
                    if (tourItem != null)
                    {
                        item.TourTitle = tourItem.Name;
                    }

                }
            }

            return new PagedResultDto<CommissionDto>
            {
                Items = list,
                TotalCount = total
            };
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Tour có commisson mới nhất
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CommissionDto>> GetLastCommissionTour(PagedInputDto input)
        {
            var query = _shareTransactionRepository.GetAll().Where(p => p.UserId == AbpSession.UserId);
            var total = query.Count();
            query = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
            var list = query.Select(p => new CommissionDto
            {
                ItemId = p.ProductId,

                Point = p.Point,

                RevenueType = p.Type,
            }).ToList();


            var tourIds = list.Select(p => p.ItemId).ToList();


            if (tourIds.Any())
            {
                var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).ToList();
                foreach (var item in list)
                {

                    var tourItem = tours.FirstOrDefault(p => p.Id == item.ItemId);
                    if (tourItem != null)
                    {
                        item.TourTitle = tourItem.Name;
                        item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.ItemId);
                    }

                }
            }

            return new PagedResultDto<CommissionDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        /// <summary>
        /// Hot Commission Tour
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CommissionDto>> GetHostCommissionTour(PagedInputDto input)
        {
            var query = _shareTransactionRepository.GetAll().Where(p => p.UserId == AbpSession.UserId);
            var total = query.Count();
            query = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount);
            var list = query.Select(p => new CommissionDto
            {
                ItemId = p.ProductId,

                Point = p.Point,

                RevenueType = p.Type,
            }).ToList();


            var tourIds = list.Select(p => p.ItemId).ToList();



            if (tourIds.Any())
            {
                var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).ToList();
                foreach (var item in list)
                {

                    var tourItem = tours.FirstOrDefault(p => p.Id == item.ItemId);
                    if (tourItem != null)
                    {
                        item.TourTitle = tourItem.Name;
                        item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.ItemId);
                    }

                }
            }

            return new PagedResultDto<CommissionDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        public async Task<string> GetConfigTitle()
        {
            return
                $"1 share is {AppConsts.SharePoint} points, 1 copy is {AppConsts.CoppyPoint} points, 1 time of book and payment of users is {AppConsts.Booking} points. Points can be exchanged for money";
        }

        /// <summary>
        /// Lấy commission list
        /// </summary>
        /// <param name="input"></param>GetCommission
        /// <returns></returns>
        public async Task<GetCommissionOutputDto> GetCommission(GetCommissionInputDto input)
        {
            var list = _shareTransactionRepository.GetAll().Where(p =>
                p.UserId == AbpSession.UserId && p.CreationTime >= input.FromDate &&
                p.CreationTime < input.ToDate).Select(p => new PartnerRevenue
                {
                    RevenueType = p.Type,
                    Point = p.Point,
                }).ToList();

            var total = _transactionRepository.GetAll()
                .Where(p => p.UserId == AbpSession.UserId && p.TransType == TransactionType.Affiliate)
                .Sum(p => p.Amount);
            var money = _transactionRepository.GetAll()
                .Where(p => p.UserId == AbpSession.UserId && p.TransType == TransactionType.Affiliate &&
                            p.CreationTime >= input.FromDate && p.CreationTime <= input.ToDate)
                .Sum(p => p.Amount);

            var item = new GetCommissionOutputDto
            {
                Total = total,
                LastPayment = money,
                Booked = money,
                ClickPoint = list.Where(p => p.RevenueType == RevenueTypeEnum.Click).Sum(p => p.Point),
                SharePoint = list.Where(p => p.RevenueType == RevenueTypeEnum.Shared).Sum(p => p.Point),
                CoppyTripPoint = list.Where(p => p.RevenueType == RevenueTypeEnum.Coppy).Sum(p => p.Point),
            };
            return item;
        }

        /// <summary>
        /// Lấy data cho biểu đồ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<CommissionChartOutputDto>> GetCommissionChart(GetChartInputDto input)
        {
            var fromDate = new DateTime(input.FromYear, input.FromMonth, 1);
            var toDate = new DateTime(input.ToYear, input.ToMonth, 1).AddMonths(1);
            var list = _shareTransactionRepository.GetAll().Where(p =>
                p.UserId == AbpSession.UserId && (int)p.Type == input.Type && p.CreationTime >= fromDate &&
                p.CreationTime < toDate).Select(p => new CommissionChartOutputDto
                {
                    Month = p.CreationTime.Month,
                    Year = p.CreationTime.Year,
                    Value = p.Point
                }).ToList();
            var data = list.GroupBy(p => new { p.Month, p.Year })
                .Select(p => new CommissionChartOutputDto
                {
                    Month = p.Key.Month,
                    Year = p.Key.Year,
                    Value = p.Sum(q => q.Value)
                }).ToList();
            for (int i = input.FromMonth; i <= input.ToMonth; i++)
            {
                if (!data.Any(p => p.Month == i))
                    data.Add(new CommissionChartOutputDto
                    {
                        Month = i,
                        Year = input.FromYear,
                        Value = 0
                    });
            }
            data = data.OrderBy(p => p.Month).ToList();
            return data;
        }

        public async Task<PagedResultDto<PointListDetail>> GetDetail(GetPointListDetail input)
        {
            var query = _shareTransactionRepository.GetAll().Where(p =>
                    p.UserId == AbpSession.UserId && p.CreationTime >= input.FromDate &&
                    p.CreationTime <= input.ToDate && (int)p.Type == input.Type)
                .GroupBy(p => new { p.ProductId, p.CreationTime.DayOfYear })
                .Select(p => new
                {
                    ItemId = p.Key.ProductId,
              
                    DOY = p.Key.DayOfYear,
                    Count = p.Count(),
                    Total = p.Sum(x => x.Point)
                });
            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

           
            var tourIds = list.Select(p => p.ItemId).ToList();
            var resultList = new List<PointListDetail>();
            if (tourIds.Any())
            {
                var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).ToList();
                foreach (var item in list)
                {
                    var rs = new PointListDetail
                    {
                        Count = item.Count,
                        Point = (int)item.Total,
                      
                        TourTitle = tours.FirstOrDefault(p => p.Id == item.ItemId)?.Name,
                        Date = new DateTime(DateTime.Now.Year, 1, 1).AddDays(item.DOY - 1)
                    };
                    resultList.Add(rs);
                }
            }


            return new PagedResultDto<PointListDetail>()
            {
                TotalCount = total,
                Items = resultList
            };
        }

        [AbpAllowAnonymous]
        public async Task ShareTrip(long tripId)
        {
           // _commonAppService.ProcessBonus(tripId, RevenueTypeEnum.Shared);
        }


    }
}
