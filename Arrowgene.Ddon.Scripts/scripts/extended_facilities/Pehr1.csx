public class NpcExtendedFacility : INpcExtendedFacility
{
    public NpcExtendedFacility()
    {
        NpcId = NpcId.Pehr1;
    }

    public override void GetExtendedOptions(DdonGameServer server, GameClient client, S2CNpcGetNpcExtendedFacilityRes result)
    {
        if (client.Character.CompletedQuests.ContainsKey((QuestId) 60300020) || (client.QuestState.IsQuestActive(60300020) && client.QuestState.GetQuestState(60300020).Step > 2))
        {
            result.ExtendedMenuItemList.Add(new CDataNpcExtendedFacilityMenuItem() { FunctionClass = NpcFunction.WarMissions, FunctionSelect = NpcFunction.HeroicSpiritSleepingPath, Unk2 = 4452 });
        }
    }
}

return new NpcExtendedFacility();
