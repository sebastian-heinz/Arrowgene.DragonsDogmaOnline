using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetLightQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_LIGHT_QUEST_LIST_RES;

        public S2CQuestGetLightQuestListRes()
        {
            LightQuestList = new List<CDataLightQuestList>();
        }

        public uint BaseId { get; set; }
        public List<CDataLightQuestList> LightQuestList { get; set; }
        public byte NotCompleteQuestNum { get; set; }
        public byte GpCompletePriceGp { get; set; }
        public bool GpCompleteEnable { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetLightQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetLightQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.BaseId);
                WriteEntityList<CDataLightQuestList>(buffer, obj.LightQuestList);
                WriteByte(buffer, obj.NotCompleteQuestNum);
                WriteByte(buffer, obj.GpCompletePriceGp);
                WriteBool(buffer, obj.GpCompleteEnable);
            }

            public override S2CQuestGetLightQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetLightQuestListRes obj = new S2CQuestGetLightQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.BaseId = ReadUInt32(buffer);
                obj.LightQuestList = ReadEntityList<CDataLightQuestList>(buffer);
                obj.NotCompleteQuestNum = ReadByte(buffer);
                obj.GpCompletePriceGp = ReadByte(buffer);
                obj.GpCompleteEnable = ReadBool(buffer);
                return obj;
            }
        }
    }
}