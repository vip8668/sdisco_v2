using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TepayLink.Sdisco.Web.Areas.Admin.Models.Chatconversations;
using TepayLink.Sdisco.Web.Controllers;
using TepayLink.Sdisco.Authorization;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Chatconversations)]
    public class ChatconversationsController : SdiscoControllerBase
    {
        private readonly IChatconversationsAppService _chatconversationsAppService;

        public ChatconversationsController(IChatconversationsAppService chatconversationsAppService)
        {
            _chatconversationsAppService = chatconversationsAppService;
        }

        public ActionResult Index()
        {
            var model = new ChatconversationsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Chatconversations_Create, AppPermissions.Pages_Administration_Chatconversations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
			GetChatconversationForEditOutput getChatconversationForEditOutput;

			if (id.HasValue){
				getChatconversationForEditOutput = await _chatconversationsAppService.GetChatconversationForEdit(new EntityDto<long> { Id = (long) id });
			}
			else {
				getChatconversationForEditOutput = new GetChatconversationForEditOutput{
					Chatconversation = new CreateOrEditChatconversationDto()
				};
			}

            var viewModel = new CreateOrEditChatconversationModalViewModel()
            {
				Chatconversation = getChatconversationForEditOutput.Chatconversation
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewChatconversationModal(long id)
        {
			var getChatconversationForViewDto = await _chatconversationsAppService.GetChatconversationForView(id);

            var model = new ChatconversationViewModel()
            {
                Chatconversation = getChatconversationForViewDto.Chatconversation
            };

            return PartialView("_ViewChatconversationModal", model);
        }


    }
}