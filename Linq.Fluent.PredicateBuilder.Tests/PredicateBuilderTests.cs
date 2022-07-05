using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using ILogger = Serilog.ILogger;

namespace Linq.Fluent.PredicateBuilder.Tests
{
    public class PredicateBuilderTests
    {
        static ILogger _logger = new LoggerConfiguration()
                    .WriteTo.File(path: "Test_Fluent_PredicateBuilder.txt", restrictedToMinimumLevel: LogEventLevel.Information)
                    .CreateLogger();

        public PredicateBuilderTests()
        {
            var serviceProvider = new ServiceCollection()
                         .AddLogging(builder => builder.AddSerilog(dispose: true))
                         .BuildServiceProvider();
        }

        [Theory(DisplayName = "Test_Fluent_PredicateBuilder")]
        [Repeat(10)]
        public void Test_Fluent_PredicateBuilder(int iterationNumber)
        {
            var list = Enumerable.Range(1, 10000000);

            Stopwatch stopWatch = Stopwatch.StartNew();

            var predicate = new PredicateBuilder<int>()
                                    .Initial(x => x >= 12 && x <= 13)
                                    .Or(x => x <= 2)
                                    .Or(x => x > 99999999)
                            .ToPredicate();

            var result = list.Where(predicate)
                             .ToList();

            stopWatch.Stop();

            var extensionElapsedTime = stopWatch.ElapsedMilliseconds;

            _logger.Information($"PredicateBuilder Elapsed Time in ms: {extensionElapsedTime}");

            Assert.Equal(4, result.Count());
        }

        [Theory(DisplayName = "Test_Fluent_PredicateBuilder_Conditional")]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_Fluent_PredicateBuilder_Conditional(bool restrict)
        {
            var list = new List<Employee>
            {
                new Employee { Id = 1, Name = "Cliff", Department = "Human Resources"},
                new Employee { Id = 2, Name = "John", Department = "IT"}
            };

            var predicate = new PredicateBuilder<Employee>()
                                    .Initial(e => true)
                                    .And(restrict,  e => e.Department == "IT")
                            .ToPredicate();

            var result = list.Where(predicate)
                             .ToList();

            if (restrict)
            {
                Assert.Single(result);
                Assert.Equal("John", result[0].Name);
            }                
            else
            {
                Assert.Equal(2, result.Count());
                Assert.Equal("Cliff", result[0].Name);
                Assert.Equal("John", result[1].Name);
            }                
        }
    }
}
