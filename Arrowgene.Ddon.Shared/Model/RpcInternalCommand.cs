namespace Arrowgene.Ddon.Shared.Model
{
    public enum RpcInternalCommand
    {
        NotifyPlayerJoin, // RpcCharacterData
        NotifyPlayerLeave, // RpcCharacterData

        SendTellMessage, // RpcChatData
        SendClanMessage // RpcChatData
    }
}
