using System;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using BuildScript;
using FluentAssertions;
using NScan.Adapter.NotifyingSupport;
using NScan.SharedKernel.WritingProgramOutput.Ports;
using TddXt.NScan;
using static Bullseye.Targets;
using static SimpleExec.Command;

var configuration = "Release";

// Define directories.
var root = AbsoluteFilePath.OfThisFile().ParentDirectory(3).Value;
var srcDir = root.AddDirectoryName("src");
var srcNetStandardDir = srcDir.AddDirectoryName("netstandard2.0");
var nugetPath = root.AddDirectoryName("nuget");
var version="5.0.1";

//////////////////////////////////////////////////////////////////////
// HELPER FUNCTIONS
//////////////////////////////////////////////////////////////////////
void Pack(AbsoluteDirectoryPath outputPath, AbsoluteDirectoryPath rootSourceDir, string projectName)
{
  Run("dotnet",
    $"pack" +
    $" -c {configuration}" +
    $" --include-symbols" +
    $" --no-build" +
    $" -p:SymbolPackageFormat=snupkg" +
    $" -p:VersionPrefix={version}" +
    $" -o {outputPath}",
    workingDirectory: rootSourceDir.AddDirectoryName(projectName).ToString());
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Target("Clean", () =>
{
  nugetPath.Delete(true);
  Run($"dotnet",
    "clean " +
    $"-c {configuration} ",
    workingDirectory: srcNetStandardDir.ToString());
});

Target("Build", () =>
{
  Run($"dotnet",
    "build " +
    $"-c {configuration} " +
    //$"-o {buildDir} " +
    $"-p:VersionPrefix={version}",
    workingDirectory: srcNetStandardDir.ToString());
});
Target((string) "NScan", DependsOn("Build"), () =>
{
  NScanMain.Run(
    new InputArgumentsDto
    {
      RulesFilePath = AbsoluteDirectoryPath.OfThisFile().AddFileName("rules.txt").AsAnyFilePath(),
      SolutionPath = srcNetStandardDir.AddFileName("XFluentAssert.sln").AsAnyFilePath()
    },
    new ConsoleOutput(),
    new ConsoleSupport()
  ).Should().Be(0);
});

Target("Test", DependsOn("Build"), () =>
{
  Run($"dotnet",
    "test" +
    $" --no-build" +
    $" -c {configuration}" +
    //$" -o {buildDir}" +
    $" -p:VersionPrefix={version}",
    workingDirectory: srcNetStandardDir.ToString());
});

Target("Pack", DependsOn("Test", (string) "NScan"), () =>
{
  Pack(nugetPath, srcNetStandardDir, "XFluentAssert");
});

Target("Push", DependsOn("Clean", "Pack"), () =>
{
    foreach (var nupkgPath in nugetPath.GetFiles("*.nupkg"))
    {
        Run("dotnet", $"nuget push {nupkgPath}" +
                      $" --source https://api.nuget.org/v3/index.json");
    }
});

Target("default", DependsOn("Pack"));

RunTargetsAndExit(args);

namespace BuildScript
{
    public class ConsoleOutput : INScanOutput
    {
        public void WriteAnalysisReport(string analysisReport)
        {
            Console.WriteLine(analysisReport);
        }

        public void WriteVersion(string coreVersion)
        {
            Console.WriteLine(coreVersion);
        }
    }
}

