namespace ICMSDemo.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}