using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaPointUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_AREA_POINT_UP_NTC;

        public QuestAreaId AreaId { get; set; }
        public uint AddPoint { get; set; }
        public uint AddPointByCharge { get; set; }
        public uint TotalPoint { get; set; }
        public uint WeekPoint { get; set; }
        public bool CanRankUp { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaPointUpNtc>
        {
            public override void Write(IBuffer buffer, S2CAreaPointUpNtc obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaId);
                WriteUInt32(buffer, obj.AddPoint);
                WriteUInt32(buffer, obj.AddPointByCharge);
                WriteUInt32(buffer, obj.TotalPoint);
                WriteUInt32(buffer, obj.WeekPoint);
                WriteBool(buffer, obj.CanRankUp);
            }

            public override S2CAreaPointUpNtc Read(IBuffer buffer)
            {
                S2CAreaPointUpNtc obj = new S2CAreaPointUpNtc();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);
                obj.AddPoint = ReadUInt32(buffer);
                obj.AddPointByCharge = ReadUInt32(buffer);
                obj.TotalPoint = ReadUInt32(buffer);
                obj.WeekPoint = ReadUInt32(buffer);
                obj.CanRankUp = ReadBool(buffer);
                return obj;
            }
        }
    }
}
