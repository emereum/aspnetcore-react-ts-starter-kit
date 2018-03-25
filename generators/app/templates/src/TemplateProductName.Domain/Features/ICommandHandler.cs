using TemplateProductName.Common.Errors;

namespace TemplateProductName.Domain.Features
{
    /// <summary>
    /// An ICommandHandler accepts a request for some business process to be
    /// carried out, carries out that business process, then returns a simple
    /// "OK" response if the process was carried out successfully or a list of
    /// errors if the process failed.
    /// </summary>
    /// <remarks>
    /// Introduction
    /// ============
    /// ICommandHandlers are implemented hand-in-hand with Command classes. A
    /// Command class should consist only of properties which describe what
    /// business process the user wants to be carried out. A Command class does
    /// not have to implement a particular interface.
    /// 
    /// Command classes and ICommandHandler implementations should form the
    /// majority of an application's "write-side" functionality. If a feature
    /// creates or modifies data in the application, it should nearly always
    /// be implemented as a Command and an ICommandHandler implementation. By
    /// the same token, Query classes and IQueryHandler implementations should
    /// form the majority of an application's "read-side" functionality.
    /// 
    /// ICommandHandlers should not make assumptions about what the consumer 
    /// wants to do after the business process has been carried out. For this
    /// reason, a ICommandHandler should not return any data if the process was
    /// carried out successfully. Instead it should simply acknowledge that the
    /// work has been done and trust the consumer to take the next appropriate
    /// steps (such as retrieving new or changed data with a Query).
    /// 
    /// By not assuming a consumer's next steps, we maintain a relatively
    /// generic Api that can be used for a variety of purposes which we may not
    /// have envisaged at the time we wrote the feature. This makes it easier
    /// for 3rd parties to use our Api and it makes it easier for us as
    /// developers to write new features that leverage existing Api
    /// functionality. In general if you want to write a command that returns
    /// something, instead write a command that returns a simple OK response
    /// and a Query that returns the stuff you want afterward, then trust the
    /// consumer to call the Query if they want the data.
    /// 
    /// This can also reduce code duplication. For example a "Create user"
    /// command might have returned the new user if not following this design,
    /// and there would also be a "Get user" query which also returns a user.
    /// This results in two sets of code returning a user which is harder to
    /// maintain than just having the command return nothing and expecting
    /// consumer to use the query if they want information about the created
    /// user.
    /// 
    /// It's okay to return error messages from a command. If a command
    /// fails it's reasonable to assume that any consumer would want to know
    /// why it failed.
    /// </remarks>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<in TCommand>
    {
        IErrorResponse Handle(TCommand command);
    }

    /// <summary>
    /// In very rare circumstances we may want to return something other than
    /// an IErrorResponse from a command (such as a job id for an async
    /// operation). In these cases the IBaseCommandHandler can be used. If the
    /// command needs to return one of several types of responses (such as a
    /// success response object or an error response object),
    /// use LanguageExt.Either or OneOf to define the TResponse type.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IBaseCommandHandler<in TCommand, out TResponse>
    {
        TResponse Handle(TCommand command);
    }
}
