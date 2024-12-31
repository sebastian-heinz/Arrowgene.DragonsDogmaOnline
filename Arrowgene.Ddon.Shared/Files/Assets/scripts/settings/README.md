# Scriptable Settings

This file attemps to establish some guidelines when defining new settings which are parsed by the scripting engine.

## General Considerations

While implementing new GameLogicSettings, consider the possibility that the file can be hotlaoded after the server starts and code according to that assumption.
If the settings interacts with a feature which uses some caching mechanism, make sure to invalid all those caches when the file is reloaded.

If a certain category has many settings files, consider to create a subdirectory which contains all related or similar settings scripts.

## Guidelines

### Chance Values

All values which describe a chance should use a value between [0.0, 1.0] where 0.0 means a 0 percent chance and 1.0 means 100 percent chance.

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
