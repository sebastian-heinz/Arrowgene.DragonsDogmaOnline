namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcChatData
    {
        public RpcChatData()
        {
            SourceData = new();
            TargetData = new();
            Message = string.Empty;
        }

        public RpcCharacterData SourceData { get; set; }
        public RpcCharacterData TargetData { get; set; }
        public uint HandleId { get; set; }
        public LobbyChatMsgType Type { get; set; }
        public byte MessageFlavor { get; set; }
        public uint PhrasesCategory { get; set; }
        public uint PhrasesIndex { get; set; }
        public string Message { get; set; }
        public bool Deliver { get; set; }
    }
}
