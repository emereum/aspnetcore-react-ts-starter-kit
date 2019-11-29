using System;
using TemplateProductName.Domain.Services;

namespace TemplateProductName.Domain.Model
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string Email { get; set; }
        public virtual HashedPassword Password { get; set; }
        public virtual DateTime LastLogin { get; set; }
    }
}
