<p align="center">
  <h2 align="center">FFXIV Backup Tool</h1>
</p>

*a simple tool for backup [Final Fantasy XIV Online (FFXIV)](https://www.finalfantasyxiv.com/) Game Data.*

[![HitCount](http://hits.dwyl.com/wbsdty331/FFXIVBackupTool.svg)](http://hits.dwyl.com/wbsdty331/FFXIVBackupTool)

## Install

- Make sure you have installed [.NET Framework 4.7 Runtime](https://dotnet.microsoft.com/download/dotnet-framework/net472).
- Download the latest release from [here](https://github.com/wbsdty331/FFXIVBackupTool/releases).

## How to Build

You should need to install:
- Visual Studio 2019
- .NET Framework 4.7 Developer Pack
- `ICSharpCode.SharpZipLib` (Download from NuGet)

Double click `BackupTool.sln`, The first time you open this project, It will download all dependencies from NuGet. when the solution is ready, the default target architecture is x64, you can change it to x86 but I don't recommend it.

Press F5, the solution should launch after building.

If you want to use your Application to backup your file, Please visit [OneDrive Dev Center](https://dev.onedrive.com) to create a new App, and replace to your `ClientId`.

## License
MIT
