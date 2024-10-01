using Microsoft.AspNetCore.Mvc;
using TrackMyBudget.Models;

namespace TrackMyBudget.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : Controller
    {
        private static readonly List<Budget> Budgets = [];

        // GET: api/budgets
        [HttpGet]
        public ActionResult<IEnumerable<Budget>> GetBudgets()
        {
            return Ok(Budgets);
        }

        // GET: api/budgets/{id}
        //[HttpGet("{id}")]
        //public ActionResult<Budget> GetBudget(Guid id)
        //{
        //    var budget = Budgets.FirstOrDefault(b => b.Id == id);
        //    if (budget == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(budget);
        //}

        // POST: api/budgets
        [HttpPost]
        public ActionResult<Budget> CreateBudget(Budget budget)
        {
            budget.Id = Guid.NewGuid();
            Budgets.Add(budget);
            return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
        }

        // PUT: api/budgets/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBudget(Guid id, Budget updatedBudget)
        {
            var budget = Budgets.FirstOrDefault(b => b.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            budget.Category = updatedBudget.Category;
            budget.Amount = updatedBudget.Amount;
            budget.StartDate = updatedBudget.StartDate;
            budget.EndDate = updatedBudget.EndDate;

            return NoContent();
        }

        // DELETE: api/budgets/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBudget(Guid id)
        {
            var budget = Budgets.FirstOrDefault(b => b.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            Budgets.Remove(budget);
            return NoContent();
        }
    }
}