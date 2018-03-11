namespace TemplateProductName.Domain.Features
{
    /// <summary>
    /// A class that accepts a command of type TCommand and performs some
    /// business operation and returns a result of type TResult. If the handler
    /// does not need to return anything, return Unit (from
    /// LanguageExt.Prelude).
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ICommandHandler<in TCommand, out TResult>
    {
        TResult Handle(TCommand command);
    }
}
