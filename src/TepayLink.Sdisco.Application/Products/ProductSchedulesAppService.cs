using TepayLink.Sdisco.Products;


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
	[AbpAuthorize(AppPermissions.Pages_Administration_ProductSchedules)]
    public class ProductSchedulesAppService : SdiscoAppServiceBase, IProductSchedulesAppService
    {
		 private readonly IRepository<ProductSchedule, long> _productScheduleRepository;
		 private readonly IProductSchedulesExcelExporter _productSchedulesExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public ProductSchedulesAppService(IRepository<ProductSchedule, long> productScheduleRepository, IProductSchedulesExcelExporter productSchedulesExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_productScheduleRepository = productScheduleRepository;
			_productSchedulesExcelExporter = productSchedulesExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetProductScheduleForViewDto>> GetAll(GetAllProductSchedulesInput input)
         {
			
			var filteredProductSchedules = _productScheduleRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter) || e.DepartureTime.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredProductSchedules = filteredProductSchedules
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var productSchedules = from o in pagedAndFilteredProductSchedules
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductScheduleForViewDto() {
							ProductSchedule = new ProductScheduleDto
							{
                                TotalSlot = o.TotalSlot,
                                TotalBook = o.TotalBook,
                                LockedSlot = o.LockedSlot,
                                TripLength = o.TripLength,
                                Note = o.Note,
                                Price = o.Price,
                                TicketPrice = o.TicketPrice,
                                CostPrice = o.CostPrice,
                                HotelPrice = o.HotelPrice,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                DepartureTime = o.DepartureTime,
                                Revenue = o.Revenue,
                                AllowBook = o.AllowBook,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredProductSchedules.CountAsync();

            return new PagedResultDto<GetProductScheduleForViewDto>(
                totalCount,
                await productSchedules.ToListAsync()
            );
         }
		 
		 public async Task<GetProductScheduleForViewDto> GetProductScheduleForView(long id)
         {
            var productSchedule = await _productScheduleRepository.GetAsync(id);

            var output = new GetProductScheduleForViewDto { ProductSchedule = ObjectMapper.Map<ProductScheduleDto>(productSchedule) };

		    if (output.ProductSchedule.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductSchedule.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductSchedules_Edit)]
		 public async Task<GetProductScheduleForEditOutput> GetProductScheduleForEdit(EntityDto<long> input)
         {
            var productSchedule = await _productScheduleRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetProductScheduleForEditOutput {ProductSchedule = ObjectMapper.Map<CreateOrEditProductScheduleDto>(productSchedule)};

		    if (output.ProductSchedule.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductSchedule.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditProductScheduleDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductSchedules_Create)]
		 protected virtual async Task Create(CreateOrEditProductScheduleDto input)
         {
            var productSchedule = ObjectMapper.Map<ProductSchedule>(input);

			
			if (AbpSession.TenantId != null)
			{
				productSchedule.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _productScheduleRepository.InsertAsync(productSchedule);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductSchedules_Edit)]
		 protected virtual async Task Update(CreateOrEditProductScheduleDto input)
         {
            var productSchedule = await _productScheduleRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, productSchedule);
         }

		 [AbpAuthorize(AppPermissions.Pages_Administration_ProductSchedules_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _productScheduleRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetProductSchedulesToExcel(GetAllProductSchedulesForExcelInput input)
         {
			
			var filteredProductSchedules = _productScheduleRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Note.Contains(input.Filter) || e.DepartureTime.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredProductSchedules
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetProductScheduleForViewDto() { 
							ProductSchedule = new ProductScheduleDto
							{
                                TotalSlot = o.TotalSlot,
                                TotalBook = o.TotalBook,
                                LockedSlot = o.LockedSlot,
                                TripLength = o.TripLength,
                                Note = o.Note,
                                Price = o.Price,
                                TicketPrice = o.TicketPrice,
                                CostPrice = o.CostPrice,
                                HotelPrice = o.HotelPrice,
                                StartDate = o.StartDate,
                                EndDate = o.EndDate,
                                DepartureTime = o.DepartureTime,
                                Revenue = o.Revenue,
                                AllowBook = o.AllowBook,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var productScheduleListDtos = await query.ToListAsync();

            return _productSchedulesExcelExporter.ExportToFile(productScheduleListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Administration_ProductSchedules)]
         public async Task<PagedResultDto<ProductScheduleProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ProductScheduleProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new ProductScheduleProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<ProductScheduleProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}