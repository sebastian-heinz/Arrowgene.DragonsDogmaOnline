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

        //InternalChatRoute
        SendTellMessage, // RpcChatData
        SendClanMessage, // RpcChatData

        //PacketRoute
        AnnouncePacketAll, // RpcPacketData
        AnnouncePacketClan, // RpcPacketData
    }
}
