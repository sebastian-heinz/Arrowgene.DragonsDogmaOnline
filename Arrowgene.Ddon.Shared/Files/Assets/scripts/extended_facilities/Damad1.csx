public class NpcExtendedFacility : INpcExtendedFacility
{
    public NpcExtendedFacility()
    {
        NpcId = NpcId.Damad1;
    }

    public override void GetExtendedOptions(DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result)
    {
        var barrier = server.EpitaphRoadManager.GetBarrier(NpcId);
        if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(barrier.EpitaphId))
        {
            if (!server.EpitaphRoadManager.CheckUnlockConditions(client, barrier))
            {
                return;
            }
            result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.GiveSpirits });
        }
    }
}

return new NpcExtendedFacility();
