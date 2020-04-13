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
  public  interface IActivityAppService: IApplicationService
    {
        Task<ActivityDetailDto> GetActivityDetail(long activityId);
        Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input);
        Task<List<BasicTourItemDto>> GetRelateActivity(long activityId);

        Task<List<BasicTourDto>> GetSuggestTour(long activityId);

        Task<LastBookedDto> GetLastBooked(long activityId);
    }
}
