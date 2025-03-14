# Quests

The quest module defines quests using c# scripts. The quest module is meant to work in tandem with the JSON mechanism.
Quests which are very simple may still benefit from the simplicity of the json system. Quests which require more custom
behavior should consider using the C# script over JSON.

> [!NOTE]
> Scripting support for quests is still in development and may change frequently for some time.

## Directory layout

The following section describes the expected organization and naming of quest related script files.

### Main Story Quests

Main story quests should be organized under `scripts/quests/msq/<season_<major>_<minor>>/q<questid>.csx` where season should include
the major and minor values. The quest file should be named as `q<questid>.csx`.

```plaintext
quests
  msq
    season1_0
      q00000001.csx
      q00000002.csx
      ...
```

### World Quests

World quests should be organized under `scripts/quests/world_quests/<area>/<questid>`. Each variant of the quest should be labeled using `q<questid>_<variant>.csx`.

```plaintext
quests
  world
    hidell_plains
      q20005010
        q20005010_0.csx
    breya_coast
    ...
```

### Seasonal Event Quests

Season event quests should be organized under `scripts/quests/seasonal_events/<event>/<year>`. Each quest file required for the event should be labeled using `q<quest_id>.csx`.
Configuration for seasonal events can be found under `scripts/settings/SeasonalEvents.csx`.

```plaintext
settings
  SeasonalEvents.csx
quests
  seasonal_events
    christmas
        2016
        2017
        2018
          q60301055.csx
          q60301056.csx
          q60301057.csx
```
