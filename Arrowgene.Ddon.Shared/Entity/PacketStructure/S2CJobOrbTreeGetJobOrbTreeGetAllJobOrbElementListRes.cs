using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes : ServerResponse
    {
        public S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes()
        {
            ElementList = new List<CDataJobOrbDevoteElement>();
            Unk0List = new List<CDataJobOrbDevoteElementUnk0>();
        }
        public override PacketId Id => PacketId.S2C_JOB_ORB_TREE_GET_ALL_JOB_ORB_ELEMENT_LIST_RES;

        public List<CDataJobOrbDevoteElement> ElementList { get; set; }
        public List<CDataJobOrbDevoteElementUnk0> Unk0List { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes>
        {
            public override void Write(IBuffer buffer, S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ElementList);
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes Read(IBuffer buffer)
            {
                S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes obj = new S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes();
                ReadServerResponse(buffer, obj);
                obj.ElementList = ReadEntityList<CDataJobOrbDevoteElement>(buffer);
                obj.Unk0List = ReadEntityList<CDataJobOrbDevoteElementUnk0>(buffer);
                return obj;
            }
        }
    }
}
