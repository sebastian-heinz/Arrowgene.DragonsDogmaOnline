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

To get code completions in the file, make a temporary file, something like `intelisense.csx`. When developing, you can add the absolute path to this file at the top. This file contains `#r` directives which import assemblies required for the code server. It also will include `#load` directives which provide the absolute path to scripting library files. This line should be removed before checking into the repository.

```csharp
/**
 * @brief Helper file which should not be checked in as dependent on system paths
 */

#r "C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Arrowgene.Ddon.Shared.dll"
#r "C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Arrowgene.Ddon.GameServer.dll"

// Point these to the ones in the output directory if you are using hot reload
// Otherwise point these into the Arrowgene.Ddon.Scripts in the source
// When using hotload we need to make sure that this file loads the same file as the server will, otherwise we will get symbol conflicts
// because both .csx files will define the same symbols.
#load "C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Files\Assets\scripts\libs\DropRate.csx"
#load "C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Files\Assets\scripts\libs\ExtremeMissionUtils.csx"
#load "C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Files\Assets\scripts\libs\ScriptUtils.csx"
#load "C:\path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Files\Assets\scripts\libs\SeasonalEvents.csx"

// Using statements added by server so scripts don't need to add them
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Runtime.CompilerServices;
global using Arrowgene.Ddon.Database;
global using Arrowgene.Ddon.Database.Model;
global using Arrowgene.Ddon.GameServer;
global using Arrowgene.Ddon.GameServer.Characters;
global using Arrowgene.Ddon.GameServer.Chat;
global using Arrowgene.Ddon.GameServer.Context;
global using Arrowgene.Ddon.GameServer.Enemies;
global using Arrowgene.Ddon.GameServer.Party;
global using Arrowgene.Ddon.GameServer.Quests;
global using Arrowgene.Ddon.GameServer.Quests.Extensions;
global using Arrowgene.Ddon.GameServer.Scripting;
global using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
global using Arrowgene.Ddon.Server.Scripting;
global using Arrowgene.Ddon.Shared;
global using Arrowgene.Ddon.Shared.Entity.PacketStructure;
global using Arrowgene.Ddon.Shared.Entity.Structure;
global using Arrowgene.Ddon.Shared.Model;
global using Arrowgene.Ddon.Shared.Model.Quest;
```

Then, in your script file, use the `#load` directive to include it in the current script.

```csharp
/**
 * @brief Recurrence of Darkness
 */

#load "C:\path\to\intelisense.csx"

#load "DropRate.csx"
#load "ExtremeMissionUtils.csx"

public class ScriptedQuest : IQuest
{
    // Implementation ...
}
```

> [!NOTE]
> Some day, the built in `Microsoft.CodeAnalysis.LanguageServer` will have the ability to pass the assembly references to it and these instructions should be updated when that happens.

> [!NOTE]
> When you update an assembly, if the symbol doesn't show up type `> .NET: Restart Language Server`