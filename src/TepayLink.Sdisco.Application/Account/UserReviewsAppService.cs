

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Account.Exporting;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Account
{
	[AbpAuthorize(AppPermissions.Pages_Administration_UserReviews)]
    public class UserReviewsAppService : SdiscoAppServiceBase, IUserReviewsAppService
    {
		 private readonly IRepository<UserReview> _userReviewRepository;
		 private readonly IUserReviewsExcelExporter _userReviewsExcelExporter;
		 

		  public UserReviewsAppService(IRepository<UserReview> userReviewRepository, IUserReviewsExcelExporter userReviewsExcelExporter ) 
		  {
			_userReviewRepository = userReviewRepository;
			_userReviewsExcelExporter = userReviewsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetUserReviewForViewDto>> GetAll(GetAllUserReviewsInput input)
         {
			
			var filteredUserReviews = _userReviewRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false );

			var pagedAndFilteredUserReviews = filteredUserReviews
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userReviews = from o in pagedAndFilteredUserReviews
                         select new GetUserReviewForViewDto() {
							UserReview = new UserReviewDto
							{
                                UserId = o.UserId,
                                ReviewCount = o.ReviewCount,
                                Itineraty = o.Itineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Rating = o.Rating,
                                Id = o.Id
							}
						};

            var totalCount = await filteredUserReviews.CountAsync();

            return new PagedResultDto<GetUserReviewForViewDto>(
                totalCount,
                await userReviews.ToListAsync()
            );
         }
		 
		 public async Task<GetUserReviewForViewDto> GetUserReviewForView(int id)
         {
            var userReview = await _userReviewRepository.GetAsync(id);

            var output = new GetUserReviewForViewDto { UserReview = ObjectMapper.Map<UserReviewDto>(userReview) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviews_Edit)]
		 public async Task<GetUserReviewForEditOutput> GetUserReviewForEdit(EntityDto input)
         {
            var userReview = await _userReviewRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserReviewForEditOutput {UserReview = ObjectMapper.Map<CreateOrEditUserReviewDto>(userReview)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserReviewDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviews_Create)]
		 protected virtual async Task Create(CreateOrEditUserReviewDto input)
         {
            var userReview = ObjectMapper.Map<UserReview>(input);

			
			if (AbpSession.TenantId != null)
			{
				userReview.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _userReviewRepository.InsertAsync(userReview);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviews_Edit)]
		 protected virtual async Task Update(CreateOrEditUserReviewDto input)
         {
            var userReview = await _userReviewRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, userReview);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviews_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _userReviewRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserReviewsToExcel(GetAllUserReviewsForExcelInput input)
         {
			
			var filteredUserReviews = _userReviewRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false );

			var query = (from o in filteredUserReviews
                         select new GetUserReviewForViewDto() { 
							UserReview = new UserReviewDto
							{
                                UserId = o.UserId,
                                ReviewCount = o.ReviewCount,
                                Itineraty = o.Itineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Rating = o.Rating,
                                Id = o.Id
							}
						 });


            var userReviewListDtos = await query.ToListAsync();

            return _userReviewsExcelExporter.ExportToFile(userReviewListDtos);
         }


    }
}