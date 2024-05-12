using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSetCommunicationShortcutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_COMMUNICATION_SHORTCUT_LIST_REQ;

        public List<CDataCommunicationShortCut> CommunicationShortCutList { get; set; }

        public C2SSetCommunicationShortcutReq() {
            CommunicationShortCutList = new List<CDataCommunicationShortCut>();
        }

        public class Serializer : PacketEntitySerializer<C2SSetCommunicationShortcutReq> {
            public override void Write(IBuffer buffer, C2SSetCommunicationShortcutReq obj)
            {
                WriteEntityList<CDataCommunicationShortCut>(buffer, obj.CommunicationShortCutList);
            }

            public override C2SSetCommunicationShortcutReq Read(IBuffer buffer)
            {
                C2SSetCommunicationShortcutReq obj = new C2SSetCommunicationShortcutReq();
                obj.CommunicationShortCutList = ReadEntityList<CDataCommunicationShortCut>(buffer);
                return obj;
            }
        }

    }

}
