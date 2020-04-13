using Abp;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Timing;
using Abp.UI;
using FireSharp;
using FireSharp.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Authorization.Users;
using TepayLink.Sdisco.Bookings;
using TepayLink.Sdisco.Chat.Dto.V2;
using TepayLink.Sdisco.Configuration;
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Chat
{
    public class ChatV2AppService : SdiscoAppServiceBase, IChatV2AppService
    {
        private readonly IRepository<ChatMessageV2, long> _chatMessageRepository;
        private readonly IRepository<Chatconversation, long> _chatConversationRepository;
        private readonly IRepository<Product, long> _tourRepository;
        private readonly IRepository<Booking, long> _bookingRepository;
        private readonly IRepository<User, long> _userRepository;
        private static List<long> _userOnline;

        private static FirebaseClient _firebaseClient;

        public ChatV2AppService(IRepository<ChatMessageV2, long> chatMessageRepository,
           IRepository<Chatconversation, long> chatConversationRepository, IRepository<Product, long> tourRepository,
           IRepository<Booking, long> bookingRepository, IRepository<User, long> userRepository,
           IWebHostEnvironment env)
        {
            _chatMessageRepository = chatMessageRepository;
            _chatConversationRepository = chatConversationRepository;
            _tourRepository = tourRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            if (_userOnline == null)
                _userOnline = new List<long>();

            if (_firebaseClient == null)
            {
                IConfigurationRoot _configuration = env.GetAppConfiguration();
                _firebaseClient = new FirebaseClient(new FirebaseConfig
                {
                    AuthSecret = _configuration["Firebase:AuthSecret"],
                    BasePath = _configuration["Firebase:BasePath"],
                });
            }
        }



        /// <summary>
        /// gửi tin nhắn 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        /// <exception cref="Exception"></exception>
        [UnitOfWork]
        public async Task<ChatMessageV2Dto> SendMessage(SendChatMessageV2Dto input)
        {
            var receiver = new UserIdentifier(AbpSession.TenantId, input.UserId);

            try
            {
                Chatconversation senderChatConversation = null;
                Chatconversation recChatConversation = null;

                if (string.IsNullOrEmpty(input.ChatConversationId))
                {
                    var connversation = _chatConversationRepository.FirstOrDefault(p =>
                        p.UserId == AbpSession.UserId && p.FriendUserId == input.UserId &&
                        p.BookingId == input.BookingId);
                    if (connversation != null)
                        input.ChatConversationId = connversation.ShardChatConversationId;
                }

                //
                if (string.IsNullOrEmpty(input.ChatConversationId))
                {
                    var id = Guid.NewGuid().ToString();
                    senderChatConversation = new Chatconversation
                    {
                        BookingId = input.BookingId,
                        UserId = AbpSession.UserId ?? 0,
                        LastMessage = input.Message,
                        Side = ChatSide.Sender,
                        ShardChatConversationId = id,
                        FriendUserId = input.UserId,
                        Status = 1
                    };
                    recChatConversation = new Chatconversation
                    {
                        BookingId = input.BookingId,
                        UserId = input.UserId,
                        LastMessage = input.Message,
                        Side = ChatSide.Receiver,
                        ShardChatConversationId = id,
                        FriendUserId = AbpSession.UserId ?? 0,
                        UnreadCount = 1,
                        Status = 1
                    };
                    senderChatConversation.Id = _chatConversationRepository.InsertAndGetId(senderChatConversation);
                    recChatConversation.Id = _chatConversationRepository.InsertAndGetId(recChatConversation);
                    input.ChatConversationId = id;
                }
                else
                {
                    var list = _chatConversationRepository.GetAll()
                        .Where(p => p.ShardChatConversationId == input.ChatConversationId).ToList();

                    senderChatConversation = list.FirstOrDefault(p => p.UserId == AbpSession.UserId);
                    recChatConversation = list.FirstOrDefault(p => p.UserId == input.UserId);
                    senderChatConversation.UnreadCount = 0;
                    if (!string.IsNullOrEmpty(input.Message))
                    {
                        senderChatConversation.LastMessage = input.Message;
                        recChatConversation.LastMessage = input.Message;
                        recChatConversation.UnreadCount = recChatConversation.UnreadCount + 1;
                    }

                    senderChatConversation.Status = 1;
                    recChatConversation.Status = 1;
                    _chatConversationRepository.Update(senderChatConversation);
                    _chatConversationRepository.Update(recChatConversation);
                }

                if (!string.IsNullOrEmpty(input.Message))
                {
                    var mesId = Guid.NewGuid();
                    var senderMessage = new ChatMessageV2
                    {
                        Message = input.Message,
                        Side = ChatSide.Sender,
                        UserId = AbpSession.UserId ?? 0,
                        SharedMessageId = mesId,
                        ChatConversationId = senderChatConversation.Id,
                        CreationTime = Clock.Now,
                    };


                    var recMessage = new ChatMessageV2
                    {
                        Message = input.Message,
                        Side = ChatSide.Receiver,
                        UserId = input.UserId,
                        SharedMessageId = mesId,
                        ChatConversationId = recChatConversation.Id,
                        CreationTime = Clock.Now,
                    };
                    recMessage.Id = _chatMessageRepository.InsertAndGetId(recMessage);


                    senderMessage.Id = _chatMessageRepository.InsertAndGetId(senderMessage);


                    var recdto = new FirebaseMessageDto
                    {
                        userId = recMessage.UserId,
                        chatConversationId = input.ChatConversationId,
                        id = recMessage.Id,
                        creationTime = recMessage.CreationTime,
                        message = recMessage.Message,
                        side = recMessage.Side,
                    };
                    var sendDto = new FirebaseMessageDto
                    {
                        userId = senderMessage.UserId,
                        chatConversationId = input.ChatConversationId,
                        id = senderMessage.Id,
                        creationTime = senderMessage.CreationTime,
                        message = senderMessage.Message,
                        side = senderMessage.Side,
                    };

                    _firebaseClient.SetTaskAsync($"message/{recdto.userId}/{recdto.chatConversationId}/" + recdto.id,
                        recdto);
                    _firebaseClient.SetTaskAsync($"message/{sendDto.userId}/{sendDto.chatConversationId}/" + sendDto.id,
                        sendDto);

                    return new ChatMessageV2Dto
                    {
                        Id = senderMessage.Id,
                        CreationTime = DateTime.Now,
                        Message = senderMessage.Message,
                        SharedMessageId = senderMessage.SharedMessageId,
                        Side = senderMessage.Side,
                        ChatConversationId = recdto.chatConversationId
                    };
                }

                return null;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                throw ex;
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not send chat message to user: " + receiver);
                Logger.Warn(ex.ToString(), ex);
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách chat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ChatSummaryDto>> GetChatSummary(GetChatSummaryDto input)
        {
            var query = from p in _chatConversationRepository.GetAll()
                        .WhereIf(input.Status == 1, x => x.UnreadCount == 0)
                        .WhereIf(input.Status == 2, x => x.UnreadCount > 0)
                        join q in _userRepository.GetAll() on p.FriendUserId equals q.Id
                        join b in _bookingRepository.GetAll() on p.BookingId equals b.Id into ps
                        from _b in ps.DefaultIfEmpty()
                        where p.UserId == AbpSession.UserId && p.Status == 1
                        select new ChatSummaryDto
                        {
                            Message = p.LastMessage,
                            BookingId = p.BookingId ?? 0,
                            SentDate = p.LastModificationTime ?? p.CreationTime,
                            ChatConversationId = p.ShardChatConversationId,
                            UnReadCount = p.UnreadCount,
                            Friend = new BasicHostUserInfo
                            {
                                Avarta = q.Avatar,
                                FullName = q.FullName,
                                UserId = q.Id
                            },
                            BookingStatus = _b != null ? (int)_b.Status : -1,
                            Price = _b != null ? _b.Amount : 0,
                        }
                ;

            var total = query.Count();
            var list = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var listBookingIds = list.Where(p => p.BookingId != 0).Select((p => p.BookingId)).Distinct().ToList();
            var bookingItems = _bookingRepository.GetAll().Where(p => listBookingIds.Contains(p.Id)).Select(p => new
            {
                BookingId = p.Id,
                p.ProductId
            }).ToList();
            //
            var itemIds = bookingItems.Select(p => p.ProductId).Distinct().ToList();
            var tourItems = _tourRepository.GetAll().Where(p => itemIds.Contains(p.Id)).Select(p => new
            {
                p.Name,
                p.Id
            }).ToList();
            foreach (var item in list)
            {
                if (item.BookingId > 0)
                {
                    var tourItem = bookingItems.FirstOrDefault(p => p.BookingId == item.BookingId);
                    if (tourItem != null)
                    {
                        var tour = tourItems.FirstOrDefault(p => p.Id == tourItem.ProductId);
                        if (tour != null)
                        {
                            item.TourTitle = tour.Name;
                        }
                    }
                }

                if (_userOnline.Any(p => p == item.Friend.UserId))
                {
                    item.Friend.IsOnline = true;
                }
            }

            var friendUserId = list.Select(p => p.Friend.UserId).ToList().Distinct();


            return new PagedResultDto<ChatSummaryDto>
            {
                Items = list,
                TotalCount = total
            };
        }

        /// <summary>
        /// Archive tin nhăn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ArchiveMessage(ArchiveMessageDto input)
        {
            var messge = _chatConversationRepository.FirstOrDefault(p =>
                p.UserId == AbpSession.UserId && p.ShardChatConversationId == input.ChatConversationId);
            if (messge != null)
            {
                messge.Status = 0;
                _chatConversationRepository.Update(messge);
            }
        }

        /// <summary>
        /// ĐỌc tin nhắn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ReadMessage(ReadMessageInputDto input)
        {
            var messge = _chatConversationRepository.FirstOrDefault(p =>
                p.UserId == AbpSession.UserId && p.ShardChatConversationId == input.ChatConversationId);
            if (messge != null)
            {
                messge.UnreadCount = 0;
                _chatConversationRepository.Update(messge);
            }
        }

        /// <summary>
        /// Xóa tin nhắn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteMessage(DeleteMessageInputDto input)
        {
            var messge = _chatConversationRepository.FirstOrDefault(p =>
                p.UserId == AbpSession.UserId && p.ShardChatConversationId == input.ChatConversationId);
            if (messge != null)
            {
                messge.LastMessage = "";
                messge.Status = 0;
                _chatConversationRepository.Update(messge);
                _chatMessageRepository.Delete(p => p.ChatConversationId == messge.Id);
                _firebaseClient.DeleteTaskAsync($"message/{AbpSession.UserId}/{input.ChatConversationId}");
            }
        }

        /// <summary>
        /// Lấy danh sách message
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ChatMessageV2Dto>> GetChatMessage(GetChatMessageDto input)
        {
            var chatConversation = _chatConversationRepository.FirstOrDefault(p =>
                p.ShardChatConversationId == input.ChatConversationId && p.UserId == AbpSession.UserId);
            if (chatConversation != null)
            {
                var mesageList = _chatMessageRepository.GetAll()
                    .Where(p => p.UserId == AbpSession.UserId && p.ChatConversationId == chatConversation.Id)
                    .WhereIf(input.LastId > 0, p => p.Id < input.LastId)
                    .Select(p => new ChatMessageV2Dto
                    {
                        Id = p.Id,
                        Message = p.Message,
                        Side = p.Side,
                        CreationTime = p.CreationTime,
                        SharedMessageId = p.SharedMessageId
                    });
                var total = mesageList.Count();
                var list = mesageList.OrderByDescending(p => p.Id).Take(input.MaxResultCount).ToList();
                list = list.OrderBy(p => p.CreationTime).ToList();
                return new PagedResultDto<ChatMessageV2Dto>
                {
                    Items = list,
                    TotalCount = total
                };
            }

            throw new NotImplementedException();
        }


        /// <summary>
        /// Lấy danh sách message
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PagedResultDto<ChatMessageV2Dto>> GetChatMessageByBookingId(GetChatMessageDto input)
        {
            var chatConversation = _chatConversationRepository.FirstOrDefault(p =>
                p.ShardChatConversationId == input.ChatConversationId && p.UserId == AbpSession.UserId);
            if (chatConversation != null)
            {
                var mesageList = _chatMessageRepository.GetAll()
                    .Where(p => p.UserId == AbpSession.UserId && p.ChatConversationId == chatConversation.Id)
                    .WhereIf(input.LastId > 0, p => p.Id < input.LastId)
                    .Select(p => new ChatMessageV2Dto
                    {
                        Id = p.Id,
                        Message = p.Message,
                        Side = p.Side,
                        CreationTime = p.CreationTime,
                        SharedMessageId = p.SharedMessageId
                    });
                var total = mesageList.Count();
                var list = mesageList.OrderByDescending(p => p.Id).Take(input.MaxResultCount).ToList();
                list = list.OrderBy(p => p.CreationTime).ToList();
                return new PagedResultDto<ChatMessageV2Dto>
                {
                    Items = list,
                    TotalCount = total
                };
            }

            throw new NotImplementedException();
        }


        /// <summary>
        /// Lấy thông tin người đang chat
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BasicHostUserInfo> GetFriendInfor(long userId)
        {
            var user = _userRepository.GetAll().Where(p => p.Id == userId).Select(p => new BasicHostUserInfo
            {
                Avarta = p.Avatar,
                FullName = p.FullName,
                UserId = p.Id
            }).FirstOrDefault();
            return user;
        }

        public async Task<List<ChatMessageV2Dto>> SearchChatMessage(SearchMessageDto input)
        {
            var chatConversation = _chatConversationRepository.FirstOrDefault(p =>
                p.ShardChatConversationId == input.ChatConversationId && p.UserId == AbpSession.UserId);

            if (chatConversation != null)
            {
                var mesageList = _chatMessageRepository.GetAll()
                    .Where(p => p.UserId == AbpSession.UserId && p.ChatConversationId == chatConversation.Id &&
                                p.Message.Contains(input.KeyWord))
                    .Select(p => new ChatMessageV2Dto
                    {
                        Id = p.Id,
                        Message = p.Message,
                        Side = p.Side,
                        CreationTime = p.CreationTime,
                        SharedMessageId = p.SharedMessageId
                    });
                var total = mesageList.Count();
                var list = mesageList.OrderByDescending(p => p.Id).ToList();
                return list;
            }

            return null;
        }

        /// <summary>
        /// Check trạng thái online / offline của 1 user ( true: online / false: offline)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserOnline(long userId)
        {
            return _userOnline.Any(p => p == userId);
        }

        /// <summary>
        /// Update trạng thái online / off line 1 user :  ( true: online, false : offline)
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdatUserStaus(bool status)
        {
            if (status)
            {
                if (!_userOnline.Any(p => p == (AbpSession.UserId ?? 0)))
                {
                    _userOnline.Add((AbpSession.UserId ?? 0));
                }
            }
            else
            {
                _userOnline.RemoveAll(p => p == (AbpSession.UserId ?? 0));
            }
        }

        public async Task<int> GetUnreadCount(string ChatConversationId)
        {
            var query = _chatConversationRepository.GetAll()
                .Where(p => p.ShardChatConversationId == ChatConversationId && p.UserId == AbpSession.UserId)
                .FirstOrDefault();
            if (query != null)
                return query.UnreadCount;
            return 0;
        }

    }

    public class FirebaseMessageDto
    {
        public long userId { get; set; }
        public ChatSide side { get; set; }
        public long id { get; set; }
        public DateTime creationTime { get; set; }
        public string message { get; set; }
        public string chatConversationId { get; set; }
    }
}
