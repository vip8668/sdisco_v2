using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace TepayLink.Sdisco.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
