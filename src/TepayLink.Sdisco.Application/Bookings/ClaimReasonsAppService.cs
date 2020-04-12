

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
	[AbpAuthorize(AppPermissions.Pages_Administration_ClaimReasons)]
    public class ClaimReasonsAppService : SdiscoAppServiceBase, IClaimReasonsAppService
    {
		 private readonly IRepository<ClaimReason> _claimReasonRepository;
		 private readonly IClaimReasonsExcelExporter _claimReasonsExcelExporter;
		 

		  public ClaimReasonsAppService(IRepository<ClaimReason> claimReasonRepository, IClaimReasonsExcelExporter claimReasonsExcelExporter ) 
		  {
			_claimReasonRepository = claimReasonRepository;
			_claimReasonsExcelExporter = claimReasonsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetClaimReasonForViewDto>> GetAll(GetAllClaimReasonsInput input)
         {
			
			var filteredClaimReasons = _claimReasonRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter);

			var pagedAndFilteredClaimReasons = filteredClaimReasons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var claimReasons = from o in pagedAndFilteredClaimReasons
                         select new GetClaimReasonForViewDto() {
							ClaimReason = new ClaimReasonDto
							{
                                Title = o.Title,
                                Id = o.Id
							}
						};

            var totalCount = await filteredClaimReasons.CountAsync();

            return new PagedResultDto<GetClaimReasonForViewDto>(
                totalCount,
                await claimReasons.ToListAsync()
            );
         }
		 
		 public async Task<GetClaimReasonForViewDto> GetClaimReasonForView(int id)
         {
            var claimReason = await _claimReasonRepository.GetAsync(id);

            var output = new GetClaimReasonForViewDto { ClaimReason = ObjectMapper.Map<ClaimReasonDto>(claimReason) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ClaimReasons_Edit)]
		 public async Task<GetClaimReasonForEditOutput> GetClaimReasonForEdit(EntityDto input)
         {
            var claimReason = await _claimReasonRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetClaimReasonForEditOutput {ClaimReason = ObjectMapper.Map<CreateOrEditClaimReasonDto>(claimReason)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditClaimReasonDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ClaimReasons_Create)]
		 protected virtual async Task Create(CreateOrEditClaimReasonDto input)
         {
            var claimReason = ObjectMapper.Map<ClaimReason>(input);

			
			if (AbpSession.TenantId != null)
			{
				claimReason.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _claimReasonRepository.InsertAsync(claimReason);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ClaimReasons_Edit)]
		 protected virtual async Task Update(CreateOrEditClaimReasonDto input)
         {
            var claimReason = await _claimReasonRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, claimReason);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ClaimReasons_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _claimReasonRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetClaimReasonsToExcel(GetAllClaimReasonsForExcelInput input)
         {
			
			var filteredClaimReasons = _claimReasonRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter);

			var query = (from o in filteredClaimReasons
                         select new GetClaimReasonForViewDto() { 
							ClaimReason = new ClaimReasonDto
							{
                                Title = o.Title,
                                Id = o.Id
							}
						 });


            var claimReasonListDtos = await query.ToListAsync();

            return _claimReasonsExcelExporter.ExportToFile(claimReasonListDtos);
         }


    }
}