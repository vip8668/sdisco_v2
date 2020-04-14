using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users.Profile.Dto;

using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Authorization.Users.Profile
{
    public interface ISupperHostProfile : IApplicationService
    {
        /// <summary>
        /// My booking
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        Task<PagedResultDto<MyBookingDto>> GetMyBooking(MyBookingInputDto inputDto);

        /// <summary>
        /// refund reason
        /// </summary>
        /// <returns></returns>
        Task<List<RefundReasonDto>> GetRefundReason();

        /// <summary>
        /// refund
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Refund(RefundDto input);

        /// <summary>
        /// My Tour
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TourOfHost>> GetMyTour(PagedInputDto input);


        /// <summary>
        /// Reivew list
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ReviewDetailDto>> GetReview(GetSupperHostProfileReview input);

        Task ReadReview(long reviewId);
        Task<List<ReviewDetailDto>> GetReply(long reviewId);

        /// <summary>
        /// Reply review
        /// </summary>
        /// <param name="reviewDto"></param>
        /// <returns></returns>
        Task ReplyReview(ReplyReviewDto reviewDto);

        /// <summary>
        /// Balance History
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<BalanceHistoryDto>> GetBalanceHistory(PagedInputDto input);

        //PaidAmount + pending amount

        Task<PagedResultDto<GetPaidAmountOutputDto>> GetPaidAmount(GetPaidAmountInputDto input);
        //Chart

        //withdraw

        Task<long> WithDrawMoney(WithDrawDto input);
        Task CancelDrawMoney(long id);

        //Get Balance
        Task<BalanceOutputDto> GetBalance();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<ChartBalanceDto>> GetChartBalance(GetChartBalanceDto input);

        Task<RevenueSummaryDto> GetRevenueSummary();

        Task<List<RefundMethodDto>> GetRefundMethod();
        Task<List<int>> GetDayBookingInMonth(BasicDateDto inputDto);
        Task<List<MyBookingGroupByDayDto>> MyBooking(MyBookingInput1Dto inputDto);

        Task DonePayment(long bookingDetailId);

        Task<PagedResultDto<WithdrawOutputDto>> GetPendingWithdraw(PagedInputDto input);

        Task ApproveBooking(long bookingDetailId, string note);
        Task RefuseBooking(long bookingDetailId, string note);
        Task UpdateBookingStatus(long bookingDetailId, int status, string note);
    }
}
