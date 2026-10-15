# SysPurge: User's Guide



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/SysPurge-VB.NET/releases).

Each release contains one folder per framework and architecture:

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net4.8 x64`|.NET Framework 4.8|64-bit|[.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) (included in Windows 10/11)|
|`net4.8 x86`|.NET Framework 4.8|32-bit|[.NET Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48) (included in Windows 10/11)|
|`net8.0 x64`|.NET 8.0|64-bit|[.NET 8.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net8.0 x86`|.NET 8.0|32-bit|[.NET 8.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net10.0 x64`|.NET 10.0|64-bit|[.NET 10.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/10.0)|
|`net10.0 x86`|.NET 10.0|32-bit|[.NET 10.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/10.0)|

> **Which one do I need?** On Windows 10/11, the **net4.8 x64** build runs without installing anything.
> For the .NET 8.0/10.0 builds, the plain ".NET Runtime" is not enough; install the **Desktop** Runtime.



## Install

SysPurge is portable: there is no installer.

1. Download and extract the folder that matches your system, e.g. `net4.8 x64`.
2. Copy the **whole folder** to a location where you have write access, for example `D:\My Apps\SysPurge\`.
3. Double-click `SysPurge.exe`, or right-click it → **Run as administrator** to see all the cleaning tasks.

> If Windows blocks the file ("Windows protected your PC"), right-click `SysPurge.exe` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Usage

1. Click a category button: **File System**, **Registry**, **MS Apps** or **Apps**.
2. Check the items you want to clean.
3. Click the **Run** button on the category's toolbar.

Close the programs you clean first (e.g. Chrome, Teams, Steam): files that are in use can't be deleted.
Options marked *admin* are shown only when SysPurge runs as administrator.

### File System

|Group|Items|
|-|-|
|**Temporary/Junk files**|User Temp folder, System Temp folder, Log files (inside Windows), Log files (System drive), Previous Windows installation *(admin)*|
|**Microsoft Windows FileSystem**|Jump List, Prefetch files, Recent Items, Windows Update cache *(admin)*|

### Registry

|Group|Items|
|-|-|
|**MRU**|Run MRU (the history of the **Win+R** Run box)|
|**Misc**|Shared DLL's: remove entries of shared DLLs that no longer exist *(admin)*|

### MS Apps

|Group|Items|
|-|-|
|**.NET**|Telemetry data|
|**EventViewer**|Logs|
|**DirectX**|Shader Cache, Direct3D: Most Recent Application|
|**Internet Explorer**|Cache, Cookies, Temporary Internet Files, Typed URLs|
|**Microsoft Management Console**|Recent File List|
|**OneDrive**|Temp folder *(admin)*|
|**PowerShell**|Console Host History|
|**Teams**|Cache|
|**Visual Studio**|Telemetry data|
|**Windows Media Player**|Recent File List, Recent URLs List|

### Apps

|Group|Items|
|-|-|
|**Adobe**|Adobe recent file history|
|**Google**|Crash Reports|
|**Google Chrome**|Crash Reports, Software Reporter Tool: Logs|
|**Java**|cache|
|**Media Player Classic**|Recent File List|
|**privacy.sexy**|runs, logs|
|**Steam**|appcache, Dumps, Traces|

> **Warning:** cleaned files and registry entries can't be restored. *Previous Windows installation* removes the
> option to go back to your previous Windows version.

Actions and errors are written to `Logs\<year>\<month>\<day>\SysPurge.log`.



## Uninstall

Delete the folder you copied in step 2 of *Install*. SysPurge doesn't write any settings.
