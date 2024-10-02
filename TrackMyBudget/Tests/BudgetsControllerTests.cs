using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Helper;
using TrackMyBudget.Controllers;
using TrackMyBudget.Models;

namespace Tests
{
    [TestCaseOrderer("Tests.Helper.PriorityOrderer", "Tests")]
    public class BudgetsControllerTests
    {
        private readonly BudgetsController _controller;
        private readonly Mock<ILogger<BudgetsController>> _loggerMock;

        public BudgetsControllerTests()
        {
            // Initialize the mock logger
            _loggerMock = new Mock<ILogger<BudgetsController>>();

            // Initialize the controller with the mock logger
            _controller = new BudgetsController(_loggerMock.Object);
        }

        [Fact, TestPriority(1)]
        public void GetBudgets_ReturnsOkResult_WithListOfBudgets()
        {
            // Act
            var result = _controller.GetBudgets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBudgets = Assert.IsType<List<Budget>>(okResult.Value);
            Assert.Empty(returnBudgets);  // Assuming initially the list is empty
        }

        [Fact, TestPriority(2)]
        public void GetBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            // Act
            var result = _controller.GetBudget(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(3)]
        public void GetBudget_ReturnsOk_WhenBudgetExists()
        {
            // Arrange
            var budget = new Budget { Id = Guid.NewGuid(), Category = "Test", Amount = 100 };
            BudgetsController.Budgets.Add(budget);  // Add a budget to the static list

            // Act
            var result = _controller.GetBudget(budget.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnBudget = Assert.IsType<Budget>(okResult.Value);
            Assert.Equal(budget.Id, returnBudget.Id);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("retrieved successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(4)]
        public void NewBudget_ReturnsCreatedResponse()
        {
            // Arrange
            var budget = new Budget { Category = "New Category", Amount = 500 };

            // Act
            var result = _controller.CreateBudget(budget);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnBudget = Assert.IsType<Budget>(createdResult.Value);
            Assert.Equal(budget.Category, returnBudget.Category);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("created successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(5)]
        public void UpdateBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            // Arrange
            var updatedBudget = new Budget { Category = "Updated Category", Amount = 300 };

            // Act
            var result = _controller.UpdateBudget(Guid.NewGuid(), updatedBudget);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found for update")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(6)]
        public void UpdateBudget_ReturnsNoContent_WhenBudgetExists()
        {
            // Arrange
            var budget = new Budget { Id = Guid.NewGuid(), Category = "Test", Amount = 100 };
            BudgetsController.Budgets.Add(budget);  // Add a budget to the static list

            var updatedBudget = new Budget { Category = "Updated Category", Amount = 500 };

            // Act
            var result = _controller.UpdateBudget(budget.Id, updatedBudget);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated Category", budget.Category);
            Assert.Equal(500, budget.Amount);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("updated successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(7)]
        public void RemoveBudget_ReturnsNotFound_WhenBudgetDoesNotExist()
        {
            // Act
            var result = _controller.DeleteBudget(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found for deletion")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact, TestPriority(8)]
        public void RemoveBudget_ReturnsNoContent_WhenBudgetExists()
        {
            // Arrange
            var budget = new Budget { Id = Guid.NewGuid(), Category = "Test", Amount = 100 };
            BudgetsController.Budgets.Add(budget);  // Add a budget to the static list

            // Act
            var result = _controller.DeleteBudget(budget.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.DoesNotContain(budget, BudgetsController.Budgets);

            // Verify logging
            _loggerMock.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("deleted successfully")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}