#tool "nuget:?package=ILRepack"

//////////////////////////////////////////////////////////////////////
// VERSION
//////////////////////////////////////////////////////////////////////

var nugetVersion = "3.1.2";

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"");
var netstandard20 = new Framework("netstandard2.0");
var solutionName = "XFluentAssert.sln";
var mainDll = "TddXt.XFluentAssertRoot.dll";


//////////////////////////////////////////////////////////////////////
// DEPENDENCIES
//////////////////////////////////////////////////////////////////////


var nSubstitute      = new[] {"NSubstitute"      , "4.0.0"};
var fluentAssertions = new[] {"FluentAssertions" , "5.6.0"};
var any              = new[] {"Any"              , "3.0.1"};
var compareNetObjects= new[] {"CompareNETObjects", "4.57.0"};


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
    .Does(() =>
{
    DotNetCoreBuild("./src/netstandard2.0/Root", new DotNetCoreBuildSettings
     {
         Configuration = configuration,
         OutputDirectory = buildNetStandardDir,
         ArgumentCustomization = args=>args.Append("/property:Version=" + nugetVersion)
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
    var assemblyPaths = GetFiles(specificVersionPublishDir + "/TddXt.XFluentAssert*.dll");
    var mainAssemblyPath = new FilePath(fullRootDllFilePath).MakeAbsolute(Context.Environment);
    assemblyPaths.Remove(mainAssemblyPath);
    foreach(var path in assemblyPaths)
    {
        Console.WriteLine(path);
    }
    ILRepack(fullRootDllFilePath, fullRootDllFilePath, assemblyPaths,  new ILRepackSettings 
	{ 
		Internalize = true
	});
    DeleteFiles(assemblyPaths);
}

Task("Pack")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() => 
    {
        CopyDirectory(buildDir, publishDir);
        BundleDependencies(publishNetStandardDir, mainDll);
        NuGetPack("./XFluentAssert.nuspec", new NuGetPackSettings()
        {
            Id = "XFluentAssert",
            Title = "XFluentAssert",
            Owners = new [] { "Grzegorz Galezowski" },
            Authors = new [] { "Grzegorz Galezowski" },
            Summary = "A set of assertions to be used together with FluentAssertions library.",
            Description = "A set of assertions to be used together with FluentAssertions library.",
            Language = "en-US",
            ReleaseNotes = new[] {"Added assertions for dependency chains and AndConstraints to existing structural assertions"},
            ProjectUrl = new Uri("https://github.com/grzesiek-galezowski/xassert"),
            OutputDirectory = "./nuget",
            Version = nugetVersion,
            Files = new [] 
            {
                new NuSpecContent {Source = @".\publish\netstandard2.0\TddXt.XFluentAssertRoot.dll", Exclude=@"**\*.json", Target = @"lib\netstandard2.0"},
            },
            Dependencies = new [] 
            {
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
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);