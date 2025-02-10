/**
 * @breif This mixin is used to calculate the exp amount for a given
 * enemy based on observations from realworld exp data. It should be
 * possible to call this script directly from the server code or from
 * other scripts.
 */

// Data-points
//  LV, EXP,        EnemyType, IsBoss, Player Level, Nameplate
//   1,   6,           Rabbit,  False
//   1,   8,               Ox,  False
//   1,  12,       Killer Bee,  False
//   1,  20,           Goblin,  False
//   1,  20,           Goblin,  False
//   1,  19,           Goblin,  False, 8
//   1,  24,           Undead,  False, , "Flocking Undead"
//   2,  12,       Killer Bee,  False
//   2,  23,   Goblin Fighter,  False
//   2,  23,           Goblin,  False
//   2,  26,           Undead,  False, , "Flocking Undead"
//   3,  26,             Wolf,  False
//   3,  24,             Wolf,  False, 11
//   3,  23,             Wolf,  False, 12
//   3,  32,    Rouge Fighter,  False
//   3,  29,    Rouge Fighter,  False
//   3,  32,     Rouge Healer,  False
//   3,  10,           Spider,  False
//   3,  9,            Spider,  False
//   3,  28,    Skeleton Mage,  False
//   3,  25,         Skeleton,  False
//   3,  22,         Skeleton,  False, 12
//   3,  28,           Undead,  False
//   3,  25,           Undead,  False, 12
//   3,  26,           Goblin,  False
//   3,  26,   Goblin Fighter,  False
//   3,  13,       Killer Bee,  False
//   4,  34,    Goblin Leader,  False
//   4,  27,    Forest Goblin,  False
//   5,  41,     Pawn Fighter,  False, , "Lost Pawn"
//   5,  38,     Pawn Fighter,  False, 12, "Lost Pawn",
//   5,  35,          Saurian,  False
//   5,  32,     Sling Goblin,  False
//   8,  50,   Redcap Slinger,  False, , "Vigilant Sling Redcap"
//   8,  50,   Redcap Fighter,  False, , "Vigilant Redcap Fighter"
//  11,  80,      Orc Soldier,  False

public class Mixin : IExpMixin
{
    public override uint GetExpValue(InstancedEnemy enemy)
    {
        // TODO: Implement algorithm to calculate exp amount
        return enemy.GetDroppedExperience();
    }
}

return new Mixin();
