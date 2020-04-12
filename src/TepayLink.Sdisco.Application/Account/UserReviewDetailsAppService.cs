

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
	[AbpAuthorize(AppPermissions.Pages_Administration_UserReviewDetails)]
    public class UserReviewDetailsAppService : SdiscoAppServiceBase, IUserReviewDetailsAppService
    {
		 private readonly IRepository<UserReviewDetail, long> _userReviewDetailRepository;
		 private readonly IUserReviewDetailsExcelExporter _userReviewDetailsExcelExporter;
		 

		  public UserReviewDetailsAppService(IRepository<UserReviewDetail, long> userReviewDetailRepository, IUserReviewDetailsExcelExporter userReviewDetailsExcelExporter ) 
		  {
			_userReviewDetailRepository = userReviewDetailRepository;
			_userReviewDetailsExcelExporter = userReviewDetailsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetUserReviewDetailForViewDto>> GetAll(GetAllUserReviewDetailsInput input)
         {
			
			var filteredUserReviewDetails = _userReviewDetailRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter));

			var pagedAndFilteredUserReviewDetails = filteredUserReviewDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userReviewDetails = from o in pagedAndFilteredUserReviewDetails
                         select new GetUserReviewDetailForViewDto() {
							UserReviewDetail = new UserReviewDetailDto
							{
                                UserId = o.UserId,
                                Itineraty = o.Itineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Rating = o.Rating,
                                Title = o.Title,
                                Comment = o.Comment,
                                Id = o.Id
							}
						};

            var totalCount = await filteredUserReviewDetails.CountAsync();

            return new PagedResultDto<GetUserReviewDetailForViewDto>(
                totalCount,
                await userReviewDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetUserReviewDetailForViewDto> GetUserReviewDetailForView(long id)
         {
            var userReviewDetail = await _userReviewDetailRepository.GetAsync(id);

            var output = new GetUserReviewDetailForViewDto { UserReviewDetail = ObjectMapper.Map<UserReviewDetailDto>(userReviewDetail) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviewDetails_Edit)]
		 public async Task<GetUserReviewDetailForEditOutput> GetUserReviewDetailForEdit(EntityDto<long> input)
         {
            var userReviewDetail = await _userReviewDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserReviewDetailForEditOutput {UserReviewDetail = ObjectMapper.Map<CreateOrEditUserReviewDetailDto>(userReviewDetail)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserReviewDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviewDetails_Create)]
		 protected virtual async Task Create(CreateOrEditUserReviewDetailDto input)
         {
            var userReviewDetail = ObjectMapper.Map<UserReviewDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				userReviewDetail.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _userReviewDetailRepository.InsertAsync(userReviewDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviewDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditUserReviewDetailDto input)
         {
            var userReviewDetail = await _userReviewDetailRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, userReviewDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_UserReviewDetails_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _userReviewDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserReviewDetailsToExcel(GetAllUserReviewDetailsForExcelInput input)
         {
			
			var filteredUserReviewDetails = _userReviewDetailRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter));

			var query = (from o in filteredUserReviewDetails
                         select new GetUserReviewDetailForViewDto() { 
							UserReviewDetail = new UserReviewDetailDto
							{
                                UserId = o.UserId,
                                Itineraty = o.Itineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Rating = o.Rating,
                                Title = o.Title,
                                Comment = o.Comment,
                                Id = o.Id
							}
						 });


            var userReviewDetailListDtos = await query.ToListAsync();

            return _userReviewDetailsExcelExporter.ExportToFile(userReviewDetailListDtos);
         }


    }
}