using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // Does exactly the same as MasterChangeNtc
    public class S2CContextMasterInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_MASTER_INFO_NTC;
        
        public S2CContextMasterInfoNtc()
        {
            Unk0 = new List<CDataMasterInfo>();
        }

        public List<CDataMasterInfo> Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextMasterInfoNtc>
        {

            public override void Write(IBuffer buffer, S2CContextMasterInfoNtc obj)
            {
                WriteEntityList<CDataMasterInfo>(buffer, obj.Unk0);
            }

            public override S2CContextMasterInfoNtc Read(IBuffer buffer)
            {
                S2CContextMasterInfoNtc obj = new S2CContextMasterInfoNtc();
                obj.Unk0 = ReadEntityList<CDataMasterInfo>(buffer);
                return obj;
            }
        }
    }
}