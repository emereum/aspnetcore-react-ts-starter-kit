using System;
using TemplateProductName.Common;
using TemplateProductName.Common.Errors;
using TemplateProductName.Domain.Model;
using TemplateProductName.Domain.Repositories;

namespace TemplateProductName.Domain.Features.CreateDinosaur
{
    public class CreateDinosaurCommandHandler : ICommandHandler<CreateDinosaurCommand>
    {
        private readonly CreateDinosaurValidator validator;

        public CreateDinosaurCommandHandler(CreateDinosaurValidator validator) => this.validator = validator;

        public IErrors Handle(CreateDinosaurCommand command)
        {
            var errors = validator.Validate(command);

            if (!errors.IsValid)
            {
                return errors.ToErrorResponse();
            }

            // If we had a database, we'd store the dinosaur here!

            // var entity = MapFromModelToEntity(command);
            // repository.Add(entity);

            return null;
        }
    }
}
