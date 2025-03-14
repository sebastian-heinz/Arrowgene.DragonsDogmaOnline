using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CEjectionNtc : IPacketStructure
    {
        public PacketId Id => PacketId.L2C_EJECTION_NTC;

        public string Message { get; set; } = string.Empty;

        public class Serializer : PacketEntitySerializer<L2CEjectionNtc>
        {

            public override void Write(IBuffer buffer, L2CEjectionNtc obj)
            {
                WriteMtString(buffer, obj.Message);
            }

            public override L2CEjectionNtc Read(IBuffer buffer)
            {
                L2CEjectionNtc obj = new L2CEjectionNtc();
                obj.Message = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
