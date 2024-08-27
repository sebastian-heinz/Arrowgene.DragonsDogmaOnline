using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftProgress
    {
        public CDataCraftProgress()
        {
            CraftMainPawnInfo = new CDataCraftPawnInfo();
            CraftSupportPawnInfoList = new List<CDataCraftPawnInfo>();
            CraftMasterLegendPawnInfoList = new List<CDataCraftPawnInfo>();
        }

        // Must be unique from other progresses sent in the same list or client associates successful crafting to both products because client-side a single pawn can only craft a single item
        public CDataCraftPawnInfo CraftMainPawnInfo { get; set; }

        public List<CDataCraftPawnInfo> CraftSupportPawnInfoList { get; set; }

        /// Directly correlated with C2SCraftStartCraftReq CraftMasterLegendPawnInfoList
        public List<CDataCraftPawnInfo> CraftMasterLegendPawnInfoList { get; set; }
        public uint RecipeId { get; set; }
        public uint Exp { get; set; }
        public NpcActionType NpcActionId { get; set; }
        public uint ItemId { get; set; }

        /// <summary>
        /// ToppingId == RefineMaterialId
        /// We store plusvalue in DB instead and can skip storing topping ID
        /// </summary>
        public uint ToppingId { get; set; }

        public ushort AdditionalStatusId { get; set; }

        /// Remaining time in seconds
        public uint RemainTime { get; set; }

        /// Determines whether GG-based bonus is active
        public bool ExpBonus { get; set; }
        public uint CreateCount { get; set; }

        public class Serializer : EntitySerializer<CDataCraftProgress>
        {
            public override void Write(IBuffer buffer, CDataCraftProgress obj)
            {
                WriteEntity<CDataCraftPawnInfo>(buffer, obj.CraftMainPawnInfo);
                WriteEntityList<CDataCraftPawnInfo>(buffer, obj.CraftSupportPawnInfoList);
                WriteEntityList<CDataCraftPawnInfo>(buffer, obj.CraftMasterLegendPawnInfoList);
                WriteUInt32(buffer, obj.RecipeId);
                WriteUInt32(buffer, obj.Exp);
                WriteInt32(buffer, (int)obj.NpcActionId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ToppingId);
                WriteUInt16(buffer, obj.AdditionalStatusId);
                WriteUInt32(buffer, obj.RemainTime);
                WriteBool(buffer, obj.ExpBonus);
                WriteUInt32(buffer, obj.CreateCount);
            }

            public override CDataCraftProgress Read(IBuffer buffer)
            {
                CDataCraftProgress obj = new CDataCraftProgress();
                obj.CraftMainPawnInfo = ReadEntity<CDataCraftPawnInfo>(buffer);
                obj.CraftSupportPawnInfoList = ReadEntityList<CDataCraftPawnInfo>(buffer);
                obj.CraftMasterLegendPawnInfoList = ReadEntityList<CDataCraftPawnInfo>(buffer);
                obj.RecipeId = ReadUInt32(buffer);
                obj.Exp = ReadUInt32(buffer);
                obj.NpcActionId = (NpcActionType)ReadInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.ToppingId = ReadUInt32(buffer);
                obj.AdditionalStatusId = ReadUInt16(buffer);
                obj.RemainTime = ReadUInt32(buffer);
                obj.ExpBonus = ReadBool(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
