using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Bookings.Dto;
using TepayLink.Sdisco.Bookings.Dtos;

namespace TepayLink.Sdisco.Bookings
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> CreateOrder(CreateOrderInputDto input);
        Task ConfirmOrder(ConfirmOrderDto input);

        Task UpdteOrderPaySucess(long orderId);
        Task UpdteOrderPayFail(long orderId);

    }
}
