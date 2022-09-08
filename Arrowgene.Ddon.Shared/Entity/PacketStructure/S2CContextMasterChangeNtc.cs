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
            Unk0 = new List<CDataMasterInfo>();
        }

        public List<CDataMasterInfo> Unk0 { get; set; } // Probably a list of heartbeats for the different contexts?

        public class Serializer : PacketEntitySerializer<S2CContextMasterChangeNtc>
        {

            public override void Write(IBuffer buffer, S2CContextMasterChangeNtc obj)
            {
                WriteEntityList<CDataMasterInfo>(buffer, obj.Unk0);
            }

            public override S2CContextMasterChangeNtc Read(IBuffer buffer)
            {
                S2CContextMasterChangeNtc obj = new S2CContextMasterChangeNtc();
                obj.Unk0 = ReadEntityList<CDataMasterInfo>(buffer);
                return obj;
            }
        }
    }
}
