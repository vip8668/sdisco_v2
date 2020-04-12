using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Products;


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
	[AbpAuthorize(AppPermissions.Pages_Partners)]
    public class PartnersAppService : SdiscoAppServiceBase, IPartnersAppService
    {
		 private readonly IRepository<Partner, long> _partnerRepository;
		 private readonly IPartnersExcelExporter _partnersExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<Detination,long> _lookup_detinationRepository;
		 

		  public PartnersAppService(IRepository<Partner, long> partnerRepository, IPartnersExcelExporter partnersExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<Detination, long> lookup_detinationRepository) 
		  {
			_partnerRepository = partnerRepository;
			_partnersExcelExporter = partnersExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_detinationRepository = lookup_detinationRepository;
		
		  }

		 public async Task<PagedResultDto<GetPartnerForViewDto>> GetAll(GetAllPartnersInput input)
         {
			
			var filteredPartners = _partnerRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DetinationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.WebsiteUrl.Contains(input.Filter) || e.Languages.Contains(input.Filter) || e.SkypeId.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.AffiliateKey.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteUrlFilter),  e => e.WebsiteUrl == input.WebsiteUrlFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LanguagesFilter),  e => e.Languages == input.LanguagesFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SkypeIdFilter),  e => e.SkypeId == input.SkypeIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter),  e => e.Comment == input.CommentFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AffiliateKeyFilter),  e => e.AffiliateKey == input.AffiliateKeyFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
						.WhereIf(input.AlreadyBecomeSdiscoPartnerFilter > -1,  e => (input.AlreadyBecomeSdiscoPartnerFilter == 1 && e.AlreadyBecomeSdiscoPartner) || (input.AlreadyBecomeSdiscoPartnerFilter == 0 && !e.AlreadyBecomeSdiscoPartner) )
						.WhereIf(input.HasDriverLicenseFilter > -1,  e => (input.HasDriverLicenseFilter == 1 && e.HasDriverLicense) || (input.HasDriverLicenseFilter == 0 && !e.HasDriverLicense) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DetinationNameFilter), e => e.DetinationFk != null && e.DetinationFk.Name == input.DetinationNameFilter);

			var pagedAndFilteredPartners = filteredPartners
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var partners = from o in pagedAndFilteredPartners
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_detinationRepository.GetAll() on o.DetinationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetPartnerForViewDto() {
							Partner = new PartnerDto
							{
                                Name = o.Name,
                                WebsiteUrl = o.WebsiteUrl,
                                Languages = o.Languages,
                                SkypeId = o.SkypeId,
                                Comment = o.Comment,
                                AffiliateKey = o.AffiliateKey,
                                Status = o.Status,
                                AlreadyBecomeSdiscoPartner = o.AlreadyBecomeSdiscoPartner,
                                HasDriverLicense = o.HasDriverLicense,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	DetinationName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredPartners.CountAsync();

            return new PagedResultDto<GetPartnerForViewDto>(
                totalCount,
                await partners.ToListAsync()
            );
         }
		 
		 public async Task<GetPartnerForViewDto> GetPartnerForView(long id)
         {
            var partner = await _partnerRepository.GetAsync(id);

            var output = new GetPartnerForViewDto { Partner = ObjectMapper.Map<PartnerDto>(partner) };

		    if (output.Partner.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Partner.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Partner.DetinationId != null)
            {
                var _lookupDetination = await _lookup_detinationRepository.FirstOrDefaultAsync((long)output.Partner.DetinationId);
                output.DetinationName = _lookupDetination.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Partners_Edit)]
		 public async Task<GetPartnerForEditOutput> GetPartnerForEdit(EntityDto<long> input)
         {
            var partner = await _partnerRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPartnerForEditOutput {Partner = ObjectMapper.Map<CreateOrEditPartnerDto>(partner)};

		    if (output.Partner.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Partner.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }

		    if (output.Partner.DetinationId != null)
            {
                var _lookupDetination = await _lookup_detinationRepository.FirstOrDefaultAsync((long)output.Partner.DetinationId);
                output.DetinationName = _lookupDetination.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPartnerDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Partners_Create)]
		 protected virtual async Task Create(CreateOrEditPartnerDto input)
         {
            var partner = ObjectMapper.Map<Partner>(input);

			
			if (AbpSession.TenantId != null)
			{
				partner.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _partnerRepository.InsertAsync(partner);
         }

		 [AbpAuthorize(AppPermissions.Pages_Partners_Edit)]
		 protected virtual async Task Update(CreateOrEditPartnerDto input)
         {
            var partner = await _partnerRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, partner);
         }

		 [AbpAuthorize(AppPermissions.Pages_Partners_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _partnerRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPartnersToExcel(GetAllPartnersForExcelInput input)
         {
			
			var filteredPartners = _partnerRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DetinationFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.WebsiteUrl.Contains(input.Filter) || e.Languages.Contains(input.Filter) || e.SkypeId.Contains(input.Filter) || e.Comment.Contains(input.Filter) || e.AffiliateKey.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteUrlFilter),  e => e.WebsiteUrl == input.WebsiteUrlFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LanguagesFilter),  e => e.Languages == input.LanguagesFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SkypeIdFilter),  e => e.SkypeId == input.SkypeIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter),  e => e.Comment == input.CommentFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AffiliateKeyFilter),  e => e.AffiliateKey == input.AffiliateKeyFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter)
						.WhereIf(input.AlreadyBecomeSdiscoPartnerFilter > -1,  e => (input.AlreadyBecomeSdiscoPartnerFilter == 1 && e.AlreadyBecomeSdiscoPartner) || (input.AlreadyBecomeSdiscoPartnerFilter == 0 && !e.AlreadyBecomeSdiscoPartner) )
						.WhereIf(input.HasDriverLicenseFilter > -1,  e => (input.HasDriverLicenseFilter == 1 && e.HasDriverLicense) || (input.HasDriverLicenseFilter == 0 && !e.HasDriverLicense) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DetinationNameFilter), e => e.DetinationFk != null && e.DetinationFk.Name == input.DetinationNameFilter);

			var query = (from o in filteredPartners
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_detinationRepository.GetAll() on o.DetinationId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetPartnerForViewDto() { 
							Partner = new PartnerDto
							{
                                Name = o.Name,
                                WebsiteUrl = o.WebsiteUrl,
                                Languages = o.Languages,
                                SkypeId = o.SkypeId,
                                Comment = o.Comment,
                                AffiliateKey = o.AffiliateKey,
                                Status = o.Status,
                                AlreadyBecomeSdiscoPartner = o.AlreadyBecomeSdiscoPartner,
                                HasDriverLicense = o.HasDriverLicense,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString(),
                         	DetinationName = s2 == null ? "" : s2.Name.ToString()
						 });


            var partnerListDtos = await query.ToListAsync();

            return _partnersExcelExporter.ExportToFile(partnerListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Partners)]
         public async Task<PagedResultDto<PartnerUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PartnerUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new PartnerUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<PartnerUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_Partners)]
         public async Task<PagedResultDto<PartnerDetinationLookupTableDto>> GetAllDetinationForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_detinationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var detinationList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<PartnerDetinationLookupTableDto>();
			foreach(var detination in detinationList){
				lookupTableDtoList.Add(new PartnerDetinationLookupTableDto
				{
					Id = detination.Id,
					DisplayName = detination.Name?.ToString()
				});
			}

            return new PagedResultDto<PartnerDetinationLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}