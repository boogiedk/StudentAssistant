# [StudentAssistant](https://trello.com/b/TXtoDDO0/student-assistant-kanban)


[![Build status](https://ci.appveyor.com/api/projects/status/qw8j6ojtbj4myiun?svg=true)](https://ci.appveyor.com/project/boogiedk/studentassistant) 
[![Pull Requests](https://img.shields.io/github/issues-pr/boogiedk/studentassistant.svg)](https://github.com/boogiedk/studentassistant/pulls) 
[![Issues](https://img.shields.io/github/issues/boogiedk/studentassistant.svg)](https://github.com/boogiedk/studentassistant/issues) 
![License](https://img.shields.io/github/license/boogiedk/studentassistant.svg) ![Size](https://img.shields.io/github/repo-size/boogiedk/studentassistant.svg) 

## Project

Student Assistant - this is a web service with an open source project. The created for a convenient and flexible way to monitor the study schedule. 

You can try use it here: http://studyass.site

## Overview

### Schedule

Student Assistant provide service education schedule for students. With help calendar widget user can choose need date time for getting schedule on day.

The schedule tab looks like this:
<p align="center">
 <img width="900" height="500" align="center" src="https://pp.userapi.com/c855420/v855420888/c6bab/-p8gIBa0Qq8.jpg">
</p>

So, thanks tag `<input />` you can use native implementation calendar in browser or mobile. For example, calendar look like this on IOS mobile:

<p align="center">
 <img width="450" height="800" align="center" src="https://pp.userapi.com/c850720/v850720007/198b4f/W4HhyGUJ98s.jpg">
</p>

Supports all browsers, including an Internet Explorer (not without problems, but it works).

## Compiling

1) To compile and run the application on Linux (similar to Windows), you need to [download and install latest .NET Core SDK](https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial). Clone the repository using [Git](https://git-scm.com/). Then, go into `StudentAssistant.Backend` directory and run `dotnet run` command.

```sh
# Remember to install .NET Core SDK, .NET Core Runtime and git before executing this.

git clone https://github.com/boogiedk/StudentAssistant
cd ./StudentAssistant.Backend/
dotnet restore
dotnet run
```
2) To get started with Reactjs and build app you need install [Node.js](https://github.com/nodesource/distributions/blob/master/README.md)

```sh
# Remember to install Nodejs, .NET Core SDK, .NET Core Runtime and git before executing this.

git clone https://github.com/boogiedk/StudentAssistant
cd StudentAssistant.Frontend/ClientApp
npm install

cd ../
dotnet run
```

## Contributing

Contributors are welcome. Please submit an issue before introducing new features, then you might create a "work in progress" (WIP) pull request to prevent other people from working on the same feature. Dev group is here: [studyAssTalks](https://t.me/studyAssTalks), please feel free to ask questions. If you are new to this project there are some entry-level issues marked with "good first issue" tag.
Also you can visit kanban board [Trello](https://trello.com/b/TXtoDDO0/student-assistant-kanban) and watch for roadmap develop.

## Technology stack

* [.NET Core](https://github.com/dotnet)
* [React](https://reactjs.org/)
