namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public enum RpcInternalCommand
    {
        NotifyPlayerJoin, // RpcCharacterData
        NotifyPlayerLeave, // RpcCharacterData

        SendTellMessage, // RpcChatData
        SendClanMessage, // RpcChatData

        AnnouncePacketAll, // RpcPacketData
        AnnouncePacketClan, // RpcPacketData
    }
}
