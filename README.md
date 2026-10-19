# SysPurge
SysPurge: a Windows cleaning tool



## Author

* [@remusrigo](https://github.com/RemusRigo)
* [@remusrigo](https://sourceforge.net/u/remusrigo/profile)



## Features

* File System:
  * Temporary/Junk files: user and system Temp folders, log files, previous Windows installation (`Windows.old`)
  * Microsoft Windows: Jump Lists, Prefetch files, Recent Items, Windows Update cache
* Registry:
  * Run MRU
  * missing Shared DLL's
* Microsoft Apps:
  * .NET and Visual Studio telemetry data
  * DirectX (shader cache, Direct3D most recent application)
  * Event Viewer (logs)
  * Internet Explorer (cache, cookies, temporary files, typed URLs)
  * Microsoft Management Console (recent files)
  * OneDrive (temp)
  * PowerShell (console history)
  * Teams (cache)
  * Windows Media Player (recent files, URLs)
* Apps:
  * Adobe (recent files)
  * Google / Google Chrome (crash reports, Software Reporter Tool logs)
  * Java (cache)
  * Media Player Classic (recent files)
  * privacy.sexy (runs, logs)
  * Steam (appcache, dumps, traces)
* Options that need administrator rights are shown only when running as administrator
* Logs actions to a file
* Tested on:
  * Windows 10 Ent x64
  * Windows 11 Pro x64



## User's Guide

See the [User's Guide](UsersGuide.md) for downloads, runtimes, installation and usage.



## Statistics

![VB.NET](https://img.shields.io/badge/Language-VB.NET-blue)
![API](https://img.shields.io/badge/API-AdvAPI32-lightblue)
![API](https://img.shields.io/badge/API-User32-lightblue)
<br>
![IDE](https://img.shields.io/badge/IDE-Visual%20Studio-purple?logo=visualstudio&logoColor=white)
<br>
![OS](https://img.shields.io/badge/OS-Windows-green?logo=windows&logoColor=white)
![Runtime](https://img.shields.io/badge/Runtime-.NET%208.0-lightgreen)
![Runtime](https://img.shields.io/badge/Runtime-.NET%2010.0-lightgreen)
<br>
![status](https://img.shields.io/badge/status-in%20progress-orange) 
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/RemusRigo/SysPurge-VB.NET/total)

 <img alt="Written in pure Rust" src="https://img.shields.io/badge/pure-Rust-6a3fd6?logo=rust&logoColor=white">


## Roadmap

* 2026-07-25: Update: Change Clean Task to delete the parent folder if empty (optional)
* 2026-07-25: Add: Clear Previous Windows Installations folder
* 2026-07-25: Add: Clear OneDrive Temp folder
* 2026-07-25: Add: Clear Steam
* 2026-07-23: Update: Change UI: replace TreeView with Buttons / split into four categories
* 2026-07-21: Update: Change UI / split into two categories
* 2026-07-18: Add: Clear Google Chrome Software Reporter Tool logs
* 2026-07-18: Add: Clear privacy.sexy runs
* 2026-07-18: Add: Clear privacy.sexy logs
* 2026-06-30: Add: Log class to report errors/warnings/info
* 2026-06-30: Add: Clear Recent files
* 2026-06-30: Add: Clear PowerShell Console Host History
* 2026-06-14: Add: Clear Microsoft Teams cache
* 2026-06-13: Add: New Task for clearing folders to support multiple paths
* 2026-06-13: Add: BlueFlame.ico
* 2026-06-10: Add: libServices.vb
* 2026-06-10: Add: Updated Messages.vb constants
* 2026-06-10: Add: Create ListView groups and items at run-time
* 2026-06-10: New: Project started from scratch / Based on the Delphi version
* 2025-11-24: New: Project started from scratch
* 2013-03-14: Add: Background Worker
* 2013-03-13: Add: Delete all files in a folder
* 2012-04-21: Add: Clean Registry RunMRU
* 2012-04-21: Add: Background Worker
* 2012-04-21: Project started



## History/Versions

* 2010-06-04: SysCleaner written in Delphi
* 2008-02-21: SysSweeper written in Delphi
