#nullable enable
namespace Arrowgene.Ddon.Shared.Model
{
    public class CraftProgress
    {
        public uint CraftCharacterId { get; set; }

        /// <summary>
        /// * The total number of Pawns participating is limited to 4 for "Production" and "Enhancement".
        /// * You can choose one main pawn as the leader, and a main pawn and a support pawn as assistants.
        /// * The sum of the crafting abilities of all participating Pawns will be reflected in the crafting work.
        /// * "Crest mounting" is a single main pawn task.
        /// * Crafting Master Pawns are pawns that excel at certain crafting skills and can be rented from Rimstone.
        /// * Crafting work can only be participated as an "assistant". You can accompany them on adventures, but they don't excel at combat at all.
        /// </summary>
        public uint CraftLeadPawnId { get; set; }


        public uint CraftSupportPawnId1 { get; set; }
        public uint CraftSupportPawnId2 { get; set; }
        public uint CraftSupportPawnId3 { get; set; }
        public uint RecipeId { get; set; }

        /// <summary>
        /// Crafting experience can only be earned by the main pawn who served as the leader.
        /// </summary>
        public uint Exp { get; set; }

        public NpcActionType NpcActionId { get; set; }

        public uint ItemId { get; set; }

        public ushort AdditionalStatusId { get; set; }
        public uint RemainTime { get; set; }
        public bool ExpBonus { get; set; }
        public uint CreateCount { get; set; }
        public uint PlusValue { get; set; }

        /// <summary>
        /// The higher the Pawn's ability to create high-quality equipment, the higher the probability of great success.
        /// In the event of a great success, the number of crest slots increases. (The number of crests is limited depending on the part)
        /// Even if it is very successful, the base performance value of the weapon will not change. (Numbers as per the recipe)
        /// </summary>
        public bool GreatSuccess { get; set; }

        public uint BonusExp { get; set; }
        public uint AdditionalQuantity { get; set; }

        public CraftProgress()
        {
        }
    }
}
