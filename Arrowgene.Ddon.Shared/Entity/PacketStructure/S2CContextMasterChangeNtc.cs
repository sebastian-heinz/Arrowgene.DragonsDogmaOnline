using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextMasterChangeNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_MASTER_CHANGE_NTC;
        
        public S2CContextMasterChangeNtc()
        {
            Info = new List<CDataMasterInfo>();
        }

        public List<CDataMasterInfo> Info { get; set; } // Probably a list of heartbeats for the different contexts?

        public class Serializer : PacketEntitySerializer<S2CContextMasterChangeNtc>
        {

            public override void Write(IBuffer buffer, S2CContextMasterChangeNtc obj)
            {
                WriteEntityList<CDataMasterInfo>(buffer, obj.Info);
            }

            public override S2CContextMasterChangeNtc Read(IBuffer buffer)
            {
                S2CContextMasterChangeNtc obj = new S2CContextMasterChangeNtc();
                obj.Info = ReadEntityList<CDataMasterInfo>(buffer);
                return obj;
            }
        }
    }
}
