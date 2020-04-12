using TepayLink.Sdisco.Chat.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.Chatconversations
{
    public class CreateOrEditChatconversationModalViewModel
    {
       public CreateOrEditChatconversationDto Chatconversation { get; set; }

	   
	   public bool IsEditMode => Chatconversation.Id.HasValue;
    }
}