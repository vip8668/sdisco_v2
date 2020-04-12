


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
	[AbpAuthorize(AppPermissions.Pages_Administration_BookingClaims)]
    public class BookingClaimsAppService : SdiscoAppServiceBase, IBookingClaimsAppService
    {
		 private readonly IRepository<BookingClaim, long> _bookingClaimRepository;
		 private readonly IBookingClaimsExcelExporter _bookingClaimsExcelExporter;
		 private readonly IRepository<ClaimReason,int> _lookup_claimReasonRepository;
		 

		  public BookingClaimsAppService(IRepository<BookingClaim, long> bookingClaimRepository, IBookingClaimsExcelExporter bookingClaimsExcelExporter , IRepository<ClaimReason, int> lookup_claimReasonRepository) 
		  {
			_bookingClaimRepository = bookingClaimRepository;
			_bookingClaimsExcelExporter = bookingClaimsExcelExporter;
			_lookup_claimReasonRepository = lookup_claimReasonRepository;
		
		  }

		 public async Task<PagedResultDto<GetBookingClaimForViewDto>> GetAll(GetAllBookingClaimsInput input)
         {
			
			var filteredBookingClaims = _bookingClaimRepository.GetAll()
						.Include( e => e.ClaimReasonFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinBookingDetailIdFilter != null, e => e.BookingDetailId >= input.MinBookingDetailIdFilter)
						.WhereIf(input.MaxBookingDetailIdFilter != null, e => e.BookingDetailId <= input.MaxBookingDetailIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ClaimReasonTitleFilter), e => e.ClaimReasonFk != null && e.ClaimReasonFk.Title == input.ClaimReasonTitleFilter);

			var pagedAndFilteredBookingClaims = filteredBookingClaims
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bookingClaims = from o in pagedAndFilteredBookingClaims
                         join o1 in _lookup_claimReasonRepository.GetAll() on o.ClaimReasonId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookingClaimForViewDto() {
							BookingClaim = new BookingClaimDto
							{
                                BookingDetailId = o.BookingDetailId,
                                Id = o.Id
							},
                         	ClaimReasonTitle = s1 == null ? "" : s1.Title.ToString()
						};

            var totalCount = await filteredBookingClaims.CountAsync();

            return new PagedResultDto<GetBookingClaimForViewDto>(
                totalCount,
                await bookingClaims.ToListAsync()
            );
         }
		 
		 public async Task<GetBookingClaimForViewDto> GetBookingClaimForView(long id)
         {
            var bookingClaim = await _bookingClaimRepository.GetAsync(id);

            var output = new GetBookingClaimForViewDto { BookingClaim = ObjectMapper.Map<BookingClaimDto>(bookingClaim) };

		    if (output.BookingClaim.ClaimReasonId != null)
            {
                var _lookupClaimReason = await _lookup_claimReasonRepository.FirstOrDefaultAsync((int)output.BookingClaim.ClaimReasonId);
                output.ClaimReasonTitle = _lookupClaimReason.Title.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingClaims_Edit)]
		 public async Task<GetBookingClaimForEditOutput> GetBookingClaimForEdit(EntityDto<long> input)
         {
            var bookingClaim = await _bookingClaimRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBookingClaimForEditOutput {BookingClaim = ObjectMapper.Map<CreateOrEditBookingClaimDto>(bookingClaim)};

		    if (output.BookingClaim.ClaimReasonId != null)
            {
                var _lookupClaimReason = await _lookup_claimReasonRepository.FirstOrDefaultAsync((int)output.BookingClaim.ClaimReasonId);
                output.ClaimReasonTitle = _lookupClaimReason.Title.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBookingClaimDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingClaims_Create)]
		 protected virtual async Task Create(CreateOrEditBookingClaimDto input)
         {
            var bookingClaim = ObjectMapper.Map<BookingClaim>(input);

			
			if (AbpSession.TenantId != null)
			{
				bookingClaim.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _bookingClaimRepository.InsertAsync(bookingClaim);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingClaims_Edit)]
		 protected virtual async Task Update(CreateOrEditBookingClaimDto input)
         {
            var bookingClaim = await _bookingClaimRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, bookingClaim);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_BookingClaims_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _bookingClaimRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBookingClaimsToExcel(GetAllBookingClaimsForExcelInput input)
         {
			
			var filteredBookingClaims = _bookingClaimRepository.GetAll()
						.Include( e => e.ClaimReasonFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinBookingDetailIdFilter != null, e => e.BookingDetailId >= input.MinBookingDetailIdFilter)
						.WhereIf(input.MaxBookingDetailIdFilter != null, e => e.BookingDetailId <= input.MaxBookingDetailIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ClaimReasonTitleFilter), e => e.ClaimReasonFk != null && e.ClaimReasonFk.Title == input.ClaimReasonTitleFilter);

			var query = (from o in filteredBookingClaims
                         join o1 in _lookup_claimReasonRepository.GetAll() on o.ClaimReasonId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetBookingClaimForViewDto() { 
							BookingClaim = new BookingClaimDto
							{
                                BookingDetailId = o.BookingDetailId,
                                Id = o.Id
							},
                         	ClaimReasonTitle = s1 == null ? "" : s1.Title.ToString()
						 });


            var bookingClaimListDtos = await query.ToListAsync();

            return _bookingClaimsExcelExporter.ExportToFile(bookingClaimListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_BookingClaims)]
         public async Task<PagedResultDto<BookingClaimClaimReasonLookupTableDto>> GetAllClaimReasonForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_claimReasonRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Title.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var claimReasonList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BookingClaimClaimReasonLookupTableDto>();
			foreach(var claimReason in claimReasonList){
				lookupTableDtoList.Add(new BookingClaimClaimReasonLookupTableDto
				{
					Id = claimReason.Id,
					DisplayName = claimReason.Title?.ToString()
				});
			}

            return new PagedResultDto<BookingClaimClaimReasonLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}