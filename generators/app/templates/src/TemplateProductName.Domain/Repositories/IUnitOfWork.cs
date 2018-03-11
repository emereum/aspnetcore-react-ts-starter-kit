namespace TemplateProductName.Domain.Repositories
{
    /// <summary>
    /// Represents an atomic set of operations which all succeed or all fail
    /// together. Constructing a UnitOfWork implies that the work is beginning.
    /// Rolling back the UnitOfWork implies the work is cancelled. Calling
    /// Complete completes the work or throws an exception if the work fails.
    /// </summary>
    public interface IUnitOfWork
    {
        void Complete();
        void Rollback();
    }
}
