using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpCogGetIdReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_COG_GET_ID_REQ;

        public C2SGpCogGetIdReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGpCogGetIdReq>
        {
            public override void Write(IBuffer buffer, C2SGpCogGetIdReq obj)
            {
            }

            public override C2SGpCogGetIdReq Read(IBuffer buffer)
            {
                C2SGpCogGetIdReq obj = new C2SGpCogGetIdReq();

                return obj;
            }
        }
    }
}
