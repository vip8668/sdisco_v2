using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Cashout
{
    public interface ICashoutMethodTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCashoutMethodTypeForViewDto>> GetAll(GetAllCashoutMethodTypesInput input);

        Task<GetCashoutMethodTypeForViewDto> GetCashoutMethodTypeForView(int id);

		Task<GetCashoutMethodTypeForEditOutput> GetCashoutMethodTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCashoutMethodTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCashoutMethodTypesToExcel(GetAllCashoutMethodTypesForExcelInput input);

		
    }
}