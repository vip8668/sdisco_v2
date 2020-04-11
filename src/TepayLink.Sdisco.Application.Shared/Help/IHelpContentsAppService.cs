using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Help
{
    public interface IHelpContentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetHelpContentForViewDto>> GetAll(GetAllHelpContentsInput input);

        Task<GetHelpContentForViewDto> GetHelpContentForView(long id);

		Task<GetHelpContentForEditOutput> GetHelpContentForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditHelpContentDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetHelpContentsToExcel(GetAllHelpContentsForExcelInput input);

		
		Task<PagedResultDto<HelpContentHelpCategoryLookupTableDto>> GetAllHelpCategoryForLookupTable(GetAllForLookupTableInput input);
		
    }
}