namespace TepayLink.Sdisco.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}