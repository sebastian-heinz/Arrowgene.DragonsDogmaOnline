#load "libs.csx"

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
                enemy.BloodOrbs = CalculateBloodOrb(enemy);
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
    private uint CalculateBloodOrb(InstancedEnemy enemy)
    {
        double baseBo = enemy.Lv;

        if (enemy.IsBossGauge)
        {
            baseBo *= 1.5;
        }

        double variance;
        if (enemy.Lv < 20)
        {
            variance = Random.Shared.NextDouble() * (0.2 * baseBo - (-0.8 * baseBo)) + (-0.8 * baseBo); // -80% to +20%
        }
        else
        {
            variance = Random.Shared.NextDouble() * (0.5 * baseBo - (-0.25 * baseBo)) + (-0.25 * baseBo); // -25% to +50%
        }

        int finalBo = (int)Math.Round(baseBo + variance);

        return (uint)Math.Max(1, Math.Min(200, finalBo));
    }
}

return new PropertyGenerator();
