using System;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.MultiTenancy.Payments;
using TepayLink.Sdisco.MultiTenancy.Payments.Paypal;
using TepayLink.Sdisco.MultiTenancy.Payments.PayPal;
using TepayLink.Sdisco.Payment;
using TepayLink.Sdisco.Web.Models.Paypal;

namespace TepayLink.Sdisco.Web.Controllers
{
    public class PayPalController : SdiscoControllerBase
    {
        private readonly PayPalPaymentGatewayConfiguration _payPalConfiguration;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IPayPalPaymentAppService _payPalPaymentAppService;
        private readonly Payment.IPaymentAppService _paymentAppService;
        private readonly IRepository<Bookings.Order, long> _orderRepository;

        public PayPalController(
            PayPalPaymentGatewayConfiguration payPalConfiguration,
            ISubscriptionPaymentRepository subscriptionPaymentRepository, 
            IPayPalPaymentAppService payPalPaymentAppService,
            Payment.IPaymentAppService paymentAppService,
             IRepository<Bookings.Order, long> orderRepository)
        {
            _payPalConfiguration = payPalConfiguration;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _payPalPaymentAppService = payPalPaymentAppService;
            _payPalConfiguration = payPalConfiguration;
            _paymentAppService = paymentAppService;
            _orderRepository = orderRepository;
        }

        public async Task<ActionResult> Purchase(long paymentId, string successUrl, string failUrl)
        {
            var uraaal = HttpContext.Session.GetString("successUrl" + paymentId);
            Console.WriteLine(uraaal);
            var order = _orderRepository.FirstOrDefault(p => p.Id == paymentId);
            if (order == null || order.Status != OrderStatus.Init)
            {
                var url = failUrl + (failUrl.Contains("?") ? "&" : "?") + "failmessage=" + "Đơn hàng không tồn tại hoặc đã được thanh toán";
                return Redirect(url);
            }

            HttpContext.Session.SetString("successUrl" + paymentId, successUrl);
            HttpContext.Session.SetString("failUrl" + paymentId, failUrl);




            var model = new PayPalPurchaseViewModel
            {
                PaymentId = order.Id,
                Amount = order.Amount,
                Description = order.Note,
                Configuration = _payPalConfiguration
            };
            return View(model);
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<ActionResult> ConfirmPayment(long paymentId, string paypalPaymentId, string paypalPayerId)
        {
            try
            {
                await _paymentAppService.ConfirmPaymentPaypal(paymentId, paypalPaymentId, paypalPayerId);

                var returnUrl = await GetSuccessUrlAsync(paymentId);
                return Redirect(returnUrl);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message, exception);

                var returnUrl = await GetErrorUrlAsync(paymentId);
                return Redirect(returnUrl);
            }
        }

        private async Task<string> GetSuccessUrlAsync(long paymentId)
        {
            var url = HttpContext.Session.GetString("successUrl" + paymentId);
            var order = _orderRepository.FirstOrDefault(p => p.Id == (paymentId));
            return url + (url.Contains("?") ? "&" : "?") + "bookingid=" + order?.BookingId;
        }

        private async Task<string> GetErrorUrlAsync(long paymentId)
        {
            var url = HttpContext.Session.GetString("failUrl" + paymentId);
            var order = _orderRepository.FirstOrDefault(p => p.Id == (paymentId));

            return url + (url.Contains("?") ? "&" : "?") + "bookingid=" + order?.BookingId + "&failmessage=Giao dịch không thành công";
        }
    }
}