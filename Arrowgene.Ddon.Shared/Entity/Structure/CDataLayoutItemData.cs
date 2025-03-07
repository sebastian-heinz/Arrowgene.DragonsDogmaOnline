using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLayoutItemData
    {
        public byte PosId { get; set; }
        /// <summary>
        /// Prevents gathering, changes the model to the "empty" version.
        /// </summary>
        public bool IsEmpty { get; set; }
        /// <summary>
        /// If true, no gathering tool is required and no animation is played.
        /// </summary>
        public bool IsGathered { get; set; }
        /// <summary>
        /// If true, the gathering point doesn't appear on the minimap. Also bookkeeping for S2C_GATHERING_ENEMY_APPEAR_NTC?
        /// </summary>
        public bool IsAppearEnemy { get; set; }
        /// <summary>
        /// Unknown function.
        /// </summary>
        public bool IsTreasurePoint { get; set; }


        public class Serializer : EntitySerializer<CDataLayoutItemData>
        {
            public override void Write(IBuffer buffer, CDataLayoutItemData obj)
            {
                WriteByte(buffer, obj.PosId);
                WriteBool(buffer, obj.IsEmpty);
                WriteBool(buffer, obj.IsGathered);
                WriteBool(buffer, obj.IsAppearEnemy);
                WriteBool(buffer, obj.IsTreasurePoint);
            }

            public override CDataLayoutItemData Read(IBuffer buffer)
            {
                CDataLayoutItemData obj = new CDataLayoutItemData();
                obj.PosId = ReadByte(buffer);
                obj.IsEmpty = ReadBool(buffer);
                obj.IsGathered = ReadBool(buffer);
                obj.IsAppearEnemy = ReadBool(buffer);
                obj.IsTreasurePoint = ReadBool(buffer);
                return obj;
            }
        }
    }
}
