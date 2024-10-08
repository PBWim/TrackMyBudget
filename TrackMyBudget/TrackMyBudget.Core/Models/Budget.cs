namespace TrackMyBudget.Core.Models
{
    public class Budget
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}