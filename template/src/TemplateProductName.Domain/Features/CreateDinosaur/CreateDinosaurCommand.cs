using System;
using TemplateProductName.Common;

namespace TemplateProductName.Domain.Features.CreateDinosaur
{
    public class CreateDinosaurCommand : IHasId
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
