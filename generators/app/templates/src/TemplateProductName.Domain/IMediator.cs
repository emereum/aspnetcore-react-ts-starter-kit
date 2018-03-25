using TemplateProductName.Common.Errors;

namespace TemplateProductName.Domain
{
    public interface IMediator
    {
        /// <summary>
        /// Instantiate a CommandHandler of type TCommandHandler and use it to
        /// handle the specified command. Assumes the return type of the
        /// CommandHandler is IErrorResponse.
        /// </summary>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        IErrorResponse Send<TCommandHandler>(object command);

        /// <summary>
        /// In rare circumstances a command may return something other than
        /// an IErrorResponse. BaseSend can be used in this case.
        /// </summary>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        TResponse BaseSend<TCommandHandler, TResponse>(object command);
    }
}