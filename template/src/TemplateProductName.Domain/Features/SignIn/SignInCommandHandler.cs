using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateProductName.Common.Errors;
using TemplateProductName.Domain.Model;
using TemplateProductName.Domain.Repositories;
using TemplateProductName.Domain.Services;

namespace TemplateProductName.Domain.Features.SignIn
{
    public class SignInCommandHandler : ICommandHandler<SignInCommand>
    {
        private IRepository repository;
        private IPasswordService passwordService;
        private IAuthenticationService authenticationService;

        public SignInCommandHandler(IRepository repository, IPasswordService passwordService, IAuthenticationService authenticationService)
        {
            this.repository = repository;
            this.passwordService = passwordService;
            this.authenticationService = authenticationService;
        }

        public IErrors Handle(SignInCommand command)
        {
            var user = repository.Query<User>().Where(x => x.Email == command.Email).SingleOrDefault();
            var isPasswordValid = user != null && passwordService.VerifyPassword(user.Password, command.Password);

            if(!isPasswordValid)
            {
                return new ValidationErrors(new[] { new ValidationError(Guid.Empty, nameof(command.Password), "invalid_password", "The username or password is invalid.") });
            }

            user.LastLogin = DateTime.Now;

            authenticationService.HttpSignIn(user);

            return null;
        }
    }
}
