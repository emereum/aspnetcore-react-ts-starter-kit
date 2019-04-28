# Template Product Name

This is the repository for Template Product Name.

## Getting started

* Run `environments\setup-dev-environment.ps1` from an elevated PowerShell shell. This script will install several IDEs, some command line tools, and PostgreSQL. Please read it before running it.
* Run `database\setup.ps1`. This script will create a PostgreSQL database on localhost with database name, username and password set to `template_product_name`.
* Launch `src\TemplateProductName.sln` with Visual Studio
* Run the `TemplateProductName.WebApi` project
* Navigate to `src\TemplateProductName.WebClient`
* Run `npm install`
* Run `npm start`
* View http://localhost:3000
* Open `src\TemplateProductName.WebClient` with Visual Studio Code

## Developer guide

### How to write api endpoints

Controllers should be thin; they only delegate `Commands` to `CommandHandlers` via an `IMediator`:

```
[Route("api/[controller]")]
public class UserController: Controller
{
    private readonly IMediator mediator
    
    public UserController(IMediator mediator) => this.mediator = mediator;
    
    [HttpPost]
    public IErrors Post(CreateUserCommand command) =>
        mediator.Send<CreateUserCommandHandler>(command);
}
```

`Commands` should be simple objects with no logic or dependencies:

```
public class CreateUserCommand
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}
```

`Commands` should be handled by a `CommandHandler`. Generally a `CommandHandler` should return nothing if the command was handled successfully, or an `IErrors` if the command was invalid. Don't assume what the consumer wants to see in response to a command, let the consumer query for that information separately. This makes for more reusable Apis:

```
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly CreateUserValidator validator;
    private readonly IRepository repository;
    
    public CreateUserCommandHandler(CreateUserValidator validator, IRepository repository)
    {
        this.validator = validator;
        this.repository = repository;
    }
    
    public IErrors Handle(CreateUserCommand command)
    {
        var errors = validator.Validate(command);
        
        if (!errors.IsValid)
        {
            return errors.ToErrorResponse();
        }
        
        repository.Add(new User { Id = command.Id, UserName = command.UserName });
        return null;
    }
}
```

Please read the source code documentation on `ICommandHandler` and `IQueryHandler` for more information about this design.

### Why do this?
* Business logic is kept out of the controller. The controller's only responsibility is to receive a command, delegate it to a handler, then pass the results back to the consumer in some HTTP-oriented fashion (such as in a JSON response).
* Testing is easier. One feature will be associated with one `CommandHandler` so we can instantiate that `CommandHandler` in a test, pass in a `Command` and verify the outputs.
* It allows us to structure the source code into feature folders where each feature folder has a single `Command` and `CommandHandler`.
* Developers can focus their effort on small vertical slices of the application without having to understand the application as a whole. This can boost a developer's productivity when they are new to the project.

### Paginating queries

To paginate a query response make your query inherit from `PaginationOptions` and use the `PaginationExtensions.Paginate` extension method in your `IQueryHandler` implementation to return a paginated response.

## Folder structure

`TemplateProductName.Domain.Features`
* Business logic should be organised into feature folders under this folder.
* A simple feature will typically consist of one `Command` and one `CommandHandler`.
* Logic that is shared between features can be pulled out into `Service` classes under a `Common` folder.

`TemplateProductName.Domain.Queries`
* Put functionality that retrieves data (without altering the state of the system) into a `Query` class under this folder.

`TemplateProductName.Domain.Model`
  * This should contain a DDD-style domain model of the business problems being solved by TemplateProductName.

## What's included

This repository contains several components:

### A web API consisting of:

* An ASP.NET Core MVC 2.0.8 application targeting `netcoreapp2.0`
* [AutoFac](https://autofac.org/)
* [FluentValidation](https://github.com/JeremySkinner/FluentValidation)
* [FluentNHibernate](http://www.fluentnhibernate.org/)
* [NHibernate](http://nhibernate.info/) + integration with PostgreSQL
* [NSubstitute](http://nsubstitute.github.io/)
* [xUnit](https://xunit.github.io/)

### A SPA web client consisting of:

* Scaffolding by [create-react-app-typescript](https://github.com/wmonk/create-react-app-typescript)
* [TypeScript](https://www.typescriptlang.org/)
* [React](https://facebook.github.io/react/)
* [MobX](https://github.com/mobxjs/mobx)
* [React Router](https://github.com/ReactTraining/react-router)
* [Semantic UI React](http://react.semantic-ui.com/)
* [Jest](https://facebook.github.io/jest/)

### A collection of scripts:

| Script | Purpose |
| --- | --- |
| `rakefile.rb` | Automate build, test, and deployment. |
| `database\setup.ps1` | Initialise a PostgreSQL database named `template_product_name` on localhost. |
| `environments\setup-*-environment.ps1` | Pave various environments (dev, build, and hosting environments) so the environments are ready for the product to be developed, built, or hosted. |
| `deploy\templateproductname.nuspec` | Pack the product into a Chocolatey package which can be used to deploy the product to a server. This script is invoked and the resulting package is deployed by rakefile.rb to the target server.
| `deploy\tools\chocolateyinstall.ps1` | Perform deployment tasks on the server to which the product is being deployed. This includes things like destroying and recreating web sites and application pools in IIS and copying the build artifacts to an IIS-visible folder. |

## Build and continuous integration server setup

Run `environments\setup-build-environment.ps1` to prepare a server for building Template Product Name from the command line.

## Test and production web server setup

Run `environments\setup-hosting-environment.ps1` to prepare a server for automated deployments and for running ASP.NET Core applications. This will install IIS, remove the default website and application pool, install the ASP.NET Core windows hosting bundle, set up OpenSSH with public key authentication and create a public and private key. The script can be used to prepare test, staging, or production servers. More information can be found within the script.
