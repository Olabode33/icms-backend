using System.Collections.Generic;
using ICMSDemo.Authorization.Users.Dto;
using ICMSDemo.Dto;

namespace ICMSDemo.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}