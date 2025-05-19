using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobMasterMarkTargetsNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_MASTER_MARK_TARGETS_NTC;

        public S2CJobMasterMarkTargetsNtc()
        {
            TargetList = new List<CDataJobMasterTargetData>();
        }

        public List<CDataJobMasterTargetData> TargetList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobMasterMarkTargetsNtc>
        {
            public override void Write(IBuffer buffer, S2CJobMasterMarkTargetsNtc obj)
            {
                WriteEntityList(buffer, obj.TargetList);
            }

            public override S2CJobMasterMarkTargetsNtc Read(IBuffer buffer)
            {
                S2CJobMasterMarkTargetsNtc obj = new S2CJobMasterMarkTargetsNtc();
                obj.TargetList = ReadEntityList<CDataJobMasterTargetData>(buffer);
                return obj;
            }
        }
    }
}
