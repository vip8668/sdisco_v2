using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Home.Dto;

namespace TepayLink.Sdisco.Home
{
    public interface IHomeAppService
    {
        
        Task<List<BasicProductCategoryDto>> GetListProductCategory();
        Task<List<BasicTourCategoryDto>> GetListTourCatgory();
        Task<PagedResultDto<BasicPlaceDto>> GetTopPlaces(PagedInputDto input );
        Task<PagedResultDto<BasicTourDto>> GetTopTours(PagedInputDto input);
        Task<PagedResultDto<BasicTourDto>> GetTopTrips(PagedInputDto input);
        Task<List<BasicTourCategoryDto>> GetListTripCatgory();
        Task<PagedResultDto<BasicActivityDto>> GetTopActivity(PagedInputDto input);
        Task<BannerDto> GetBanner(GetBannerDto input);
        
    }
}
