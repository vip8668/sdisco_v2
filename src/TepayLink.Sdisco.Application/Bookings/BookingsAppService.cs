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
	[AbpAuthorize(AppPermissions.Pages_Bookings)]
    public class BookingsAppService : SdiscoAppServiceBase, IBookingsAppService
    {
		 private readonly IRepository<Booking, long> _bookingRepository;
		 private readonly IBookingsExcelExporter _bookingsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public BookingsAppService(IRepository<Booking, long> bookingRepository, IBookingsExcelExporter bookingsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_bookingRepository = bookingRepository;
			_bookingsExcelExporter = bookingsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetBookingForViewDto>> GetAll(GetAllBookingsInput input)
         {
			var statusFilter = (BookingStatusEnum) input.StatusFilter;
			
			var filteredBookings = _bookingRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter) || e.GuestInfo.Contains(input.Filter) || e.CouponCode.Contains(input.Filter) || e.Contact.Contains(input.Filter))
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredBookings = filteredBookings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bookings = from o in pagedAndFilteredBookings
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookingForViewDto() {
							Booking = new BookingDto
							{
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                TripLength = o.TripLength,
                                Status = o.Status,
                                Quantity = o.Quantity,
                                Amount = o.Amount,
                                Fee = o.Fee,
                                Note = o.Note,
                                GuestInfo = o.GuestInfo,
                                CouponCode = o.CouponCode,
                                BonusAmount = o.BonusAmount,
                                Contact = o.Contact,
                                CouponId = o.CouponId,
                                TotalAmount = o.TotalAmount,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredBookings.CountAsync();

            return new PagedResultDto<GetBookingForViewDto>(
                totalCount,
                await bookings.ToListAsync()
            );
         }
		 
		 public async Task<GetBookingForViewDto> GetBookingForView(long id)
         {
            var booking = await _bookingRepository.GetAsync(id);

            var output = new GetBookingForViewDto { Booking = ObjectMapper.Map<BookingDto>(booking) };

		    if (output.Booking.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.Booking.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Bookings_Edit)]
		 public async Task<GetBookingForEditOutput> GetBookingForEdit(EntityDto<long> input)
         {
            var booking = await _bookingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBookingForEditOutput {Booking = ObjectMapper.Map<CreateOrEditBookingDto>(booking)};

		    if (output.Booking.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.Booking.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBookingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Bookings_Create)]
		 protected virtual async Task Create(CreateOrEditBookingDto input)
         {
            var booking = ObjectMapper.Map<Booking>(input);

			
			if (AbpSession.TenantId != null)
			{
				booking.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bookingRepository.InsertAsync(booking);
         }

		 [AbpAuthorize(AppPermissions.Pages_Bookings_Edit)]
		 protected virtual async Task Update(CreateOrEditBookingDto input)
         {
            var booking = await _bookingRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, booking);
         }

		 [AbpAuthorize(AppPermissions.Pages_Bookings_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _bookingRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBookingsToExcel(GetAllBookingsForExcelInput input)
         {
			var statusFilter = (BookingStatusEnum) input.StatusFilter;
			
			var filteredBookings = _bookingRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter) || e.GuestInfo.Contains(input.Filter) || e.CouponCode.Contains(input.Filter) || e.Contact.Contains(input.Filter))
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredBookings
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookingForViewDto() { 
							Booking = new BookingDto
							{
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                TripLength = o.TripLength,
                                Status = o.Status,
                                Quantity = o.Quantity,
                                Amount = o.Amount,
                                Fee = o.Fee,
                                Note = o.Note,
                                GuestInfo = o.GuestInfo,
                                CouponCode = o.CouponCode,
                                BonusAmount = o.BonusAmount,
                                Contact = o.Contact,
                                CouponId = o.CouponId,
                                TotalAmount = o.TotalAmount,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var bookingListDtos = await query.ToListAsync();

            return _bookingsExcelExporter.ExportToFile(bookingListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Bookings)]
         public async Task<PagedResultDto<BookingProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BookingProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new BookingProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<BookingProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}