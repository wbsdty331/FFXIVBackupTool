<p align="center">
  <h2 align="center">FFXIV Backup Tool</h1>
</p>

a simple tool for backup [Final Fantasy XIV Online (FFXIV)](https://www.finalfantasyxiv.com/) Game Data.

## Install

- Make sure you have installed [.NET 6.0 Runtime](https://dotnet.microsoft.com/download).
- Download the latest release from [here](https://github.com/wbsdty331/FFXIVBackupTool/releases).

## How to Build

You should need to install:
- Visual Studio 2019/2022
- .NET 6.0 SDK
- `ICSharpCode.SharpZipLib` (Download from NuGet)

Clone this project and use NuGet to download dependencies, Then you may build it.

To use your own Application ClientId to Login Microsoft Account and use OneDrive backup feature, Please visit [Azure Active Directory](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade) and create a new app.

> 🔴 Note: FFXIVBackupTool requires these permission:

```
Files.Read.All
Files.ReadWrite.All
Sites.Read.All
Sites.ReadWrite.All
User.Read
```

Then replace `clientId` in code.
## License
MIT
