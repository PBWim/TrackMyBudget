using Microsoft.AspNetCore.Mvc;
using TrackMyBudget.Core.Contract;
using TrackMyBudget.Core.Models;

namespace TrackMyBudget.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : Controller
    {
        private readonly ILogger<BudgetsController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public BudgetsController(ILogger<BudgetsController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        // GET: api/budgets
        [HttpGet]
        public async Task<ActionResult> GetBudgets()
        {
            _logger.LogInformation("GetBudgets action called.");
            var budgets = await _unitOfWork.Budgets.GetAllAsync();

            if (budgets == null || !budgets.Any())
            {
                _logger.LogWarning("No budgets found.");
            }

            return Ok(budgets);
        }

        // GET: api/budgets/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetBudget(Guid id)
        {
            _logger.LogInformation("GetBudget action called with id: {Id}", id);

            var budget = await _unitOfWork.Budgets.GetByIdAsync(id);
            if (budget == null)
            {
                _logger.LogWarning("Budget with id {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Budget with id {Id} retrieved successfully.", id);
            return Ok(budget);
        }

        // POST: api/budgets
        [HttpPost]
        public async Task<ActionResult> CreateBudget(Budget budget)
        {
            _logger.LogInformation("CreateBudget action called.");

            budget.Id = Guid.NewGuid();
            await _unitOfWork.Budgets.AddAsync(budget);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Budget created successfully with id {Id}.", budget.Id);

            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

        // PUT: api/budgets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBudget(Guid id, Budget updatedBudget)
        {
            _logger.LogInformation("UpdateBudget action called with id: {Id}", id);

            var budget = await _unitOfWork.Budgets.GetByIdAsync(id);
            if (budget == null)
            {
                _logger.LogWarning("Budget with id {Id} not found for update.", id);
                return NotFound();
            }

            budget.Category = updatedBudget.Category;
            budget.Amount = updatedBudget.Amount;
            budget.StartDate = updatedBudget.StartDate;
            budget.EndDate = updatedBudget.EndDate;

            _unitOfWork.Budgets.Update(budget);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Budget with id {Id} updated successfully.", id);

            return NoContent();
        }

        // DELETE: api/budgets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(Guid id)
        {
            _logger.LogInformation("DeleteBudget action called with id: {Id}", id);

            var budget = await _unitOfWork.Budgets.GetByIdAsync(id);
            if (budget == null)
            {
                _logger.LogWarning("Budget with id {Id} not found for deletion.", id);
                return NotFound();
            }

            _unitOfWork.Budgets.Remove(budget);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Budget with id {Id} deleted successfully.", id);

            return NoContent();
        }
    }
}