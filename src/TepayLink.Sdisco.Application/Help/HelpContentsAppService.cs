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
	[AbpAuthorize(AppPermissions.Pages_Administration_HelpContents)]
    public class HelpContentsAppService : SdiscoAppServiceBase, IHelpContentsAppService
    {
		 private readonly IRepository<HelpContent, long> _helpContentRepository;
		 private readonly IHelpContentsExcelExporter _helpContentsExcelExporter;
		 private readonly IRepository<HelpCategory,long> _lookup_helpCategoryRepository;
		 

		  public HelpContentsAppService(IRepository<HelpContent, long> helpContentRepository, IHelpContentsExcelExporter helpContentsExcelExporter , IRepository<HelpCategory, long> lookup_helpCategoryRepository) 
		  {
			_helpContentRepository = helpContentRepository;
			_helpContentsExcelExporter = helpContentsExcelExporter;
			_lookup_helpCategoryRepository = lookup_helpCategoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetHelpContentForViewDto>> GetAll(GetAllHelpContentsInput input)
         {
			
			var filteredHelpContents = _helpContentRepository.GetAll()
						.Include( e => e.HelpCategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Question.Contains(input.Filter) || e.Answer.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.HelpCategoryCategoryNameFilter), e => e.HelpCategoryFk != null && e.HelpCategoryFk.CategoryName == input.HelpCategoryCategoryNameFilter);

			var pagedAndFilteredHelpContents = filteredHelpContents
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var helpContents = from o in pagedAndFilteredHelpContents
                         join o1 in _lookup_helpCategoryRepository.GetAll() on o.HelpCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetHelpContentForViewDto() {
							HelpContent = new HelpContentDto
							{
                                Question = o.Question,
                                Answer = o.Answer,
                                Id = o.Id
							},
                         	HelpCategoryCategoryName = s1 == null ? "" : s1.CategoryName.ToString()
						};

            var totalCount = await filteredHelpContents.CountAsync();

            return new PagedResultDto<GetHelpContentForViewDto>(
                totalCount,
                await helpContents.ToListAsync()
            );
         }
		 
		 public async Task<GetHelpContentForViewDto> GetHelpContentForView(long id)
         {
            var helpContent = await _helpContentRepository.GetAsync(id);

            var output = new GetHelpContentForViewDto { HelpContent = ObjectMapper.Map<HelpContentDto>(helpContent) };

		    if (output.HelpContent.HelpCategoryId != null)
            {
                var _lookupHelpCategory = await _lookup_helpCategoryRepository.FirstOrDefaultAsync((long)output.HelpContent.HelpCategoryId);
                output.HelpCategoryCategoryName = _lookupHelpCategory.CategoryName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpContents_Edit)]
		 public async Task<GetHelpContentForEditOutput> GetHelpContentForEdit(EntityDto<long> input)
         {
            var helpContent = await _helpContentRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetHelpContentForEditOutput {HelpContent = ObjectMapper.Map<CreateOrEditHelpContentDto>(helpContent)};

		    if (output.HelpContent.HelpCategoryId != null)
            {
                var _lookupHelpCategory = await _lookup_helpCategoryRepository.FirstOrDefaultAsync((long)output.HelpContent.HelpCategoryId);
                output.HelpCategoryCategoryName = _lookupHelpCategory.CategoryName.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditHelpContentDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpContents_Create)]
		 protected virtual async Task Create(CreateOrEditHelpContentDto input)
         {
            var helpContent = ObjectMapper.Map<HelpContent>(input);

			
			if (AbpSession.TenantId != null)
			{
				helpContent.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _helpContentRepository.InsertAsync(helpContent);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpContents_Edit)]
		 protected virtual async Task Update(CreateOrEditHelpContentDto input)
         {
            var helpContent = await _helpContentRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, helpContent);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_HelpContents_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _helpContentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetHelpContentsToExcel(GetAllHelpContentsForExcelInput input)
         {
			
			var filteredHelpContents = _helpContentRepository.GetAll()
						.Include( e => e.HelpCategoryFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Question.Contains(input.Filter) || e.Answer.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.HelpCategoryCategoryNameFilter), e => e.HelpCategoryFk != null && e.HelpCategoryFk.CategoryName == input.HelpCategoryCategoryNameFilter);

			var query = (from o in filteredHelpContents
                         join o1 in _lookup_helpCategoryRepository.GetAll() on o.HelpCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetHelpContentForViewDto() { 
							HelpContent = new HelpContentDto
							{
                                Question = o.Question,
                                Answer = o.Answer,
                                Id = o.Id
							},
                         	HelpCategoryCategoryName = s1 == null ? "" : s1.CategoryName.ToString()
						 });


            var helpContentListDtos = await query.ToListAsync();

            return _helpContentsExcelExporter.ExportToFile(helpContentListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_HelpContents)]
         public async Task<PagedResultDto<HelpContentHelpCategoryLookupTableDto>> GetAllHelpCategoryForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_helpCategoryRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.CategoryName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var helpCategoryList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<HelpContentHelpCategoryLookupTableDto>();
			foreach(var helpCategory in helpCategoryList){
				lookupTableDtoList.Add(new HelpContentHelpCategoryLookupTableDto
				{
					Id = helpCategory.Id,
					DisplayName = helpCategory.CategoryName?.ToString()
				});
			}

            return new PagedResultDto<HelpContentHelpCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}