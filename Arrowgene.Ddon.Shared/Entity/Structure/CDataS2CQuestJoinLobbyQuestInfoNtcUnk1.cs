using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CQuestJoinLobbyQuestInfoNtcUnk1
    {
        public CDataS2CQuestJoinLobbyQuestInfoNtcUnk1() {
            Unk0 = new CDataQuestOrderList();
        }
    
        public CDataQuestOrderList Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public byte Unk2 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CQuestJoinLobbyQuestInfoNtcUnk1>
        {
            public override void Write(IBuffer buffer, CDataS2CQuestJoinLobbyQuestInfoNtcUnk1 obj)
            {
                WriteEntity<CDataQuestOrderList>(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
            }
        
            public override CDataS2CQuestJoinLobbyQuestInfoNtcUnk1 Read(IBuffer buffer)
            {
                CDataS2CQuestJoinLobbyQuestInfoNtcUnk1 obj = new CDataS2CQuestJoinLobbyQuestInfoNtcUnk1();
                obj.Unk0 = ReadEntity<CDataQuestOrderList>(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}