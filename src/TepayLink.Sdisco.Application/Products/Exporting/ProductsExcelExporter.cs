using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductsExcelExporter : EpPlusExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {
            return CreateExcelPackage(
                "Products.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Products"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Type"),
                        L("Status"),
                        L("Description"),
                        L("Policies"),
                        L("Duration"),
                        L("StartTime"),
                        L("IncludeTourGuide"),
                        L("AllowRetail"),
                        L("TotalSlot"),
                        L("Price"),
                        L("InstantBook"),
                        L("TripLengh"),
                        L("IsHotDeal"),
                        L("IsBestSeller"),
                        L("IsTrending"),
                        L("IsTop"),
                        L("ExtraData"),
                        L("WhatWeDo"),
                        L("LastBookTime"),
                        (L("Category")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("Place")) + L("Name"),
                        (L("ApplicationLanguage")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, products,
                        _ => _.Product.Name,
                        _ => _.Product.Type,
                        _ => _.Product.Status,
                        _ => _.Product.Description,
                        _ => _.Product.Policies,
                        _ => _.Product.Duration,
                        _ => _.Product.StartTime,
                        _ => _.Product.IncludeTourGuide,
                        _ => _.Product.AllowRetail,
                        _ => _.Product.TotalSlot,
                        _ => _.Product.Price,
                        _ => _.Product.InstantBook,
                        _ => _.Product.TripLengh,
                        _ => _.Product.IsHotDeal,
                        _ => _.Product.IsBestSeller,
                        _ => _.Product.IsTrending,
                        _ => _.Product.IsTop,
                        _ => _.Product.ExtraData,
                        _ => _.Product.WhatWeDo,
                        _ => _timeZoneConverter.Convert(_.Product.LastBookTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CategoryName,
                        _ => _.UserName,
                        _ => _.PlaceName,
                        _ => _.ApplicationLanguageName
                        );

					var lastBookTimeColumn = sheet.Column(20);
                    lastBookTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					lastBookTimeColumn.AutoFit();
					

                });
        }
    }
}
