namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public enum RpcInternalCommand
    {
        //CommandRoute
        Ping, // null

        NotifyPlayerList, // List<RpcCharacterData>
        NotifyClanQuestCompletion, //RpcQuestCompletionData

        KickInternal, // int

        EpitaphRoadWeeklyReset, // null
        AreaRankResetStart, //null
        AreaRankResetEnd, //null
        BoardQuestDailyRotation, //null
        
        UpdateCrafting, // RpcCraftingTimerData

        //InternalChatRoute
        SendTellMessage, // RpcChatData
        SendClanMessage, // RpcChatData
        SendShoutMessage, // RpcChatData

        //PacketRoute
        AnnouncePacketAll, // RpcPacketData
        AnnouncePacketClan, // RpcPacketData
    }
}
