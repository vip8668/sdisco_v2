using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public interface ITripPlanAppService : IApplicationService
    {
        Task<TourDetailDto> GetTripPlanDetail(long tripPlanId, DateTime? date = null);

        Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input);

        //    Task<BasicTripPlanDto> GetTripPlan(GetTripPlanInput input);

        Task<List<BasicTourDto>> GetSimilarTripPlan(long tripPlanId);
        Task<List<BasicTourDto>> GetSuggestTour(long tripPlanId);
        //  Task<List<BasicTourCategoryDto>>  GetCategory();
    }
}
