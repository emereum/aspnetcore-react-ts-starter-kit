using System.Linq;
using TemplateProductName.Common.Errors;
using TemplateProductName.Domain;
using TemplateProductName.Domain.Features.SignIn;
using TemplateProductName.Domain.Model;
using TemplateProductName.Domain.Repositories;
using TemplateProductName.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemplateProductName.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationService authenticationService;
        private IMediator mediator;

        public AuthenticationController(IAuthenticationService authenticationService, IMediator mediator)
        {
            this.authenticationService = authenticationService;
            this.mediator = mediator;
        }

        [HttpPost("signin")]
        public IErrors SignIn(SignInCommand command) =>
            mediator.Send<SignInCommandHandler>(command);

        [HttpPost("signout")]
        public void SignOut() => authenticationService.HttpSignOut();

        [HttpGet("me")]
        public UserModel Me() => authenticationService.GetCurrentUser();
    }
}
