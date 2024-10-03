using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class BoardCategory
    {
        public BoardCategory()
        {
            CategoryName = string.Empty;
        }

        public string CategoryName {  get; set; }
        public uint CategoryId { get; set; }
        public ushort PartyMin { get; set; }
        public ushort PartyMax { get; set; }
    }

    public class RecruitmentBoardCategoryAsset
    {
        public RecruitmentBoardCategoryAsset()
        {
            RecruitmentBoardCategories = new Dictionary<uint, BoardCategory>();
        }

        public Dictionary<uint, BoardCategory> RecruitmentBoardCategories { get; set; }
    }
}
