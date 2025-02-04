namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public enum RpcInternalCommand
    {
        //CommandRoute
        NotifyPlayerList, // List<RpcCharacterData>
        NotifyClanQuestCompletion, //RpcQuestCompletionData
        EpitaphRoadWeeklyReset, // null
        KickInternal, // int

        //InternalChatRoute
        SendTellMessage, // RpcChatData
        SendClanMessage, // RpcChatData

        //PacketRoute
        AnnouncePacketAll, // RpcPacketData
        AnnouncePacketClan, // RpcPacketData
    }
}
