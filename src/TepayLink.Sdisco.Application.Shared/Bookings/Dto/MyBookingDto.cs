using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.Utils;

namespace TepayLink.Sdisco.Booking.Dtos
{
    public class MyBookingDto
    {
        /// <summary>
        /// Id Booking / trường này duy nhất
        /// </summary>
        public long BookingDetailId { get; set; }

        /// <summary>
        ///  trường này không duy nhất
        /// </summary>
        public long BookingId { get; set; }

        public string Slug
        {
            get { return this.Title.GenerateSlug(); }
        }

        /// <summary>
        /// Id Tour book
        /// </summary>
        public long ItemId { get; set; }

        public ProductTypeEnum ItemType { get; set; }
        public long ItemScheduleId { get; set; }

        /// <summary>
        /// T
        /// </summary>
        public BookingStatusEnum Status { get; set; }

        /// <summary>
        /// Tiêu để
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Giá
        /// </summary>
        public BasicPriceDto Price { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Số % được refund
        /// </summary>
        public decimal RefundPercent
        {
            get
            {
                try
                {
                    if ((DateTime.Now - StartDate).TotalDays >= AppConsts.X_DAY_1)
                    {
                        return AppConsts.REFUND_PERCENT_1;
                    }
                    else if ((DateTime.Now - StartDate).TotalDays >= AppConsts.X_DAY_2)
                    {
                        return AppConsts.REFUND_PERCENT_2;
                    }

                    return 0;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Có được refund hay không
        /// </summary>
        public bool CanRefund => RefundPercent != 0;

        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// Độ dài chuyến đi 
        /// </summary>
        public int TripLength { get; set; }

        /// <summary>
        /// Review
        /// </summary>
        public ReviewSummaryDto Review { get; set; }

        /// <summary>
        /// Tổng số chỗ của tour
        /// </summary>
        public int TotalSeat { get; set; }

        /// <summary>
        /// Số ghế đã book
        /// </summary>
        public int TotalBook { get; set; }

        public List<PhotoDto> ThumbImages { get; set; }
    }

    public class MyBookingGroupByDayDto
    {
        public DateTime Date { get; set; }
        public PagedResultDto<MyBookingDto> Data { get; set; }
    }
}