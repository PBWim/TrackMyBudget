using TrackMyBudget.Core.Contract;
using TrackMyBudget.Infrastructure.Implementation;

namespace TrackMyBudget.Infrastructure.Data
{
    /// <summary>
    /// The UnitOfWork class will hold instances of the repositories and call SaveChangesAsync() 
    /// on the DbContext to commit the changes in a single transaction.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private BudgetRepository _budgetRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IBudgetRepository Budgets
        {
            get
            {
                return _budgetRepository ??= new BudgetRepository(_context);
            }
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}