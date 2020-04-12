using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Chat.Exporting
{
    public class ChatconversationsExcelExporter : EpPlusExcelExporterBase, IChatconversationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChatconversationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChatconversationForViewDto> chatconversations)
        {
            return CreateExcelPackage(
                "Chatconversations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Chatconversations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("FriendUserId"),
                        L("UnreadCount"),
                        L("ShardChatConversationId"),
                        L("BookingId"),
                        L("LastMessage"),
                        L("Side"),
                        L("Status")
                        );

                    AddObjects(
                        sheet, 2, chatconversations,
                        _ => _.Chatconversation.UserId,
                        _ => _.Chatconversation.FriendUserId,
                        _ => _.Chatconversation.UnreadCount,
                        _ => _.Chatconversation.ShardChatConversationId,
                        _ => _.Chatconversation.BookingId,
                        _ => _.Chatconversation.LastMessage,
                        _ => _.Chatconversation.Side,
                        _ => _.Chatconversation.Status
                        );

					

                });
        }
    }
}
