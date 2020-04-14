using TepayLink.Sdisco.Products;

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
	[AbpAuthorize(AppPermissions.Pages_Administration_BookingDetails)]
    public class BookingDetailsAppService : SdiscoAppServiceBase, IBookingDetailsAppService
    {
		 private readonly IRepository<BookingDetail, long> _bookingDetailRepository;
		 private readonly IBookingDetailsExcelExporter _bookingDetailsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public BookingDetailsAppService(IRepository<BookingDetail, long> bookingDetailRepository, IBookingDetailsExcelExporter bookingDetailsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_bookingDetailRepository = bookingDetailRepository;
			_bookingDetailsExcelExporter = bookingDetailsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetBookingDetailForViewDto>> GetAll(GetAllBookingDetailsInput input)
         {
			
			var filteredBookingDetails = _bookingDetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter))
						.WhereIf(input.MinRefundAmountFilter != null, e => e.RefundAmount >= input.MinRefundAmountFilter)
						.WhereIf(input.MaxRefundAmountFilter != null, e => e.RefundAmount <= input.MaxRefundAmountFilter)
						.WhereIf(input.MinProductDetailComboIdFilter != null, e => e.ProductDetailComboId >= input.MinProductDetailComboIdFilter)
						.WhereIf(input.MaxProductDetailComboIdFilter != null, e => e.ProductDetailComboId <= input.MaxProductDetailComboIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredBookingDetails = filteredBookingDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bookingDetails = from o in pagedAndFilteredBookingDetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookingDetailForViewDto() {
							BookingDetail = new BookingDetailDto
							{
                                BookingId = o.BookingId,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                TripLength = o.TripLength,
                                Status = o.Status,
                                ProductScheduleId = o.ProductScheduleId,
                                Quantity = o.Quantity,
                                Amount = o.Amount,
                                Fee = o.Fee,
                                HostPaymentStatus = o.HostPaymentStatus,
                                HostUserId = o.HostUserId,
                                BookingUserId = o.BookingUserId,
                                IsDone = o.IsDone,
                                AffiliateUserId = o.AffiliateUserId,
                                RoomId = o.RoomId,
                                Note = o.Note,
                                CancelDate = o.CancelDate,
                                RefundAmount = o.RefundAmount,
                                ProductDetailComboId = o.ProductDetailComboId??0,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredBookingDetails.CountAsync();

            return new PagedResultDto<GetBookingDetailForViewDto>(
                totalCount,
                await bookingDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetBookingDetailForViewDto> GetBookingDetailForView(long id)
         {
            var bookingDetail = await _bookingDetailRepository.GetAsync(id);

            var output = new GetBookingDetailForViewDto { BookingDetail = ObjectMapper.Map<BookingDetailDto>(bookingDetail) };

		    if (output.BookingDetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.BookingDetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingDetails_Edit)]
		 public async Task<GetBookingDetailForEditOutput> GetBookingDetailForEdit(EntityDto<long> input)
         {
            var bookingDetail = await _bookingDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBookingDetailForEditOutput {BookingDetail = ObjectMapper.Map<CreateOrEditBookingDetailDto>(bookingDetail)};

		    if (output.BookingDetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.BookingDetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBookingDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingDetails_Create)]
		 protected virtual async Task Create(CreateOrEditBookingDetailDto input)
         {
            var bookingDetail = ObjectMapper.Map<BookingDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				bookingDetail.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bookingDetailRepository.InsertAsync(bookingDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditBookingDetailDto input)
         {
            var bookingDetail = await _bookingDetailRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, bookingDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingDetails_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _bookingDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBookingDetailsToExcel(GetAllBookingDetailsForExcelInput input)
         {
			
			var filteredBookingDetails = _bookingDetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter))
						.WhereIf(input.MinRefundAmountFilter != null, e => e.RefundAmount >= input.MinRefundAmountFilter)
						.WhereIf(input.MaxRefundAmountFilter != null, e => e.RefundAmount <= input.MaxRefundAmountFilter)
						.WhereIf(input.MinProductDetailComboIdFilter != null, e => e.ProductDetailComboId >= input.MinProductDetailComboIdFilter)
						.WhereIf(input.MaxProductDetailComboIdFilter != null, e => e.ProductDetailComboId <= input.MaxProductDetailComboIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredBookingDetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookingDetailForViewDto() { 
							BookingDetail = new BookingDetailDto
							{
                                BookingId = o.BookingId,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                TripLength = o.TripLength,
                                Status = o.Status,
                                ProductScheduleId = o.ProductScheduleId,
                                Quantity = o.Quantity,
                                Amount = o.Amount,
                                Fee = o.Fee,
                                HostPaymentStatus = o.HostPaymentStatus,
                                HostUserId = o.HostUserId,
                                BookingUserId = o.BookingUserId,
                                IsDone = o.IsDone,
                                AffiliateUserId = o.AffiliateUserId,
                                RoomId = o.RoomId,
                                Note = o.Note,
                                CancelDate = o.CancelDate,
                                RefundAmount = o.RefundAmount,
                                ProductDetailComboId = o.ProductDetailComboId??0,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var bookingDetailListDtos = await query.ToListAsync();

            return _bookingDetailsExcelExporter.ExportToFile(bookingDetailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_BookingDetails)]
         public async Task<PagedResultDto<BookingDetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BookingDetailProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new BookingDetailProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<BookingDetailProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}