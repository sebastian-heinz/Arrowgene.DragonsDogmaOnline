using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyRoomOption
    {
        /// <summary>
        /// List of acquired sheet music furniture items, layout ID should be 0 for all non-active sheets & 61 for the active one
        /// </summary>
        public List<CDataCommonU32> BgmAcquirementNoList { get; set; } = new List<CDataCommonU32>();

        /// <summary>
        /// The acive music sheet furniture item ID which has layout ID 61
        /// </summary>
        public ItemId BgmAcquirementNo { get; set; }
        /// <summary>
        /// The active light vision / planetarium furniture item ID which has layout ID 63
        /// </summary>
        public ItemId ActivePlanetariumNo { get; set; }


        public class Serializer : EntitySerializer<CDataMyRoomOption>
        {
            public override void Write(IBuffer buffer, CDataMyRoomOption obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.BgmAcquirementNoList);
                WriteUInt32(buffer, (uint)obj.BgmAcquirementNo);
                WriteUInt32(buffer, (uint)obj.ActivePlanetariumNo);
            }

            public override CDataMyRoomOption Read(IBuffer buffer)
            {
                CDataMyRoomOption obj = new CDataMyRoomOption();
                obj.BgmAcquirementNoList = ReadEntityList<CDataCommonU32>(buffer);
                obj.BgmAcquirementNo = (ItemId)ReadUInt32(buffer);
                obj.ActivePlanetariumNo = (ItemId)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
