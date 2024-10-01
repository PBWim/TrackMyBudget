# TrackMyBudget API

**TrackMyBudget** is a simple .NET Core Web API for managing budget entries. It supports basic CRUD operations to create, view, update, and delete budget records.

## Features
- **Create**: Add a new budget entry (category, amount, start date, end date).
- **View All**: Retrieve a list of all budgets.
- **View by ID**: Get details of a specific budget by ID.
- **Update**: Modify an existing budget entry.
- **Delete**: Remove a budget entry by its ID.

## Endpoints

1. **GET /api/budgets** - Retrieve all budgets.
2. **GET /api/budgets/{id}** - Get a budget by ID.
3. **POST /api/budgets** - Create a new budget.
4. **PUT /api/budgets/{id}** - Update a budget.
5. **DELETE /api/budgets/{id}** - Delete a budget.

## Swagger API Documentation

You can explore and test the API using Swagger:

- **Swagger UI**: Navigate to `https://localhost:<port>/swagger` to access the API documentation and test the endpoints interactively.

## How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/TrackMyBudget.git
   cd TrackMyBudget

2. Build and run the application:
   ```bash
    dotnet run
   ```

