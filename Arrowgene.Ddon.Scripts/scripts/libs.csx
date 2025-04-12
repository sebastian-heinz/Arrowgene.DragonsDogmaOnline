/**
 * @brief File which contains all the common script libraries.
 * 
 * @note Only make local edits in the file which is copied to the build output directory.
 */

// [!NOTE]
// To get syntax completion in visual studio and vscode, uncomment the #r lines below and
// update "path\to" to be the path to the builds output directory.
//
// #r "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Arrowgene.Ddon.Shared.dll"
// #r "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Arrowgene.Ddon.GameServer.dll"

// Point these to the ones in the output directory if you are using hot reload
// Otherwise point these into the Arrowgene.Ddon.Scripts in the source
// When using hotload we need to make sure that this file loads the same file as the server will, otherwise we will get symbol conflicts
// because both .csx files will define the same symbols.
#load "libs/DropRate.csx"
#load "libs/ExtremeMissionUtils.csx"
#load "libs/ScriptUtils.csx"
#load "libs/SeasonalEvents.csx"

// Using statements added by server so scripts don't need to add them
global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
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
