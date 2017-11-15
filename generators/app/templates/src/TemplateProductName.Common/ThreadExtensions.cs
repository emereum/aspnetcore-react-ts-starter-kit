using System.Security.Principal;
using System.Threading;

namespace TemplateProductName.Common
{
    public static class ThreadExtensions
    {
        public static void SetCurrentPrincipal(string name, params string[] roles)
        {
            var identity = new GenericIdentity(name);
            var principal = new GenericPrincipal(identity, roles);
            Thread.CurrentPrincipal = principal;
        }
    }
}
