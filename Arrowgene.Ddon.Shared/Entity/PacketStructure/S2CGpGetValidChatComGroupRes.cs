using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGetValidChatComGroupRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GET_VALID_CHAT_COM_GROUP_RES;

        public S2CGpGetValidChatComGroupRes()
        {
            ValidChatComGroups = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> ValidChatComGroups { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGetValidChatComGroupRes>
        {
            public override void Write(IBuffer buffer, S2CGpGetValidChatComGroupRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.ValidChatComGroups);
            }

            public override S2CGpGetValidChatComGroupRes Read(IBuffer buffer)
            {
                S2CGpGetValidChatComGroupRes packet = new S2CGpGetValidChatComGroupRes();
                ReadServerResponse(buffer, packet);
                packet.ValidChatComGroups = ReadEntityList<CDataCommonU32>(buffer);
                return packet;
            }
        }
    }
}