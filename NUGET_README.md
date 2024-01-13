## **Nuget publish instruction**  
<br/>

1. Make sure Nuget.exe is installed on your machine.
2. Update `Version` and `FileVersion` and `<ReleaseNotes>Some note</ReleaseNotes>``  in `Authin.Api.Sdk.csproj`.
3. Use `dotnet pack` command to generates `.nupkg` file.
4. Use [authin.iam@gmail.com](authin.iam@gmail.com) Microsoft Account to login into [Nuget](https://nuget.org) and upload generated package.