using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealUnk0
    {
        public CDataSoulOrdealUnk0()
        {
            Unk1List = new List<CDataSoulOrdealUnk1>();
        }

        public uint Unk0 { get; set; }
        public List<CDataSoulOrdealUnk1> Unk1List { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealUnk0>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealUnk0 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1List);
            }

            public override CDataSoulOrdealUnk0 Read(IBuffer buffer)
            {
                CDataSoulOrdealUnk0 obj = new CDataSoulOrdealUnk0();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1List = ReadEntityList<CDataSoulOrdealUnk1>(buffer);
                return obj;
            }
        }
    }
}
