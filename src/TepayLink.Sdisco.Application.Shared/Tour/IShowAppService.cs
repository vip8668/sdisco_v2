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
    public interface IShowAppService : IApplicationService
    {
        Task<ShowDetailDetailDto> GetShowDetail(long showId);
        Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input);
        Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input);

        Task<List<BasicTourItemDto>> GetPopularShow(long showId);
        Task<List<BasicTourDto>> GetRelateTour(long showId);
    }
}
