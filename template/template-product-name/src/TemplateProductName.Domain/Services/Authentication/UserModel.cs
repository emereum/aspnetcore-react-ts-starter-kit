using System;

namespace TemplateProductName.Domain.Services
{
    /// <summary>
    /// Represents the currently logged in user
    /// </summary>
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
