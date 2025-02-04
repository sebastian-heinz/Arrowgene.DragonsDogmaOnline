using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSubstoryQuestOrderList
    {
        public CDataSubstoryQuestOrderList() {
            Unk1 = new List<CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1>();
        }
    
        public uint Unk0 { get; set; }
        public List<CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1> Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataSubstoryQuestOrderList>
        {
            public override void Write(IBuffer buffer, CDataSubstoryQuestOrderList obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList<CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1>(buffer, obj.Unk1);
            }
        
            public override CDataSubstoryQuestOrderList Read(IBuffer buffer)
            {
                CDataSubstoryQuestOrderList obj = new CDataSubstoryQuestOrderList();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadEntityList<CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1>(buffer);
                return obj;
            }
        }
    }
}