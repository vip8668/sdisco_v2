using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account
{
    public interface IUserReviewDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserReviewDetailForViewDto>> GetAll(GetAllUserReviewDetailsInput input);

        Task<GetUserReviewDetailForViewDto> GetUserReviewDetailForView(long id);

		Task<GetUserReviewDetailForEditOutput> GetUserReviewDetailForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditUserReviewDetailDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetUserReviewDetailsToExcel(GetAllUserReviewDetailsForExcelInput input);

		
    }
}