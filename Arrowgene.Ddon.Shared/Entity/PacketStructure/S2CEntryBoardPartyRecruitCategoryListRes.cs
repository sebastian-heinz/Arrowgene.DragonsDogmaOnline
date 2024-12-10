using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardPartyRecruitCategoryListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_RES;

        public S2CEntryBoardPartyRecruitCategoryListRes()
        {
            Unk1List = new List<CDataEntryRecruitCategoryData>();
        }

        public uint Unk0 { get; set; }
        public List<CDataEntryRecruitCategoryData> Unk1List { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardPartyRecruitCategoryListRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardPartyRecruitCategoryListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1List);
            }

            public override S2CEntryBoardPartyRecruitCategoryListRes Read(IBuffer buffer)
            {
                S2CEntryBoardPartyRecruitCategoryListRes obj = new S2CEntryBoardPartyRecruitCategoryListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1List = ReadEntityList<CDataEntryRecruitCategoryData>(buffer);
                return obj;
            }
        }
    }
}
