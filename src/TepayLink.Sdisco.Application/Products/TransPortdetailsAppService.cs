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
	[AbpAuthorize(AppPermissions.Pages_TransPortdetails)]
    public class TransPortdetailsAppService : SdiscoAppServiceBase, ITransPortdetailsAppService
    {
		 private readonly IRepository<TransPortdetail, long> _transPortdetailRepository;
		 private readonly ITransPortdetailsExcelExporter _transPortdetailsExcelExporter;
		 private readonly IRepository<Product,long> _lookup_productRepository;
		 

		  public TransPortdetailsAppService(IRepository<TransPortdetail, long> transPortdetailRepository, ITransPortdetailsExcelExporter transPortdetailsExcelExporter , IRepository<Product, long> lookup_productRepository) 
		  {
			_transPortdetailRepository = transPortdetailRepository;
			_transPortdetailsExcelExporter = transPortdetailsExcelExporter;
			_lookup_productRepository = lookup_productRepository;
		
		  }

		 public async Task<PagedResultDto<GetTransPortdetailForViewDto>> GetAll(GetAllTransPortdetailsInput input)
         {
			
			var filteredTransPortdetails = _transPortdetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.From.Contains(input.Filter) || e.To.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.FromFilter),  e => e.From == input.FromFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ToFilter),  e => e.To == input.ToFilter)
						.WhereIf(input.MinTotalSeatFilter != null, e => e.TotalSeat >= input.MinTotalSeatFilter)
						.WhereIf(input.MaxTotalSeatFilter != null, e => e.TotalSeat <= input.MaxTotalSeatFilter)
						.WhereIf(input.IsTaxiFilter > -1,  e => (input.IsTaxiFilter == 1 && e.IsTaxi) || (input.IsTaxiFilter == 0 && !e.IsTaxi) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var pagedAndFilteredTransPortdetails = filteredTransPortdetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var transPortdetails = from o in pagedAndFilteredTransPortdetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTransPortdetailForViewDto() {
							TransPortdetail = new TransPortdetailDto
							{
                                From = o.From,
                                To = o.To,
                                TotalSeat = o.TotalSeat,
                                IsTaxi = o.IsTaxi,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredTransPortdetails.CountAsync();

            return new PagedResultDto<GetTransPortdetailForViewDto>(
                totalCount,
                await transPortdetails.ToListAsync()
            );
         }
		 
		 public async Task<GetTransPortdetailForViewDto> GetTransPortdetailForView(long id)
         {
            var transPortdetail = await _transPortdetailRepository.GetAsync(id);

            var output = new GetTransPortdetailForViewDto { TransPortdetail = ObjectMapper.Map<TransPortdetailDto>(transPortdetail) };

		    if (output.TransPortdetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.TransPortdetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TransPortdetails_Edit)]
		 public async Task<GetTransPortdetailForEditOutput> GetTransPortdetailForEdit(EntityDto<long> input)
         {
            var transPortdetail = await _transPortdetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTransPortdetailForEditOutput {TransPortdetail = ObjectMapper.Map<CreateOrEditTransPortdetailDto>(transPortdetail)};

		    if (output.TransPortdetail.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.TransPortdetail.ProductId);
                output.ProductName = _lookupProduct.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTransPortdetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TransPortdetails_Create)]
		 protected virtual async Task Create(CreateOrEditTransPortdetailDto input)
         {
            var transPortdetail = ObjectMapper.Map<TransPortdetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				transPortdetail.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _transPortdetailRepository.InsertAsync(transPortdetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_TransPortdetails_Edit)]
		 protected virtual async Task Update(CreateOrEditTransPortdetailDto input)
         {
            var transPortdetail = await _transPortdetailRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, transPortdetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_TransPortdetails_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _transPortdetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTransPortdetailsToExcel(GetAllTransPortdetailsForExcelInput input)
         {
			
			var filteredTransPortdetails = _transPortdetailRepository.GetAll()
						.Include( e => e.ProductFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.From.Contains(input.Filter) || e.To.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.FromFilter),  e => e.From == input.FromFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ToFilter),  e => e.To == input.ToFilter)
						.WhereIf(input.MinTotalSeatFilter != null, e => e.TotalSeat >= input.MinTotalSeatFilter)
						.WhereIf(input.MaxTotalSeatFilter != null, e => e.TotalSeat <= input.MaxTotalSeatFilter)
						.WhereIf(input.IsTaxiFilter > -1,  e => (input.IsTaxiFilter == 1 && e.IsTaxi) || (input.IsTaxiFilter == 0 && !e.IsTaxi) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

			var query = (from o in filteredTransPortdetails
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTransPortdetailForViewDto() { 
							TransPortdetail = new TransPortdetailDto
							{
                                From = o.From,
                                To = o.To,
                                TotalSeat = o.TotalSeat,
                                IsTaxi = o.IsTaxi,
                                Id = o.Id
							},
                         	ProductName = s1 == null ? "" : s1.Name.ToString()
						 });


            var transPortdetailListDtos = await query.ToListAsync();

            return _transPortdetailsExcelExporter.ExportToFile(transPortdetailListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TransPortdetails)]
         public async Task<PagedResultDto<TransPortdetailProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_productRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TransPortdetailProductLookupTableDto>();
			foreach(var product in productList){
				lookupTableDtoList.Add(new TransPortdetailProductLookupTableDto
				{
					Id = product.Id,
					DisplayName = product.Name?.ToString()
				});
			}

            return new PagedResultDto<TransPortdetailProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}