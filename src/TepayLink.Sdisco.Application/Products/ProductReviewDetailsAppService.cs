

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Products.Exporting;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Products
{
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails)]
    public class ProductReviewDetailsAppService : SdiscoAppServiceBase, IProductReviewDetailsAppService
    {
		 private readonly IRepository<ProductReviewDetail, long> _productReviewDetailRepository;
		 private readonly IProductReviewDetailsExcelExporter _productReviewDetailsExcelExporter;
		 

		  public ProductReviewDetailsAppService(IRepository<ProductReviewDetail, long> productReviewDetailRepository, IProductReviewDetailsExcelExporter productReviewDetailsExcelExporter ) 
		  {
			_productReviewDetailRepository = productReviewDetailRepository;
			_productReviewDetailsExcelExporter = productReviewDetailsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetProductReviewDetailForViewDto>> GetAll(GetAllProductReviewDetailsInput input)
         {
			
			var filteredProductReviewDetails = _productReviewDetailRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.ReplyComment.Contains(input.Filter) || e.Avatar.Contains(input.Filter) || e.Reviewer.Contains(input.Filter));

			var pagedAndFilteredProductReviewDetails = filteredProductReviewDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productReviewDetails = from o in pagedAndFilteredProductReviewDetails
                         select new GetProductReviewDetailForViewDto() {
							ProductReviewDetail = new ProductReviewDetailDto
							{
                                RatingAvg = o.RatingAvg,
                                Intineraty = o.Intineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Title = o.Title,
                                Comment = o.Comment,
                                BookingId = o.BookingId,
                                Read = o.Read,
                                ReplyComment = o.ReplyComment,
                                ReplyId = o.ReplyId,
                                Avatar = o.Avatar,
                                Reviewer = o.Reviewer,
                                Id = o.Id
							}
						};

            var totalCount = await filteredProductReviewDetails.CountAsync();

            return new PagedResultDto<GetProductReviewDetailForViewDto>(
                totalCount,
                await productReviewDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetProductReviewDetailForViewDto> GetProductReviewDetailForView(long id)
         {
            var productReviewDetail = await _productReviewDetailRepository.GetAsync(id);

            var output = new GetProductReviewDetailForViewDto { ProductReviewDetail = ObjectMapper.Map<ProductReviewDetailDto>(productReviewDetail) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Edit)]
		 public async Task<GetProductReviewDetailForEditOutput> GetProductReviewDetailForEdit(EntityDto<long> input)
         {
            var productReviewDetail = await _productReviewDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductReviewDetailForEditOutput {ProductReviewDetail = ObjectMapper.Map<CreateOrEditProductReviewDetailDto>(productReviewDetail)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductReviewDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Create)]
		 protected virtual async Task Create(CreateOrEditProductReviewDetailDto input)
         {
            var productReviewDetail = ObjectMapper.Map<ProductReviewDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				productReviewDetail.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productReviewDetailRepository.InsertAsync(productReviewDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditProductReviewDetailDto input)
         {
            var productReviewDetail = await _productReviewDetailRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productReviewDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductReviewDetails_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productReviewDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductReviewDetailsToExcel(GetAllProductReviewDetailsForExcelInput input)
         {
			
			var filteredProductReviewDetails = _productReviewDetailRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Title.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.ReplyComment.Contains(input.Filter) || e.Avatar.Contains(input.Filter) || e.Reviewer.Contains(input.Filter));

			var query = (from o in filteredProductReviewDetails
                         select new GetProductReviewDetailForViewDto() { 
							ProductReviewDetail = new ProductReviewDetailDto
							{
                                RatingAvg = o.RatingAvg,
                                Intineraty = o.Intineraty,
                                Service = o.Service,
                                Transport = o.Transport,
                                GuideTour = o.GuideTour,
                                Food = o.Food,
                                Title = o.Title,
                                Comment = o.Comment,
                                BookingId = o.BookingId,
                                Read = o.Read,
                                ReplyComment = o.ReplyComment,
                                ReplyId = o.ReplyId,
                                Avatar = o.Avatar,
                                Reviewer = o.Reviewer,
                                Id = o.Id
							}
						 });


            var productReviewDetailListDtos = await query.ToListAsync();

            return _productReviewDetailsExcelExporter.ExportToFile(productReviewDetailListDtos);
         }


    }
}