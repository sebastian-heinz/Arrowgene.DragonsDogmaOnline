using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Rpc
{
    public class RpcQuestCompletionData
    {
        public RpcQuestCompletionData() { }

        public uint CharacterId { get; set; }
        public Dictionary<uint, uint> QuestStatus {  get; set; }
    }
}
