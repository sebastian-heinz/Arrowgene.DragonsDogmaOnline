using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpCogGetIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_COG_GET_ID_RES;

        public string CogId { get; set; }

        public S2CGpCogGetIdRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CGpCogGetIdRes>
        {
            public override void Write(IBuffer buffer, S2CGpCogGetIdRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteMtString(buffer, obj.CogId);
            }

            public override S2CGpCogGetIdRes Read(IBuffer buffer)
            {
                S2CGpCogGetIdRes obj = new S2CGpCogGetIdRes();

                ReadServerResponse(buffer, obj);

                // は忘れないように注意しましょう。
                obj.CogId = ReadMtString(buffer);

                return obj;
            }
        }
    }
}
