

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Products.Exporting;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Products
{
	[AbpAuthorize(AppPermissions.Pages_Administration_Utilities)]
    public class UtilitiesAppService : SdiscoAppServiceBase, IUtilitiesAppService
    {
		 private readonly IRepository<Utility> _utilityRepository;
		 private readonly IUtilitiesExcelExporter _utilitiesExcelExporter;
		 

		  public UtilitiesAppService(IRepository<Utility> utilityRepository, IUtilitiesExcelExporter utilitiesExcelExporter ) 
		  {
			_utilityRepository = utilityRepository;
			_utilitiesExcelExporter = utilitiesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetUtilityForViewDto>> GetAll(GetAllUtilitiesInput input)
         {
			
			var filteredUtilities = _utilityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Icon.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter),  e => e.Icon == input.IconFilter);

			var pagedAndFilteredUtilities = filteredUtilities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var utilities = from o in pagedAndFilteredUtilities
                         select new GetUtilityForViewDto() {
							Utility = new UtilityDto
							{
                                Name = o.Name,
                                Icon = o.Icon,
                                Id = o.Id
							}
						};

            var totalCount = await filteredUtilities.CountAsync();

            return new PagedResultDto<GetUtilityForViewDto>(
                totalCount,
                await utilities.ToListAsync()
            );
         }
		 
		 public async Task<GetUtilityForViewDto> GetUtilityForView(int id)
         {
            var utility = await _utilityRepository.GetAsync(id);

            var output = new GetUtilityForViewDto { Utility = ObjectMapper.Map<UtilityDto>(utility) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Utilities_Edit)]
		 public async Task<GetUtilityForEditOutput> GetUtilityForEdit(EntityDto input)
         {
            var utility = await _utilityRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUtilityForEditOutput {Utility = ObjectMapper.Map<CreateOrEditUtilityDto>(utility)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUtilityDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Utilities_Create)]
		 protected virtual async Task Create(CreateOrEditUtilityDto input)
         {
            var utility = ObjectMapper.Map<Utility>(input);

			
			if (AbpSession.TenantId != null)
			{
				utility.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _utilityRepository.InsertAsync(utility);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Utilities_Edit)]
		 protected virtual async Task Update(CreateOrEditUtilityDto input)
         {
            var utility = await _utilityRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, utility);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Utilities_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _utilityRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUtilitiesToExcel(GetAllUtilitiesForExcelInput input)
         {
			
			var filteredUtilities = _utilityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Icon.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IconFilter),  e => e.Icon == input.IconFilter);

			var query = (from o in filteredUtilities
                         select new GetUtilityForViewDto() { 
							Utility = new UtilityDto
							{
                                Name = o.Name,
                                Icon = o.Icon,
                                Id = o.Id
							}
						 });


            var utilityListDtos = await query.ToListAsync();

            return _utilitiesExcelExporter.ExportToFile(utilityListDtos);
         }


    }
}