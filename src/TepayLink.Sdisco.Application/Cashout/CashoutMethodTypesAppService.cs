

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Cashout.Exporting;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Cashout
{
	[AbpAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes)]
    public class CashoutMethodTypesAppService : SdiscoAppServiceBase, ICashoutMethodTypesAppService
    {
		 private readonly IRepository<CashoutMethodType> _cashoutMethodTypeRepository;
		 private readonly ICashoutMethodTypesExcelExporter _cashoutMethodTypesExcelExporter;
		 

		  public CashoutMethodTypesAppService(IRepository<CashoutMethodType> cashoutMethodTypeRepository, ICashoutMethodTypesExcelExporter cashoutMethodTypesExcelExporter ) 
		  {
			_cashoutMethodTypeRepository = cashoutMethodTypeRepository;
			_cashoutMethodTypesExcelExporter = cashoutMethodTypesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCashoutMethodTypeForViewDto>> GetAll(GetAllCashoutMethodTypesInput input)
         {
			
			var filteredCashoutMethodTypes = _cashoutMethodTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Note.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter),  e => e.Note == input.NoteFilter);

			var pagedAndFilteredCashoutMethodTypes = filteredCashoutMethodTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var cashoutMethodTypes = from o in pagedAndFilteredCashoutMethodTypes
                         select new GetCashoutMethodTypeForViewDto() {
							CashoutMethodType = new CashoutMethodTypeDto
							{
                                Title = o.Title,
                                Note = o.Note,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCashoutMethodTypes.CountAsync();

            return new PagedResultDto<GetCashoutMethodTypeForViewDto>(
                totalCount,
                await cashoutMethodTypes.ToListAsync()
            );
         }
		 
		 public async Task<GetCashoutMethodTypeForViewDto> GetCashoutMethodTypeForView(int id)
         {
            var cashoutMethodType = await _cashoutMethodTypeRepository.GetAsync(id);

            var output = new GetCashoutMethodTypeForViewDto { CashoutMethodType = ObjectMapper.Map<CashoutMethodTypeDto>(cashoutMethodType) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes_Edit)]
		 public async Task<GetCashoutMethodTypeForEditOutput> GetCashoutMethodTypeForEdit(EntityDto input)
         {
            var cashoutMethodType = await _cashoutMethodTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCashoutMethodTypeForEditOutput {CashoutMethodType = ObjectMapper.Map<CreateOrEditCashoutMethodTypeDto>(cashoutMethodType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCashoutMethodTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes_Create)]
		 protected virtual async Task Create(CreateOrEditCashoutMethodTypeDto input)
         {
            var cashoutMethodType = ObjectMapper.Map<CashoutMethodType>(input);

			
			if (AbpSession.TenantId != null)
			{
				cashoutMethodType.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _cashoutMethodTypeRepository.InsertAsync(cashoutMethodType);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes_Edit)]
		 protected virtual async Task Update(CreateOrEditCashoutMethodTypeDto input)
         {
            var cashoutMethodType = await _cashoutMethodTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, cashoutMethodType);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_CashoutMethodTypes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _cashoutMethodTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCashoutMethodTypesToExcel(GetAllCashoutMethodTypesForExcelInput input)
         {
			
			var filteredCashoutMethodTypes = _cashoutMethodTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Note.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter),  e => e.Title == input.TitleFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoteFilter),  e => e.Note == input.NoteFilter);

			var query = (from o in filteredCashoutMethodTypes
                         select new GetCashoutMethodTypeForViewDto() { 
							CashoutMethodType = new CashoutMethodTypeDto
							{
                                Title = o.Title,
                                Note = o.Note,
                                Id = o.Id
							}
						 });


            var cashoutMethodTypeListDtos = await query.ToListAsync();

            return _cashoutMethodTypesExcelExporter.ExportToFile(cashoutMethodTypeListDtos);
         }


    }
}