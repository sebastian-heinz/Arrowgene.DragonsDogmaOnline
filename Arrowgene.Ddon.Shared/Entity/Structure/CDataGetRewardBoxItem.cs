using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataGetRewardBoxItem
{
    public string UID { get; set; } = string.Empty;

    public class Serializer : EntitySerializer<CDataGetRewardBoxItem>
    {
        public override void Write(IBuffer buffer, CDataGetRewardBoxItem obj)
        {
            WriteMtString(buffer, obj.UID);
        }

        public override CDataGetRewardBoxItem Read(IBuffer buffer)
        {
            CDataGetRewardBoxItem obj = new CDataGetRewardBoxItem();
            obj.UID = ReadMtString(buffer);
            return obj;
        }
    }
}
