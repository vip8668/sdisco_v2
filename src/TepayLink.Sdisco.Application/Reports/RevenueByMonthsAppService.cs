

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Reports.Exporting;
using TepayLink.Sdisco.Reports.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Reports
{
	[AbpAuthorize(AppPermissions.Pages_Administration_RevenueByMonths)]
    public class RevenueByMonthsAppService : SdiscoAppServiceBase, IRevenueByMonthsAppService
    {
		 private readonly IRepository<RevenueByMonth, long> _revenueByMonthRepository;
		 private readonly IRevenueByMonthsExcelExporter _revenueByMonthsExcelExporter;
		 

		  public RevenueByMonthsAppService(IRepository<RevenueByMonth, long> revenueByMonthRepository, IRevenueByMonthsExcelExporter revenueByMonthsExcelExporter ) 
		  {
			_revenueByMonthRepository = revenueByMonthRepository;
			_revenueByMonthsExcelExporter = revenueByMonthsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetRevenueByMonthForViewDto>> GetAll(GetAllRevenueByMonthsInput input)
         {
			
			var filteredRevenueByMonths = _revenueByMonthRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinRevenueFilter != null, e => e.Revenue >= input.MinRevenueFilter)
						.WhereIf(input.MaxRevenueFilter != null, e => e.Revenue <= input.MaxRevenueFilter)
						.WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
						.WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter);

			var pagedAndFilteredRevenueByMonths = filteredRevenueByMonths
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var revenueByMonths = from o in pagedAndFilteredRevenueByMonths
                         select new GetRevenueByMonthForViewDto() {
							RevenueByMonth = new RevenueByMonthDto
							{
                                UserId = o.UserId,
                                Revenue = o.Revenue,
                                Date = o.Date,
                                Id = o.Id
							}
						};

            var totalCount = await filteredRevenueByMonths.CountAsync();

            return new PagedResultDto<GetRevenueByMonthForViewDto>(
                totalCount,
                await revenueByMonths.ToListAsync()
            );
         }
		 
		 public async Task<GetRevenueByMonthForViewDto> GetRevenueByMonthForView(long id)
         {
            var revenueByMonth = await _revenueByMonthRepository.GetAsync(id);

            var output = new GetRevenueByMonthForViewDto { RevenueByMonth = ObjectMapper.Map<RevenueByMonthDto>(revenueByMonth) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_RevenueByMonths_Edit)]
		 public async Task<GetRevenueByMonthForEditOutput> GetRevenueByMonthForEdit(EntityDto<long> input)
         {
            var revenueByMonth = await _revenueByMonthRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRevenueByMonthForEditOutput {RevenueByMonth = ObjectMapper.Map<CreateOrEditRevenueByMonthDto>(revenueByMonth)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRevenueByMonthDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RevenueByMonths_Create)]
		 protected virtual async Task Create(CreateOrEditRevenueByMonthDto input)
         {
            var revenueByMonth = ObjectMapper.Map<RevenueByMonth>(input);

			
			if (AbpSession.TenantId != null)
			{
				revenueByMonth.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _revenueByMonthRepository.InsertAsync(revenueByMonth);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RevenueByMonths_Edit)]
		 protected virtual async Task Update(CreateOrEditRevenueByMonthDto input)
         {
            var revenueByMonth = await _revenueByMonthRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, revenueByMonth);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RevenueByMonths_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _revenueByMonthRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetRevenueByMonthsToExcel(GetAllRevenueByMonthsForExcelInput input)
         {
			
			var filteredRevenueByMonths = _revenueByMonthRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinRevenueFilter != null, e => e.Revenue >= input.MinRevenueFilter)
						.WhereIf(input.MaxRevenueFilter != null, e => e.Revenue <= input.MaxRevenueFilter)
						.WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
						.WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter);

			var query = (from o in filteredRevenueByMonths
                         select new GetRevenueByMonthForViewDto() { 
							RevenueByMonth = new RevenueByMonthDto
							{
                                UserId = o.UserId,
                                Revenue = o.Revenue,
                                Date = o.Date,
                                Id = o.Id
							}
						 });


            var revenueByMonthListDtos = await query.ToListAsync();

            return _revenueByMonthsExcelExporter.ExportToFile(revenueByMonthListDtos);
         }


    }
}