using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SDisco.Booking.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Booking.Dto;
using TepayLink.Sdisco.Booking.Dtos;
using TepayLink.Sdisco.Payment.Dto;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Bookings
{
    public interface IBookingAppService : IApplicationService
    {
        /// <summary>
        /// Lấy thời gian có thể đặt chuyến/ mua vé, Item....
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<AvaiableTourDto> GetAvaiableTimeOfTour(GetAvaiableTimeOfTourDto input);
        /// <summary>
        /// Đặt tuor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<NapasOutputDto> BookingAndPay(BookingAndPayTourDto input);

        Task<TourDetailDto> GetTourDetail(GetTourDetailInputDto input);



        Task<PagedResultDto<MyBookingDto>> GetMyBooking(MyBookingInputDto inputDto);

        Task<List<int>> GetDayBookingInMonth(BasicDateDto inputDto);

        Task UpdateBookingStatus(UpdateBookingStatusDto input);
        Task Report(ClaimInputDto claimInput);
        Task WriteReview(WriteReviewInputDto inputDto);
        Task<List<ClaimDto>> GetClaim();

        Task<CheckCouponOutputDto> CheckCouponCode(CheckCouponInputDto input);

        Task<List<AvaiableTimeDto>> GetAvaiableTimesInMonthOfTour(DateTime month, long tourId, int itemType);
        Task<BookingDetailDto> GetBookingDetail(long bookingId);
        Task<List<MyBookingGroupByDayDto>> MyBooking(MyBookingInput1Dto inputDto);
        Task<NapasOutputDto> BookingPedingTour(PayPendingBooking input);
    }
}
