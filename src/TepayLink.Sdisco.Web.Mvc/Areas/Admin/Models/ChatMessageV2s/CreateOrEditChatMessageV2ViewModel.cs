using TepayLink.Sdisco.Chat.Dtos;
using Abp.Extensions;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.ChatMessageV2s
{
    public class CreateOrEditChatMessageV2ModalViewModel
    {
       public CreateOrEditChatMessageV2Dto ChatMessageV2 { get; set; }

	   
	   public bool IsEditMode => ChatMessageV2.Id.HasValue;
    }
}