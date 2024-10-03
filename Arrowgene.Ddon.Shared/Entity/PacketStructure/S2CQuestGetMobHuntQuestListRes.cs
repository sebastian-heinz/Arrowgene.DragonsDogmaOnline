using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetMobHuntQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_MOB_HUNT_QUEST_LIST_RES;

        public S2CQuestGetMobHuntQuestListRes()
        {
            QuestList = new List<CDataQuestMobHuntQuestInfo>();
        }

        public uint Unk0 {  get; set; }
        public byte Unk1 {  get; set; }
        public byte Unk2 { get; set; }
        public List<CDataQuestMobHuntQuestInfo> QuestList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetMobHuntQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetMobHuntQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteEntityList<CDataQuestMobHuntQuestInfo>(buffer, obj.QuestList);
            }

            public override S2CQuestGetMobHuntQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetMobHuntQuestListRes obj = new S2CQuestGetMobHuntQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.QuestList = ReadEntityList<CDataQuestMobHuntQuestInfo>(buffer);
                return obj;
            }
        }
    }
}
