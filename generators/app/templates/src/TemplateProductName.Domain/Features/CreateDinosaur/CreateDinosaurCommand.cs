using System;
using TemplateProductName.Common;

namespace TemplateProductName.Domain.Features.CreateDinosaur
{
    public class CreateDinosaurCommand : IHasGuid
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }
}
