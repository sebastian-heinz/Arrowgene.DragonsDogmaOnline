using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataOrbCategoryStatus
    {
        public byte CategoryID;
        public byte ReleaseNum;
    }

    public class CDataOrbCategoryStatusSerializer : EntitySerializer<CDataOrbCategoryStatus>
    {
        public override void Write(IBuffer buffer, CDataOrbCategoryStatus obj)
        {
            WriteByte(buffer, obj.CategoryID);
            WriteByte(buffer, obj.ReleaseNum);
        }

        public override CDataOrbCategoryStatus Read(IBuffer buffer)
        {
            CDataOrbCategoryStatus obj = new CDataOrbCategoryStatus();
            obj.CategoryID = ReadByte(buffer);
            obj.ReleaseNum = ReadByte(buffer);
            return obj;
        }
    }
}
