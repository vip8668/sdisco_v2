using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface ITransactionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input);

        Task<GetTransactionForViewDto> GetTransactionForView(long id);

		Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditTransactionDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetTransactionsToExcel(GetAllTransactionsForExcelInput input);

		
    }
}