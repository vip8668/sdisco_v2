using Abp.Application.Services;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Products;

namespace TepayLink.Sdisco.Bookings
{
    public class OrderAppService : SdiscoAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Order, long> _orderRepository;
        private readonly IRepository<Booking, long> _bookingRepository;
        private readonly IRepository<BookingDetail, long> _bookingDetailRepository;
        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<ProductSchedule, long> _productScheduleRepository;
 



        public async Task<Dto.OrderDto> CreateOrder(CreateOrderInputDto input)
        {
            var order = new Order
            {
                Status = OrderStatus.Init,
                Amount = input.Amount,
                Note = input.Note,
                OrderType = OrderTypeEnum.AddCard,
                UserId = input.UserId,
                Currency = input.Currency,
                OrderCode = DateTime.Now.ToString("ddMMyyyyHHmmssfff"),
                BookingId = input.BookingId
            };
            order.Id = _orderRepository.InsertAndGetId(order);
            return new Dto.OrderDto
            {
                Amount = input.Amount,
                Id = order.Id,
                Status = order.Status,
                OrderCode = order.OrderCode,
                OrderType = order.OrderType
            };
        }


        public async Task ConfirmOrder(ConfirmOrderDto input)
        {
            var order = _orderRepository.FirstOrDefault(p => p.Id == input.Id);
            if (order != null)
            {
                order.Status = input.Status;
                _orderRepository.Update(order);
            }
        }




        public async Task Refund()
        {

        }

        [RemoteService(false)]
        public async Task UpdteOrderPaySucess1(long orderId)
        {
            var order = _orderRepository.FirstOrDefault(p => p.Id == orderId);
            var bookingId = order.BookingId;

            var booking = _bookingRepository.FirstOrDefault(p => p.Id == bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatusEnum.WaitConfirm;
                _bookingRepository.Update(booking);
                var bookingDetails =
                    _bookingDetailRepository.GetAll().Where(p => p.BookingId == booking.Id);





                var productIds= bookingDetails
                    .Select(p => p.ProductId).ToList();

                var tour = _tourRepository.GetAll().Where(p => productIds.Contains(p.Id)).Select(a => new
                {
                    a.Id,
                    a.InstantBook
                }).ToList();

                

                foreach (var bookingDetail in bookingDetails)
                {
                    //check Instant Book 
                    //var itemList = bookingDetail.ItemType == ItemTypeEnum.TourItem ? tourItem : tour;
                    var checkItem = tour.FirstOrDefault(p => p.Id == booking.ProductId);

                    bookingDetail.Status = checkItem?.InstantBook == true
                        ? BookingStatusEnum.Accepted
                        : BookingStatusEnum.WaitConfirm;
                    _bookingDetailRepository.Update(bookingDetail);
                }
                //todo update booking count and revenue;
            }

        }

        [RemoteService(false)]
        public async Task UpdteOrderPaySucess(long orderId)
        {
            var order = _orderRepository.FirstOrDefault(p => p.Id == orderId);
            var bookingId = order.BookingId;

            var booking = _bookingRepository.FirstOrDefault(p => p.Id == bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatusEnum.WaitConfirm;
                _bookingRepository.Update(booking);
                var bookingDetails =
                    _bookingDetailRepository.GetAll().Where(p => p.BookingId == booking.Id);

                var tourScheduleIds = bookingDetails.Select(p => p.ProductScheduleId);
                

                var tourSchedules = _productScheduleRepository.GetAll().Where(p => tourScheduleIds.Contains(p.Id));
              //  var itemSchedules = _itemScheduleRepository.GetAll().Where(p => itemScheduleIds.Contains(p.Id));
                var tourIds = bookingDetails
                    .Select(p => p.ProductId).ToList();
                
                var tours = _tourRepository.GetAll().Where(p => tourIds.Contains(p.Id)).ToList();
                var toursTmp = tours.Select(p => new { p.Id, p.InstantBook });

               
                foreach (var bookingDetail in bookingDetails)
                {
                    //check Instant Book 
                  
                    var checkItem = toursTmp.FirstOrDefault(p => p.Id == bookingDetail.ProductId);

                    bookingDetail.Status = checkItem?.InstantBook == true
                        ? BookingStatusEnum.Accepted
                        : BookingStatusEnum.WaitConfirm;
                    _bookingDetailRepository.Update(bookingDetail);
                }


                foreach (var item in tourSchedules)
                {
                    var bookingDetail = bookingDetails.FirstOrDefault(p => p.ProductScheduleId == item.Id );
                    var tour = tours.FirstOrDefault(p => p.Id == item.ProductId);
                    if (tour != null)
                    {
                        tour.BookingCount += bookingDetail.Quantity;
                     //   tour.TotalRevenew += bookingDetail.Amount;
                        item.LockedSlot -= bookingDetail.Quantity;
                        item.TotalBook += bookingDetail.Quantity;
                        item.Revenue += bookingDetail.Amount;
                        _tourRepository.Update(tour);
                        _productScheduleRepository.Update(item);
                    }
                }
              
                //if (booking.BookingType == ItemTypeEnum.Tour)
                //{
                //    var tour = _tourRepository.GetAll().FirstOrDefault(p => p.Id == booking.ItemId);
                //    //todo tính hoa hồng booking.
                //}

            }

        }
        [RemoteService(false)]
        public async Task UpdteOrderPayFail(long orderId)
        {
            //todo rollback mã ưu đãi

            // unblock lockedSlot;

        }

    }
}
