using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGetGpDetailRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GET_GP_DETAIL_RES;

        public S2CGpGetGpDetailRes()
        {
            GPList = new List<CDataGPDetail>();
        }

        public List<CDataGPDetail> GPList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGetGpDetailRes>
        {
            public override void Write(IBuffer buffer, S2CGpGetGpDetailRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataGPDetail>(buffer, obj.GPList);
            }

            public override S2CGpGetGpDetailRes Read(IBuffer buffer)
            {
                S2CGpGetGpDetailRes obj = new S2CGpGetGpDetailRes();
                ReadServerResponse(buffer, obj);
                obj.GPList = ReadEntityList<CDataGPDetail>(buffer);
                return obj;
            }
        }
    }
}
