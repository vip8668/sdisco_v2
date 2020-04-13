
using TepayLink.Sdisco.KOL;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.KOL.Exporting;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.KOL
{
	[AbpAuthorize(AppPermissions.Pages_Administration_ShareTransactions)]
    public class ShareTransactionsAppService : SdiscoAppServiceBase, IShareTransactionsAppService
    {
		 private readonly IRepository<ShareTransaction, long> _shareTransactionRepository;
		 private readonly IShareTransactionsExcelExporter _shareTransactionsExcelExporter;
		 

		  public ShareTransactionsAppService(IRepository<ShareTransaction, long> shareTransactionRepository, IShareTransactionsExcelExporter shareTransactionsExcelExporter ) 
		  {
			_shareTransactionRepository = shareTransactionRepository;
			_shareTransactionsExcelExporter = shareTransactionsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetShareTransactionForViewDto>> GetAll(GetAllShareTransactionsInput input)
         {
			var typeFilter = (RevenueTypeEnum) input.TypeFilter;
			
			var filteredShareTransactions = _shareTransactionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.IP.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IPFilter),  e => e.IP == input.IPFilter)
						.WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
						.WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
						.WhereIf(input.MinProductIdFilter != null, e => e.ProductId >= input.MinProductIdFilter)
						.WhereIf(input.MaxProductIdFilter != null, e => e.ProductId <= input.MaxProductIdFilter);

			var pagedAndFilteredShareTransactions = filteredShareTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var shareTransactions = from o in pagedAndFilteredShareTransactions
                         select new GetShareTransactionForViewDto() {
							ShareTransaction = new ShareTransactionDto
							{
                                UserId = o.UserId,
                                Type = o.Type,
                                IP = o.IP,
                                Point = o.Point,
                                ProductId = o.ProductId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredShareTransactions.CountAsync();

            return new PagedResultDto<GetShareTransactionForViewDto>(
                totalCount,
                await shareTransactions.ToListAsync()
            );
         }
		 
		 public async Task<GetShareTransactionForViewDto> GetShareTransactionForView(long id)
         {
            var shareTransaction = await _shareTransactionRepository.GetAsync(id);

            var output = new GetShareTransactionForViewDto { ShareTransaction = ObjectMapper.Map<ShareTransactionDto>(shareTransaction) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ShareTransactions_Edit)]
		 public async Task<GetShareTransactionForEditOutput> GetShareTransactionForEdit(EntityDto<long> input)
         {
            var shareTransaction = await _shareTransactionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetShareTransactionForEditOutput {ShareTransaction = ObjectMapper.Map<CreateOrEditShareTransactionDto>(shareTransaction)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditShareTransactionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ShareTransactions_Create)]
		 protected virtual async Task Create(CreateOrEditShareTransactionDto input)
         {
            var shareTransaction = ObjectMapper.Map<ShareTransaction>(input);

			
			if (AbpSession.TenantId != null)
			{
				shareTransaction.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _shareTransactionRepository.InsertAsync(shareTransaction);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ShareTransactions_Edit)]
		 protected virtual async Task Update(CreateOrEditShareTransactionDto input)
         {
            var shareTransaction = await _shareTransactionRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, shareTransaction);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ShareTransactions_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _shareTransactionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetShareTransactionsToExcel(GetAllShareTransactionsForExcelInput input)
         {
			var typeFilter = (RevenueTypeEnum) input.TypeFilter;
			
			var filteredShareTransactions = _shareTransactionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.IP.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.TypeFilter > -1, e => e.Type == typeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.IPFilter),  e => e.IP == input.IPFilter)
						.WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
						.WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
						.WhereIf(input.MinProductIdFilter != null, e => e.ProductId >= input.MinProductIdFilter)
						.WhereIf(input.MaxProductIdFilter != null, e => e.ProductId <= input.MaxProductIdFilter);

			var query = (from o in filteredShareTransactions
                         select new GetShareTransactionForViewDto() { 
							ShareTransaction = new ShareTransactionDto
							{
                                UserId = o.UserId,
                                Type = o.Type,
                                IP = o.IP,
                                Point = o.Point,
                                ProductId = o.ProductId,
                                Id = o.Id
							}
						 });


            var shareTransactionListDtos = await query.ToListAsync();

            return _shareTransactionsExcelExporter.ExportToFile(shareTransactionListDtos);
         }


    }
}