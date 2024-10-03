using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeason62_26_16Ntc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SEASON_62_26_16_NTC;

        public S2CSeason62_26_16Ntc()
        {
            StageLayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId StageLayoutId { get; set; }
        public uint Unk0 { get; set; }
        public byte Unk1 {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeason62_26_16Ntc>
        {
            public override void Write(IBuffer buffer, S2CSeason62_26_16Ntc obj)
            {
                WriteEntity(buffer, obj.StageLayoutId);
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
            }

            public override S2CSeason62_26_16Ntc Read(IBuffer buffer)
            {
                S2CSeason62_26_16Ntc obj = new S2CSeason62_26_16Ntc();
                obj.StageLayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
