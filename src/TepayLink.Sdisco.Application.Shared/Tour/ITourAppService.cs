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
 public   interface ITourAppService: IApplicationService
    {
        Task<TourDetailDto> GetTourDetail(long tourId);

        Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input);

        Task<BasicTripPlanDto> GetTripPlan(GetTripPlanInput input);

        Task<List<BasicTourDto>> GetSimilarTour(long tourId);
        Task<List<BasicTourDto>> GetSuggestTripPlan(long tourId);
        Task<List<BasicTourCategoryDto>> GetCategory();
        Task<long> ChangeInfo(GetTripPlanInput input);
    }
}
