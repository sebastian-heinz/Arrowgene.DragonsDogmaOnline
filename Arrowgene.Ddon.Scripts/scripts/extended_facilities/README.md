# NPC Extended Facilities

It is possible to inject new NPC options by defining what the game calls `NpcExtendedFacilities`. The type of menu options what can be added are defined in the class [NpcFunction.cs](https://github.com/sebastian-heinz/Arrowgene.DragonsDogmaOnline/blob/develop/Arrowgene.Ddon.Shared/Model/NpcFunction.cs).

## Guidelines

- Name the file after the named constant in [NpcId.cs](https://github.com/sebastian-heinz/Arrowgene.DragonsDogmaOnline/blob/develop/Arrowgene.Ddon.Shared/Model/NpcId.cs)
- The abstract class `INpcExtendedFacility` is used as the interface module.
- When extending `INpcExtendedFacility` keep it simple and name it `NpcExtendedFacility`. The script engine will mangle the object name this avoiding any sort of class name conflicts.
- At the bottom of the file return a new `NpcExtendedFacility`.