using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_SEASON_62_28_16_NTC : IPacketStructure
    {
        public S2C_SEASON_62_28_16_NTC()
        {
            Message = string.Empty;
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.S2C_SEASON_62_28_16_NTC;

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public string Message { get; set; } // Makes blue text pop up
        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_SEASON_62_28_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_SEASON_62_28_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteMtString(buffer, obj.Message);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override S2C_SEASON_62_28_16_NTC Read(IBuffer buffer)
            {
                S2C_SEASON_62_28_16_NTC obj = new S2C_SEASON_62_28_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
