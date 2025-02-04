# Holiday Personal Quests

There appears to be a set of holiday/seasonal personal quests. Some which don't have any quest text, but spawn NPCs and others which actually have quest information.

Seasonal events are controlled by the settings file `<scriptroot>/settings/SeasonalEvents.csx`

## Halloween

- To enable the Halloween seasonal eveny, the field `EnableHalloweenEvent`.
- To select which event is active, the field `HalloweenEventYear`.
- The default period for the quest is "10/1" to "10/31" and can be modified by setting `HalloweenValidPeriod`.

### Halloween 2016 (not implemented)

```c#
uint HalloweenEventYear = 2016;
```

- [Light a Pumpkin Lantern? (2016) (パンプキンランタンが照らすのは？) ](https://h1g.jp/dd-on/?%E3%83%91%E3%83%B3%E3%83%97%E3%82%AD%E3%83%B3%E3%83%A9%E3%83%B3%E3%82%BF%E3%83%B3%E3%81%8C%E7%85%A7%E3%82%89%E3%81%99%E3%81%AE%E3%81%AF%EF%BC%9F)
    - Light a Pumpkin Lantern? (1) q60200015
    - Light a Pumpkin Lantern? (2) q60200016

### Halloween 2017

```c#
// Enables 60301000 and 60301001.
uint HalloweenEventYear = 2017;
```

- [The Darkness of Halloween (2017) (ハロウィンの闇)](https://h1g.jp/dd-on/?%E3%83%8F%E3%83%AD%E3%82%A6%E3%82%A3%E3%83%B3%E3%81%AE%E9%97%87)
    - q60301000

### Halloween 2018

```c#
// Enables q60301052, q60301053 and q60301054.
uint HalloweenEventYear = 2018;
```

- [Emergency! Not Enough Candy! (2018) (緊急！　お菓子が足りない！)](https://h1g.jp/dd-on/?%E3%83%9B%E3%83%A9%E3%83%BC%E3%83%8A%E3%82%A4%E3%83%88%E3%81%AA%E3%83%AC%E3%82%B9%E3%82%BF%E3%83%8B%E3%82%A2)
    - Emergency! Not Enough Candy! (1) (q60301052)
    - Emergency! Not Enough Candy! (2) (q60301053)

### Resources

- https://moreali523425.com/2018/10/02/post-5695/
- quest\q60301001\quest\60301001\q60301001_st0200.qst.json (mentions pumpkins)
- quest\q60301054\quest\60301054\q60301054_st0200.qst.json (also mentions pumpkins)

## Christmas 

- To enable the Christmas seasonal eveny, the field `EnableChristmasEvent`.
- To select which event is active, the field `ChristmasEventYear`.
- The default period for the quest is "12/1" to "12/31" and can be modified by setting `ChristmasValidPeriod`.

### Christmas 2016 (not implemented)

```c#
// Enables q60200017, q60200018, q60200019, q60200020, q60200021 and q60200022.
uint ChristmasEventYear = 2016;
```

- Path to Miracles (2016)
    - Path to Miracles I (q60200017)
    - Path to Miracles II (q60200018)
    - Path to Miracles III (q60200019)
- With Great Desire to Gift (2016)
    - With Great Desire to Gift: Upper (q60200020)
    - With Great Desire to Gift: Lower (q60200021)
- Christmas NPCs and Decorations 2016 (q60200022)

### Christmas 2017 (not implemented)

```c#
// Enables q60301002, q60301003, q60301004, q60301005 and q60301006.
uint ChristmasEventYear = 2017;
```

- Wish on a Shooting Star (2017)
    - Wish on a Shooting Star I (q60301002)
    - Wish on a Shooting Star II (q60301003)
    - Wish on a Shooting Star III (q60301004)
    - Wish on a Shooting Star IV (q60301005)
- Christmas NPCs and Decorations 2017 (q60301006)

### Christmas 2018

```c#
// Enables q60301055, q60301056 and q60301057.
uint ChristmasEventYear = 2018;
```

- Merry Christmas with Smiles (2018)
    - Merry Christmas with Smiles I (q60301055)
    - Merry Christmas with Smiles II (q60301056)
- Christmas NPCs and Decorations 2018 (q60301057)

## Valentines Day/White Day

- quest\q60340004\quest\60340004\q60340004_st0200.qst.json (Angelo and Shelly in)
- quest\q60301031\quest\60301031\q60301031_st0200.qst.json (Only one in my thoughts (for Women))

- quest\q60340006\quest\60340006\q60340006_st0200.qst.json (White Day Event)
- quest\q60301041\quest\60301041\q60301041_st0200.qst.json (A Gift for You, My Love (1))
- quest\q60200032\quest\60200032\q60200032_st0200.qst.json (For White Day Set)
- quest\q60301043\quest\60301043\q60301043_st0200.qst.json (A Gift for Yoy, My Love (2)) 