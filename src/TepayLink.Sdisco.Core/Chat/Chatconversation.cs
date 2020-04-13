using TepayLink.Sdisco.Chat;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TepayLink.Sdisco.Chat
{
	[Table("Chatconversations")]
    public class Chatconversation : AuditedEntity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long UserId { get; set; }
		
		public virtual long FriendUserId { get; set; }
		
		public virtual int UnreadCount { get; set; }
		
		public virtual string ShardChatConversationId { get; set; }
		
		public virtual long? BookingId { get; set; }
		
		public virtual string LastMessage { get; set; }
		
		public virtual ChatSide Side { get; set; }
		
		public virtual int Status { get; set; }
		

    }
}