using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
