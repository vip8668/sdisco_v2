
using TepayLink.Sdisco.Account;
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
	[AbpAuthorize(AppPermissions.Pages_Administration_Transactions)]
    public class TransactionsAppService : SdiscoAppServiceBase, ITransactionsAppService
    {
		 private readonly IRepository<Transaction, long> _transactionRepository;
		 private readonly ITransactionsExcelExporter _transactionsExcelExporter;
		 

		  public TransactionsAppService(IRepository<Transaction, long> transactionRepository, ITransactionsExcelExporter transactionsExcelExporter ) 
		  {
			_transactionRepository = transactionRepository;
			_transactionsExcelExporter = transactionsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input)
         {
			var transTypeFilter = (TransactionType) input.TransTypeFilter;
			var walletTypeFilter = (WalletTypeEnum) input.WalletTypeFilter;
			
			var filteredTransactions = _transactionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Descrition.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.MinSideFilter != null, e => e.Side >= input.MinSideFilter)
						.WhereIf(input.MaxSideFilter != null, e => e.Side <= input.MaxSideFilter)
						.WhereIf(input.TransTypeFilter > -1, e => e.TransType == transTypeFilter)
						.WhereIf(input.WalletTypeFilter > -1, e => e.WalletType == walletTypeFilter)
						.WhereIf(input.MinBookingDetailIdFilter != null, e => e.BookingDetailId >= input.MinBookingDetailIdFilter)
						.WhereIf(input.MaxBookingDetailIdFilter != null, e => e.BookingDetailId <= input.MaxBookingDetailIdFilter)
						.WhereIf(input.MinRefIdFilter != null, e => e.RefId >= input.MinRefIdFilter)
						.WhereIf(input.MaxRefIdFilter != null, e => e.RefId <= input.MaxRefIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescritionFilter),  e => e.Descrition == input.DescritionFilter);

			var pagedAndFilteredTransactions = filteredTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var transactions = from o in pagedAndFilteredTransactions
                         select new GetTransactionForViewDto() {
							Transaction = new TransactionDto
							{
                                UserId = o.UserId,
                                Amount = o.Amount,
                                Side = o.Side,
                                TransType = o.TransType,
                                WalletType = o.WalletType,
                                BookingDetailId = o.BookingDetailId,
                                RefId = o.RefId,
                                Descrition = o.Descrition,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTransactions.CountAsync();

            return new PagedResultDto<GetTransactionForViewDto>(
                totalCount,
                await transactions.ToListAsync()
            );
         }
		 
		 public async Task<GetTransactionForViewDto> GetTransactionForView(long id)
         {
            var transaction = await _transactionRepository.GetAsync(id);

            var output = new GetTransactionForViewDto { Transaction = ObjectMapper.Map<TransactionDto>(transaction) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_Transactions_Edit)]
		 public async Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto<long> input)
         {
            var transaction = await _transactionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTransactionForEditOutput {Transaction = ObjectMapper.Map<CreateOrEditTransactionDto>(transaction)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTransactionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Transactions_Create)]
		 protected virtual async Task Create(CreateOrEditTransactionDto input)
         {
            var transaction = ObjectMapper.Map<Transaction>(input);

			
			if (AbpSession.TenantId != null)
			{
				transaction.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _transactionRepository.InsertAsync(transaction);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Transactions_Edit)]
		 protected virtual async Task Update(CreateOrEditTransactionDto input)
         {
            var transaction = await _transactionRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, transaction);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_Transactions_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _transactionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTransactionsToExcel(GetAllTransactionsForExcelInput input)
         {
			var transTypeFilter = (TransactionType) input.TransTypeFilter;
			var walletTypeFilter = (WalletTypeEnum) input.WalletTypeFilter;
			
			var filteredTransactions = _transactionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Descrition.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.MinSideFilter != null, e => e.Side >= input.MinSideFilter)
						.WhereIf(input.MaxSideFilter != null, e => e.Side <= input.MaxSideFilter)
						.WhereIf(input.TransTypeFilter > -1, e => e.TransType == transTypeFilter)
						.WhereIf(input.WalletTypeFilter > -1, e => e.WalletType == walletTypeFilter)
						.WhereIf(input.MinBookingDetailIdFilter != null, e => e.BookingDetailId >= input.MinBookingDetailIdFilter)
						.WhereIf(input.MaxBookingDetailIdFilter != null, e => e.BookingDetailId <= input.MaxBookingDetailIdFilter)
						.WhereIf(input.MinRefIdFilter != null, e => e.RefId >= input.MinRefIdFilter)
						.WhereIf(input.MaxRefIdFilter != null, e => e.RefId <= input.MaxRefIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescritionFilter),  e => e.Descrition == input.DescritionFilter);

			var query = (from o in filteredTransactions
                         select new GetTransactionForViewDto() { 
							Transaction = new TransactionDto
							{
                                UserId = o.UserId,
                                Amount = o.Amount,
                                Side = o.Side,
                                TransType = o.TransType,
                                WalletType = o.WalletType,
                                BookingDetailId = o.BookingDetailId,
                                RefId = o.RefId,
                                Descrition = o.Descrition,
                                Id = o.Id
							}
						 });


            var transactionListDtos = await query.ToListAsync();

            return _transactionsExcelExporter.ExportToFile(transactionListDtos);
         }


    }
}