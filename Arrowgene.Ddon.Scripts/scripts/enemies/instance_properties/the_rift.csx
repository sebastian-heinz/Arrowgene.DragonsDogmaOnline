#load "libs.csx"

public class PropertyGenerator : IInstanceEnemyPropertyGenerator
{
    public override void ApplyChanges(GameClient client, StageLayoutId stageLayoutId, byte subGroupId, InstancedEnemy enemy)
    {
        if (stageLayoutId.Id != Stage.TheRift0.StageId &&
            stageLayoutId.Id != Stage.TheRift1.StageId)
        {
            return;
        }

        // Scale dungeon to have enemies match the party leader
        ushort playerLevel = (ushort) client.Character.ActiveCharacterJobData.Lv;
        if (client.Party.Leader != null)
        {
            playerLevel = (ushort) client.Party.Leader.Client.Character.ActiveCharacterJobData.Lv;
        }
        
        // Calculate a level value +/- 1 of the highest player in the party
        ushort minLv = (ushort)((playerLevel > 1) ? playerLevel - 1 : playerLevel);
        ushort maxLv = (ushort)((playerLevel < 120) ? playerLevel + 1 : playerLevel);

        enemy.Lv = (ushort) Random.Shared.Next(minLv, maxLv + 1);
        enemy.ExpScheme = EnemyExpScheme.Automatic;
    }
}

return new PropertyGenerator();
