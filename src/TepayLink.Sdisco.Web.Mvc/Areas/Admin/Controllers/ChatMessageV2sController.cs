using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.ChatMessageV2s;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_ChatMessageV2s)]
    public class ChatMessageV2sController : SdiscoControllerBase
    {
        private readonly IChatMessageV2sAppService _chatMessageV2sAppService;

        public ChatMessageV2sController(IChatMessageV2sAppService chatMessageV2sAppService)
        {
            _chatMessageV2sAppService = chatMessageV2sAppService;
        }

        public ActionResult Index()
        {
            var model = new ChatMessageV2sViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_ChatMessageV2s_Create, AppPermissions.Pages_ChatMessageV2s_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetChatMessageV2ForEditOutput getChatMessageV2ForEditOutput;

			if (id.HasValue){
				getChatMessageV2ForEditOutput = await _chatMessageV2sAppService.GetChatMessageV2ForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getChatMessageV2ForEditOutput = new GetChatMessageV2ForEditOutput{
					ChatMessageV2 = new CreateOrEditChatMessageV2Dto()
				};
				getChatMessageV2ForEditOutput.ChatMessageV2.CreationTime = DateTime.Now;
			}

            var viewModel = new CreateOrEditChatMessageV2ModalViewModel()
            {
				ChatMessageV2 = getChatMessageV2ForEditOutput.ChatMessageV2
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewChatMessageV2Modal(long id)
        {
			var getChatMessageV2ForViewDto = await _chatMessageV2sAppService.GetChatMessageV2ForView(id);

            var model = new ChatMessageV2ViewModel()
            {
                ChatMessageV2 = getChatMessageV2ForViewDto.ChatMessageV2
            };

            return PartialView("_ViewChatMessageV2Modal", model);
        }


    }
}