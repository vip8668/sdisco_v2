using System.Threading.Tasks;
using TepayLink.Sdisco.Sessions.Dto;

namespace TepayLink.Sdisco.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
