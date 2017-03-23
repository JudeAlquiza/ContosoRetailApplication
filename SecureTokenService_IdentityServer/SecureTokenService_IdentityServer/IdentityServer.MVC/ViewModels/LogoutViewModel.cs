using IdentityServer.MVC.Models;

namespace IdentityServer.MVC.ViewModels
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; }
    }
}
