using Xunit.Abstractions;
using Xunit.Sdk;

namespace TrackMyBudget.Tests.Helper
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedTestCases = testCases.OrderBy(testCase =>
            {
                var priorityAttribute = testCase.TestMethod.Method
                    .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
                    .FirstOrDefault();

                // Default priority is 0 if no attribute is found.
                return priorityAttribute == null ? 0 : priorityAttribute.GetNamedArgument<int>("Priority");
            });

            return sortedTestCases;
        }

        public string DisplayName => nameof(PriorityOrderer);
    }
}