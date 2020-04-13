using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Tour
{
    public interface IFavouriteAppService : IApplicationService
    {
        Task<PagedResultDto<BasicTourDto>> GetFavouriteTour(PagedInputDto input);
        Task<PagedResultDto<BasicTourDto>> GetFavouriteTripPlan(PagedInputDto input);
        Task RemoveFavouriteTour(long tourId);
        Task RemoveFavouriteTripPlan(long tripPlanId);

        Task AddFavouriteTour(long tourId);
        Task AddFavouriteTripPlan(long tripPlanId);

        Task AddFavouriteItem(long itemId);
        Task RemoveFavouriteItem(long itemId);
    }
}
