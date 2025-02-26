#load "libs.csx"

public class PropertyGenerator : IInstanceEnemyPropertyGenerator
{
    public override void ApplyChanges(GameClient client, StageLayoutId stageLayoutId, byte subGroupId, InstancedEnemy enemy)
    {
        if (StageManager.IsEpitaphRoadStageId(stageLayoutId))
        {
            enemy.BloodOrbs = LibDdon.GetEpitaphRoadManager().CalculateBloodOrbBonus(client.Party, enemy);
        }
    }
}

return new PropertyGenerator();
