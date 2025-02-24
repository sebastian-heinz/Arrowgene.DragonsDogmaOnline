using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobUpdateExpModeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_UPDATE_EXP_MODE_REQ;

        public List<CDataJobExpMode> UpdateExpModeList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SJobUpdateExpModeReq>
        {
            public override void Write(IBuffer buffer, C2SJobUpdateExpModeReq obj)
            {
                WriteEntityList<CDataJobExpMode>(buffer, obj.UpdateExpModeList);
            }
        
            public override C2SJobUpdateExpModeReq Read(IBuffer buffer)
            {
                C2SJobUpdateExpModeReq obj = new C2SJobUpdateExpModeReq();
                obj.UpdateExpModeList = ReadEntityList<CDataJobExpMode>(buffer);
                return obj;
            }
        }
    }
}
