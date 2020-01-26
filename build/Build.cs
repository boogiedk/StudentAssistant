using System;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotCover;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.RunInteractive);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    [Parameter] readonly bool Interactive;

    AbsolutePath TestsDirectory => RootDirectory / "StudentAssistant.Tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    const string CoverageFileName = "coverage.cobertura.xml";

    Target Clean => _ => _
        .Before(RunUnitTests)
        .Executes(() =>
        {
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target RunUnitTests => _ => _
        .DependsOn(Clean)
        .Executes(() =>
            RootDirectory
                .GlobFiles("**/*.Tests.csproj")
                .ForEach(path =>
                    DotNetTest(settings => settings
                        .SetProjectFile(path)
                        .SetConfiguration(Configuration)
                        .SetLogger($"trx;LogFileName={ArtifactsDirectory / "report.trx"}")
                        .SetLogOutput(true)
                        .SetResultsDirectory(ArtifactsDirectory)
                        .AddProperty("CollectCoverage", true)
                        .AddProperty("CoverletOutputFormat", "cobertura")
                        .AddProperty("Exclude", "[xunit.*]*")
                        .AddProperty("CoverletOutput", ArtifactsDirectory / CoverageFileName))));

    Target CompileStudentAssistant => _ => _
        .DependsOn(RunUnitTests)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    Target RunInteractive => _ => _
        .DependsOn(CompileStudentAssistant)
        .Executes(() => RootDirectory
            .GlobFiles($"**/StudentAssistant.Backend.csproj")
            .Where(x => Interactive)
            .ForEach(path =>
                DotNetRun(settings => settings
                    .SetProjectFile(path)
                    .SetConfiguration(Configuration)
                    .EnableNoRestore()
                    .EnableNoBuild())));
}