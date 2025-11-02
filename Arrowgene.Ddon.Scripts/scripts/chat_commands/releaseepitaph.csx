using System.Collections.Generic;

public class ChatCommand : IChatCommand
{
    public override AccountStateType AccountState => AccountStateType.GameMaster;
    public override string CommandName => "releaseepitaph";
    public override string HelpText => "usage: `/releaseepitaph` - Release all Epitaph conditions?";

    public override void Execute(DdonGameServer server, string[] command, GameClient client, ChatMessage message, List<ChatResponse> responses)
    {
        LibDdon.EpitaphRoadMgr.HandleStatueUnlock(client, Stage.HeroicSpiritSleepingPathCave.AsStageLayoutId(38), 0);
        LibDdon.EpitaphRoadMgr.HandleStatueUnlock(client, Stage.HeroicSpiritSleepingPathCave.AsStageLayoutId(88), 0);
        LibDdon.EpitaphRoadMgr.HandleStatueUnlock(client, Stage.HeroicSpiritSleepingPathWaterway.AsStageLayoutId(33), 0);
        LibDdon.EpitaphRoadMgr.HandleStatueUnlock(client, Stage.HeroicSpiritSleepingPathWell.AsStageLayoutId(101), 0);
        LibDdon.EpitaphRoadMgr.HandleStatueUnlock(client, Stage.HeroicSpiritSleepingPathTomb.AsStageLayoutId(101), 0);
        LibDdon.EpitaphRoadMgr.HandleStatueUnlock(client, Stage.HeroicSpiritSleepingPathRuinsDeepestLevel.AsStageLayoutId(101), 0);
        responses.Add(ChatResponse.CommandError(client, "all epitaph points unlocked"));
    }
}

return new ChatCommand();
