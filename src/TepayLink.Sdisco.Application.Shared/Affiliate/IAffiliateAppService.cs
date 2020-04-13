using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Affiliate.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Affiliate.Dto;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Affiliate
{
    public interface IAffiliateAppService : IApplicationService
    {
        Task CreatePartner(CreatePartnerInputDto input);
        Task<PagedResultDto<AffiliateTourDTo>> GetTripCrated(GetTripCreated input);
        Task<CreateShortlinkOutput> CreateShortLink(CreateShortLinkInput inputDto);
        Task<string> GetFullLink(CheckFullLinkInput input);

        Task<PagedResultDto<PayoutHistoryDto>> GetPayoutHistory(PayOutInputDto input);
        Task<PayoutDto> GetPayoutList(PayOutInputDto input);

        Task<PagedResultDto<CommissionDto>> GetLastCommission(PagedInputDto input);
        Task<PagedResultDto<CommissionDto>> GetLastCommissionTour(PagedInputDto input);
        Task<PagedResultDto<CommissionDto>> GetHostCommissionTour(PagedInputDto input);

        Task<string> GetConfigTitle();

        Task<GetCommissionOutputDto> GetCommission(GetCommissionInputDto input);

        Task<List<CommissionChartOutputDto>> GetCommissionChart(GetChartInputDto input);

        Task<PagedResultDto<PointListDetail>> GetDetail(GetPointListDetail input);

        Task ShareTrip(long tripId);



    }
}
