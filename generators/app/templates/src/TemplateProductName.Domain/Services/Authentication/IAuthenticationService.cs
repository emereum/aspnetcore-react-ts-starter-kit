using System;
using TemplateProductName.Domain.Model;

namespace TemplateProductName.Domain.Services
{
    public interface IAuthenticationService
    {
        void HttpSignIn(User user);
        void HttpSignOut();
        bool IsAuthenticated();
        UserModel GetCurrentUser();
    }
}
