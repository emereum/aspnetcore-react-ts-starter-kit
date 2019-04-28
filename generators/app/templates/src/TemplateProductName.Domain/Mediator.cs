using System.Diagnostics;
using Autofac;
using TemplateProductName.Common.Errors;

namespace TemplateProductName.Domain
{
    /// <summary>
    /// The Mediator is used to handle commands. It accepts a command object and
    /// the type of a command handler. It is responsible for locating,
    /// instantiating, and invoking the command handler against the command.
    /// </summary>
    /// <remarks>
    /// The type checks on this class are very loose. It is possible to get
    /// runtime cast exceptions if a mismatch command handler type and command
    /// object pair are provided. This loose type checking is allowed here
    /// because it reduces the usage of tag interfaces.
    /// 
    /// We require the consumer to specify the type of command handler rather
    /// than guessing it ourselves because it provides a more traceable
    /// flow of control when navigating and debugging the source.
    /// </remarks>
    public class Mediator : IMediator
    {
        private readonly IComponentContext context;

        public Mediator(IComponentContext context) => this.context = context;

        /// <summary>
        /// Instantiate a CommandHandler of type TCommandHandler and use it to
        /// handle the specified command. Assumes the return type of the
        /// CommandHandler is IErrors.
        /// </summary>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IErrors Send<TCommandHandler>(object command) =>
            BaseSend<TCommandHandler, IErrors>(command);

        /// <summary>
        /// In rare circumstances a command may return something other than
        /// an IErrors. BaseSend can be used in this case.
        /// </summary>
        /// <typeparam name="TCommandHandler"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public TResponse BaseSend<TCommandHandler, TResponse>(object command)
        {
            var commandHandler = context.Resolve<TCommandHandler>();
            return (TResponse)((dynamic)commandHandler).Handle((dynamic)command);
        }
    }
}
