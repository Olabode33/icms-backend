using System.Collections.Generic;
using ICMSDemo.Authorization.Users.Importing.Dto;
using ICMSDemo.Dto;

namespace ICMSDemo.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
