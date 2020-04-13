

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Affiliate.Exporting;
using TepayLink.Sdisco.Affiliate.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Affiliate
{
	[AbpAuthorize(AppPermissions.Pages_Administration_ShortLinks)]
    public class ShortLinksAppService : SdiscoAppServiceBase, IShortLinksAppService
    {
		 private readonly IRepository<ShortLink, long> _shortLinkRepository;
		 private readonly IShortLinksExcelExporter _shortLinksExcelExporter;
		 

		  public ShortLinksAppService(IRepository<ShortLink, long> shortLinkRepository, IShortLinksExcelExporter shortLinksExcelExporter ) 
		  {
			_shortLinkRepository = shortLinkRepository;
			_shortLinksExcelExporter = shortLinksExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetShortLinkForViewDto>> GetAll(GetAllShortLinksInput input)
         {
			
			var filteredShortLinks = _shortLinkRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FullLink.Contains(input.Filter) || e.ShortCode.Contains(input.Filter));

			var pagedAndFilteredShortLinks = filteredShortLinks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shortLinks = from o in pagedAndFilteredShortLinks
                         select new GetShortLinkForViewDto() {
							ShortLink = new ShortLinkDto
							{
                                UserId = o.UserId,
                                FullLink = o.FullLink,
                                ShortCode = o.ShortCode,
                                Id = o.Id
							}
						};

            var totalCount = await filteredShortLinks.CountAsync();

            return new PagedResultDto<GetShortLinkForViewDto>(
                totalCount,
                await shortLinks.ToListAsync()
            );
         }
		 
		 public async Task<GetShortLinkForViewDto> GetShortLinkForView(long id)
         {
            var shortLink = await _shortLinkRepository.GetAsync(id);

            var output = new GetShortLinkForViewDto { ShortLink = ObjectMapper.Map<ShortLinkDto>(shortLink) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ShortLinks_Edit)]
		 public async Task<GetShortLinkForEditOutput> GetShortLinkForEdit(EntityDto<long> input)
         {
            var shortLink = await _shortLinkRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShortLinkForEditOutput {ShortLink = ObjectMapper.Map<CreateOrEditShortLinkDto>(shortLink)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShortLinkDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ShortLinks_Create)]
		 protected virtual async Task Create(CreateOrEditShortLinkDto input)
         {
            var shortLink = ObjectMapper.Map<ShortLink>(input);

			
			if (AbpSession.TenantId != null)
			{
				shortLink.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _shortLinkRepository.InsertAsync(shortLink);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ShortLinks_Edit)]
		 protected virtual async Task Update(CreateOrEditShortLinkDto input)
         {
            var shortLink = await _shortLinkRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, shortLink);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ShortLinks_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _shortLinkRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShortLinksToExcel(GetAllShortLinksForExcelInput input)
         {
			
			var filteredShortLinks = _shortLinkRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.FullLink.Contains(input.Filter) || e.ShortCode.Contains(input.Filter));

			var query = (from o in filteredShortLinks
                         select new GetShortLinkForViewDto() { 
							ShortLink = new ShortLinkDto
							{
                                UserId = o.UserId,
                                FullLink = o.FullLink,
                                ShortCode = o.ShortCode,
                                Id = o.Id
							}
						 });


            var shortLinkListDtos = await query.ToListAsync();

            return _shortLinksExcelExporter.ExportToFile(shortLinkListDtos);
         }


    }
}