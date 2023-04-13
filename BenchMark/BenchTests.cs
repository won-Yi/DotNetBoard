using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;


namespace BenchMark
{
    public class BenchTests
    {
        [Benchmark]
        public int TestSum()
        {
            int a = 2;
            int b = 3;
            return a + b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenchTests>();
            var config = ManualConfig.Create(DefaultConfig.Instance)
                .WithSummaryStyle(SummaryStyle.Default.WithOutputTarget(OutputTarget.Create(Path.Combine(summary.ResultsDirectoryPath, $"{summary.Title}.md"), OutputKind.Markdown)));
            var resultFolderPath = Path.Combine(summary.ResultsDirectoryPath, summary.Title);
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new CustomConfig(resultFolderPath, config));
        }
    }
}