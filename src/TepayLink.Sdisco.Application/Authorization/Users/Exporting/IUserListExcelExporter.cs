using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Users.Dto;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}