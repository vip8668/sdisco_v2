using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Reports.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Reports
{
    public interface IRevenueByMonthsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRevenueByMonthForViewDto>> GetAll(GetAllRevenueByMonthsInput input);

        Task<GetRevenueByMonthForViewDto> GetRevenueByMonthForView(long id);

		Task<GetRevenueByMonthForEditOutput> GetRevenueByMonthForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditRevenueByMonthDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetRevenueByMonthsToExcel(GetAllRevenueByMonthsForExcelInput input);

		
    }
}