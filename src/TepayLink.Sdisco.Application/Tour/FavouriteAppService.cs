using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using SDisco.Home.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Account;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Products;
using System.Linq;

namespace TepayLink.Sdisco.Tour
{
    [AbpAuthorize()]
    public class FavouriteAppService : SdiscoAppServiceBase, IFavouriteAppService
    {
        private readonly IRepository<Product, long> _tourRepository;


       // private readonly IRepository<Place, long> _placeRepository;

        private readonly ICommonAppService _commonAppService;

        private readonly IRepository<SaveItem, long> _saveItemRepository;


        public async Task<PagedResultDto<BasicTourDto>> GetFavouriteTour(PagedInputDto input)
        {
            var query = from p in _tourRepository.GetAll()
                        join q in _saveItemRepository.GetAll() on p.Id equals q.ProductId
                      //  join place in _placeRepository.GetAll() on p.PlaceId equals place.Id
                        where p.Type == ProductTypeEnum.Tour && q.CreatorUserId == AbpSession.UserId

                        select new BasicTourDto
                        {
                            Id = p.Id,
                            Title = p.Name,
                            PlaceName = p.Name,
                            TripLength = p.TripLengh
                        };

            var total = query.Count();

            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in list)
            {
                item.Review = await _commonAppService.GetTourReviewSummary(item.Id);
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
            }

            return new PagedResultDto<BasicTourDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        public async Task<PagedResultDto<BasicTourDto>> GetFavouriteTripPlan(PagedInputDto input)
        {
            var query = from p in _tourRepository.GetAll()
                        join q in _saveItemRepository.GetAll() on p.Id equals q.ProductId
                     //   join place in _placeRepository.GetAll() on p.PlaceId equals place.Id
                        where p.Type == ProductTypeEnum.Trip && q.CreatorUserId == AbpSession.UserId

                        select new BasicTourDto
                        {
                            Id = p.Id,
                            Title = p.Name,
                            PlaceName = p.Name,
                            TripLength = p.TripLengh
                        };

            var total = query.Count();

            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in list)
            {
                item.Review = await _commonAppService.GetTourReviewSummary(item.Id);
                item.ThumbImages = await _commonAppService.GetTourThumbPhoto(item.Id);
            }

            return new PagedResultDto<BasicTourDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        public async Task RemoveFavouriteTour(long tourId)
        {
            _saveItemRepository.Delete(p =>
                p.ProductId == tourId && p.CreatorUserId == AbpSession.UserId);
        }

        public async Task RemoveFavouriteTripPlan(long tripPlanId)
        {
            _saveItemRepository.Delete(p =>
                p.ProductId == tripPlanId && p.CreatorUserId == AbpSession.UserId);
        }

        public async Task AddFavouriteTour(long tourId)
        {
            var saveItem = new SaveItem
            {
                ProductId = tourId,



            };
            _saveItemRepository.Insert(saveItem);

        }

        public async Task AddFavouriteTripPlan(long tripPlanId)
        {
            var saveItem = new SaveItem
            {
                ProductId = tripPlanId


            };
            _saveItemRepository.Insert(saveItem);

        }
        /// <summary>
        /// Add Favoutite
        /// </summary>
        /// <param name="itemId"> Id của item</param>
        /// <returns></returns>
        public async Task AddFavouriteItem(long itemId)
        {
            var saveItem = new SaveItem
            {
                ProductId = itemId

            };
            _saveItemRepository.Insert(saveItem);
        }

        public async Task RemoveFavouriteItem(long itemId)
        {
            _saveItemRepository.Delete(p =>
                p.ProductId == itemId && p.CreatorUserId == AbpSession.UserId);
        }

    }
}
