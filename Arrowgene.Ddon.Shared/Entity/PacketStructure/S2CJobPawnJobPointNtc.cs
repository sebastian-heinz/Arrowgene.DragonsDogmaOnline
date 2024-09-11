using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobPawnJobPointNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_PAWN_JOB_POINT_NTC;

        public S2CJobPawnJobPointNtc()
        {
        }

        public uint PawnId {  get; set; }
        public JobId Job { get; set; }
        public uint AddJobPoint { get; set; }
        public uint ExtraBonusJobPoint { get; set; }
        public uint TotalJobPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobPawnJobPointNtc>
        {
            public override void Write(IBuffer buffer, S2CJobPawnJobPointNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte)obj.Job);
                WriteUInt32(buffer, obj.AddJobPoint);
                WriteUInt32(buffer, obj.ExtraBonusJobPoint);
                WriteUInt32(buffer, obj.TotalJobPoint);
            }

            public override S2CJobPawnJobPointNtc Read(IBuffer buffer)
            {
                S2CJobPawnJobPointNtc obj = new S2CJobPawnJobPointNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId)ReadByte(buffer);
                obj.AddJobPoint = ReadUInt32(buffer);
                obj.ExtraBonusJobPoint = ReadUInt32(buffer);
                obj.TotalJobPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
