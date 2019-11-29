using System;
using TemplateProductName.Common;

namespace TemplateProductName.Domain.Features.SignIn
{
    public class SignInCommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
