using System.Collections.Generic;
using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1
    {
        public CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1() {
            Unk3 = new List<CDataCommonU32>();
            Unk5 = new List<CDataQuestOrderList>();
        }
    
        public uint Unk0 { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; }
        public bool Unk4 { get; set; }
        public List<CDataQuestOrderList> Unk5 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1>
        {
            public override void Write(IBuffer buffer, CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteInt32(buffer, obj.Unk1);
                WriteInt32(buffer, obj.Unk2);
                WriteEntityList(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.Unk5);
            }
        
            public override CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1 Read(IBuffer buffer)
            {
                CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1 obj = new CDataS2CQuestJoinLobbyQuestInfoNtcUnk0Unk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadInt32(buffer);
                obj.Unk2 = ReadInt32(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadEntityList<CDataQuestOrderList>(buffer);
                return obj;
            }
        }
    }
}