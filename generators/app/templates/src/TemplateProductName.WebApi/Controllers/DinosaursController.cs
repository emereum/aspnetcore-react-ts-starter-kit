using Microsoft.AspNetCore.Mvc;
using TemplateProductName.Common.Errors;
using TemplateProductName.Domain;
using TemplateProductName.Domain.Features.CreateDinosaur;

namespace TemplateProductName.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class DinosaursController : Controller
    {
        private readonly IMediator mediator;

        public DinosaursController(IMediator mediator) => this.mediator = mediator;

        [HttpPost]
        public IErrorResponse Post(CreateDinosaurCommand command) =>
            mediator.Send<CreateDinosaurCommandHandler>(command);
    }
}
