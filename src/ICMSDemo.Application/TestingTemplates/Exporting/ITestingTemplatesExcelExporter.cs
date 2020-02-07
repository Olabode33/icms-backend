using System.Collections.Generic;
using ICMSDemo.TestingTemplates.Dtos;
using ICMSDemo.Dto;

namespace ICMSDemo.TestingTemplates.Exporting
{
    public interface ITestingTemplatesExcelExporter
    {
        FileDto ExportToFile(List<GetTestingTemplateForViewDto> testingTemplates);
    }
}