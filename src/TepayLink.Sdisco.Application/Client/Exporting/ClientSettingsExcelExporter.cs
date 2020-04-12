using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Client.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Client.Exporting
{
    public class ClientSettingsExcelExporter : EpPlusExcelExporterBase, IClientSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ClientSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetClientSettingForViewDto> clientSettings)
        {
            return CreateExcelPackage(
                "ClientSettings.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ClientSettings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Key"),
                        L("Value")
                        );

                    AddObjects(
                        sheet, 2, clientSettings,
                        _ => _.ClientSetting.Key,
                        _ => _.ClientSetting.Value
                        );

					

                });
        }
    }
}
