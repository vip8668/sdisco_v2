using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IWithDrawRequestsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetWithDrawRequestForViewDto>> GetAll(GetAllWithDrawRequestsInput input);

        Task<GetWithDrawRequestForViewDto> GetWithDrawRequestForView(long id);

		Task<GetWithDrawRequestForEditOutput> GetWithDrawRequestForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditWithDrawRequestDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetWithDrawRequestsToExcel(GetAllWithDrawRequestsForExcelInput input);

		
    }
}