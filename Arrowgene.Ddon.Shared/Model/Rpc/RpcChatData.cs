namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcChatData
    {
        public RpcCharacterData SourceData { get; set; } = new();
        public RpcCharacterData TargetData { get; set; } = new();
        public uint HandleId { get; set; }
        public LobbyChatMsgType Type { get; set; }
        public byte MessageFlavor { get; set; }
        public uint PhrasesCategory { get; set; }
        public uint PhrasesIndex { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Deliver { get; set; }
    }
}
