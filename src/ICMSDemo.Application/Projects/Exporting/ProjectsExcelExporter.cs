using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ICMSDemo.DataExporting.Excel.EpPlus;
using ICMSDemo.Projects.Dtos;
using ICMSDemo.Dto;
using ICMSDemo.Storage;

namespace ICMSDemo.Projects.Exporting
{
    public class ProjectsExcelExporter : EpPlusExcelExporterBase, IProjectsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectForViewDto> projects)
        {
            return CreateExcelPackage(
                "Projects.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Projects"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Code"),
                        L("Description"),
                        L("StartDate"),
                        L("EndDate"),
                        L("BudgetedStartDate"),
                        L("BudgetedEndDate"),
                        L("Title"),
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("OrganizationUnit")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, projects,
                        _ => _.Project.Code,
                        _ => _.Project.Description,
                        _ => _timeZoneConverter.Convert(_.Project.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Project.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Project.BudgetedStartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Project.BudgetedEndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Project.Title,
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.OrganizationUnitDisplayName2
                        );

					var startDateColumn = sheet.Column(3);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(4);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					var budgetedStartDateColumn = sheet.Column(5);
                    budgetedStartDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					budgetedStartDateColumn.AutoFit();
					var budgetedEndDateColumn = sheet.Column(6);
                    budgetedEndDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					budgetedEndDateColumn.AutoFit();
					
					
                });
        }
    }
}
