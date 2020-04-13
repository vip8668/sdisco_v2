using Abp.Domain.Entities;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Authorization.Users.Profile.Dto
{
    public class RefundReasonDto : Entity
    {
        public string ReasonText { get; set; }

    }

    public class RefundMethod
    {

    }
    public class RefundDto
    {
        public long BookingDetailId { get; set; }
        public int RefundMethodId { get; set; }
    }
    public class TourOfHost : BasicTourDto
    {
        /// <summary>
        /// Doanh thu
        /// </summary>
        public decimal Revenue { get; set; }
    }
    public class GetSupperHostProfileReview : PagedInputDto
    {
        /// <summary>
        /// 1: tất cả , 0 chưa đọc
        /// </summary>
        public int ReadStatus { get; set; }


    }
    public class ReplyReviewDto
    {
        public long ReviewId { get; set; }
        public string Comment { get; set; }
    }
    public class BalanceHistoryDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public byte Side { get; set; }
        public string Note { get; set; }
        public string Policy { get; set; }
        public string Description { get; set; }
        public long ItemId { get; set; }
        public string ItemTitle { get; set; }
    }
    public class GetPaidAmountInputDto : PagedInputDto
    {
        /// <summary>
        /// 1: Paid, 0 Pending
        /// </summary>
        public int Type { get; set; }
    }

    public class GetPaidAmountOutputDto
    {
        public long BookingDetailId { get; set; }
        public long Id { get; set; }
        public ReviewSummaryDto Review { get; set; }
        public string Title { get; set; }
        public long ItemId { get; set; }
        public ProductTypeEnum Type { get; set; }
        public string Place { get; set; }
        public BasicPriceDto Price { get; set; }
        public decimal Amount { get; set; }
        public List<PhotoDto> ThumbImages { get; set; }
        public bool IsDone { get; set; }
        public int DayLeft { get; set; }
    }
    public class WithDrawDto
    {
        /// <summary>
        /// Số tiền cần rút
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// ID TK ngân hàng
        /// </summary>
        public long BankAccountId { get; set; }
    }
    public class BalanceOutputDto
    {
        public decimal Balance { get; set; }


    }
    public class ChartBalanceDto
    {
        public DateTime KeyDate { get; set; }

        public int Key { get; set; }
        public decimal Value { get; set; }
        public string CurrencySign => "$";
    }
    public class GetChartBalanceDto
    {
        // public DateTime FromDate { get; set; }

        // public  DateTime ToDate { get; set; }

        /// <summary>
        /// Loại : 1 Theo tháng
        /// 
        ///         2: Theo năm
        /// 3: tuần
        /// </summary>
        public int Type { get; set; }
    }
    public class RevenueSummaryDto
    {
        public int TotalTrip { get; set; }
        public decimal TotalRevenue { get; set; }
        public float Percent { get; set; }
    }
    public class RefundMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
 
  
    public class WithdrawOutputDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }

    }
}
