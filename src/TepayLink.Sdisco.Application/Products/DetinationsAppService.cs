
using TepayLink.Sdisco.Products;

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
	[AbpAuthorize(AppPermissions.Pages_Administration_Detinations)]
    public class DetinationsAppService : SdiscoAppServiceBase, IDetinationsAppService
    {
		 private readonly IRepository<Detination, long> _detinationRepository;
		 private readonly IDetinationsExcelExporter _detinationsExcelExporter;
		 

		  public DetinationsAppService(IRepository<Detination, long> detinationRepository, IDetinationsExcelExporter detinationsExcelExporter ) 
		  {
			_detinationRepository = detinationRepository;
			_detinationsExcelExporter = detinationsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetDetinationForViewDto>> GetAll(GetAllDetinationsInput input)
         {
			var statusFilter = (DetinationStatusEnum) input.StatusFilter;
			
			var filteredDetinations = _detinationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Image.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.IsTopFilter > -1,  e => (input.IsTopFilter == 1 && e.IsTop) || (input.IsTopFilter == 0 && !e.IsTop) )
						.WhereIf(input.MinBookingCountFilter != null, e => e.BookingCount >= input.MinBookingCountFilter)
						.WhereIf(input.MaxBookingCountFilter != null, e => e.BookingCount <= input.MaxBookingCountFilter);

			var pagedAndFilteredDetinations = filteredDetinations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var detinations = from o in pagedAndFilteredDetinations
                         select new GetDetinationForViewDto() {
							Detination = new DetinationDto
							{
                                Image = o.Image,
                                Name = o.Name,
                                Status = o.Status,
                                IsTop = o.IsTop,
                                BookingCount = o.BookingCount,
                                Id = o.Id
							}
						};

            var totalCount = await filteredDetinations.CountAsync();

            return new PagedResultDto<GetDetinationForViewDto>(
                totalCount,
                await detinations.ToListAsync()
            );
         }
		 
		 public async Task<GetDetinationForViewDto> GetDetinationForView(long id)
         {
            var detination = await _detinationRepository.GetAsync(id);

            var output = new GetDetinationForViewDto { Detination = ObjectMapper.Map<DetinationDto>(detination) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Detinations_Edit)]
		 public async Task<GetDetinationForEditOutput> GetDetinationForEdit(EntityDto<long> input)
         {
            var detination = await _detinationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDetinationForEditOutput {Detination = ObjectMapper.Map<CreateOrEditDetinationDto>(detination)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDetinationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Detinations_Create)]
		 protected virtual async Task Create(CreateOrEditDetinationDto input)
         {
            var detination = ObjectMapper.Map<Detination>(input);

			
			if (AbpSession.TenantId != null)
			{
				detination.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _detinationRepository.InsertAsync(detination);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Detinations_Edit)]
		 protected virtual async Task Update(CreateOrEditDetinationDto input)
         {
            var detination = await _detinationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, detination);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Detinations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _detinationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDetinationsToExcel(GetAllDetinationsForExcelInput input)
         {
			var statusFilter = (DetinationStatusEnum) input.StatusFilter;
			
			var filteredDetinations = _detinationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Image.Contains(input.Filter) || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.IsTopFilter > -1,  e => (input.IsTopFilter == 1 && e.IsTop) || (input.IsTopFilter == 0 && !e.IsTop) )
						.WhereIf(input.MinBookingCountFilter != null, e => e.BookingCount >= input.MinBookingCountFilter)
						.WhereIf(input.MaxBookingCountFilter != null, e => e.BookingCount <= input.MaxBookingCountFilter);

			var query = (from o in filteredDetinations
                         select new GetDetinationForViewDto() { 
							Detination = new DetinationDto
							{
                                Image = o.Image,
                                Name = o.Name,
                                Status = o.Status,
                                IsTop = o.IsTop,
                                BookingCount = o.BookingCount,
                                Id = o.Id
							}
						 });


            var detinationListDtos = await query.ToListAsync();

            return _detinationsExcelExporter.ExportToFile(detinationListDtos);
         }


    }
}