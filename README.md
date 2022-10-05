Dragons Dogma Online - Server
===
Server Emulator for the Game Dragons Dogma Online.   

## Table of contents
- [Disclaimer](#disclaimer)
- [Setup](#setup)
  - [Visual Studio](#visual-studio)
  - [VS Code](#vs-code)
  - [IntelliJ Rider](#intellij-rider)
- [Server](#server)
- [Client](#client)
- [Progress](#progress)
- [Guidelines](#guidelines)
- [Attribution](#attribution)
  - [Contributers](#contributers)
  - [3rd Parties and Libraries](#3rd-parties-and-libraries)

# Disclaimer
The project is intended for educational purpose only.

# Setup
## 1) Clone the repository  
`git clone https://github.com/sebastian-heinz/ddo-server.git`

## 2) Install .Net 6.0 SDK or later  
https://dotnet.microsoft.com/download

## 3) Use your IDE of choice:

## 3.1) Visual Studio
### Notice:
Minimum version of "Visual Studio 2022" or later.

### Open Project:
Open the `DragonsDogmaOnline.sln`-file

## 3.2) VS Code
Download IDE: https://code.visualstudio.com/download  
C# Plugin: https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp  

### Open Project:
Open the Project Folder:  
`\Arrowgene.DragonsDogmaOnline`

## 3.3) IntelliJ Rider
https://www.jetbrains.com/rider/
### Notice:
Minimum version of "IntelliJ Rider 2021.3" or later.

### Open Project:  
Open the `DragonsDogmaOnline.sln`-file

## 4) Debug the Project
Run the `Ddon.Cli`-Project

# Server
With default configuration the server will listen on following ports:
```
52099 - http/download
52000 - tcp/gameserver
52100 - tcp/loginserver
```
ensure that no other local services listen on these ports.

# Client
Launch the client with the following args:
`"DDO.exe" "addr=localhost port=52100 token=00000000000000000000 DL=http://127.0.0.1:52099/win/ LVer=03.04.003.20181115.0 RVer=3040008"`

# Progress

## Login Server

- [x] Account
- [x] Character Creation


## Game Server

### Tutorial
- [x] Entry
### Party
- [x] Invitation
- [x] Kick
- [ ] Change Leader
### Pawn


# Guidelines
## Git 
### Workflow
The work on this project should happen via `feature-branches`
   
Feature branches (or sometimes called topic branches) are used to develop new features for the upcoming or a distant future release. 
When starting development of a feature, the target release in which this feature will be incorporated may well be unknown at that point. 
The essence of a feature branch is that it exists as long as the feature is in development, 
but will eventually be merged back into develop (to definitely add the new feature to the upcoming release) or discarded (in case of a disappointing experiment).
   
1) Create a new `feature/feature-name` or `fix/bug-fix-name` branch from master
2) Push all your changes to that branch
3) Create a Pull Request to merge that branch into `master`

## Best Practise
- Do not use Console.WriteLine etc, use the specially designed logger.
- Own the Code: extract solutions, discard libraries.
- Annotate functions with documentation comments (https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments).

## C# Coding Standards and Naming Conventions
| Object Name               | Notation    | Char Mask          | Underscores |
|:--------------------------|:------------|:-------------------|:------------|
| Class name                | PascalCase  | [A-z][0-9]         | No          |
| Constructor name          | PascalCase  | [A-z][0-9]         | No          |
| Method name               | PascalCase  | [A-z][0-9]         | No          |
| Method arguments          | camelCase   | [A-z][0-9]         | No          |
| Local variables           | camelCase   | [A-z][0-9]         | No          |
| Constants name            | PascalCase  | [A-z][0-9]         | No          |
| Field name                | _camelCase  | [A-z][0-9]         | Yes         |
| Properties name           | PascalCase  | [A-z][0-9]         | No          |
| Delegate name             | PascalCase  | [A-z]              | No          |
| Enum type name            | PascalCase  | [A-z]              | No          |

# Attribution
## Contributors / Making It Happening
Let me preface with that this work could not exist without the excellent work of various individuals
- Ando - Reverse Engineering & Tooling (Session Splitter, Camellia Key Cracker)
- David - Reverse Engineering (unpacking PC Executable, defeating Anti Debug and CRC checks)
- The White Dragon Temple
- Nothilvien [@sebastian-heinz](https://github.com/sebastian-heinz) - Reverse Engineering & Server Code
  
(if you have been forgotten please reach out)

## 3rd Parties and Libraries
- System.Data.SQLite (https://system.data.sqlite.org/)
- KaitaiStruct.Runtime.Csharp (https://kaitai.io/)
- Arrowgene.Networking (https://github.com/sebastian-heinz/Arrowgene.Networking)
- .NET Standard (https://github.com/dotnet/standard)
