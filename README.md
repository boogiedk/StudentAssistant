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

Student Assistant provide service education schedule for students. With help calendar widget user can choose need date time for getting schedule on day. It looks like this:

![screenshot](https://pp.userapi.com/c851036/v851036091/133d32/QneItsuGjkY.jpg)

Source code:
```cs
    public async Task GetCourseScheduleSelected(DateTimeOffset dateTimeOffset)
    {
        var requestModel = new CourseScheduleRequestModel()
        {
            DateTimeRequest = dateTimeOffset
        };

        _courseScheduleViewModel = await Http.PostJsonAsync<CourseScheduleViewModel>(
                                                           $"{Startup.url}/api/schedule/selected", requestModel);

        Console.WriteLine(_courseScheduleViewModel.NameOfDayWeek
                          + " " + _courseScheduleViewModel.CoursesViewModel.Count);

        Console.WriteLine($"SelectedDateChanged: {dateTimeOffset}");

        StateHasChanged();
    }
```

### Parity of the day

The second function of the Student Assistant "Parity of the day" - special data for students to navigate in the schedule. With help calendar widget user can choose need date time too:

![screenshot](https://pp.userapi.com/c848520/v848520091/1a229c/XgUw1M66zaQ.jpg)

```cs
    public async Task GetParityOfTheWeek(DateTimeOffset dateTimeOffset)
    {
        var requestModel = new ParityOfTheWeekRequestModel()
        {
            SelectedDateTime = dateTimeOffset
        };

        _parityOfTheWeekViewModel = await Http.PostJsonAsync<ParityOfTheWeekViewModel>(
                                                            $"{Startup.url}/api/parity/selected", requestModel);

        _parityStyle = $"parity{_parityOfTheWeekViewModel.IsParity}";

        StateHasChanged();
    }
```

### Calendar widget

Separately, I want to highlight source code convert method for calendar widget. This code convert localdate(custom type from
calendar developer) to ``` DateTimeOffset```.

```cs
public DateTimeOffset ConvertLocalDateToDateTimeOffset(LocalDate? localDate)
    {
        try
        {
            if (!localDate.HasValue)
            {
                return DateTimeOffset.UtcNow;
            }
            var dateTimeString = localDate.Value.AtMidnight().ToDateTimeUnspecified()
                .ToString(CultureInfo.InvariantCulture);

            var result = DateTimeOffset.Parse(dateTimeString);

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return DateTimeOffset.UtcNow;
        }
    }
```

## Compiling

1) To compile and run the application, you need to [download and install latest .NET Core SDK](https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial). Clone the repository using [Git](https://git-scm.com/). Then, go into `StudentAssistant.Backend` directory and run `dotnet run` command.

```sh
# Remember to install .NET Core SDK and git before executing this.

git clone https://github.com/boogiedk/StudentAssistant
cd .\StudentAssistant.Backend\
dotnet restore
dotnet run
```
2) To get started with Blazor and build your first Blazor web app check out [Blazor's getting started guide](https://blazor.net/docs/get-started.html). Then, go to into `StudentAssistant.Frontend` directory and run `dotnet run` command.

```sh
# Remember to install Blazor and git before executing this.

git clone https://github.com/boogiedk/StudentAssistant
cd .\StudentAssistant.Frontend\
dotnet restore
dotnet run
```

## Contributing

Contributors are welcome. Please submit an issue before introducing new features, then you might create a "work in progress" (WIP) pull request to prevent other people from working on the same feature. Dev group is here: [studyAssTalks](https://t.me/studyAssTalks), please feel free to ask questions. If you are new to this project there are some entry-level issues marked with "good first issue" tag.
Also you can visit kanban board [Trello](https://trello.com/b/TXtoDDO0/student-assistant-kanban) and watch for roadmap develop.

## Technology stack

* [.NET Core](https://github.com/dotnet)
* [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/client)
