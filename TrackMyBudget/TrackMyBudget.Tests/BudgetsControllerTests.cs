using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrackMyBudget.Application.Controllers;
using TrackMyBudget.Core.Contract;
using TrackMyBudget.Core.Models;
using TrackMyBudget.Tests.Helper;

namespace TrackMyBudget.Tests
{
    [TestCaseOrderer("Tests.Helper.PriorityOrderer", "Tests")]
    public class BudgetsControllerTests
    {
        private readonly BudgetsController _controller;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBudgetRepository> _mockBudgetRepository;
        private readonly Mock<ILogger<BudgetsController>> _mockLogger;

        public BudgetsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBudgetRepository = new Mock<IBudgetRepository>();

            // Initialize the mock logger
            _mockLogger = new Mock<ILogger<BudgetsController>>();

            // Setup IUnitOfWork to return the mock budget repository
            _mockUnitOfWork.Setup(u => u.Budgets).Returns(_mockBudgetRepository.Object);

            // Initialize the controller with the mock logger
            _controller = new BudgetsController(_mockLogger.Object, _mockUnitOfWork.Object);
        }

        [Fact, TestPriority(1)]
        public async Task GetBudgets_ReturnsOkResult_WithListOfBudgets()
        {
            // Arrange
            var budgets = new List<Budget> { new() { Id = Guid.NewGuid(), Category = "Test", Amount = 100 } };
            _mockBudgetRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(budgets);

            // Act
            var result = await _controller.GetBudgets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBudgets = Assert.IsType<List<Budget>>(okResult.Value);
            Assert.NotEmpty(returnBudgets);
        }

        [Fact, TestPriority(2)]
        public async Task GetBudgets_ReturnsEmptyList_WhenNoBudgets()
        {
            // Arrange
            _mockBudgetRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync([]);

            // Act
            var result = await _controller.GetBudgets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBudgets = Assert.IsType<List<Budget>>(okResult.Value);
            Assert.Empty(returnBudgets);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No budgets found.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(3)]
        public async Task GetBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            // Arrange
            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Budget)null);

            // Act
            var result = await _controller.GetBudget(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Budget with id")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(4)]
        public async Task GetBudget_ReturnsOk_WhenBudgetExists()
        {
            // Arrange
            var budget = new Budget { Id = Guid.NewGuid(), Category = "Test", Amount = 100 };
            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(budget.Id)).ReturnsAsync(budget);

            // Act
            var result = await _controller.GetBudget(budget.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBudget = Assert.IsType<Budget>(okResult.Value);
            Assert.Equal(budget.Id, returnBudget.Id);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("retrieved successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(5)]
        public async Task CreateBudget_ReturnsCreatedResponse()
        {
            // Arrange
            var budget = new Budget { Category = "New Category", Amount = 500 };

            // Act
            var result = await _controller.CreateBudget(budget);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnBudget = Assert.IsType<Budget>(createdResult.Value);
            Assert.Equal(budget.Category, returnBudget.Category);

            // Verify that the AddAsync method was called on the repository
            _mockBudgetRepository.Verify(repo => repo.AddAsync(It.IsAny<Budget>()), Times.Once);

            // Verify that CommitAsync was called on the UnitOfWork
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("created successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(6)]
        public async Task UpdateBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            // Arrange
            var updatedBudget = new Budget { Category = "Updated Category", Amount = 300 };
            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Budget)null);

            // Act
            var result = await _controller.UpdateBudget(Guid.NewGuid(), updatedBudget);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found for update")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(7)]
        public async Task UpdateBudget_ReturnsNoContent_WhenBudgetExists()
        {
            // Arrange
            var budget = new Budget { Id = Guid.NewGuid(), Category = "Test", Amount = 100 };
            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(budget.Id)).ReturnsAsync(budget);

            var updatedBudget = new Budget { Category = "Updated Category", Amount = 500 };

            // Act
            var result = await _controller.UpdateBudget(budget.Id, updatedBudget);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated Category", budget.Category);
            Assert.Equal(500, budget.Amount);

            // Verify that the Update method was called on the repository
            _mockBudgetRepository.Verify(repo => repo.Update(It.IsAny<Budget>()), Times.Once);

            // Verify that CommitAsync was called on the UnitOfWork
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("updated successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(8)]
        public async Task DeleteBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            // Arrange
            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Budget)null);

            // Act
            var result = await _controller.DeleteBudget(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found for deletion")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(9)]
        public async Task DeleteBudget_ReturnsNoContent_WhenBudgetExists()
        {
            // Arrange
            var budget = new Budget { Id = Guid.NewGuid(), Category = "Test", Amount = 100 };
            _mockBudgetRepository.Setup(repo => repo.GetByIdAsync(budget.Id)).ReturnsAsync(budget);

            // Act
            var result = await _controller.DeleteBudget(budget.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Verify that the Remove method was called on the repository
            _mockBudgetRepository.Verify(repo => repo.Remove(It.IsAny<Budget>()), Times.Once);

            // Verify that CommitAsync was called on the UnitOfWork
            _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);

            // Verify logging
            _mockLogger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("deleted successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}