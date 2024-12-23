# Scriptable Settings

This file attemps to establish some guidelines when defining new settings which are parsed by the scripting engine.

## General Considerations

While implementing new GameLogicSettings, consider the possibility that the file can be hotlaoded after the server starts and code according to that assumption.
If the settings interacts with a feature which uses some caching mechanism, make sure to invalid all those caches when the file is reloaded.

If a certain category has many settings files, consider to create a subdirectory which contains all related or similar settings scripts.

## Naming Guidelines

#### Enable Variables

All settings which can enable a feature when set to `true`, should start with the prefix `Enable`.

### Disable Variables

All settings which can disable a feature when set to `true`, should start with the prefix `Disable`.
