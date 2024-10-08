using Microsoft.EntityFrameworkCore;
using TrackMyBudget.Core.Contract;
using TrackMyBudget.Core.Models;
using TrackMyBudget.Infrastructure.Data;

namespace TrackMyBudget.Infrastructure.Implementation
{
    /// <summary>
    /// The BudgetRepository class will implement the IBudgetRepository interface 
    /// and will interact with ApplicationDbContext for Budget-related data.
    /// </summary>
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetAllAsync()
        {
            return await _context.Budgets.ToListAsync();
        }

        public async Task<Budget> GetByIdAsync(Guid id)
        {
            return await _context.Budgets.FindAsync(id);
        }

        public async Task AddAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
        }

        public void Update(Budget budget)
        {
            _context.Budgets.Update(budget);
        }

        public void Remove(Budget budget)
        {
            _context.Budgets.Remove(budget);
        }
    }
}