using System.Collections.Generic;
using MvvmHelpers;
using ICMSDemo.Models.NavigationMenu;

namespace ICMSDemo.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}