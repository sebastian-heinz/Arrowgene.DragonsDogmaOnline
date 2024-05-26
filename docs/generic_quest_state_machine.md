# Generic Quest State Machine

The generic quest state machine is a first attempt and providing a way for simple quests such as world quests or personal quests without needing "too much" technical knowledge.

The generic implementation is intended to handle the following types of quests
- Find and kill a world boss
- Find and kill multiple groups of enemies
- Talk to an NPC to receive directions for some task. Complete this task and return.
  - Deliveries not implemented yet (but plan to do so)


## High Level overview of how a quest works

The quest system in DDon seems to be fairly complicated. A quest can define multiple processes and each process can have what is known as a sequence and block number. It is suspected that a process is akin to a thread in the quest state machine. Each process block has 2 lists of quest commands which are known as check commands and result commands.

The "result commands" are commands which actually do something (display the progress banner, teleport a player, start a custscene, etc.) The list of result commands are executed instantly when the block is fetched by the client.  The "check commands" are commands which wait for some condition to be satisfied. The quest process will only progress to the next block when all the check commands in the current block for the process are satisfied. The server can also send "work commands" which are async to the normal quest progress mechanism. The content of these commands looks similar to check commands.

Researching the packet capture for the main quest "Hope's Bitter End", the following patterns were observed.

- A quest can spawn multiple processes
- As each processes moves to the next state the block number is incrementd by 1
- When a process is completed, the block number is incremented by 1 and the sequence number is set to 1.

## Details about the generic quest implementation

To simplify the enabling of simple quests, the generic quest state machine executes everything within a single process. The state machine assumes the following steps

![](images/state-machine-general-steps.png)

Each quest has some starting condition, either being ordered from an NPC or being discovered by chance out in the field. A number of intermediate steps are required by the quest. These steps can be, killing enemies, running to marked enemies, discovering enemies, delivering items to an NPC or talking to an NPC. Finally when all intermediate steps are completed, the quest is marked as complete and rewards are distributed.

![](images/example-quest-steps.png)

## What works in the current implementation

- Currently only world quests are activated.
- A small amount of world quests around the starting areas in Lestania has been added.
  - [The Knights' Bitter Enemy (q20005010)](http://ddon.wikidot.com/wq:theknightsbitterenemy)
  - [Confrontation With Scouts (q20005001)](http://ddon.wikidot.com/wq:confrontationwithscouts)
  - [Ambush in the Well's Depths (q20005002)](http://ddon.wikidot.com/wq:ambushinthewellsdepths)
  - [Sky-Concealing Wings (q20005000)](http://ddon.wikidot.com/wq:skyconcealingwings)
  - [Dweller In The Darkness (q20005003)](http://ddon.wikidot.com/wq:dwellerinthedarkness)
  - [Dispatch A Clamor of Harpies (q20015005)](http://ddon.wikidot.com/wq:dispatchaclamorofharpies)
  - [Boats Buddy (q20010001)](http://ddon.wikidot.com/wq:boatsbuddy)
  - [Beach Bandits (q20010000)](http://ddon.wikidot.com/wq:beachbandits)
  - Knight and Arisen (q20000015)
  - Request For Medicine (q20000009)
  - The Woes of A Merchant (q20000001)
- Quest rewards can be claimed from the reward box after completing a quest.
  - ![](images/reward-box.png) 
- New quests can be defined by updating the file `world_quests.json` in `Arrowgene.Ddon.Shared/files/assets`.

> [!WARNING]
> The quest system generally doesn't work well in parties with multiple players. The world quests will reward all players in the party, but only the party leader will get quest banners as the quest progresses and completes.

> [!WARNING]
> The reward box is not currently saved into the database. Claim all rewards before exiting the game.

> [!WARNING]
> In the quests `Boat's Buddy` and `Beach Bandits`, the nodes which appear to be used for the quest don't behave properly. You may need to reset the instance by going to WDT to get the group to spawn with quest monsters.

> [!WARNING]
> In the quest `Confrontation With Scouts`, if you kill monsters in a group which is used in the quest before starting the quest, those monsters will not respawn until you reset the instance in WDT.

> [!NOTE]
> If a quest completes in a safe area, the party leader needs to exit the area and reenter to restart the quest.

> [!NOTE]
> The server currently treats every completion of the quest as the first time.

## Overview of the generic quest JSON format

The JSON for the state machine is split into 3 major parts.
- Generic details about the quest.
- Rewards that can be earned by completing the quest.
- Steps required for the quest to execute.

```json
{
    "type": string,
    "comment": string,
    "quest_id": int,
    "base_level": int,
    "minimum_item_rank": int,
    "discoverable": bool,
    "rewards": [],
    "blocks": []
}
```

A proper definition of the format should be defined but for now we will use an example to show how one can create a new quest.

## How to add new quests

There currently does not exist any tools to aid in adding new world quests. I suggest to install the following tools.

- [ripgrep](https://github.com/BurntSushi/ripgrep)
- [git](https://git-scm.com/download/win)

After installing `git`, clone the following repositories locally
- [DDON-translation](https://github.com/Sapphiratelaemara/DDON-translation)
- [DDon-tools](https://github.com/alborrajo/DDOn-Tools)

Build `DDon-tools` from source to get the latest build.

Use a wiki like [http://ddon.wikidot.com/](http://ddon.wikidot.com) to find the name of the quest in japanese.

### Example: Implementing "The Knights' Bitter Enemy"

Go to [http://ddon.wikidot.com/](http://ddon.wikidot.com/wq:theknightsbitterenemy) and copy the Japanese name of the quest `騎士団の仇敵`. Using `ripgrep`, search for this text in the DDON-translation repository you cloned and look for a match which looks like `DDON-Translation-TOML\ui\00_message\quest_info\q20005010_00.toml`

```plaintext
$ rg 騎士団の仇敵
// lot's of text prints out ...
DDON-Translation-CSV\ui\00_message\quest_info\q20005010_00.csv
2:q20005010_00_289,騎士団の仇敵,The Knights' Bitter Enemy
```

The first part of this file name `q20005010` (really `20005010`) this is the quest ID used by the client to display information about the quest. I would also suggest to lookup the quest in [youtube](https://www.youtube.com/watch?v=eXns7McFY1E) using the japanese name to see how the quest should work.

Using the wiki, we can learn the following details:

- The quest is located at (x 193, Y 294)
- The recommended level is 12
- The exp and currency rewards from the quest
    - 590 XP
    - 390 G
    - 70 R
    - 80 AP (not implemented)
- The selectable rewards of the quest (1 of the following three items)
  - 1x [Knight's Honour](http://ddon.wikidot.com/weapons:knightshonour)
  - 3x [Lumber Knife](http://ddon.wikidot.com/consumables:lumberknife)
  - 1x [Cathedral Fire](http://ddon.wikidot.com/weapons:cathedralfire)

Let's open up DDon-tools and find the information we require. After opening the tool, select the Lestania map and click on the enemies tab. Then start to move around the mouse until we are at the location (X:193, Y:294). We can tell the position in DDon-tools by looking in the upper right corner.

![](images/tutorial-image-1.png)

Once you have found the region, zoom in and we can see a bunch of nodes close to the area (red square boxes). This next part takes a bit of guess work. For this particular quest, the node with the cyclops in it is what we are interested in. Record the StageId values `(1, 0, 26, 0)` in the upper right corner after hovering over the node.

![](images/tutorial-image-2.png)

Next let's search for the Cyclops enemy ID. The enemies tab has a search input box you can use. Type in the name `cyclops` and record the hex number `0x015000` next to the name in the list.

![](images/tutorial-image-3.png)

Next select the item tab. Similar to enemies, we can search item ID's quickly.

![](images/tutorial-image-4.png)

After doing this for all three items, you should find

- Knight's Honor (95)
- Lumber Knife (58)
- Cathedral Fire (10047)

In this particular quest, this is all the information we need. Let's use it to construct the quest JSON object parsed by the server.

### Populating the common information

New quests will be added to the array under the `quests` key. Each quest has the following pattern.
```json
{
    "type": string,
    "comment": string,
    "quest_id": int,
    "base_level": int,
    "minimum_item_rank": int,
    "discoverable": bool,
    "rewards": [],
    "blocks": []
}
```

First fill in the common items using the values we collected from the wiki. Currently only `"World"` quest type will be used the server. The `discoverable` field controls if the quest shows up on the map before accepting it. In this quest we will set it to `false`.
```json
{
    "type": "World",
    "comment": "q20005010_TheKnightsBitterEnemy",
    "quest_id": 20005010,
    "base_level": 12,
    "minimum_item_rank": 0,
    "discoverable": false,
    "rewards": [],
    "blocks": []
}
```

### Adding Rewards

Next we can define the rewards. The rewards have a variable format depending on the `type` field.

```json
{
    "type": string
}
```

- If the type is `wallet`, then it will contain the fields `wallet_type` and `amount`.
- If the type is `exp`, then it will contain the field `amount`.
- If the type is `select` it will describe a reward where 1 item can be selected.
- If the type is `random` it will describe a reward where 1 random item will be selected.
- If the type is `fixed` it will always reward the fixed item.

Putting this all together, we will get a reward list which looks like
```json
{
    "type": "wallet",
    "wallet_type": "Gold",
    "amount": 390
},
{
    "type": "wallet",
    "wallet_type": "RiftPoints",
    "amount": 70
},
{
    "type": "exp",
    "amount": 590
},
{
    "type": "select",
    "loot_pool": [
        {
            "item_id": 95,
            "num": 1
        },
        {
            "item_id": 58,
            "num": 3
        },
        {
            "item_id": 10047,
            "num": 1
        }
    ]
}
```

This should be added into the `rewards` array in the parent object.

### Defining the quest blocks

Finally, we need to add members to the `block` array. This describes all the steps required to start and complete the quest. As mentioned earlier in the document, there are 3 different types of `block` elements. There are blocks which are generally used to start a quest. Either `DiscoverEnemy` or `NpcOrder`. Then some variable amount of intermediate steps `DiscoverEnemy`, `KillGroup`, `TalkToNpc`, `SeekOutMarkedEnemy` or `DeliverItems`. Finally there is the end block type which is implicitly added by the generic quest state machine and doesn't need to be provided in the quest json.

This quest is simple in that it only has 2 major steps.
- We discover the enemy
- We kill the enemy

We need to define a rule which tells the quest state machine where we want to discover the enemy. Recall the stage ID value `(1, 0, 26, 0)` we recorded from DDon-tools. We would create the first block as a `DiscoverEnemy` block. This block takes information about the location of the enemy and the node on the map they are associated with.

```json
{
    "type": "DiscoverEnemy",
    "stage_id": {
        "id": 1,
        "group_no": 26
    }
}
```

Next we define a rule that we want to spawn some enemies and wait for them to be dead. First thing we need to do is when we move from the first state `DiscoverEnemy` to the next state `KillGroup` is that we need to "announce" that we have accepted this quest. We do this by setting the `announce_type` key to `Accept`. We will use the values we collected about the monster from DDon-tools. Again, just like the previous rule, we need to tell the quest state machine where to find this group of enemies.

```json
{
    "type": "KillGroup",
    "stage_id": {
        "id": 1,
        "group_no": 26
    },
    "announce_type": "Accept",
    "group": []
}
```

In the group members, we then populate the information about the required monsters.
```json
{
    "type": "KillGroup",
    "stage_id": {
        "id": 1,
        "group_no": 26
    },
    "announce_type": "Accept",
    "group": [
         {
            "enemy_id": "0x015000",
            "level": 12,
            "exp": 1860,
            "is_boss": true
        }
    ]
}
```

It is possible to define the other attributes of the enemy seen in DDon-tools, but they are considered optional by the parser. If not provided, sane defaults will be selected.

> [!NOTE]
> The `enemy_id` is a hexstring that way we can see the information about the enemy easier (json doesn't allow hex literals). Presenting the number in hexadecimal allows easy visualization of certain attributes of the enemy encoded in it's ID.

Finally, your file should look like below. Save the file, reload the server and try out your quest.

```json
{
    "state_machine": "GenericStateMachine",
    "comment": [
        "Handles quests which are simple enough where a single process is enough",
        "to handle all conditions in the quest. Typcial kill x, y, z and other",
        "types of fetch quests should use this state machine."
    ],
    "quests": [
        {
            "type": "World",
            "comment": "q20005010_TheKnightsBitterEnemy",
            "quest_id": 20005010,
            "base_level": 12,
            "minimum_item_rank": 0,
            "discoverable": false,
            "rewards": [
                {
                    "type": "wallet",
                    "wallet_type": "Gold",
                    "amount": 390
                },
                {
                    "type": "wallet",
                    "wallet_type": "RiftPoints",
                    "amount": 70
                },
                {
                    "type": "exp",
                    "amount": 590
                },
                {
                    "type": "select",
                    "loot_pool": [
                        {
                            "item_id": 95,
                            "num": 1
                        },
                        {
                            "item_id": 58,
                            "num": 3
                        },
                        {
                            "item_id": 10047,
                            "num": 1
                        }
                    ]
                }
            ],
            "blocks": [
                {
                    "type": "DiscoverEnemy",
                    "stage_id": {
                        "id": 1,
                        "group_id": 26
                    }
                },
                {
                    "type": "KillGroup",
                    "stage_id": {
                        "id": 1,
                        "group_id": 26
                    },
                    "announce_type": "Accept",
                    "group": [
                        {
                            "enemy_id": "0x015000",
                            "level": 12,
                            "exp": 1860,
                            "is_boss": true
                        }
                    ]
                }
            ]
        }
    ]
}
```