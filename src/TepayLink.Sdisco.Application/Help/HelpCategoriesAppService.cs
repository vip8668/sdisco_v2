
using TepayLink.Sdisco.Help;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Help.Exporting;
using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Help
{
	[AbpAuthorize(AppPermissions.Pages_Administration_HelpCategories)]
    public class HelpCategoriesAppService : SdiscoAppServiceBase, IHelpCategoriesAppService
    {
		 private readonly IRepository<HelpCategory, long> _helpCategoryRepository;
		 private readonly IHelpCategoriesExcelExporter _helpCategoriesExcelExporter;
		 

		  public HelpCategoriesAppService(IRepository<HelpCategory, long> helpCategoryRepository, IHelpCategoriesExcelExporter helpCategoriesExcelExporter ) 
		  {
			_helpCategoryRepository = helpCategoryRepository;
			_helpCategoriesExcelExporter = helpCategoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetHelpCategoryForViewDto>> GetAll(GetAllHelpCategoriesInput input)
         {
			var typeFilter = (HelpTypeEnum) input.TypeFilter;
			
			var filteredHelpCategories = _helpCategoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CategoryName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter),  e => e.CategoryName == input.CategoryNameFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter);

			var pagedAndFilteredHelpCategories = filteredHelpCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var helpCategories = from o in pagedAndFilteredHelpCategories
                         select new GetHelpCategoryForViewDto() {
							HelpCategory = new HelpCategoryDto
							{
                                CategoryName = o.CategoryName,
                                Type = o.Type,
                                Id = o.Id
							}
						};

            var totalCount = await filteredHelpCategories.CountAsync();

            return new PagedResultDto<GetHelpCategoryForViewDto>(
                totalCount,
                await helpCategories.ToListAsync()
            );
         }
		 
		 public async Task<GetHelpCategoryForViewDto> GetHelpCategoryForView(long id)
         {
            var helpCategory = await _helpCategoryRepository.GetAsync(id);

            var output = new GetHelpCategoryForViewDto { HelpCategory = ObjectMapper.Map<HelpCategoryDto>(helpCategory) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpCategories_Edit)]
		 public async Task<GetHelpCategoryForEditOutput> GetHelpCategoryForEdit(EntityDto<long> input)
         {
            var helpCategory = await _helpCategoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHelpCategoryForEditOutput {HelpCategory = ObjectMapper.Map<CreateOrEditHelpCategoryDto>(helpCategory)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHelpCategoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpCategories_Create)]
		 protected virtual async Task Create(CreateOrEditHelpCategoryDto input)
         {
            var helpCategory = ObjectMapper.Map<HelpCategory>(input);

			
			if (AbpSession.TenantId != null)
			{
				helpCategory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _helpCategoryRepository.InsertAsync(helpCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditHelpCategoryDto input)
         {
            var helpCategory = await _helpCategoryRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, helpCategory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpCategories_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _helpCategoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHelpCategoriesToExcel(GetAllHelpCategoriesForExcelInput input)
         {
			var typeFilter = (HelpTypeEnum) input.TypeFilter;
			
			var filteredHelpCategories = _helpCategoryRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.CategoryName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.CategoryNameFilter),  e => e.CategoryName == input.CategoryNameFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter);

			var query = (from o in filteredHelpCategories
                         select new GetHelpCategoryForViewDto() { 
							HelpCategory = new HelpCategoryDto
							{
                                CategoryName = o.CategoryName,
                                Type = o.Type,
                                Id = o.Id
							}
						 });


            var helpCategoryListDtos = await query.ToListAsync();

            return _helpCategoriesExcelExporter.ExportToFile(helpCategoryListDtos);
         }


    }
}