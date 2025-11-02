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
        WorldQuestReset, // long (seed)

        StampReset, //null

        //InternalChatRoute
        SendTellMessage, // RpcChatData
        SendClanMessage, // RpcChatData
        SendShoutMessage, // RpcChatData
        SendMail, // RpcChatData
        SendGroupMessage, // RpcChatData
        JoinGroupChat, // RpcPacketData
        LeaveGroupChat, // RpcPacketData

        //PacketRoute
        AnnouncePacketAll, // RpcPacketData
        AnnouncePacketClan, // RpcPacketData
    }
}
