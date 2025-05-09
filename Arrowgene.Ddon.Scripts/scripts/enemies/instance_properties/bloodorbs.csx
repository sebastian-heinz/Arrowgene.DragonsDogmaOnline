#load "C:\Users\Paul\Git\Arrowgene.DragonsDogmaOnline\Arrowgene.Ddon.Cli\bin\Debug\net9.0\Files\Assets\scripts\libs.csx"

public class PropertyGenerator : IInstanceEnemyPropertyGenerator
{
    private uint BloodOrbEnemyId = 448;

    public override void ApplyChanges(GameClient client, StageLayoutId stageLayoutId, byte subGroupId, InstancedEnemy enemy)
    {
        if (StageManager.IsEpitaphRoadStageId(stageLayoutId))
        {
            enemy.BloodOrbs = LibDdon.GetEpitaphRoadManager().CalculateBloodOrbBonus(client.Party, enemy);
        }
        else if (LibDdon.GetSetting<bool>("GameServerSettings", "EnableRandomizedBoEnemies"))
        {
            if (enemy.QuestScheduleId != 0 ||
                client.GameMode == GameMode.BitterblackMaze ||
                enemy.BloodOrbs > 0 ||
                enemy.NamedEnemyParams.Id != NamedParam.DEFAULT_NAMED_PARAM.Id ||
                !client.Party.Leader.Client.Character.HasContentReleased(ContentsRelease.OrbEnemy))
            {
                return;
            }

            var chance = LibDdon.GetSetting<double>("GameServerSettings", "RandomizedBoEnemyChance");
            if (Random.Shared.NextDouble() <= chance)
            {
                enemy.NamedEnemyParams = LibDdon.Enemy.GetNamedParam(BloodOrbEnemyId);
                enemy.IsBloodOrbEnemy = true;
                enemy.Scale = (ushort) Random.Shared.Next(110, 141);
                enemy.Lv += (ushort) Math.Round((enemy.Scale - 110) * 0.25);
                enemy.ExpScheme = EnemyExpScheme.Automatic;
                enemy.BloodOrbs = CalculateBloodOrb(client.Party, enemy);
            }
        }
    }

    // References
    // https://www.youtube.com/watch?v=xwTszTF7VLM&t=524s
    // https://youtu.be/EJ_ybIhKJF0?t=33
    // https://youtu.be/O5YEu0pd49c?t=781

    // Level 10 Cyclops = 10BO
    // Level 17 undead = 1BO
    // Level 17 undead = 4BO
    // Level 95 Mudman = 96BO
    // Level 95 Eliminator = 144 BO
    // Level 78 High Pixue Biff = 77BO
    // Level 78 High Pixue Biff = 57BO
    // Level 95 Orc 144 BO
    private uint CalculateBloodOrb(PartyGroup party, InstancedEnemy enemy)
    {
        double avgPartyIR = 0;
        foreach (var member in party.Members)
        {
            var characterCommon = LibDdon.Character.GetCharacterCommon(member);
            avgPartyIR += LibDdon.Character.CalculateItemRank(member);
        }
        avgPartyIR /= party.MemberCount;

        var lvRatio = enemy.Lv / avgPartyIR;

        double variance;
        double baseBo = enemy.Lv;
        if (enemy.IsBossGauge)
        {
            baseBo *= 2.0;
            variance = Random.Shared.Next(0, enemy.Lv * 0.5);
        }
        else
        {
            baseBo *= 0.5;
            variance = Random.Shared.Next(0, enemy.Lv * 0.25);
        }

        int finalBo = (int)Math.Round((baseBo * lvRatio) + variance);

        return (uint)Math.Max(1, Math.Min(200, finalBo));
    }
}

return new PropertyGenerator();
