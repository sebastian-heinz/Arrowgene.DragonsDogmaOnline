using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_63_7_16_NTC : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_63_7_16_NTC;

        public S2C_63_7_16_NTC()
        {
            Unk0List = new List<CDataS2C_63_7_16>();
        }
    
        public List<CDataS2C_63_7_16> Unk0List {  get; set; }

        public class Serializer : PacketEntitySerializer<S2C_63_7_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_63_7_16_NTC obj)
            {
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override S2C_63_7_16_NTC Read(IBuffer buffer)
            {
                S2C_63_7_16_NTC obj = new S2C_63_7_16_NTC();
                obj.Unk0List = ReadEntityList<CDataS2C_63_7_16>(buffer);
                return obj;
            }
        }
    }

    public class CDataS2C_63_7_16
    {
        public CDataS2C_63_7_16()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId LayoutId { get; set; }
        public uint Unk0 { get; set; }

        public class Serializer : EntitySerializer<CDataS2C_63_7_16>
        {
            public override void Write(IBuffer buffer, CDataS2C_63_7_16 obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.Unk0);
            }

            public override CDataS2C_63_7_16 Read(IBuffer buffer)
            {
                CDataS2C_63_7_16 obj = new CDataS2C_63_7_16();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
