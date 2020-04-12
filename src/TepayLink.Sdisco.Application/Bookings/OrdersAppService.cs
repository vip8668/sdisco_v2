
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Bookings;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Bookings.Exporting;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Bookings
{
	[AbpAuthorize(AppPermissions.Pages_Administration_Orders)]
    public class OrdersAppService : SdiscoAppServiceBase, IOrdersAppService
    {
		 private readonly IRepository<Order, long> _orderRepository;
		 private readonly IOrdersExcelExporter _ordersExcelExporter;
		 

		  public OrdersAppService(IRepository<Order, long> orderRepository, IOrdersExcelExporter ordersExcelExporter ) 
		  {
			_orderRepository = orderRepository;
			_ordersExcelExporter = ordersExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input)
         {
			
			var filteredOrders = _orderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.OrderCode.Contains(input.Filter) || e.Note.Contains(input.Filter) || e.OrderRef.Contains(input.Filter) || e.BankCode.Contains(input.Filter) || e.Currency.Contains(input.Filter) || e.IssueDate.Contains(input.Filter) || e.NameOnCard.Contains(input.Filter) || e.TransactionId.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.IssueDateFilter),  e => e.IssueDate == input.IssueDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameOnCardFilter),  e => e.NameOnCard == input.NameOnCardFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TransactionIdFilter),  e => e.TransactionId == input.TransactionIdFilter);

			var pagedAndFilteredOrders = filteredOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var orders = from o in pagedAndFilteredOrders
                         select new GetOrderForViewDto() {
							Order = new OrderDto
							{
                                OrderCode = o.OrderCode,
                                OrderType = o.OrderType,
                                Amount = o.Amount,
                                Note = o.Note,
                                Status = o.Status,
                                OrderRef = o.OrderRef,
                                UserId = o.UserId,
                                BankCode = o.BankCode,
                                CardId = o.CardId,
                                CardNumber = o.CardNumber,
                                Currency = o.Currency,
                                IssueDate = o.IssueDate,
                                NameOnCard = o.NameOnCard,
                                TransactionId = o.TransactionId,
                                BookingId = o.BookingId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredOrders.CountAsync();

            return new PagedResultDto<GetOrderForViewDto>(
                totalCount,
                await orders.ToListAsync()
            );
         }
		 
		 public async Task<GetOrderForViewDto> GetOrderForView(long id)
         {
            var order = await _orderRepository.GetAsync(id);

            var output = new GetOrderForViewDto { Order = ObjectMapper.Map<OrderDto>(order) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Orders_Edit)]
		 public async Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<long> input)
         {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetOrderForEditOutput {Order = ObjectMapper.Map<CreateOrEditOrderDto>(order)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditOrderDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Orders_Create)]
		 protected virtual async Task Create(CreateOrEditOrderDto input)
         {
            var order = ObjectMapper.Map<Order>(input);

			
			if (AbpSession.TenantId != null)
			{
				order.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _orderRepository.InsertAsync(order);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Orders_Edit)]
		 protected virtual async Task Update(CreateOrEditOrderDto input)
         {
            var order = await _orderRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, order);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Orders_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _orderRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input)
         {
			
			var filteredOrders = _orderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.OrderCode.Contains(input.Filter) || e.Note.Contains(input.Filter) || e.OrderRef.Contains(input.Filter) || e.BankCode.Contains(input.Filter) || e.Currency.Contains(input.Filter) || e.IssueDate.Contains(input.Filter) || e.NameOnCard.Contains(input.Filter) || e.TransactionId.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.IssueDateFilter),  e => e.IssueDate == input.IssueDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameOnCardFilter),  e => e.NameOnCard == input.NameOnCardFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TransactionIdFilter),  e => e.TransactionId == input.TransactionIdFilter);

			var query = (from o in filteredOrders
                         select new GetOrderForViewDto() { 
							Order = new OrderDto
							{
                                OrderCode = o.OrderCode,
                                OrderType = o.OrderType,
                                Amount = o.Amount,
                                Note = o.Note,
                                Status = o.Status,
                                OrderRef = o.OrderRef,
                                UserId = o.UserId,
                                BankCode = o.BankCode,
                                CardId = o.CardId,
                                CardNumber = o.CardNumber,
                                Currency = o.Currency,
                                IssueDate = o.IssueDate,
                                NameOnCard = o.NameOnCard,
                                TransactionId = o.TransactionId,
                                BookingId = o.BookingId,
                                Id = o.Id
							}
						 });


            var orderListDtos = await query.ToListAsync();

            return _ordersExcelExporter.ExportToFile(orderListDtos);
         }


    }
}