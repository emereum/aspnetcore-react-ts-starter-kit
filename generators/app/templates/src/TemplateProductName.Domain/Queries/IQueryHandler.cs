namespace TemplateProductName.Domain.Queries
{
    /// <summary>
    /// An IQueryHandler accepts a request for information, attempts to locate
    /// that information, and returns it to the consumer.
    /// </summary>
    /// <remarks>
    /// Introduction
    /// ============
    /// IQueryHandlers are implemented hand-in-hand with Query classes. A Query
    /// class should consist only of properties which describe what the user
    /// is attempting to locate. A Query class doesn't have to implement a
    /// particular interface.
    /// 
    /// Query classes and IQueryHandler implementations should form the majority
    /// of an application's "read-side" functionality. If a feature retrieves
    /// data from the application, it should nearly always be implemented as a
    /// Query and an IQueryHandler implementation. By the same token, Command
    /// classes and ICommandHandler implementations should form the majority of
    /// an application's "write-side" functionality.
    /// 
    /// When to use this
    /// ================
    /// Implement an IQueryHandler when you are developing a feature that
    /// allows a user to retrieve information. Some examples include:
    /// 
    ///   * Enable a user to search for movies, such as by author or title.
    ///   * Enable a user to see their account information.
    ///   * Upon logging into the application, show the user the 10 most
    ///     popular restaurants in their area.
    /// 
    /// When NOT to use this
    /// ====================
    /// Do not use an IQueryHandler when you want to save changes to the system.
    /// For example, do not use an IQueryHandler for any of these scenarios:
    /// 
    ///   * Updating the user's last login date.
    ///   * Adding an item to the user's shopping cart.
    ///   * Creating a new appointment request.
    /// </remarks>
    /// <typeparam name="TQuery">
    /// The type of the object that describes what the user is trying to locate.
    /// This object should be a plain-old-csharp-object. In other words, it
    /// should contain only properties. It should not have any behaviour
    /// (methods) because it only describes WHAT the user wants, not HOW to get
    /// it. It is the responsibility of the IQueryHandler to determine how to
    /// get information.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the object that contains the information the user was trying
    /// to locate.
    /// </typeparam>
    public interface IQueryHandler<in TQuery, out TResult>
    {
        TResult Handle(TQuery query);
    }
}
