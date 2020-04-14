

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
	[AbpAuthorize(AppPermissions.Pages_Administration_RefundReasons)]
    public class RefundReasonsAppService : SdiscoAppServiceBase, IRefundReasonsAppService
    {
		 private readonly IRepository<RefundReason> _refundReasonRepository;
		 private readonly IRefundReasonsExcelExporter _refundReasonsExcelExporter;
		 

		  public RefundReasonsAppService(IRepository<RefundReason> refundReasonRepository, IRefundReasonsExcelExporter refundReasonsExcelExporter ) 
		  {
			_refundReasonRepository = refundReasonRepository;
			_refundReasonsExcelExporter = refundReasonsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetRefundReasonForViewDto>> GetAll(GetAllRefundReasonsInput input)
         {
			
			var filteredRefundReasons = _refundReasonRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ReasonText.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReasonTextFilter),  e => e.ReasonText == input.ReasonTextFilter);

			var pagedAndFilteredRefundReasons = filteredRefundReasons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var refundReasons = from o in pagedAndFilteredRefundReasons
                         select new GetRefundReasonForViewDto() {
							RefundReason = new RefundReasonDto
							{
                                ReasonText = o.ReasonText,
                                Id = o.Id
							}
						};

            var totalCount = await filteredRefundReasons.CountAsync();

            return new PagedResultDto<GetRefundReasonForViewDto>(
                totalCount,
                await refundReasons.ToListAsync()
            );
         }
		 
		 public async Task<GetRefundReasonForViewDto> GetRefundReasonForView(int id)
         {
            var refundReason = await _refundReasonRepository.GetAsync(id);

            var output = new GetRefundReasonForViewDto { RefundReason = ObjectMapper.Map<RefundReasonDto>(refundReason) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_RefundReasons_Edit)]
		 public async Task<GetRefundReasonForEditOutput> GetRefundReasonForEdit(EntityDto input)
         {
            var refundReason = await _refundReasonRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRefundReasonForEditOutput {RefundReason = ObjectMapper.Map<CreateOrEditRefundReasonDto>(refundReason)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRefundReasonDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RefundReasons_Create)]
		 protected virtual async Task Create(CreateOrEditRefundReasonDto input)
         {
            var refundReason = ObjectMapper.Map<RefundReason>(input);

			
			if (AbpSession.TenantId != null)
			{
				refundReason.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _refundReasonRepository.InsertAsync(refundReason);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RefundReasons_Edit)]
		 protected virtual async Task Update(CreateOrEditRefundReasonDto input)
         {
            var refundReason = await _refundReasonRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, refundReason);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_RefundReasons_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _refundReasonRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetRefundReasonsToExcel(GetAllRefundReasonsForExcelInput input)
         {
			
			var filteredRefundReasons = _refundReasonRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ReasonText.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ReasonTextFilter),  e => e.ReasonText == input.ReasonTextFilter);

			var query = (from o in filteredRefundReasons
                         select new GetRefundReasonForViewDto() { 
							RefundReason = new RefundReasonDto
							{
                                ReasonText = o.ReasonText,
                                Id = o.Id
							}
						 });


            var refundReasonListDtos = await query.ToListAsync();

            return _refundReasonsExcelExporter.ExportToFile(refundReasonListDtos);
         }


    }
}