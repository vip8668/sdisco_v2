using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IUserSubcribersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserSubcriberForViewDto>> GetAll(GetAllUserSubcribersInput input);

        Task<GetUserSubcriberForViewDto> GetUserSubcriberForView(long id);

		Task<GetUserSubcriberForEditOutput> GetUserSubcriberForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditUserSubcriberDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetUserSubcribersToExcel(GetAllUserSubcribersForExcelInput input);

		
    }
}