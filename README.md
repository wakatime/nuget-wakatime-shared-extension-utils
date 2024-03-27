WakaTime Shared Extension Utils
=====================

[![Nuget](https://img.shields.io/nuget/vpre/WakaTime.Shared.ExtensionUtils?label=release)](https://www.nuget.org/packages/WakaTime.Shared.ExtensionUtils/)

C# nuget library to extend your extension using WakaTime core.

Installation
------------

1. Open `Manage NuGet Packages...`

2. Find for `WakaTime.Shared.ExtensionUtils`

.Net Version
--------------------------------

* .Net Standard 2.0

Contributing
------------

To open and build this project, please use Visual Studio 2019.


How to use
------------

1. Initialize WakaTime in your extension
```csharp
using WakaTime.Shared.ExtensionUtils;

public class YourExtension : IExtension
{
    public void Initialize()
    {
        // Your code here
        
        var metadata = new Metadata() // The metadata is used in the heartbeat to identify the editor and plugin
        {
            EditorName = "MyEditor", // Your editor name
            EditorVersion = "1.0.0", // Your editor version
            PluginName = "MyPlugin", // Your plugin name
            PluginVersion = "2.0.0", // Your plugin version
        };
        
        var logger = new MyLogger(); // Your logger class implementing WakaTime.Shared.ExtensionUtils.ILogger
        var wakaTime = new WakaTime(metadata, logger); // Initialize WakaTime
        
        // Optional: Add flags which will be apllied to all heartbeats
        wakaTime.CommonFlags.AddFlagLanguage("MyLanguage");
        wakaTime.CommonFlags.AddFlagProject("MyProjectName");
        // + more options available
        
        Task.Run(async () => await WakaTime.InitializeAsync()).GetAwaiter().GetResult(); // Start WakaTime
        
        var myPlugin = new MyPlugin(wakaTime); // Your plugin class containing event handlers for editor events
    }
}
```

2. Send heartbeats  
There are two ways to send heartbeats:
- _legacy_ using `WakaTime.HandleActivity` method
- _new_ using `WakaTime.CreateHeartbeat().Send()` method

_Legacy_:
```csharp
using WakaTime.Shared.ExtensionUtils;

public class MuPlugin
{
    private readonly WakaTime _wakaTime;
    
    // constructor
    public MuPlugin(WakaTime wakaTime)
    {
        _wakaTime = wakaTime;        
        // Your code here
    }
    
    public void OnEditorEvent()
    {
        // Your code here
        string file = "path/to/file";
        bool isWrite = true;
        string alternateProject = "MyAlternateProject";
        HeartbeatCategory category = HeartbeatCategory.Code;
        EntityType entityType = EntityType.File;
        
        _wakaTime.HandleActivity(file, isWrite, alternateProject, category, entityType);
    }
}
```

_New_:
```csharp
using WakaTime.Shared.ExtensionUtils;

public class MuPlugin
{
    private readonly WakaTime _wakaTime;
    
    // constructor
    public MuPlugin(WakaTime wakaTime)
    {
        _wakaTime = wakaTime;        
        // Your code here
    }
    
    public void OnEditorEvent()
    {
        // Your code here
        string file = "path/to/file";
        bool isWrite = true;
        string alternateProject = "MyAlternateProject";
        HeartbeatCategory category = HeartbeatCategory.Code;
        EntityType entityType = EntityType.File;
        
        // additional flags:
        string language = "MyLanguage";
        string alternateLanguage = "MyAlternateLanguage";
        string project = "MyProject";
        bool verbose = true;
        
        // Send heartbeat - option 1
        _wakaTime.CreateHeartbeat(file, isWrite, alternateProject, category, entityType)
            .AddFlagLanguage(language)
            .AddFlagAlternateLanguage(alternateLanguage)
            .AddFlagProject(project)
            .AddFlagVerbose(verbose)
            .Send();
        
        // Send heartbeat - option 2
        _wakaTime.CreateHeartbeat()
            .AddFlagEntity(file)
            .AddFlagWrite(isWrite)
            .AddFlagAlternateProject(alternateProject)
            .AddFlagCategory(category)
            .AddFlagEntityType(entityType)
            .AddFlagLanguage(language)
            .AddFlagAlternateLanguage(alternateLanguage)
            .AddFlagProject(project)
            .AddFlagVerbose(verbose)
            .Send();
        
        // Send heartbeat - option 3
        var heartbeat = _wakaTime.CreateHeartbeat(file);
        heartbeat.AddFlagWrite(isWrite);
        heartbeat.AddFlagLanguage(language).AddFlagAlternateLanguage(alternateLanguage);
        heartbeat.AddFlagProject(project);
        heartbeat.AddFlagCategory(category);
        heartbeat.AddFlagEntityType(entityType);
        heartbeat.AddFlagAlternateProject(alternateProject);
        heartbeat.AddFlagVerbose(verbose);
        heartbeat.Send();
        
        // Send heartbeat - option 4
        var heartbeat = _wakaTime.CreateHeartbeat();
        _wakatime.HandleActivity(heartbeat);
    }
}
```


**Note:**   
`WakaTime.CommonFlags.AddFlag*` will be applied to all heartbeats.  
Adding flags with `Heartbeat.AddFlag*` to a heartbeat will override the values set in `WakaTime.CommonFlags`.  
If that is not desired, set the parameter `overwrite` to `false` in the `AddFlag*` method.