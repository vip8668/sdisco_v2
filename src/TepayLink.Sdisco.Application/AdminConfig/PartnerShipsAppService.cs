

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.AdminConfig.Exporting;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.AdminConfig
{
	[AbpAuthorize(AppPermissions.Pages_Administration_PartnerShips)]
    public class PartnerShipsAppService : SdiscoAppServiceBase, IPartnerShipsAppService
    {
		 private readonly IRepository<PartnerShip> _partnerShipRepository;
		 private readonly IPartnerShipsExcelExporter _partnerShipsExcelExporter;
		 

		  public PartnerShipsAppService(IRepository<PartnerShip> partnerShipRepository, IPartnerShipsExcelExporter partnerShipsExcelExporter ) 
		  {
			_partnerShipRepository = partnerShipRepository;
			_partnerShipsExcelExporter = partnerShipsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPartnerShipForViewDto>> GetAll(GetAllPartnerShipsInput input)
         {
			
			var filteredPartnerShips = _partnerShipRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Logo.Contains(input.Filter) || e.Title.Contains(input.Filter) || e.Link.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LogoFilter),  e => e.Logo == input.LogoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LinkFilter),  e => e.Link == input.LinkFilter);

			var pagedAndFilteredPartnerShips = filteredPartnerShips
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var partnerShips = from o in pagedAndFilteredPartnerShips
                         select new GetPartnerShipForViewDto() {
							PartnerShip = new PartnerShipDto
							{
                                Logo = o.Logo,
                                Title = o.Title,
                                Link = o.Link,
                                Order = o.Order,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPartnerShips.CountAsync();

            return new PagedResultDto<GetPartnerShipForViewDto>(
                totalCount,
                await partnerShips.ToListAsync()
            );
         }
		 
		 public async Task<GetPartnerShipForViewDto> GetPartnerShipForView(int id)
         {
            var partnerShip = await _partnerShipRepository.GetAsync(id);

            var output = new GetPartnerShipForViewDto { PartnerShip = ObjectMapper.Map<PartnerShipDto>(partnerShip) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerShips_Edit)]
		 public async Task<GetPartnerShipForEditOutput> GetPartnerShipForEdit(EntityDto input)
         {
            var partnerShip = await _partnerShipRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPartnerShipForEditOutput {PartnerShip = ObjectMapper.Map<CreateOrEditPartnerShipDto>(partnerShip)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPartnerShipDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerShips_Create)]
		 protected virtual async Task Create(CreateOrEditPartnerShipDto input)
         {
            var partnerShip = ObjectMapper.Map<PartnerShip>(input);

			
			if (AbpSession.TenantId != null)
			{
				partnerShip.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _partnerShipRepository.InsertAsync(partnerShip);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerShips_Edit)]
		 protected virtual async Task Update(CreateOrEditPartnerShipDto input)
         {
            var partnerShip = await _partnerShipRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, partnerShip);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_PartnerShips_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _partnerShipRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPartnerShipsToExcel(GetAllPartnerShipsForExcelInput input)
         {
			
			var filteredPartnerShips = _partnerShipRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Logo.Contains(input.Filter) || e.Title.Contains(input.Filter) || e.Link.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LogoFilter),  e => e.Logo == input.LogoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LinkFilter),  e => e.Link == input.LinkFilter);

			var query = (from o in filteredPartnerShips
                         select new GetPartnerShipForViewDto() { 
							PartnerShip = new PartnerShipDto
							{
                                Logo = o.Logo,
                                Title = o.Title,
                                Link = o.Link,
                                Order = o.Order,
                                Id = o.Id
							}
						 });


            var partnerShipListDtos = await query.ToListAsync();

            return _partnerShipsExcelExporter.ExportToFile(partnerShipListDtos);
         }


    }
}