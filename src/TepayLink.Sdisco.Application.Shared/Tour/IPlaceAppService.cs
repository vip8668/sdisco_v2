using Abp.Application.Services.Dto;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public interface IPlaceAppService
    {
        Task<PlaceDetailDto> GetPlaceDetail(long placeId);
        Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input);

        Task<List<BasicTourItemDto>> GetRelatePlace(long placeId);

        Task<List<BasicTourDto>> GetSuggestTour(long activityId);

        //  Task<List<BasicActivityDto>> GetRelateActivity(long activityId);
    }
}
