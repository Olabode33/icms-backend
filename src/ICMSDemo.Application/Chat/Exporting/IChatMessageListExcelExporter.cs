using System.Collections.Generic;
using ICMSDemo.Chat.Dto;
using ICMSDemo.Dto;

namespace ICMSDemo.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(List<ChatMessageExportDto> messages);
    }
}
