
using TepayLink.Sdisco.KOL;
using TepayLink.Sdisco.KOL;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.KOL.Exporting;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.KOL
{
	[AbpAuthorize(AppPermissions.Pages_Administration_PartnerRevenues)]
    public class PartnerRevenuesAppService : SdiscoAppServiceBase, IPartnerRevenuesAppService
    {
		 private readonly IRepository<PartnerRevenue, long> _partnerRevenueRepository;
		 private readonly IPartnerRevenuesExcelExporter _partnerRevenuesExcelExporter;
		 

		  public PartnerRevenuesAppService(IRepository<PartnerRevenue, long> partnerRevenueRepository, IPartnerRevenuesExcelExporter partnerRevenuesExcelExporter ) 
		  {
			_partnerRevenueRepository = partnerRevenueRepository;
			_partnerRevenuesExcelExporter = partnerRevenuesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPartnerRevenueForViewDto>> GetAll(GetAllPartnerRevenuesInput input)
         {
			var revenueTypeFilter = (RevenueTypeEnum) input.RevenueTypeFilter;
			var statusFilter = (RevenueStatusEnum) input.StatusFilter;
			
			var filteredPartnerRevenues = _partnerRevenueRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinUseridFilter != null, e => e.Userid >= input.MinUseridFilter)
						.WhereIf(input.MaxUseridFilter != null, e => e.Userid <= input.MaxUseridFilter)
						.WhereIf(input.RevenueTypeFilter > -1, e => e.RevenueType == revenueTypeFilter)
						.WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
						.WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
						.WhereIf(input.MinMoneyFilter != null, e => e.Money >= input.MinMoneyFilter)
						.WhereIf(input.MaxMoneyFilter != null, e => e.Money <= input.MaxMoneyFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter);

			var pagedAndFilteredPartnerRevenues = filteredPartnerRevenues
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var partnerRevenues = from o in pagedAndFilteredPartnerRevenues
                         select new GetPartnerRevenueForViewDto() {
							PartnerRevenue = new PartnerRevenueDto
							{
                                Userid = o.Userid,
                                RevenueType = o.RevenueType,
                                ProductId = o.ProductId,
                                Point = o.Point,
                                Money = o.Money,
                                Status = o.Status,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPartnerRevenues.CountAsync();

            return new PagedResultDto<GetPartnerRevenueForViewDto>(
                totalCount,
                await partnerRevenues.ToListAsync()
            );
         }
		 
		 public async Task<GetPartnerRevenueForViewDto> GetPartnerRevenueForView(long id)
         {
            var partnerRevenue = await _partnerRevenueRepository.GetAsync(id);

            var output = new GetPartnerRevenueForViewDto { PartnerRevenue = ObjectMapper.Map<PartnerRevenueDto>(partnerRevenue) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerRevenues_Edit)]
		 public async Task<GetPartnerRevenueForEditOutput> GetPartnerRevenueForEdit(EntityDto<long> input)
         {
            var partnerRevenue = await _partnerRevenueRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPartnerRevenueForEditOutput {PartnerRevenue = ObjectMapper.Map<CreateOrEditPartnerRevenueDto>(partnerRevenue)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPartnerRevenueDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerRevenues_Create)]
		 protected virtual async Task Create(CreateOrEditPartnerRevenueDto input)
         {
            var partnerRevenue = ObjectMapper.Map<PartnerRevenue>(input);

			
			if (AbpSession.TenantId != null)
			{
				partnerRevenue.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _partnerRevenueRepository.InsertAsync(partnerRevenue);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerRevenues_Edit)]
		 protected virtual async Task Update(CreateOrEditPartnerRevenueDto input)
         {
            var partnerRevenue = await _partnerRevenueRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, partnerRevenue);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerRevenues_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _partnerRevenueRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPartnerRevenuesToExcel(GetAllPartnerRevenuesForExcelInput input)
         {
			var revenueTypeFilter = (RevenueTypeEnum) input.RevenueTypeFilter;
			var statusFilter = (RevenueStatusEnum) input.StatusFilter;
			
			var filteredPartnerRevenues = _partnerRevenueRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinUseridFilter != null, e => e.Userid >= input.MinUseridFilter)
						.WhereIf(input.MaxUseridFilter != null, e => e.Userid <= input.MaxUseridFilter)
						.WhereIf(input.RevenueTypeFilter > -1, e => e.RevenueType == revenueTypeFilter)
						.WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
						.WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
						.WhereIf(input.MinMoneyFilter != null, e => e.Money >= input.MinMoneyFilter)
						.WhereIf(input.MaxMoneyFilter != null, e => e.Money <= input.MaxMoneyFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter);

			var query = (from o in filteredPartnerRevenues
                         select new GetPartnerRevenueForViewDto() { 
							PartnerRevenue = new PartnerRevenueDto
							{
                                Userid = o.Userid,
                                RevenueType = o.RevenueType,
                                ProductId = o.ProductId,
                                Point = o.Point,
                                Money = o.Money,
                                Status = o.Status,
                                Id = o.Id
							}
						 });


            var partnerRevenueListDtos = await query.ToListAsync();

            return _partnerRevenuesExcelExporter.ExportToFile(partnerRevenueListDtos);
         }


    }
}