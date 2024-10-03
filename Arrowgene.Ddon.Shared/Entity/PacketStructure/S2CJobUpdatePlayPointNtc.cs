using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobUpdatePlayPointNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_PLAY_POINT_NTC;

        public S2CJobUpdatePlayPointNtc()
        {
            JobId = 0;
            UpdatePoint = 0;
            ExtraBonusPoint = 0;
            TotalPoint = 0;
            Type = 0;
        }

        public JobId JobId;
        public uint UpdatePoint;
        public uint ExtraBonusPoint;
        public uint TotalPoint;
        public byte Type;

        public class Serializer : PacketEntitySerializer<S2CJobUpdatePlayPointNtc>
        {
            public override void Write(IBuffer buffer, S2CJobUpdatePlayPointNtc obj)
            {
                WriteByte(buffer, (byte)obj.JobId);
                WriteUInt32(buffer, obj.UpdatePoint);
                WriteUInt32(buffer, obj.ExtraBonusPoint);
                WriteUInt32(buffer, obj.TotalPoint);
                WriteByte(buffer, obj.Type);
            }

            public override S2CJobUpdatePlayPointNtc Read(IBuffer buffer)
            {
                S2CJobUpdatePlayPointNtc obj = new S2CJobUpdatePlayPointNtc();
                obj.JobId = (JobId)ReadByte(buffer);
                obj.UpdatePoint = ReadUInt32(buffer);
                obj.ExtraBonusPoint = ReadUInt32(buffer);
                obj.TotalPoint = ReadUInt32(buffer);
                obj.Type = ReadByte(buffer);
                return obj;
            }
        }
    }
}
