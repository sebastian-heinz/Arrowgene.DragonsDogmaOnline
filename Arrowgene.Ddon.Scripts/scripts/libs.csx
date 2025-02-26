/**
 * @brief Helper file which should not be checked in as dependent on system paths
 * 
 * @note This file was added with the command:
 *     git update-index --assume-unchanged Arrowgene.Ddon.Scripts/scripts/libs.csx
 * This was done to prevent a developer for accidentally making a commit where
 * the #r paths are uncommented and contain the users local directory structure.
 * 
 * If you need any permanent changes to this file, execute the command:
 *     git update-index --no-assume-unchanged Arrowgene.Ddon.Scripts/scripts/libs.csx
 *
 * Make your changes and commit them. After, execute the following command
 * to prevent changes being made to the file again.
 *     git update-index --assume-unchanged Arrowgene.Ddon.Scripts/scripts/libs.csx
 */


// [!NOTE]
// To get syntax completion in vscode, uncomment the #r lines below and
// update "path\to" to be the path to the builds output directory.
//
// #r "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Arrowgene.Ddon.Shared.dll"
// #r "path\to\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net6.0\Arrowgene.Ddon.GameServer.dll"

// Point these to the ones in the output directory if you are using hot reload
// Otherwise point these into the Arrowgene.Ddon.Scripts in the source
// When using hotload we need to make sure that this file loads the same file as the server will, otherwise we will get symbol conflicts
// because both .csx files will define the same symbols.
#load "libs\DropRate.csx"
#load "libs\ExtremeMissionUtils.csx"
#load "libs\ScriptUtils.csx"
#load "libs\SeasonalEvents.csx"

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
