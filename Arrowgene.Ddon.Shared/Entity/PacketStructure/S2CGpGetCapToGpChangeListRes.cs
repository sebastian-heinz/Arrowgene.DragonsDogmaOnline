using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGetCapToGpChangeListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GET_CAP_TO_GP_CHANGE_LIST_RES;

        public List<CDataCAPtoGPChangeElement> List { get; set; }

        public S2CGpGetCapToGpChangeListRes()
        {
            List = new List<CDataCAPtoGPChangeElement>();
        }

        public class Serializer : PacketEntitySerializer<S2CGpGetCapToGpChangeListRes>
        {
            public override void Write(IBuffer buffer, S2CGpGetCapToGpChangeListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList(buffer, obj.List);
            }

            public override S2CGpGetCapToGpChangeListRes Read(IBuffer buffer)
            {
                S2CGpGetCapToGpChangeListRes obj = new S2CGpGetCapToGpChangeListRes();

                ReadServerResponse(buffer, obj);

                obj.List = ReadEntityList<CDataCAPtoGPChangeElement>(buffer);

                return obj;
            }
        }
    }
}
