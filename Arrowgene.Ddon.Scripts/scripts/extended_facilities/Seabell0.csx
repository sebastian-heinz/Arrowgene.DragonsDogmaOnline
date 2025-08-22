public class NpcExtendedFacility : INpcExtendedFacility
{
    public NpcExtendedFacility()
    {
        NpcId = NpcId.Seabell0;
    }

    public override void GetExtendedOptions(DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result)
    {
        result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.ExtremeMissions, FunctionSelect = NpcFunction.ExtremeMissions });
    }
}

return new NpcExtendedFacility();
