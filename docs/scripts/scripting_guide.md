# Scripting Guide

The DDON emulated server utilizes the Roslyn .NET compiler to implement C# scripts which utilize the `.csx` extension. Scripts can be used to expose parts of the server which we want customizable but may be too difficult to express in JSON and CSV configuration files.

The server provides the abstract class `ScriptManager` which handles all the low level details of reading, compiling and detecting changes to the script files. There are two concrete implementations of the `ScriptManager` class located in the `Arrowgene.Ddon.Server` and `Arrowgene.Ddon.GameServer` projects. We need to do this because certain classes which are required for the `GameServer` can't be included in the `Server` project.

## Scripting Modules

All scripts for the project are located in the `Arrowgene.Ddon.Shared` project in the `Files/Assets/scripts` directory. This directory is considered the "script root". All scripts loaded by the server must be located in this heirachy.

At the top-level of the scripts directory is a `README.md` file. This is a document which gives some general guidelines to the organization and purpose of the different modules. A module is a directory of scripts which has a dedicated file system watcher assigned to it. Each module generally has a collection of related scripts. Each module also has it's own `README.md` file describing the best practices and expectations of that module.

There is also a special `module` which is not directly loaded by any of the projects, called `libs`. This module is a collection of scripts which can be loaded by other scripts in the `Arrowgene.Ddon.GameServer` project using the `#load` directive.

### Implementing a module

To implement a new module, add a new file to the `Scripting/Modules` directory in the `Arrowgene.Ddon.Server` or `Arrowgene.Ddon.GameServer` project. The name of this file should end with the word `Module`. This file should implement a new class which extends the `ScriptModule` class.

When implementing your module, the field `ModuleRoot` represents the directory which the scripts for this module are located inside the scripts root directory. The `Filter` field determines which files to load. This usually is a glob `*.csx` but it is also possible to give the name of a single file. There are also bools to determine if a module should scan subdirectories from the `ModuleRoot` or only look in the `ModuleRoot`. You can also control if scripts for the module support hotloading by configuring the property `EnableHotLoad`.

There are two functions which the new module must override `Options` and `EvaluateResult`.

The method `Options` returns a list of all imports (`using` statements) and references which should be available to the script. The Roslyn project gives us `ScriptOption.Default` which gives us most things required. Then We can use the method `AddImports` to add all the `using` statements required for the scripts in the module.

The script manager will then use the options and attemt to read, compile and execute each script in the module. For each script which is successfully executed, the method `EvaluateResult` is called. The `result` parameter contains the value that has been returned after executing the script. It also captures all the globals declared in the script. In this method you will write the hanndling required to save and use the result in other places of the server.

```c#
public class MyModule : ScriptModule
{
    public override string ModuleRoot => "tutorial_module";
    public override string Filter => "*.csx";
    public override bool ScanSubdirectories => true;
    public override bool EnableHotLoad => true;

    public MyModule();
    public override ScriptOptions Options();
    public override bool EvaluateResult(string path, ScriptState<object> result);
}
```

### Adding module to script manager

After creating the module class, add a new member to the `ServerScriptManager` or `GameServerScriptManager` and assign it to the `ScriptModules` object in the constructor. This is all that is required to connect the module to the server code.

Inside the `GameServer`, the module can be reached through the `DdonGameServer` object (`Server.ScriptManager.MyModule`). You can use this to access the other members of your module object in the desired places in the server code.

### Creating scripts

Generally, it is preferred to use an interface or abstract class to have scripts implement a concrete class which is returned by the script. It is also possible to have a script which returns nothing and use the globals declared in the file (this is how the `settings` module works). It is also possible to have a module where all scripts return something different (look at the `mixin` module for an example).

If creating an interface or abstract class for your module scripts, add the file to the `Interfaces` directory in the associated project. Each script can implement a class with the same name. The object name will get mangled by the Roslyn compiler enforcing that each object is unique.

For example suppose we implemented an interface `IMyModuleObj`. Each script can implement a `MyModuleObj` concrete implementation and return it and these values will be passed to the `EvaluateResult` function when called by the script manager.

```c#
public interface IMyModuleObj
{
    public void DoThing();
}
```

Then we implemented two scripts using this interface

```c#
// myscript1.csx 
public class MyModuleObj : IMyModuleObj
{
    public void DoThing()
    {
        Console.WriteLine("Hello, World!");
    }
}

return new MyModuleObj();
```

```c#
// myscript2.csx
public class MyModuleObj : IMyModuleObj
{
    public void DoThing()
    {
        Console.WriteLine("Goodbye, World!");
    }
}

return new MyModuleObj();
```

## Libraries

There exists a few different libraries that can be used by the `.csx` scripts. Below will explain their scope and purpose. All libraries intended for scripts should be located in the `Scripting` directory in their respective projects. The file and classes whould be prefixed with the word `Lib`.

### Common Libraries

The library `LibUtils` can be used in both the `Server` and `GameServer` projects. This library consists of utility functions for certain operations common in the project. You can locate it at `Arrowgene.Ddon.Server/Scripting/LibUtils.cs`.

### GameServer Libraries

The library `LibDdon` is a library which can be included by scripts loaded in the `Arrowgene.Ddon.GameServer` project. You can locate it at `Arrowgene.Ddon.GameServer/Scripting/LibDdon.cs`.

### GameServer Script Libraries

There are libraries written in C# scripts which can be included by other C# scripts. They are located in `<scriptroot>/libs`. Scripts loaded by the `GameServer` can include them by using the `#load` directive.

#### DropRate.csx

Contains constants related to common drop rate values to assign to item drops.

#### SeasonalEvents.csx

This script contains shared utility functions used across various Seasonal Quests.