using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
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
    }
}
