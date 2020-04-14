using Abp.Application.Services;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Banks.Dtos;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Payment.Dto;

namespace TepayLink.Sdisco.Payment
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<List<PaymentMethodDto>> GetPaymentMethod();
        Task DeletePaymentMethod(int id);

        Task<NapasOutputDto> AddPaymentMethod(AddPaymentMethodInputDto input);

        Task<NapasOutputDto> PaymentOrder(PaymentInputDto input);


        Task CreateUpdatePayment(PaymentResultDto paymentResult);
        Task ConfirmPaymentPaypal(long paymentId, string paypalOrderId);

        Task ConfirmPaymentVtc(string ordercode, string transerf, string paymentType,
            OrderStatus status);
    }
}
