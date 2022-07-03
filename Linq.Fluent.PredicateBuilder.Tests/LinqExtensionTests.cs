using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.Linq;
using Xunit;
using ILogger = Serilog.ILogger;

namespace Linq.Fluent.PredicateBuilder.Tests
{
    public class LinqExtensionTests
    {
        static ILogger _logger = new LoggerConfiguration()
                    .WriteTo.File(path: "Test_Linq_Extension.txt", restrictedToMinimumLevel: LogEventLevel.Information)
                    .CreateLogger();

        public LinqExtensionTests()
        {
            var serviceProvider = new ServiceCollection()
                         .AddLogging(builder => builder.AddSerilog(dispose: true))
                         .BuildServiceProvider();
        }

        [Theory(DisplayName = "Test_Linq_Extension_With_PredicateBuilder")]
        [Repeat(10)]
        public void Test_Linq_Extension_With_PredicateBuilder(int iterationNumber)
        {
            var list = Enumerable.Range(1, 10000000);

            Stopwatch stopWatch = Stopwatch.StartNew();

            var result = list.Where(builder => builder.Initial(x => x >= 12 && x <= 13)
                                                      .Or(x => x <= 2)
                                                      .Or(x => x > 99999999)
                                               .ToPredicate())
                             .ToList();

            stopWatch.Stop();

            var extensionElapsedTime = stopWatch.ElapsedMilliseconds;

            _logger.Information($"Extension with PredicateBuilder Elapsed Time in ms: {extensionElapsedTime}");

            Assert.Equal(4, result.Count());
        }

        [Theory(DisplayName = "Test_Linq_Extension")]
        [Repeat(10)]
        public void Test_Linq_Extension(int iterationNumber)
        {
            var list = Enumerable.Range(1, 10000000);

            Stopwatch stopWatch = Stopwatch.StartNew();

            var result = list.Where(Operation.Or,
                                    x => x >= 12 && x <= 13,
                                    x => x <= 2,
                                    x => x > 99999999)
                             .ToList();

            stopWatch.Stop();

            var extensionElapsedTime = stopWatch.ElapsedMilliseconds;

            _logger.Information($"Extension Elapsed Time in ms: {extensionElapsedTime}");

            Assert.Equal(4, result.Count());
        }
    }
}
