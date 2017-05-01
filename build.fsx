// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"
#r "./packages/FAKE.Dotnet/tools/Fake.Dotnet.dll"

open Fake
open Fake.Dotnet

let solutionOrFsProjFileName = "sample.fsproj"


Target "Clean" (fun _ ->
    !! "artifacts" ++ "/bin" ++ "test/bin" ++ "/obj"
        |> DeleteDirs
)

Target "InstallDotnet" (fun _ ->
    DotnetSdkInstall SdkVersions.NetCore101
)

Target "RestorePackages" (fun _ ->
    DotnetRestore id solutionOrFsProjFileName
)

Target "Build" (fun _ ->
    DotnetBuild (fun c -> 
        { c with 
            Configuration = Debug;
            VersionSuffix = Some "ci";
            OutputPath = Some (currentDirectory @@ "bin");
        }) solutionOrFsProjFileName
)

"Clean"
    ==> "InstallDotnet"
    ==> "RestorePackages"
    ==> "Build"

RunTargetOrDefault "Build"