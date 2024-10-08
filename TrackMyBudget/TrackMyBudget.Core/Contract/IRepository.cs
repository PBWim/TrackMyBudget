namespace TrackMyBudget.Core.Contract
{
    /// <summary>
    /// The repository pattern abstracts the data access layer and provides a cleaner, 
    /// testable way to work with data. Start by defining a generic IRepository interface 
    /// that will handle basic CRUD operations:
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}