using Microsoft.AspNetCore.Mvc;
using TrackMyBudget.Models;

namespace TrackMyBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : Controller
    {
        public static readonly List<Budget> Budgets = [];
        private readonly ILogger<BudgetsController> _logger;

        public BudgetsController(ILogger<BudgetsController> logger)
        {
            _logger = logger;
        }

        // GET: api/budgets
        [HttpGet]
        public ActionResult<IEnumerable<Budget>> GetBudgets()
        {
            _logger.LogInformation("GetBudgets action called.");

            if (Budgets.Count == 0)
            {
                _logger.LogWarning("No budgets found.");
            }

            return Ok(Budgets);
        }

        // GET: api/budgets/{id}
        [HttpGet("{id}")]
        public ActionResult<Budget> GetBudget(Guid id)
        {
            _logger.LogInformation("GetBudget action called with id: {Id}", id);

            var budget = Budgets.FirstOrDefault(b => b.Id == id);
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
        public ActionResult<Budget> CreateBudget(Budget budget)
        {
            _logger.LogInformation("CreateBudget action called.");

            budget.Id = Guid.NewGuid();
            Budgets.Add(budget);

            _logger.LogInformation("Budget created successfully with id {Id}.", budget.Id);

            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

        // PUT: api/budgets/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBudget(Guid id, Budget updatedBudget)
        {
            _logger.LogInformation("UpdateBudget action called with id: {Id}", id);

            var budget = Budgets.FirstOrDefault(b => b.Id == id);
            if (budget == null)
            {
                _logger.LogWarning("Budget with id {Id} not found for update.", id);
                return NotFound();
            }

            budget.Category = updatedBudget.Category;
            budget.Amount = updatedBudget.Amount;
            budget.StartDate = updatedBudget.StartDate;
            budget.EndDate = updatedBudget.EndDate;

            _logger.LogInformation("Budget with id {Id} updated successfully.", id);

            return NoContent();
        }

        // DELETE: api/budgets/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBudget(Guid id)
        {
            _logger.LogInformation("DeleteBudget action called with id: {Id}", id);

            var budget = Budgets.FirstOrDefault(b => b.Id == id);
            if (budget == null)
            {
                _logger.LogWarning("Budget with id {Id} not found for deletion.", id);
                return NotFound();
            }

            Budgets.Remove(budget);
            _logger.LogInformation("Budget with id {Id} deleted successfully.", id);

            return NoContent();
        }
    }
}