
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
	[AbpAuthorize(AppPermissions.Pages_Administration_Coupons)]
    public class CouponsAppService : SdiscoAppServiceBase, ICouponsAppService
    {
		 private readonly IRepository<Coupon, long> _couponRepository;
		 private readonly ICouponsExcelExporter _couponsExcelExporter;
		 

		  public CouponsAppService(IRepository<Coupon, long> couponRepository, ICouponsExcelExporter couponsExcelExporter ) 
		  {
			_couponRepository = couponRepository;
			_couponsExcelExporter = couponsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCouponForViewDto>> GetAll(GetAllCouponsInput input)
         {
			var statusFilter = (CouponStatusEnum) input.StatusFilter;
			
			var filteredCoupons = _couponRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter);

			var pagedAndFilteredCoupons = filteredCoupons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var coupons = from o in pagedAndFilteredCoupons
                         select new GetCouponForViewDto() {
							Coupon = new CouponDto
							{
                                Code = o.Code,
                                Amount = o.Amount,
                                Status = o.Status,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCoupons.CountAsync();

            return new PagedResultDto<GetCouponForViewDto>(
                totalCount,
                await coupons.ToListAsync()
            );
         }
		 
		 public async Task<GetCouponForViewDto> GetCouponForView(long id)
         {
            var coupon = await _couponRepository.GetAsync(id);

            var output = new GetCouponForViewDto { Coupon = ObjectMapper.Map<CouponDto>(coupon) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Coupons_Edit)]
		 public async Task<GetCouponForEditOutput> GetCouponForEdit(EntityDto<long> input)
         {
            var coupon = await _couponRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCouponForEditOutput {Coupon = ObjectMapper.Map<CreateOrEditCouponDto>(coupon)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCouponDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Coupons_Create)]
		 protected virtual async Task Create(CreateOrEditCouponDto input)
         {
            var coupon = ObjectMapper.Map<Coupon>(input);

			
			if (AbpSession.TenantId != null)
			{
				coupon.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _couponRepository.InsertAsync(coupon);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Coupons_Edit)]
		 protected virtual async Task Update(CreateOrEditCouponDto input)
         {
            var coupon = await _couponRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, coupon);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Coupons_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _couponRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCouponsToExcel(GetAllCouponsForExcelInput input)
         {
			var statusFilter = (CouponStatusEnum) input.StatusFilter;
			
			var filteredCoupons = _couponRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Code.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter),  e => e.Code == input.CodeFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter);

			var query = (from o in filteredCoupons
                         select new GetCouponForViewDto() { 
							Coupon = new CouponDto
							{
                                Code = o.Code,
                                Amount = o.Amount,
                                Status = o.Status,
                                Id = o.Id
							}
						 });


            var couponListDtos = await query.ToListAsync();

            return _couponsExcelExporter.ExportToFile(couponListDtos);
         }


    }
}