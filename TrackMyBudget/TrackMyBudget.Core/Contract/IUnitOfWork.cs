namespace TrackMyBudget.Core.Contract
{
    public interface IUnitOfWork : IDisposable
    {
        IBudgetRepository Budgets { get; }
        Task<int> CommitAsync();
    }
}