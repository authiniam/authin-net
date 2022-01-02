## **Nuget publish instruction**  
<br/>

1. Make sure Nuget.exe is installed on your machine.
2. Update `AssemblyVersion` and `AssemblyFileVersion` in `AssemblyInfo.cs`.
3. Update `<version>x.y.z</version>` and `<releaseNotes>Some note</releaseNotes>` in `Authin.Api.Sdk.nuspec` file.
4. Use `nuget.exe pack` command to generates `.nupkg` file.
5. Use [authin.iam@gmail.com](authin.iam@gmail.com) Microsoft Account to login into [Nuget](https://nuget.org) and upload generated package.