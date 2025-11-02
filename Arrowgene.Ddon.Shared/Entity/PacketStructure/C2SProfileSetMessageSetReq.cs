using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SProfileSetMessageSetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_MESSAGE_SET_REQ;

        public List<CDataCharacterMsgSet> MessageSetList { get; set; } = [];

        public class Serializer : PacketEntitySerializer<C2SProfileSetMessageSetReq>
        {
            public override void Write(IBuffer buffer, C2SProfileSetMessageSetReq obj)
            {
                WriteEntityList(buffer, obj.MessageSetList);
            }

            public override C2SProfileSetMessageSetReq Read(IBuffer buffer)
            {
                C2SProfileSetMessageSetReq obj = new C2SProfileSetMessageSetReq();

                obj.MessageSetList = ReadEntityList<CDataCharacterMsgSet>(buffer);

                return obj;
            }
        }
    }
}
