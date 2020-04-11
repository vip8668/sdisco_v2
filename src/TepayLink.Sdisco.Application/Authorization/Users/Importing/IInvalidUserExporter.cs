using System.Collections.Generic;
using TepayLink.Sdisco.Authorization.Users.Importing.Dto;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
