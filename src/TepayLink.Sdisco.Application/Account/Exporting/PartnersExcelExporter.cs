using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class PartnersExcelExporter : EpPlusExcelExporterBase, IPartnersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PartnersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPartnerForViewDto> partners)
        {
            return CreateExcelPackage(
                "Partners.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Partners"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("WebsiteUrl"),
                        L("Languages"),
                        L("SkypeId"),
                        L("Comment"),
                        L("AffiliateKey"),
                        L("Status"),
                        L("AlreadyBecomeSdiscoPartner"),
                        L("HasDriverLicense"),
                        (L("User")) + L("Name"),
                        (L("Detination")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, partners,
                        _ => _.Partner.Name,
                        _ => _.Partner.WebsiteUrl,
                        _ => _.Partner.Languages,
                        _ => _.Partner.SkypeId,
                        _ => _.Partner.Comment,
                        _ => _.Partner.AffiliateKey,
                        _ => _.Partner.Status,
                        _ => _.Partner.AlreadyBecomeSdiscoPartner,
                        _ => _.Partner.HasDriverLicense,
                        _ => _.UserName,
                        _ => _.DetinationName
                        );

					

                });
        }
    }
}
