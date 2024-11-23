namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public enum RpcInternalCommand
    {
        NotifyPlayerList, // List<RpcCharacterData>

        SendTellMessage, // RpcChatData
        SendClanMessage, // RpcChatData

        AnnouncePacketAll, // RpcPacketData
        AnnouncePacketClan, // RpcPacketData
    }
}
