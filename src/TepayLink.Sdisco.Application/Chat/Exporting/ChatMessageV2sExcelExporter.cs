using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Chat.Exporting
{
    public class ChatMessageV2sExcelExporter : EpPlusExcelExporterBase, IChatMessageV2sExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChatMessageV2sExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChatMessageV2ForViewDto> chatMessageV2s)
        {
            return CreateExcelPackage(
                "ChatMessageV2s.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ChatMessageV2s"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ChatConversationId"),
                        L("UserId"),
                        L("Message"),
                        L("CreationTime"),
                        L("Side"),
                        L("ReadState"),
                        L("ReceiverReadState"),
                        L("SharedMessageId")
                        );

                    AddObjects(
                        sheet, 2, chatMessageV2s,
                        _ => _.ChatMessageV2.ChatConversationId,
                        _ => _.ChatMessageV2.UserId,
                        _ => _.ChatMessageV2.Message,
                        _ => _timeZoneConverter.Convert(_.ChatMessageV2.CreationTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChatMessageV2.Side,
                        _ => _.ChatMessageV2.ReadState,
                        _ => _.ChatMessageV2.ReceiverReadState,
                        _ => _.ChatMessageV2.SharedMessageId
                        );

					var creationTimeColumn = sheet.Column(4);
                    creationTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					creationTimeColumn.AutoFit();
					

                });
        }
    }
}
