using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextMasterThrowNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_MASTER_THROW_NTC;

        public S2CContextMasterThrowNtc()
        {
            Info = new List<CDataMasterInfo>();
        }

        public List<CDataMasterInfo> Info { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextMasterThrowNtc>
        {
            public override void Write(IBuffer buffer, S2CContextMasterThrowNtc obj)
            {
                WriteEntityList<CDataMasterInfo>(buffer, obj.Info);
            }

            public override S2CContextMasterThrowNtc Read(IBuffer buffer)
            {
                S2CContextMasterThrowNtc obj = new S2CContextMasterThrowNtc();
                obj.Info = ReadEntityList<CDataMasterInfo>(buffer);
                return obj;
            }
        }

    }
}