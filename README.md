# [StudentAssistant](https://trello.com/b/TXtoDDO0/student-assistant-kanban) - StudentAssistant


| Platform | Status | Download |
| -------- | ------ | -------- |
| MacOS    | [![Build Status]()]() | **[.dmg](https://github.com/boogiedk/StudentAssistant/releases)** |
| Windows  | [![Build Status]()]() | **[.exe](https://github.com/boogiedk/StudentAssistant/releases)** **[.zip](https://github.com/boogiedk/StudentAssistant/releases)** |
| Linux    | [![Build Status]()]() | **[.tar.gz](https://github.com/boogiedk/StudentAssistant/releases)** |

## Project

Student Assistant - this is a web service with an open source project. The created for a convenient and flexible way to monitor the study schedule. 

![screenshot](https://pp.userapi.com/c849124/v849124521/11f18e/WP265gLwf8Y.jpg)

## Compiling

To compile and run the application, you need to [download and install latest .NET Core SDK](https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial). Clone the repository using [Git](https://git-scm.com/). Then, go into `StudentAssistant.Backend` directory and run `dotnet run` command.

```sh
# Remember to install .NET Core SDK and git before executing this.

git clone https://github.com/boogiedk/StudentAssistant
cd .\StudentAssistant.Backend\
dotnet restore
dotnet run
```

You also need to install Node.js and execute the command:

```sh
cd .\StudentAssistant.Frontend\
npm i node-modules
```

## Contributing

Contributors are welcome. Please submit an issue before introducing new features, then you might create a "work in progress" (WIP) pull request to prevent other people from working on the same feature. Dev group is here: [studyAssTalks](https://t.me/studyAssTalks), please feel free to ask questions. If you are new to this project there are some entry-level issues marked with "good first issue" tag.
Also you can visit kanban board [Trello](https://trello.com/b/TXtoDDO0/student-assistant-kanban) and watch for roadmap develop.

## Technology stack

* [.NET Core](https://github.com/dotnet)
* [Angular](https://github.com/angular)
* [Trello](https://trello.com)
* [TypeScript](https://www.typescriptlang.org/)
* [TeamCity](https://jetbrains.ru/products/teamcity/)
* [Azure](https://azure.microsoft.com/ru-ru/)
