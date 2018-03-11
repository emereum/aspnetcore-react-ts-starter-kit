# Template Product Name

This is the repository for Template Product Name.

## Getting started

* Install [Visual Studio 2017](https://www.visualstudio.com/downloads/)
  * After installation, launch Visual Studio Installer, modify Visual Studio, and tick the .NET 4.7 and 4.7.1 SDK and targeting packs. This may not need to be done in future releases of Visual Studio.
* Install [.NET Framework 4.6.2 Developer Pack](https://www.microsoft.com/en-us/download/details.aspx?id=53321)
* Install [.NET Core SDK 2.0.0](https://download.microsoft.com/download/0/F/D/0FD852A4-7EA1-4E2A-983A-0484AC19B92C/dotnet-sdk-2.0.0-win-x64.exe)
* Install [Node 7 or higher](https://nodejs.org/en/)
* Install [PostgreSQL 9.5 or higher](https://www.postgresql.org/download/windows/)
* Put your PostgreSQL bin directory on the PATH so psql.exe can be executed on the command line
* Run `database\setup.ps1` (This will create a PostgreSQL database on localhost with database name, username and password set to "templateproductname")
* Launch `src\TemplateProductName.sln` with Visual Studio
* If you are using a database with different connection details than what are created in the database setup script, create `appsettings.local.json` in `TemplateProductName.WebApi` and enter a connection string per `appsettings.json`. Do not edit `appsettings.json`.
* Run the `TemplateProductName.WebApi` project
* Navigate to `src\TemplateProductName.WebClient`
* Run `npm install`
* Run `npm start`
* View http://localhost:3000
* Open `src\TemplateProductName.WebClient` with Visual Studio Code
* Click the TypeScript version at the bottom-right corner of VS Code and choose "Use workspace version"

## Test and production web server setup

Run `server\install-iis-aspnetcore-ssh.ps1` to prepare a server for automated deployments and for running ASP.NET Core applications. This will install IIS, remove the default website and application pool, install the ASP.NET Core windows hosting bundle, set up OpenSSH with public key authentication and create a public and private key. The script can be used to prepare test, staging, or production servers. More information can be found within the script.

## What's included

This repository contains several components:

### A web API consisting of:

* An ASP.NET Core MVC 2.0.2 application running on .NET Framework 4.7.1
* [AutoFac](https://autofac.org/)
* [FluentValidation](https://github.com/JeremySkinner/FluentValidation)
* [FluentNHibernate](http://www.fluentnhibernate.org/)
* [NHibernate](http://nhibernate.info/)
* A PostgreSQL database session factory for NHibernate (An Oracle one is included too)
* [NSubstitute](http://nsubstitute.github.io/)
* [NUnit](https://www.nunit.org/)

### A SPA web client consisting of:

* Scaffolding by [create-react-app-typescript](https://github.com/wmonk/create-react-app-typescript)
* [TypeScript](https://www.typescriptlang.org/)
* [React](https://facebook.github.io/react/)
* [MobX](https://github.com/mobxjs/mobx)
* [React Router](https://github.com/ReactTraining/react-router)
* [Semantic UI React](http://react.semantic-ui.com/)
* [Jest](https://facebook.github.io/jest/)

### A collection of scripts:

+----------------------------------------+-----------------------------------------------------------------+
| Script                                 | Purpose                                                         |
+========================================+=================================================================+
| rakefile.rb                            | Automate build, test, and deployment.                           |
+----------------------------------------+-----------------------------------------------------------------+
| database\setup.ps1                     | Initialise a PostgreSQL database on localhost.                  |
+----------------------------------------+-----------------------------------------------------------------+
| server\install-iis-aspnetcore-sshd.ps1 | Configure a clean Windows Server to support automated           |
|                                        | deployments and hosting of the product.                         |
+----------------------------------------+-----------------------------------------------------------------+
| deploy\templateproductname.nuspec      | Pack the product into a Chocolatey package which can be used to |
|                                        | to deploy the application to a server. This script is invoked   |
|                                        | and the resulting package is deployed by rakefile.rb to the     |
|                                        | target server.                                                  |
+----------------------------------------+-----------------------------------------------------------------+
| deploy\tools\chocolateyinstall.ps1     | Perform deployment tasks on the server to which the product     |
|                                        | is being deployed. This includes things like destroying and     |
|                                        | recreating web sites and application pools in IIS and copying   |
|                                        | the build artifacts to an IIS-visible folder.                   |
+----------------------------------------+-----------------------------------------------------------------+


## Developing features

Controller actions should accept `Command` classes which are simple POCOs with no logic or dependencies:

```
public class CreateUserCommand
{
    public string UserName { get; set; }
}
```

Commands should be executed by a `CommandHandler`:

```
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Maybe<Error>>
{
    private readonly IRepository repository;
    
    public CreateUserCommandHandler(IRepository repository)
    {
        this.repository = repository;
    }
    
    public Maybe<Error> Handle(CreateUserCommand command)
    {
        if( /* username is taken */ )
        {
            return Some(new Error("Username is taken"));
        }
        
        // If the validation is complex, implement a validator
        // using FluentValidation by creating a new class and
        // inheriting from AbstractValidator<CreateUserCommandHandler>
        
        repository.Add(new User { UserName = command.UserName });
        return None();
    }
}
```

Controllers should be thin; they only delegate `Commands` to `CommandHandlers`, set HTTP Response codes, and return some JSON data if necessary:

```
[Route("api/[controller]")]
public class UserController: Controller
{
    private readonly CreateUserCommandHandler createUserCommandHandler;
    
    public UserController(CreateUserCommandHandler createUserCommandHandler)
    {
        this.createUserCommandHandler = createUserCommandHandler;
    }
    
    [HttpPost]
    public ActionResult Post([FromBody] CreateUserCommand command) =>
        createUserCommandHandler
        .Handle(command)
        .Match(
            some => this.JsonBadRequest(some),
            none => HttpOk()
        );
}
```

### Why do this?
* Business logic is kept out of the controller. The controller's only responsibility is to receive a command, delegate it to a handler, then pass the results back to the consumer in some HTTP-oriented fashion (such as in a JSON response).
* Testing is easier. One feature will be associated with one `CommandHandler` so we can instantiate that `CommandHandler` in a test, pass in a `Command` and verify the outputs.
* It allows us to structure the source code into feature folders where each feature folder has a single `Command` and `CommandHandler`.
* Developers can focus their effort on small vertical slices of the application without having to understand the application as a whole. This can boost a developer's productivity when they are new to the project.

## Folder structure

`TemplateProductName.Domain.Features`
* Business logic should be organised into feature folders under this folder.
* A simple feature will typically consist of one `Command` and one `CommandHandler`.
* Logic that is shared between features can be pulled out into `Service` classes under a `Common` folder.

`TemplateProductName.Domain.Queries`
* Put functionality that retrieves data (without altering the state of the system) into a `Query` class under this folder.

`TemplateProductName.Domain.Model`
  * This should contain a DDD-style domain model of the business problems being solved by TemplateProductName.

## Other development tips

* When working on TemplateProductName.WebClient remember to `npm shrinkwrap` and commit the shrinkwrap file to keep the version numbers of all dependencies stable.