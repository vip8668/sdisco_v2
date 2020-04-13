
using TepayLink.Sdisco.Account;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Account.Exporting;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Account
{
	[AbpAuthorize(AppPermissions.Pages_Administration_WithDrawRequests)]
    public class WithDrawRequestsAppService : SdiscoAppServiceBase, IWithDrawRequestsAppService
    {
		 private readonly IRepository<WithDrawRequest, long> _withDrawRequestRepository;
		 private readonly IWithDrawRequestsExcelExporter _withDrawRequestsExcelExporter;
		 

		  public WithDrawRequestsAppService(IRepository<WithDrawRequest, long> withDrawRequestRepository, IWithDrawRequestsExcelExporter withDrawRequestsExcelExporter ) 
		  {
			_withDrawRequestRepository = withDrawRequestRepository;
			_withDrawRequestsExcelExporter = withDrawRequestsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetWithDrawRequestForViewDto>> GetAll(GetAllWithDrawRequestsInput input)
         {
			var statusFilter = (WithDrawRequestStatus) input.StatusFilter;
			
			var filteredWithDrawRequests = _withDrawRequestRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.MinTransactionIdFilter != null, e => e.TransactionId >= input.MinTransactionIdFilter)
						.WhereIf(input.MaxTransactionIdFilter != null, e => e.TransactionId <= input.MaxTransactionIdFilter)
						.WhereIf(input.MinBankAccountIdFilter != null, e => e.BankAccountId >= input.MinBankAccountIdFilter)
						.WhereIf(input.MaxBankAccountIdFilter != null, e => e.BankAccountId <= input.MaxBankAccountIdFilter);

			var pagedAndFilteredWithDrawRequests = filteredWithDrawRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var withDrawRequests = from o in pagedAndFilteredWithDrawRequests
                         select new GetWithDrawRequestForViewDto() {
							WithDrawRequest = new WithDrawRequestDto
							{
                                UserId = o.UserId,
                                Amount = o.Amount,
                                Status = o.Status,
                                TransactionId = o.TransactionId,
                                BankAccountId = o.BankAccountId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredWithDrawRequests.CountAsync();

            return new PagedResultDto<GetWithDrawRequestForViewDto>(
                totalCount,
                await withDrawRequests.ToListAsync()
            );
         }
		 
		 public async Task<GetWithDrawRequestForViewDto> GetWithDrawRequestForView(long id)
         {
            var withDrawRequest = await _withDrawRequestRepository.GetAsync(id);

            var output = new GetWithDrawRequestForViewDto { WithDrawRequest = ObjectMapper.Map<WithDrawRequestDto>(withDrawRequest) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_WithDrawRequests_Edit)]
		 public async Task<GetWithDrawRequestForEditOutput> GetWithDrawRequestForEdit(EntityDto<long> input)
         {
            var withDrawRequest = await _withDrawRequestRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetWithDrawRequestForEditOutput {WithDrawRequest = ObjectMapper.Map<CreateOrEditWithDrawRequestDto>(withDrawRequest)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditWithDrawRequestDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_WithDrawRequests_Create)]
		 protected virtual async Task Create(CreateOrEditWithDrawRequestDto input)
         {
            var withDrawRequest = ObjectMapper.Map<WithDrawRequest>(input);

			
			if (AbpSession.TenantId != null)
			{
				withDrawRequest.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _withDrawRequestRepository.InsertAsync(withDrawRequest);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_WithDrawRequests_Edit)]
		 protected virtual async Task Update(CreateOrEditWithDrawRequestDto input)
         {
            var withDrawRequest = await _withDrawRequestRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, withDrawRequest);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_WithDrawRequests_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _withDrawRequestRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetWithDrawRequestsToExcel(GetAllWithDrawRequestsForExcelInput input)
         {
			var statusFilter = (WithDrawRequestStatus) input.StatusFilter;
			
			var filteredWithDrawRequests = _withDrawRequestRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.StatusFilter > -1, e => e.Status == statusFilter)
						.WhereIf(input.MinTransactionIdFilter != null, e => e.TransactionId >= input.MinTransactionIdFilter)
						.WhereIf(input.MaxTransactionIdFilter != null, e => e.TransactionId <= input.MaxTransactionIdFilter)
						.WhereIf(input.MinBankAccountIdFilter != null, e => e.BankAccountId >= input.MinBankAccountIdFilter)
						.WhereIf(input.MaxBankAccountIdFilter != null, e => e.BankAccountId <= input.MaxBankAccountIdFilter);

			var query = (from o in filteredWithDrawRequests
                         select new GetWithDrawRequestForViewDto() { 
							WithDrawRequest = new WithDrawRequestDto
							{
                                UserId = o.UserId,
                                Amount = o.Amount,
                                Status = o.Status,
                                TransactionId = o.TransactionId,
                                BankAccountId = o.BankAccountId,
                                Id = o.Id
							}
						 });


            var withDrawRequestListDtos = await query.ToListAsync();

            return _withDrawRequestsExcelExporter.ExportToFile(withDrawRequestListDtos);
         }


    }
}