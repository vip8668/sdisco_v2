using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Editions;
using TepayLink.Sdisco.MultiTenancy;
using TepayLink.Sdisco.MultiTenancy.Dto;
//using TepayLink.Sdisco.MultiTenancy.Payments;
//using TepayLink.Sdisco.MultiTenancy.Payments.Dto;
using TepayLink.Sdisco.Url;
using TepayLink.Sdisco.Web.Models.Payment;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Authorization.Roles;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Identity;
using Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using TepayLink.Sdisco.Utils;
using Abp.Auditing;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Linq;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Payment;
using TepayLink.Sdisco.MultiTenancy.Payments;
using TepayLink.Sdisco.Configuration;

namespace TepayLink.Sdisco.Web.Controllers
{
    public class PaymentController : SdiscoControllerBase
    {
        private readonly Payment.IPaymentAppService _paymentAppService;
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly IWebUrlService _webUrlService;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly UserClaimsPrincipalFactory<User, Role> _userClaimsPrincipalFactory;
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;


        private readonly IRepository<Bookings.Order, long> _orderRepository;
        IAbpSession _session;
        private readonly IConfigurationRoot _configuration;

        private static int[] FAILS = new[] { -1, -9, -3, -4, -5, -6, -7, -8, -22, -24, -25, -28, -29, };
        private static int[] SUCCESS = new[] { 1 };
        private static int[] NEED_CHECK = new[] { 0, 7, 99, -21, -23, };



        public PaymentController(
            Payment.IPaymentAppService paymentAppService,
            ITenantRegistrationAppService tenantRegistrationAppService,
            TenantManager tenantManager,
            EditionManager editionManager,
            IWebUrlService webUrlService,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            UserClaimsPrincipalFactory<User, Role> userClaimsPrincipalFactory,
            UserManager userManager,
            SignInManager signInManager,
            IRepository<Bookings.Order, long> orderRepository,
            IWebHostEnvironment hostingEnvironment,
              IAbpSession session)
        {
            _paymentAppService = paymentAppService;
            _tenantRegistrationAppService = tenantRegistrationAppService;
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _webUrlService = webUrlService;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            orderRepository = _orderRepository;
            _configuration = hostingEnvironment.GetAppConfiguration();
            _session = session;

        }

        public async Task<IActionResult> Buy(int tenantId, int editionId, int? subscriptionStartType, int? editionPaymentType)
        {
            return null;
            //SetTenantIdCookie(tenantId);

            //var edition = await _tenantRegistrationAppService.GetEdition(editionId);

            //var model = new BuyEditionViewModel
            //{
            //    Edition = edition,
            //    PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            //};

            //if (editionPaymentType.HasValue)
            //{
            //    model.EditionPaymentType = (EditionPaymentType)editionPaymentType.Value;
            //}

            //if (subscriptionStartType.HasValue)
            //{
            //    model.SubscriptionStartType = (SubscriptionStartType)subscriptionStartType.Value;
            //}

            //return View("Buy", model);
        }

        public async Task<IActionResult> Upgrade(int upgradeEditionId)
        {
            return null;
            //if (!AbpSession.TenantId.HasValue)
            //{
            //    throw new ArgumentNullException();
            //}

            //SubscriptionPaymentType subscriptionPaymentType;

            //using (CurrentUnitOfWork.SetTenantId(null))
            //{
            //    var tenant = await _tenantManager.GetByIdAsync(AbpSession.GetTenantId());
            //    subscriptionPaymentType = tenant.SubscriptionPaymentType;

            //    if (tenant.EditionId.HasValue)
            //    {
            //        var currentEdition = await _editionManager.GetByIdAsync(tenant.EditionId.Value);
            //        if (((SubscribableEdition)currentEdition).IsFree)
            //        {
            //            var upgradeEdition = await _editionManager.GetByIdAsync(upgradeEditionId);
            //            if (((SubscribableEdition)upgradeEdition).IsFree)
            //            {
            //                await _paymentAppService.SwitchBetweenFreeEditions(upgradeEditionId);
            //                return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
            //            }

            //            return RedirectToAction("Buy", "Payment", new
            //            {
            //                tenantId = AbpSession.GetTenantId(),
            //                editionId = upgradeEditionId,
            //                editionPaymentType = (int)EditionPaymentType.BuyNow
            //            });
            //        }

            //        if (!await _paymentAppService.HasAnyPayment())
            //        {
            //            return RedirectToAction("Buy", "Payment", new
            //            {
            //                tenantId = AbpSession.GetTenantId(),
            //                editionId = upgradeEditionId,
            //                editionPaymentType = (int)EditionPaymentType.Upgrade
            //            });
            //        }
            //    }
            //}

            //var paymentInfo = await _paymentAppService.GetPaymentInfo(new PaymentInfoInput { UpgradeEditionId = upgradeEditionId });

            //if (paymentInfo.IsLessThanMinimumUpgradePaymentAmount())
            //{
            //    await _paymentAppService.UpgradeSubscriptionCostsLessThenMinAmount(upgradeEditionId);
            //    return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
            //}
            //var edition = await _tenantRegistrationAppService.GetEdition(upgradeEditionId);

            //var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(
            //    tenantId: AbpSession.GetTenantId(),
            //    gateway: null,
            //    isRecurring: null);

            //var model = new UpgradeEditionViewModel
            //{
            //    Edition = edition,
            //    AdditionalPrice = paymentInfo.AdditionalPrice,
            //    SubscriptionPaymentType = subscriptionPaymentType,
            //    PaymentPeriodType = lastPayment.GetPaymentPeriodType()
            //};

            //if (subscriptionPaymentType.IsRecurring())
            //{
            //    model.PaymentGateways = new List<PaymentGatewayModel>
            //    {
            //        new PaymentGatewayModel
            //        {
            //            GatewayType = lastPayment.Gateway,
            //            SupportsRecurringPayments = true
            //        }
            //    };
            //}
            //else
            //{
            //    model.PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput());
            //}

            //return View("Upgrade", model);
        }




        public async Task<ActionResult> VtcPayment(long paymentId, string successUrl, string failUrl)
        {


            var order = _orderRepository.FirstOrDefault(p => p.Id == paymentId);
            if (order == null || order.Status != Bookings.OrderStatus.Init)
            {
                var url = failUrl + (failUrl.Contains("?") ? "&" : "?") + "failmessage=" +
                         WebUtility.UrlEncode("Đơn hàng không tồn tại hoặc đã được thanh toán");
                return Redirect(url);
            }

            HttpContext.Session.SetString("successUrl" + order.OrderCode, successUrl);
            HttpContext.Session.SetString("failUrl" + order.OrderCode, failUrl);


            string baseUrl = _configuration["Payment:VTC:BaseUrl"];
            string Security_Key =
                _configuration["Payment:VTC:SecretKey"]; // Key bảo mật dùng trong chuỗi tạo chữ ký

            string amount = ((int)order.Amount).ToString();
            string currency = "USD";
            string receiver_account =
                _configuration["Payment:VTC:ReceiveAccount"]; // Tài khoản hứng tiền của đối tác tại VTC
            string
                reference_number =
                    order.OrderCode; // Mã đơn hàng của đối tác, VTC và đối tác dùng đơn hàng này làm cơ sở đối soát
            string transaction_type = "sale";
            string website_id = _configuration["Payment:VTC:WebsiteCode"];


            string plaintext = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", amount, currency, receiver_account,
                reference_number, transaction_type, website_id, Security_Key);
            string signature = StringUtils.SHA256encrypt(plaintext);
            string listparam = string.Format(
                "website_id={0}&amount={1}&receiver_account={2}&reference_number={3}&currency={4}&signature={5}&transaction_type={6}",
                website_id, amount, receiver_account, reference_number, currency, signature, transaction_type
            );

            string urlRedirect = string.Format("{0}?{1}", baseUrl, listparam);
            return Redirect(urlRedirect);
        }

        [Audited]
        public async Task<ActionResult> VtcPaymentDone()
        {
            string Security_Key = _configuration["Payment:VTC:SecretKey"];

            string merchantSign = string.Empty;
            bool isVerify = false;
            string websitecode = _configuration["Payment:VTC:WebsiteCode"];
            if (Request.Method.ToUpper() == "POST")
            {

                var data = HttpContext.Request.Form["data"].ToString();
                var sign = HttpContext.Request.Form["signature"].ToString();
                Logger.Info("===> VTC payment return  data : " + data);
                Logger.Info("===> VTC payment return  sign : " + sign);
                //   Logger.Info("paymet return ")
                merchantSign = StringUtils.SHA256encrypt(data + "|" + Security_Key);

                isVerify = merchantSign == sign;
                var arr = data.Split('|');
                if (arr[6] != websitecode)
                    return null;
                if (isVerify)
                {
                    //amount|message|payment_type|reference_number| status|trans_ref_no|website_id
                    Bookings.OrderStatus orderStatus = Bookings.OrderStatus.Unknow;
                    //Thành công
                    if (SUCCESS.Contains(int.Parse(arr[4])))
                    {
                        orderStatus = OrderStatus.Success;
                    }

                    else if (FAILS.Contains(int.Parse(arr[4])))
                    {
                        orderStatus = OrderStatus.Fail;
                    }
                    else
                    {
                        orderStatus = OrderStatus.Unknow;
                    }

                    _paymentAppService.ConfirmPaymentVtc(arr[3], arr[5], arr[2], orderStatus);
                }
                return Json("ok");
            }

            // Lay cac ket qua tra ve v1.5
            double amount = Convert.ToDouble(Request.Query["amount"]);
            string message = Request.Query["message"];
            string payment_type = Request.Query["payment_type"];
            string reference_number = Request.Query["reference_number"];
            int status = Convert.ToInt32(Request.Query["status"]);
            string trans_ref_no = Request.Query["trans_ref_no"];
            int website_id = Convert.ToInt32(Request.Query["website_id"]);
            string signature = WebUtility.HtmlDecode(Request.Query["signature"].ToString().Replace(" ", "+"));

            if (website_id != int.Parse(websitecode))
            {
                string failUrl = await GetErrorUrlAsync(reference_number, "Giao dịch không thành công");

                return Redirect(failUrl);
            }

            object[] arrParamReturn = new object[]
                {amount, message, payment_type, reference_number, status, trans_ref_no, website_id};
            string textSign = string.Join("|", arrParamReturn) + "|" + Security_Key;

            merchantSign = StringUtils.SHA256encrypt(textSign);
            isVerify = (merchantSign == signature);


            if (isVerify)
            {

                if (SUCCESS.Contains(status))
                {
                    //  _paymentAppService.ConfirmPaymentVtc(reference_number, trans_ref_no, payment_type, OrderStatusEnum.Success);
                    string successUrl = await GetSuccessUrlAsync(reference_number);

                    return Redirect(successUrl);
                }

                else if (FAILS.Contains(status))
                {
                    //  _paymentAppService.ConfirmPaymentVtc(reference_number, trans_ref_no, payment_type, OrderStatusEnum.Fail);
                    string failUrl = await GetErrorUrlAsync(reference_number, "Giao dịch không thành công");

                    return Redirect(failUrl);
                }
                else
                {
                    // _paymentAppService.ConfirmPaymentVtc(reference_number, trans_ref_no, payment_type, OrderStatusEnum.Unknow);
                    string failUrl = await GetErrorUrlAsync(reference_number, "Giao dịch chưa rõ kết quả");
                    return Redirect(failUrl);
                }

            }
            else
            {
                string failUrl = await GetErrorUrlAsync(reference_number, "Giao dịch không thành công");
                _paymentAppService.ConfirmPaymentVtc(reference_number, trans_ref_no, payment_type, OrderStatus.Fail);
                return Redirect(failUrl);
            }



        }



        public async Task<IActionResult> Extend(int upgradeEditionId, EditionPaymentType editionPaymentType)
        {
            return null;
            //var edition = await _tenantRegistrationAppService.GetEdition(upgradeEditionId);

            //var model = new ExtendEditionViewModel
            //{
            //    Edition = edition,
            //    PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput())
            //};

            //return View("Extend", model);
        }

        [HttpPost]
        public async Task<JsonResult> CreatePayment(CreatePaymentModel model)
        {
            return null;
            //var paymentId = await _paymentAppService.CreatePayment(new CreatePaymentDto
            //{
            //    PaymentPeriodType = model.PaymentPeriodType,
            //    EditionId = model.EditionId,
            //    EditionPaymentType = model.EditionPaymentType,
            //    RecurringPaymentEnabled = model.RecurringPaymentEnabled.HasValue && model.RecurringPaymentEnabled.Value,
            //    SubscriptionPaymentGatewayType = model.Gateway,
            //    SuccessUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/" + model.EditionPaymentType + "Succeed",
            //    ErrorUrl = _webUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "Payment/PaymentFailed"
            //});

            //return Json(new AjaxResponse
            //{
            //    TargetUrl = Url.Action("Purchase", model.Gateway.ToString(), new
            //    {
            //        paymentId = paymentId,
            //        isUpgrade = model.EditionPaymentType == EditionPaymentType.Upgrade
            //    })
            //});
        }

        [HttpPost]
        public async Task CancelPayment(CancelPaymentModel model)
        {
            //await _paymentAppService.CancelPayment(new CancelPaymentDto
            //{
            //    Gateway = model.Gateway,
            //    PaymentId = model.PaymentId
            //});
        }

        public async Task<IActionResult> BuyNowSucceed(long paymentId)
        {
            return null;
            //await _paymentAppService.BuyNowSucceed(paymentId);

            //return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
        }

        public async Task<IActionResult> NewRegistrationSucceed(long paymentId)
        {
            return null;
            //await _paymentAppService.NewRegistrationSucceed(paymentId);

            //await LoginAdminAsync();

            //return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
        }

        public async Task<IActionResult> UpgradeSucceed(long paymentId)
        {
            return null;
            //await _paymentAppService.UpgradeSucceed(paymentId);

            //return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
        }

        public async Task<IActionResult> ExtendSucceed(long paymentId)
        {
            return null;
            //await _paymentAppService.ExtendSucceed(paymentId);

            //return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
        }

        public async Task<IActionResult> PaymentFailed(long paymentId)
        {
            return null;
            //await _paymentAppService.PaymentFailed(paymentId);

            //if (AbpSession.UserId.HasValue)
            //{
            //    return RedirectToAction("Index", "SubscriptionManagement", new { area = "Admin" });
            //}

            //return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        private async Task LoginAdminAsync()
        {
            var user = await _userManager.GetAdminAsync();
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(principal.Identity as ClaimsIdentity, false);
        }

        public IActionResult PaymentCompleted()
        {
            return View();
        }

        private async Task<string> GetSuccessUrlAsync(string paymentId)
        {
            var url = HttpContext.Session.GetString("successUrl" + paymentId);

            var order = _orderRepository.FirstOrDefault(p => p.Id == long.Parse(paymentId));

            return url + (url.Contains("?") ? "&" : "?") + "bookingid=" + order?.BookingId;
        }

        private async Task<string> GetErrorUrlAsync(string paymentId, string errorMessage = "")
        {
            var order = _orderRepository.FirstOrDefault(p => p.Id == long.Parse(paymentId));
            var url = HttpContext.Session.GetString("failUrl" + paymentId);
            return url + (url.Contains("?") ? "&" : "?") + "bookingid=" + order?.BookingId + "&failmessage=" + WebUtility.UrlEncode(errorMessage);
        }
    }
}