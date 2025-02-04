using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CNpcGetNpcExtendedFacilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_NPC_GET_NPC_EXTENDED_FACILITY_RES;

        public S2CNpcGetNpcExtendedFacilityRes()
        {
            ExtendedMenuItemList = new List<CDataNpcExtendedFacilityMenuItem>();
        }

        public NpcId NpcId { get; set; } // Controls Message which prints when the NPC is selected.
        public List<CDataNpcExtendedFacilityMenuItem> ExtendedMenuItemList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CNpcGetNpcExtendedFacilityRes>
        {
            public override void Write(IBuffer buffer, S2CNpcGetNpcExtendedFacilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, (uint) obj.NpcId);
                WriteEntityList(buffer, obj.ExtendedMenuItemList);
            }

            public override S2CNpcGetNpcExtendedFacilityRes Read(IBuffer buffer)
            {
                S2CNpcGetNpcExtendedFacilityRes obj = new S2CNpcGetNpcExtendedFacilityRes();
                ReadServerResponse(buffer, obj);
                obj.NpcId = (NpcId) ReadUInt32(buffer);
                obj.ExtendedMenuItemList = ReadEntityList<CDataNpcExtendedFacilityMenuItem>(buffer);
                return obj;
            }
        }
    }
}
