using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IUserReviewsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserReviewForViewDto>> GetAll(GetAllUserReviewsInput input);

        Task<GetUserReviewForViewDto> GetUserReviewForView(int id);

		Task<GetUserReviewForEditOutput> GetUserReviewForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUserReviewDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUserReviewsToExcel(GetAllUserReviewsForExcelInput input);

		
    }
}