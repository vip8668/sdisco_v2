using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.KOL
{
    public interface IShareTransactionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetShareTransactionForViewDto>> GetAll(GetAllShareTransactionsInput input);

        Task<GetShareTransactionForViewDto> GetShareTransactionForView(long id);

		Task<GetShareTransactionForEditOutput> GetShareTransactionForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditShareTransactionDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetShareTransactionsToExcel(GetAllShareTransactionsForExcelInput input);

		
    }
}