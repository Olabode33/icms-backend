using System.Collections.Generic;
using ICMSDemo.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace ICMSDemo.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
