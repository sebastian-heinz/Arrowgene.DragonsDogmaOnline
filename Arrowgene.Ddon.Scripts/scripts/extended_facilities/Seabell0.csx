public class NpcExtendedFacility : INpcExtendedFacility
{
    public NpcExtendedFacility()
    {
        NpcId = NpcId.Seabell0;
    }

    public override void GetExtendedOptions(DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result)
    {
        if (QuestManager.GetQuestsByType(QuestType.ExtremeMission)
            .Select(x => QuestManager.GetQuestByScheduleId(x))
            .Where(x => x.MissionParams.Group == 3) 
            .Where(x => x.IsActive(client))
            .Any()
        )
        {
            result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem()
            {
                FunctionClass = NpcFunction.ExtremeMissions,
                FunctionSelect = NpcFunction.ExtremeMissions
            });
        }
    }
}

return new NpcExtendedFacility();
