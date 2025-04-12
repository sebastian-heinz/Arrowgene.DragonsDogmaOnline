# DDON Scripting

The DDON scripting is intended to expose certain server internal details of the game server that a server admin wish to be configure. While initially implemented as JSON file, as more complex features come into the picture, a more complex configuration archirecture is required.

The scripting root is `scripts` directory inside the assets directory. Internally the functions which perform reverse lookups for modules will
terminate once reaching the root.

Each directory inside scripts defines the scripting module. A module name should be all lowercase. Inside each module, there should be a `README.md` file which describes the purpose and usage of the module. It should also describe any guidelines required. When implementing a module, be aware if you want the module to be hotloadable. If you do, make sure to program in such a way that the settings can reflect as such after an update.

## Setting up code completion

Next section describes how to configure code completion for your scripts.

### vscode

- Install the Microsoft C# extension
  - You don't need the C# Dev Kit extension by Microsoft.

In your `settings`, add the following configs
```json
"csharp.maxProjectFileCountForDiagnosticAnalysis": -1,
"dotnet.server.useOmnisharp": false
```

To get code completions in the file, you need to update the `libs.csx` file in your output directory. Uncomment the two `#r` directives at the top of the file and update the path for your local build.

```csharp
// [!NOTE]
// To get syntax completion in vscode, uncomment the #r lines below and
// update "path\to" to be the path to the builds output directory.
//
#r "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Arrowgene.Ddon.Shared.dll"
#r "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Arrowgene.Ddon.GameServer.dll"
```

When implementing a script, if it doesn't exist, add a `#load` directive for the file `libs.csx` at the top.

To enable auto completion, the path required for vscode unfortunately requires the absolute path so the editor can locate the file.
> [!WARNING]
> Don't commit the file with the absolute path. The server code is able to resolve `#load "libs.csx"` correctly.

```csharp
/**
 * @brief Recurrence of Darkness
 */

#load "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\libs.csx"

public class ScriptedQuest : IQuest
{
    // Implementation ...
}
```

> [!NOTE]
> Some day, the built in `Microsoft.CodeAnalysis.LanguageServer` will have the ability to pass the assembly references to it and these instructions should be updated when that happens.

> [!NOTE]
> When you update an assembly, if the symbol doesn't show up type `> .NET: Restart Language Server`