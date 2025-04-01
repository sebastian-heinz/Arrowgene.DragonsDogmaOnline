using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CProfileSetMatchingProfileRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_SET_MATCHING_PROFILE_RES;

        public class Serializer : PacketEntitySerializer<S2CProfileSetMatchingProfileRes>
        {
            public override void Write(IBuffer buffer, S2CProfileSetMatchingProfileRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CProfileSetMatchingProfileRes Read(IBuffer buffer)
            {
                S2CProfileSetMatchingProfileRes obj = new S2CProfileSetMatchingProfileRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
