# Building Epitaph Content

## Main configuration file

The treasure and gimmicks of all epitaph dungeons is are defined the file `EpitaphRoad.json`. The individual trials are defined in their own trial files located in the `epitaph` directory.

## Legacy vs New dungeons

The structure of an epitaph dungeon changes after the first 2 dungeons. You may see in the code these first two dungeons are referenced as legacy dungeons.

In the legacy dungeons, there are multiple side areas called "spaces", doors locked with mysterious powers and statues that need to be collected. The dungeon rewards are located in gold and bronze chests which can be claimed once a week. The main area is called the "main road" and has 3-4 free trials. The spaces are blocked by a wall which can be removed by clicking on it and spending the soul cost. Once unlocked, they stay unlocked forever. The trial in the space has both a free and paid option. When defeating the last trial on the main road, there is a door which becomes unlocked. It has a golden chest and a stone statue which ungates the season 3 BO/HO tree progress. To progress to the next teir, there is a NPC in each dungeon who stands in front of a wall. Talking to this NPC and giving them the required souls will unlock the next tier.

In the newer dungeons, there is no longer a main road or spaces. Instead everything takes place mostly on a few large maps. There are no longer small walls blocking progress to different areas of the map. Instead, some trials have a unlock cost. There is also a large wall which blocks progress to the next tier in the dungeon. To unlock the next tier, it can be purchased at the NPC who allows the player to enter the dungeon.

A new buff reward is present which allows the player to select a temporary buff after defeating a trial. This buff lasts until the dungeon is left. Depending on the type of buff, it is possible to upgrade it 4 times, by selecting the same buff type if present when receiving a buff reward. A final "big" trial was also introduced in palce of the golden chests. The big trial is indicated on the map by a larger headstone. Defeating this trial will give you a weekly reward similar to golden chests.

| Dungeon Name        | Type   | Comment
|---------------------|--------|-----------------------|
| Rathnite Foothills  | Legacy | Relased in Season 3.0 |
| Rathnite Foothills  | Legacy | Relased in Season 3.1 |
| Memory of Megadosys | New    | Relased in Season 3.2 |
| memory of Urteca    | New    | Relased in Season 3.3 |

Due to these differences, in the main `EpitaphRoad.json` file, there will be fields which account for all these differences. You only need to fill out the fields required for the dungeon. The rest can be left empty.

### Important OMs

| Type               | OM ID    | Comment
|:-------------------|:--------:|:-------
| Bare Walls         | om522922 | Walls which removed by spending souls
| NPC Wall           | om523102 | Wall which is cleared buy spending souls at an NPC. The `NpcGetExtendedFacilityHandler` needs to be updated to give the NPC correct menu options (`NpcFunction.GiveSpirits`).
| Stone Statue Space | om511320 | Red door which blocks the entrance to the Stone Statue
| Mysterious Doors   | om522554 | Doors with Green light that can be unlocked
| Mysterious Powers  | om522552 | Green light pillars spawned when touching door (gathering points)
| Brown Chest        | om513051 | Dungeon trash (souls)
| Bronze Chest       | om513053 | Weekly reward
| Gold Chest         | om513055 | Weekly reward

### Pattern for Mysterious doors

After searching for the Mysterious Door OM id (om522554), if you search for the group id +1, this seems to be the associated gathering points (om522552).

For example, the first door is at `p39` and the lights are distributed into `p40`. There is a single `p39` file but multiple `p40` files with the OM.
> [!NOTE]
> Unsure if this pattern holds true in all locations. Needs to be verified.

```plaintext
Mysterious Door Main Road - lot\st1142_04m00n\scr\st1142\etc\st1142_04m00n_p39.lot.json
	- find -name "*p40*" -print0 | xargs -0 rg "om522552"
Mysterious Door 1st Space - lot\st1142_24m00n\scr\st1142\etc\st1142_24m00n_p50.lot.json
	- find -name "*p51*" -print0 | xargs -0 rg "om522552"
Mysterious Door 2nd Space - lot\st1142_31m00n\scr\st1142\etc\st1142_31m00n_p60.lot.json
	- find -name "*p61*" -print0 | xargs -0 rg "om522552"
Mysterious Door 3rd Space - lot\st1142_49m00n\scr\st1142\etc\st1142_49m00n_p80.lot.json
	- find -name "*p81*" -print0 | xargs -0 rg "om522552"
Mysterious Door 4th Space - lot\st1142_41m00n\scr\st1142\etc\st1142_41m00n_p70.lot.json
	- find -name "*p71*" -print0 | xargs -0 rg "om522552"
```

## Trials

Trials are located in the `epitaph` directory. They have a format similar to quests, although much simpler. They provide a list of enemies, objectives and rewards. Trials typically have 1-2 options (although they can support up to 4). There is a free option called "Trial of the Heroes". There is a paid option called "Trial of the Souls" which requires souls to start. Free trials reward souls, HO and sometimes crafting items. Paid trials reward more HO than free trials and rare crests. Each trial has a set of objectives/conditions which need to be met in order to successfully complete them. Failing any of these will cause the party to fail the trial.

The files are named in `<path>_<section>_<name>_trial<n>.json`.

### Trial Example

File: `rathnite_foothills_section1_0_main_road_trial2.json`
```json
{
    "comment": "Heroic Spirit Sleeping Path: Rathnite Foothills - Section 1: Main Road Trial 2",
    "stage_id": {
        "id": 550,
        "group_id": 33
    },
    "subgroup_id": 1,
    "unlock_cost": [],
    "unlocks": [],
    "options": [
        {
            "trial_name": "Trial of the Heroes",
            "constraints": [
                {"type": "EliminateTheEnemy", "priority": "Primary"},
                {"type": "ItemNoteUsedMoreThanOnce", "priority": "Secondary"},
                {"type": "CannotDieMoreThanOnce", "priority": "Secondary"}
            ],
            "cost": [],
            "item_rewards": [
                {"item_id": 18656, "amount": 10},
                {"item_id": 18742, "amount": 10}
            ],
            "enemy_groups": [
                {
                    "stage_id": {
                        "id": 550,
                        "group_id": 14
                    },
                    "enemies": [
                        {
                            "comment" : "Gorechimera",
                            "enemy_id": "0x070820",
                            "level": 83,
                            "exp": 84000,
                            "named_enemy_params_id": 2287,
                            "is_boss": true
                        },
                        {
                            "comment" : "Skeleton Brute",
                            "enemy_id": "0x010307",
                            "level": 83,
                            "exp": 8400,
                            "named_enemy_params_id": 2286,
                            "repop_count": 50,
                            "repop_wait_second": 30,
                            "is_required": false
                        },
                        {
                            "comment" : "Skeleton Brute",
                            "enemy_id": "0x010307",
                            "level": 83,
                            "exp": 8400,
                            "named_enemy_params_id": 2286,
                            "repop_count": 50,
                            "repop_wait_second": 30,
                            "is_required": false
                        }
                    ]
                }
            ]
        }
    ]
}
```