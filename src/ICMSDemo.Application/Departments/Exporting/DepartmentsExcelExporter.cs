using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.Departments.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.Departments.Exporting
{
    public class DepartmentsExcelExporter : EpPlusExcelExporterBase, IDepartmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentForViewDto> departments)
        {
            return CreateExcelPackage(
                "Departments.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Departments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Name"),
                        L("Description"),
                        L("IsAbstract"),
                        L("IsControlTeam"),
                        (L("User")) + L("Name"),
                        (L("User")) + L("Name"),
                        (L("OrganizationUnit")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, departments,
                        _ => _.Department.Code,
                        _ => _.Department.Name,
                        _ => _.Department.Description,
                        _ => _.Department.IsAbstract,
                        _ => _.Department.IsControlTeam,
                        _ => _.UserName,
                        _ => _.UserName2,
                        _ => _.OrganizationUnitDisplayName
                        );

					

                });
        }
    }
}
