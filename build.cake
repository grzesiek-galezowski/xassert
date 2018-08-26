#tool "nuget:?package=ILRepack"
#tool "nuget:?package=GitVersion.CommandLine"

//////////////////////////////////////////////////////////////////////
// VERSION
//////////////////////////////////////////////////////////////////////

//var nugetVersion = "0.0.2";
GitVersion nugetVersion = null; 

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");
//var net45 = new Framework("net45");
//var net462 = new Framework("net462");
var netstandard20 = new Framework("netstandard2.0");
var solutionName = "XFluentAssert.sln";
var mainDll = "TddXt.XFluentAssert.Root.dll";


//////////////////////////////////////////////////////////////////////
// DEPENDENCIES
//////////////////////////////////////////////////////////////////////


//todo change
var castleCore       = new[] {"Castle.Core"      , "4.3.1"};
var nSubstitute      = new[] {"NSubstitute"      , "3.1.0"};
var fluentAssertions = new[] {"FluentAssertions" , "5.4.1"};
var any              = new[] {"Any"              , "1.1.3"};
var compareNetObjects= new[] {"CompareNETObjects", "4.55.0"};


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./build") + Directory(configuration);
var publishDir = Directory("./publish");
var srcDir = Directory("./src");
var specificationDir = Directory("./specification") + Directory(configuration);
var buildNetStandardDir = buildDir + Directory("netstandard2.0");
var publishNetStandardDir = publishDir + Directory("netstandard2.0");
var srcNetStandardDir = srcDir + Directory("netstandard2.0");
var slnNetStandard = srcNetStandardDir + File(solutionName);
var specificationNetStandardDir = specificationDir + Directory("netcoreapp2.0");
//var buildNet45Dir = buildDir + Directory("net45");
//var publishNet45Dir = publishDir + Directory("net45");
//var srcNet45Dir = srcDir + Directory("net45");
//var specificationNet45Dir = specificationDir + Directory("netstandard2.0");
//var slnNet45 = srcNet45Dir + File(solutionName);



//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(publishDir);
    CleanDirectory("./nuget");
});

Task("Build")
    .IsDependentOn("GitVersion")
    .Does(() =>
{
    DotNetCoreBuild("./src/netstandard2.0/Root", new DotNetCoreBuildSettings
     {
         Configuration = configuration,
         OutputDirectory = buildNetStandardDir,
         ArgumentCustomization = args=>args.Append("/property:Version=" + nugetVersion.NuGetVersionV2)
     });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projectFiles = GetFiles(srcNetStandardDir.ToString() + "/**/*Specification.csproj");
    foreach(var file in projectFiles)
    {
        DotNetCoreTest(file.FullPath, new DotNetCoreTestSettings           
        {
           Configuration = configuration
        });
    }
});

public void BundleDependencies(DirectoryPath specificVersionPublishDir, string rootDllName)
{
    var fullRootDllFilePath = specificVersionPublishDir + "/" + rootDllName;
    var assemblyPaths = GetFiles(specificVersionPublishDir + "/TddXt.XFluentAssert.*.dll");
    var mainAssemblyPath = new FilePath(fullRootDllFilePath).MakeAbsolute(Context.Environment);
    assemblyPaths.Remove(mainAssemblyPath);
    foreach(var path in assemblyPaths)
    {
        Console.WriteLine(path);
    }
    ILRepack(fullRootDllFilePath, fullRootDllFilePath, assemblyPaths);
    DeleteFiles(assemblyPaths);
}

Task("GitVersion")
    .Does(() =>
{
    nugetVersion = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
    });
    Console.WriteLine(nugetVersion.NuGetVersionV2);
});


Task("Pack")
    .IsDependentOn("Build")
    .Does(() => 
    {
        CopyDirectory(buildDir, publishDir);
        BundleDependencies(publishNetStandardDir, mainDll);
        //BundleDependencies(publishNet45Dir, mainDll);
        NuGetPack("./XFluentAssert.nuspec", new NuGetPackSettings()
        {
            Id = "XFluentAssert",
            Title = "XFluentAssert",
            Owners = new [] { "Grzegorz Galezowski" },
            Authors = new [] { "Grzegorz Galezowski" },
            Summary = "A set of assertions to be used together with FluentAssertions library.",
            Description = "A set of assertions to be used together with FluentAssertions library.",
            Language = "en-US",
            ReleaseNotes = new[] {"Added assertions for structural inspection"},
            ProjectUrl = new Uri("https://github.com/grzesiek-galezowski/xassert"),
            OutputDirectory = "./nuget",
            Version = nugetVersion.NuGetVersionV2,
            Files = new [] 
            {
                new NuSpecContent {Source = @".\publish\netstandard2.0\TddXt.XFluentAssert.*", Exclude=@"**\*.json", Target = @"lib\netstandard2.0"},
            },

            Dependencies = new [] 
            {
                netstandard20.Dependency(castleCore),
                netstandard20.Dependency(nSubstitute),
                netstandard20.Dependency(fluentAssertions),
                netstandard20.Dependency(compareNetObjects),
                netstandard20.Dependency(any),
            }

        });  
    });

    public class Framework
    {
        string _name;

        public Framework(string name)
        {
            _name = name;
        }

        public NuSpecDependency Dependency(params string[] idAndVersion)
        {
            return new NuSpecDependency { Id = idAndVersion[0], Version = idAndVersion[1], TargetFramework = _name };
        }
    }



//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("GitVersion")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);