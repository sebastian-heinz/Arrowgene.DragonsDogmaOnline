#nullable enable
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class CraftProgress : CharacterCommon
    {
        public uint CraftCharacterId  { get; set; }
        public uint CraftLeadPawnId { get; set; }
        public uint CraftSupportPawnId1 { get; set; }
        public uint CraftSupportPawnId2 { get; set; }
        public uint CraftSupportPawnId3 { get; set; }
        public uint RecipeId { get; set; }
        public uint Exp { get; set; }
        public NpcActionType NpcActionId { get; set; }
        public uint ItemId { get; set; }
        // Directly correlated with C2SCraftStartCraftReq Unk0
        public ushort Unk0 { get; set; }
        public uint RemainTime { get; set; }
        public bool ExpBonus { get; set; }
        public uint CreateCount { get; set; }
        
        public CraftProgress()
        {
        }
    }
}
