using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Tour
{
    public class ThingToBuyAppService : SdiscoAppServiceBase, IThingToBuyAppService
    {

        private readonly IRepository<Product, long> _tourRepository;

        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ApplicationLanguage> _langRepository;

        private readonly IRepository<Place, long> _placeRepository;
        private readonly IRepository<ProductSchedule, long> _itemScheduleRepository;
        private readonly IRepository<Category> _tourItemCategoryRepository;
        private readonly ICommonAppService _commonAppService;

        public ThingToBuyAppService(IRepository<Product, long> tourRepository, IRepository<User, long> userRepository, IRepository<ApplicationLanguage> langRepository, IRepository<Place, long> placeRepository, IRepository<ProductSchedule, long> itemScheduleRepository, IRepository<Category> tourItemCategoryRepository, ICommonAppService commonAppService)
        {
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _langRepository = langRepository;
            _placeRepository = placeRepository;
            _itemScheduleRepository = itemScheduleRepository;
            _tourItemCategoryRepository = tourItemCategoryRepository;
            _commonAppService = commonAppService;
        }

        public async Task<ThingToBuyDetailDto> GetProductDetail(long id)
        {
            var itemSchedule = _itemScheduleRepository.GetAll().Where(p => p.ProductId == id).OrderBy(p => p.Price)
                .FirstOrDefault();

            var tourItem = (from p in _tourRepository.GetAll()
                            join l in _langRepository.GetAll() on p.LanguageId equals l.Id
                            join place in _placeRepository.GetAll() on p.PlaceId equals place.Id
                            join user in _userRepository.GetAll() on p.CreatorUserId equals user.Id
                            join cate in _tourItemCategoryRepository.GetAll() on p.CategoryId equals cate.Id
                            where p.Id == id
                            select new ThingToBuyDetailDto
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Policies = p.Policies,

                                Overview = p.Description,
                                BookCount = p.BookingCount,
                                HowtoUse = p.ExtraData,
                                InstantBook = p.InstantBook
                            }).FirstOrDefault();
            if (tourItem == null)
                return null;
            tourItem.Price =
                new BasicPriceDto
                {
                    Price = itemSchedule != null ? itemSchedule.Price : 0,
                    ServiceFee = 0,
                    OldPrice = 0,
                };
            tourItem.Images = await _commonAppService.GetTourItemPhoto(id);
            tourItem.IsLove = await _commonAppService.IsSave(id);
            tourItem.Review = await _commonAppService.GetTourItemReviewSummary(id);
            return tourItem;
        }
        public async Task<PagedResultDto<ReviewDetailDto>> GetReviewDetail(GetReviewDetailInput input)
        {
            return await _commonAppService.GetTourItemReviewDetail(input);
        }

        public async Task<GuestPhotoDto> GetGuestPhoto(GetGuestPhotoInput input)
        {
            return await _commonAppService.GetTourItemGuestPhoto(input);
        }



    }
}
