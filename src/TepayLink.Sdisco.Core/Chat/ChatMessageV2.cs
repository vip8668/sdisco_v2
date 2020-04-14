using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp;
using Abp.Timing;

namespace TepayLink.Sdisco.Chat
{
	[Table("ChatMessageV2s")]
    public class ChatMessageV2 : Entity<long> , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		public virtual long ChatConversationId { get; set; }
		
		public virtual long UserId { get; set; }
		
		public virtual string Message { get; set; }
		
		public virtual DateTime CreationTime { get; set; }
		
		public virtual ChatSide Side { get; set; }
		
		public virtual ChatMessageReadState ReadState { get; set; }
		
		public virtual ChatMessageReadState ReceiverReadState { get; set; }
		
		public virtual Guid SharedMessageId { get; set; }

        public ChatMessageV2(
           UserIdentifier user,
           UserIdentifier targetUser,
           ChatSide side,
           string message,
           ChatMessageReadState readState,
           Guid sharedMessageId,
           ChatMessageReadState receiverReadState)
        {
            UserId = user.UserId;
            TenantId = user.TenantId;
            //   TargetUserId = targetUser.UserId;
            //  TargetTenantId = targetUser.TenantId;
            Message = message;
            Side = side;
            ReadState = readState;
            SharedMessageId = sharedMessageId;
            ReceiverReadState = receiverReadState;

            CreationTime = Clock.Now;
        }

        public void ChangeReadState(ChatMessageReadState newState)
        {
            ReadState = newState;
        }

        public ChatMessageV2()
        {
        }

        public void ChangeReceiverReadState(ChatMessageReadState newState)
        {
            ReceiverReadState = newState;
        }


    }
}