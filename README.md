# [StudentAssistant](https://trello.com/b/TXtoDDO0/student-assistant-kanban)

[![Build Status](https://dev.azure.com/boogiedkcore/StudentAssistant/_apis/build/status/boogiedk.StudentAssistant?branchName=master)](https://dev.azure.com/boogiedkcore/StudentAssistant/_build/latest?definitionId=13&branchName=master) 
[![Pull Requests](https://img.shields.io/github/issues-pr/boogiedk/studentassistant.svg)](https://github.com/boogiedk/studentassistant/pulls) 
[![Issues](https://img.shields.io/github/issues/boogiedk/studentassistant.svg)](https://github.com/boogiedk/studentassistant/issues) 
![License](https://img.shields.io/github/license/boogiedk/studentassistant.svg) ![Size](https://img.shields.io/github/repo-size/boogiedk/studentassistant.svg)
[![Code Coverage](https://img.shields.io/azure-devops/coverage/boogiedkcore/StudentAssistant/13)](https://dev.azure.com/boogiedkcore/StudentAssistant/_build/latest?definitionId=13&branchName=master)

## Project

Student Assistant - this is a web service with an open source project. The created for a convenient and flexible way to monitor the study schedule. 

You can try use it here: http://studyass.site

## Overview

### Schedule

Student Assistant provide service education schedule for students. With help calendar widget user can choose need date time for getting schedule on day.

The schedule tab looks like this:
<p align="center">
 <img width="900" height="500" align="center" src="https://sun9-20.userapi.com/c851532/v851532849/1b0fa3/CQ77BSThUcE.jpg">
</p>

So in the screenshot below you can see what can Student Asssitant application. For example, thanks tag `<input />` you can use native implementation calendar in browser or mobile or thanks other npm packages you can use popup window and calendar widget.

<p align="center">
 <img align="center" src="https://sun9-27.userapi.com/c857736/v857736849/7557c/wUWPWA9ZD4s.jpg">
</p>

Supports all browsers, including an Internet Explorer (not without problems, but it works).

## Compiling

 To compile and run the application on Linux or Windows, you need to [download and install latest .NET Core SDK](https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial), install [Node.js](https://github.com/nodesource/distributions/blob/master/README.md) and clone the repository using [Git](https://git-scm.com/). Then, go into `StudentAssistant.Backend` and `StudentAssistant.Frontend` directory and run `dotnet run` command. 
Or you can use build scripts:

1) For Linux or MacOS shell:
```sh
# Linux or MacOS shell.

git clone https://github.com/boogiedk/StudentAssistant
cd ./StudentAssistant && bash ./build.sh

# Go into `StudentAssistant.Backend` and `StudentAssistant.Frontend` directory and run `dotnet run` command
```

2) For Windows command line:
```sh
# Windows command line.
# Use the '--configuration Release' option to generate app packages.

git clone https://github.com/boogiedk/StudentAssistant
cd ./StudentAssistant && powershell -ExecutionPolicy Unrestricted ./build.ps1

# Go into `StudentAssistant.Backend` and `StudentAssistant.Frontend` directory and run `dotnet run` command
```

## Contributing

Contributors are welcome. Please submit an issue before introducing new features, then you might create a "work in progress" (WIP) pull request to prevent other people from working on the same feature. Dev group is here: [studyAssTalks](https://t.me/studyAssTalks), please feel free to ask questions. If you are new to this project there are some entry-level issues marked with "good first issue" tag.
Also you can visit kanban board [Trello](https://trello.com/b/TXtoDDO0/student-assistant-kanban) and watch for roadmap develop.

## Technology stack

* [.NET Core](https://github.com/dotnet)
* [React](https://reactjs.org/)
* [EF Core](https://github.com/dotnet/efcore)
* [SQLite](https://www.sqlite.org/index.html)
* [Nuke](https://nuke.build/)

