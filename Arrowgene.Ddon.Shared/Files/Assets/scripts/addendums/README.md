# Addendum Module

The addendum module is a way for a server administrator to provide fine grained edits to server script files while still consuming the updates of the original server files. In the server repository, only the addendum directory and the README file will be tracked. After all other modules are loaded, the addendum module will then any amendments required to the script files required by the server admin. The general intention of this module is to mainly override quest parameters although it may find other uses. Amendments are limited to the type of objects that `LibDdon` can access. If you require a more invasive modification, you should use the custom module functionality instead.

## Example overrding quest rewards

In the `<script_root>/addendums` directory create a new file called `q50300004.csx`. Suppose we wanted to overwrite the rewards of the quest such that:

- A value of 42 experience points are rewarded.
- A value of 1337 gold is rewarded.
- A single Healing Poition is reward.

```c#
public class Addendum : IAddendum
{
    public override void Amend()
    {
        var quest = QuestManager.GetQuestByQuestId((QuestId) 50300004);
        quest.ClearAllRewards();
        quest.AddPointReward(PointType.ExperiencePoints, 42);
        quest.AddWalletReward(WalletType.Gold, 1337);
        quest.AddItemReward(QuestFixedRewardItem.Create(ItemId.HealingPotion, 1));
    }
}

return new Addendum();
```