

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
	[AbpAuthorize(AppPermissions.Pages_Administration_BookingRefunds)]
    public class BookingRefundsAppService : SdiscoAppServiceBase, IBookingRefundsAppService
    {
		 private readonly IRepository<BookingRefund, long> _bookingRefundRepository;
		 private readonly IBookingRefundsExcelExporter _bookingRefundsExcelExporter;
		 

		  public BookingRefundsAppService(IRepository<BookingRefund, long> bookingRefundRepository, IBookingRefundsExcelExporter bookingRefundsExcelExporter ) 
		  {
			_bookingRefundRepository = bookingRefundRepository;
			_bookingRefundsExcelExporter = bookingRefundsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetBookingRefundForViewDto>> GetAll(GetAllBookingRefundsInput input)
         {
			
			var filteredBookingRefunds = _bookingRefundRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(input.MinBookingDetailIdFilter != null, e => e.BookingDetailId >= input.MinBookingDetailIdFilter)
						.WhereIf(input.MaxBookingDetailIdFilter != null, e => e.BookingDetailId <= input.MaxBookingDetailIdFilter)
						.WhereIf(input.MinRefundMethodIdFilter != null, e => e.RefundMethodId >= input.MinRefundMethodIdFilter)
						.WhereIf(input.MaxRefundMethodIdFilter != null, e => e.RefundMethodId <= input.MaxRefundMethodIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter);

			var pagedAndFilteredBookingRefunds = filteredBookingRefunds
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bookingRefunds = from o in pagedAndFilteredBookingRefunds
                         select new GetBookingRefundForViewDto() {
							BookingRefund = new BookingRefundDto
							{
                                BookingDetailId = o.BookingDetailId,
                                RefundMethodId = o.RefundMethodId,
                                Description = o.Description,
                                Status = o.Status,
                                Amount = o.Amount,
                                Id = o.Id
							}
						};

            var totalCount = await filteredBookingRefunds.CountAsync();

            return new PagedResultDto<GetBookingRefundForViewDto>(
                totalCount,
                await bookingRefunds.ToListAsync()
            );
         }
		 
		 public async Task<GetBookingRefundForViewDto> GetBookingRefundForView(long id)
         {
            var bookingRefund = await _bookingRefundRepository.GetAsync(id);

            var output = new GetBookingRefundForViewDto { BookingRefund = ObjectMapper.Map<BookingRefundDto>(bookingRefund) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingRefunds_Edit)]
		 public async Task<GetBookingRefundForEditOutput> GetBookingRefundForEdit(EntityDto<long> input)
         {
            var bookingRefund = await _bookingRefundRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBookingRefundForEditOutput {BookingRefund = ObjectMapper.Map<CreateOrEditBookingRefundDto>(bookingRefund)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBookingRefundDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingRefunds_Create)]
		 protected virtual async Task Create(CreateOrEditBookingRefundDto input)
         {
            var bookingRefund = ObjectMapper.Map<BookingRefund>(input);

			
			if (AbpSession.TenantId != null)
			{
				bookingRefund.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bookingRefundRepository.InsertAsync(bookingRefund);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingRefunds_Edit)]
		 protected virtual async Task Update(CreateOrEditBookingRefundDto input)
         {
            var bookingRefund = await _bookingRefundRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, bookingRefund);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingRefunds_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _bookingRefundRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBookingRefundsToExcel(GetAllBookingRefundsForExcelInput input)
         {
			
			var filteredBookingRefunds = _bookingRefundRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter))
						.WhereIf(input.MinBookingDetailIdFilter != null, e => e.BookingDetailId >= input.MinBookingDetailIdFilter)
						.WhereIf(input.MaxBookingDetailIdFilter != null, e => e.BookingDetailId <= input.MaxBookingDetailIdFilter)
						.WhereIf(input.MinRefundMethodIdFilter != null, e => e.RefundMethodId >= input.MinRefundMethodIdFilter)
						.WhereIf(input.MaxRefundMethodIdFilter != null, e => e.RefundMethodId <= input.MaxRefundMethodIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter);

			var query = (from o in filteredBookingRefunds
                         select new GetBookingRefundForViewDto() { 
							BookingRefund = new BookingRefundDto
							{
                                BookingDetailId = o.BookingDetailId,
                                RefundMethodId = o.RefundMethodId,
                                Description = o.Description,
                                Status = o.Status,
                                Amount = o.Amount,
                                Id = o.Id
							}
						 });


            var bookingRefundListDtos = await query.ToListAsync();

            return _bookingRefundsExcelExporter.ExportToFile(bookingRefundListDtos);
         }


    }
}