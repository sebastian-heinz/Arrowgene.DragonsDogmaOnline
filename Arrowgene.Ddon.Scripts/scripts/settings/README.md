# Scriptable Settings

The GameServer has a set of expected settings which are defined in the server scripting module. It is possible to then provide additional script .csx files to override those default settings.

When the server first starts, it will generate a set of settings template files in `<scripts_root>/settings/templates`. These files will contain all variables which the server software can have configured, their description and default values. If the server admin wants to modify any settings, they can copy these files into the `<scripts_root>/settings` directory, maintaining the same file name as the template. The settings file does not need to contain every setting, only the setting which you desire to modify.

- ChatCommandSettings.csx
- GameServerSettings.csx
- PointModifierSettings.csx
- SeasonalEventSettings.csx

In the settings module, it is also possible to define additional variables, in existing settings scripts or new settings scripts. While the server won't directly access these variables, it is possible to read them from other script modules in the scripting directory.

## General Considerations

This section attemps to establish some guidelines when defining new settings which are parsed by the scripting engine.

While implementing new configurable game settings, consider the possibility that the file can be hotlaoded after the server starts and code according to that assumption.
If the settings interacts with a feature which uses some caching mechanism, make sure to invalid all those caches when the file is reloaded.

If a certain category has many settings files, consider to create a subdirectory which contains all related or similar settings scripts.

## Guidelines

### Chance Values

Values which describe a chance should use a value between [0.0, 1.0] where 0.0 means a 0 percent chance and 1.0 means 100 percent chance.

### Drop Rates

When configuring drop rates, unless a very specific value is required, use the constants defined in `DropRate.csx`.

- DropRate.VERY_RARE
- DropRate.RARE
- DropRate.UNCOMMON
- DropRate.COMMON
- DropRate.VERY_COMMON
- DropRate.ALWAYS

### Enable Variables

All settings which can enable a feature when set to `true`, should start with the prefix `Enable`.

### Disable Variables

All settings which can disable a feature when set to `true`, should start with the prefix `Disable`.
