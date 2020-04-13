using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.AdminConfig;
using TepayLink.Sdisco.Banks.Dtos;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.MultiTenancy.Payments.Paypal;
using TepayLink.Sdisco.MultiTenancy.Payments.PayPal;
using TepayLink.Sdisco.Payment.Dto;
using TepayLink.Sdisco.Products;

namespace TepayLink.Sdisco.Payment
{
    [AbpAuthorize]
    public class PaymentAppService : SdiscoAppServiceBase, IPaymentAppService
    {
        private readonly IRepository<Bank> _bankRepository;
       // private readonly IRepository<PaymentCardInfo, long> _paymentCardInfoRepository;
        private readonly IRepository<Bookings.Order, long> _orderRepository;

        private readonly IOrderAppService _orderAppService;
        private readonly IRepository<Bookings.Booking, long> _bookingRepository;
        private readonly IRepository<Product, long> _tourRepository;
     
        private readonly IRepository<BookingDetail, long> _bookingDetailRepository;
        private readonly IConfigurationRoot _configuration;



        private readonly PayPalGatewayManager _payPalGatewayManager;

        private readonly PayPalPaymentGatewayConfiguration _payPalPaymentGatewayConfiguration;


        private readonly IPayPalPaymentAppService _payPalPaymentAppService;



        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<PaymentMethodDto>> GetPaymentMethod()
        {
            //var items = (from p in _paymentCardInfoRepository.GetAll()
            //    join q in _bankRepository.GetAll() on p.BankId equals q.Id
            //    where p.UserId == AbpSession.UserId
            //    select new PaymentMethodDto
            //    {
            //        Id = p.Id,
            //        CardName = p.CardName,
            //        Logo = q.Logo,
            //        CardImage = q.CardImage,
            //        IsDefault = p.IsDefault,
            //        HintCardNumber = p.HintCardNumber,
            //        ExpiredDate = p.ExpiryDate,
            //        CardType = p.CardType
            //    }).ToList();
            var items = new List<PaymentMethodDto>();
            items.Add(new PaymentMethodDto
            {
                Id = 0,
                Logo = "https://www.paypalobjects.com/digitalassets/c/website/logo/full-text/pp_fc_hl.svg",
                CardType = BankTypeEnum.Paypal,
            });
            items.Add(new PaymentMethodDto
            {
                Id = 0,
                Logo = "http://naptiendienthoai365.com/wp-content/uploads/2018/10/Logo-VTC-Pay-xanh-vi.png",
                CardType = BankTypeEnum.VTC,
            });

            return items;
        }

        /// <summary>
        /// Xóa phương thức thanh toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeletePaymentMethod(int id)
        {
          //  _paymentCardInfoRepository.Delete(p => p.Id == id);
        }

        /// <summary>
        /// Thêm mới phương thức thanh toán (Kết quả trả về là 1 url của trang thanh toán  redirect sang trang đó)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<NapasOutputDto> AddPaymentMethod(AddPaymentMethodInputDto input)
        {
            return null;
            //string cardTypeSchemer = input.CardType.ToString("G");
            ////Create Order
            //OrderDto order = await _orderAppService.CreateOrder(new CreateOrderInputDto
            //{
            //    OrderType = OrderTypeEnum.AddCard,
            //    Amount = 5000,
            //    Note = "Thêm thẻ thanh toán",
            //    UserId = AbpSession.UserId ?? 0,
            //    Currency = "VND"
            //});
            //if (order == null)
            //{
            //    throw new UserFriendlyException("99", "Có lỗi trong quá trình xử lý");
            //}

            ////Create Add object
            //var url = string.Format(_configuration["Payment:Napas:PayAddTokenUrl"].ToString(),
            //    order.Id.ToString("00000000000000"), order.Amount, order.OrderCode, cardTypeSchemer,
            //    WebUtility.UrlEncode(input.SuccessUrl), WebUtility.UrlEncode(input.FailUrl));
            //return new NapasOutputDto
            //{
            //    RedirectUrl = _configuration["Payment:Napas:BaseUrl"].ToString() + url
            //};
        }

        /// <summary>
        /// Thanh toán đơn hàng (Kết quả trả về là 1 url của trang thanh toán  redirect sang trang đó)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public async Task<NapasOutputDto> PaymentOrder(PaymentInputDto input)
        {
            var order = _orderRepository.FirstOrDefault(p => p.Id == input.OrderId);
            //todo check đơn hàn quá hạn



            if (order == null)
            {
                throw new UserFriendlyException("Đơn hàng không tồn tại");
            }

            if (order.Status != OrderStatus.Init)
            {
                throw new UserFriendlyException("Đơn hàng đã được xử lý");
            }

            if ((DateTime.Now - order.CreationTime).TotalMinutes > 10)
            {
                throw new UserFriendlyException("Đơn hàng quá hạn xử lý");
            }

            //if (input.CardId != 0)
            //{
            //    var card = _paymentCardInfoRepository.FirstOrDefault(p =>
            //        p.Id == input.CardId && p.UserId == AbpSession.UserId);
            //    if (card == null)
            //    {
            //        throw new UserFriendlyException("Thẻ thanh toán không hợp lệ");
            //    }

            //    var url = string.Format(_configuration["Payment:Napas:PayWithToken"].ToString(),
            //        order.Id.ToString("00000000000000"), order.Amount.ToString("##"), order.OrderCode,
            //        card.CardType.ToString("G"), card.Token, WebUtility.UrlEncode(input.SuccessUrl),
            //        WebUtility.UrlEncode(input.FailUrl));
            //    return new NapasOutputDto
            //    {
            //        RedirectUrl = _configuration["Payment:Napas:BaseUrl"].ToString() + url
            //    };
            //}
            else if (input.CardType == BankTypeEnum.Paypal)
            {
                var url = string.Format(_configuration["Payment:PayPal:PayUrl"].ToString(),
                    order.Id, WebUtility.UrlEncode(input.SuccessUrl),
                    WebUtility.UrlEncode(input.FailUrl));
                return new NapasOutputDto
                {
                    RedirectUrl = _configuration["Payment:PayPal:Url"].ToString() + url
                };

                // _payPalPaymentAppService.ConfirmPayment()
            }
            else if (input.CardType == BankTypeEnum.VTC)
            {
                var url = string.Format(_configuration["Payment:VTC:PayUrl"].ToString(),
                      order.Id, WebUtility.UrlEncode(input.SuccessUrl),
                      WebUtility.UrlEncode(input.FailUrl));
                return new NapasOutputDto
                {
                    RedirectUrl = _configuration["Payment:VTC:Url"].ToString() + url
                };
            }

            return null;
        }

        [AbpAllowAnonymous]
        [UnitOfWork]
        public async Task CreateUpdatePayment(PaymentResultDto paymentResult)
        {
           //// PaymentCardInfo paymentCardInfo = null;
           // Order order = null;
           // if (paymentResult.tokenResult != null && paymentResult.tokenResult.result == PaymentResultDto.CODE_SUCCESS)
           // {
           //     if (paymentResult.tokenResult.response.gatewayCode == PaymentResultDto.CODE_SUCCESS)
           //     {
           //         //Update or create Token
           //         //Get Order
           //         order = _orderRepository.FirstOrDefault(p =>
           //             p.Id == long.Parse(paymentResult.tokenResult.deviceId));

           //         if (order != null)
           //         {
           //             var bank = _bankRepository.GetAll()
           //                 .FirstOrDefault(p => p.BankCode == paymentResult.tokenResult.card.brand);
           //             paymentCardInfo = new PaymentCardInfo
           //             {
           //                 BankId = bank != null ? bank.Id : 0,
           //                 BankCode = paymentResult.tokenResult.card.brand,
           //                 CardName = paymentResult.tokenResult.card.nameOnCard,
           //                 CardIssuer = paymentResult.tokenResult.card.scheme,
           //                 ExpiryDate = paymentResult.tokenResult.card.issueDate,
           //                 HintCardNumber = paymentResult.tokenResult.card.number,
           //                 CardType = bank != null ? bank.Type : BankTypeEnum.CreditCard,
           //                 Token = paymentResult.tokenResult.token,
           //                 Status = 1,
           //                 UserId = order.UserId,
           //                 RefOrderID = order.OrderCode,
           //             };
           //             paymentCardInfo.Id = _paymentCardInfoRepository.InsertAndGetId(paymentCardInfo);
           //         }
           //     }
           // }

           // if (paymentResult.paymentResult != null)
           // {
           //     if (order == null)
           //         order = _orderRepository.FirstOrDefault(p =>
           //             p.Id == long.Parse(paymentResult.paymentResult.order.id));
           //     if (order != null && order.Status == OrderStatusEnum.Init)
           //     {
           //         if (paymentResult.paymentResult.result == PaymentResultDto.CODE_ERROR)
           //         {
           //             order.Status = OrderStatusEnum.Fail;
           //             _orderAppService.UpdteOrderPaySucess(order.Id);
           //         }
           //         else if (paymentResult.paymentResult.result == PaymentResultDto.CODE_PENDING)
           //         {
           //             order.Status = OrderStatusEnum.Pending;
           //         }
           //         else if (paymentResult.paymentResult.result == PaymentResultDto.CODE_SUCCESS)
           //         {
           //             order.Status = OrderStatusEnum.Success;
           //             order.OrderRef = paymentResult.paymentResult.transaction.id;
           //             order.TransactionId = paymentResult.paymentResult.transaction.acquirer.transactionId;
           //             order.NameOnCard = paymentResult.paymentResult.sourceOfFunds.provided.card.nameOnCard;
           //             order.IssueDate = paymentResult.paymentResult.sourceOfFunds.provided.card.issueDate;
           //             order.BankCode = paymentResult.paymentResult.sourceOfFunds.provided.card.brand;
           //             order.CardNumber = paymentResult.paymentResult.sourceOfFunds.provided.card.number;
           //             if (paymentCardInfo != null)
           //                 order.CardId = paymentCardInfo.Id;
           //             _orderAppService.UpdteOrderPaySucess(order.Id);
           //         }
           //         _orderRepository.Update(order);
           //     }
           // }
        }
        [RemoteService(false)]
        [AbpAllowAnonymous]
        [UnitOfWork]
        public async Task ConfirmPaymentPaypal(long paymentId, string paypalPaymentId, string paypalPayerId)
        {
            var order = _orderRepository.FirstOrDefault(p => p.Id == paymentId);

            await _payPalGatewayManager.CaptureOrderAsync(
                new PayPalCaptureOrderRequestInput(paymentId.ToString())
            );
            order.Status = OrderStatus.Success;
            order.OrderRef = paypalPaymentId;
            order.TransactionId = paypalPayerId;
            order.NameOnCard = paypalPayerId;
            order.BankCode = "PAYPAL";
            order.CardNumber = paypalPayerId;
            _orderRepository.Update(order);
            _orderAppService.UpdteOrderPaySucess(order.Id);
        }

        [RemoteService(false)]
        [AbpAllowAnonymous]
        [UnitOfWork]
        public async Task ConfirmPaymentVtc(string ordercode, string transerf, string paymentType,
            OrderStatus status)
        {
            var order = _orderRepository.FirstOrDefault(p => p.OrderCode == ordercode);
            order.Status = status;
            order.OrderRef = transerf;
            order.TransactionId = transerf;
            order.NameOnCard = "";
            order.BankCode = paymentType;
            order.CardNumber = "";
            _orderRepository.Update(order);
            if (status == OrderStatus.Success)
                _orderAppService.UpdteOrderPaySucess(order.Id);

            if (status == OrderStatus.Fail)
            {
                _orderAppService.UpdteOrderPayFail(order.Id);

            }
        }

    }
}
