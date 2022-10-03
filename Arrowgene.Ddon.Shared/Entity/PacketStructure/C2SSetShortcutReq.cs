using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSetShortcutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PROFILE_SET_SHORTCUT_LIST_REQ;

        public List<CDataShortCut> ShortCutList { get; set; }

        public C2SSetShortcutReq() {
            ShortCutList = new List<CDataShortCut>();
        }

        public class Serializer : PacketEntitySerializer<C2SSetShortcutReq> {
            public override void Write(IBuffer buffer, C2SSetShortcutReq obj)
            {
                WriteEntityList<CDataShortCut>(buffer, obj.ShortCutList);
            }

            public override C2SSetShortcutReq Read(IBuffer buffer)
            {
                C2SSetShortcutReq obj = new C2SSetShortcutReq();
                obj.ShortCutList = ReadEntityList<CDataShortCut>(buffer);
                return obj;
            }
        }

    }

}
