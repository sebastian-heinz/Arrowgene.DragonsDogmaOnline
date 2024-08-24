using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataAchieveRewardCommon
{
    /// <summary>
    ///     Achievement type 1 == Backgrounds for arisen profile, type 2 == Crafting recipes for furniture
    /// </summary>
    public byte Type { get; set; }

    public uint RewardId { get; set; }

    public class Serializer : EntitySerializer<CDataAchieveRewardCommon>
    {
        public override void Write(IBuffer buffer, CDataAchieveRewardCommon obj)
        {
            WriteByte(buffer, obj.Type);
            WriteUInt32(buffer, obj.RewardId);
        }

        public override CDataAchieveRewardCommon Read(IBuffer buffer)
        {
            CDataAchieveRewardCommon obj = new CDataAchieveRewardCommon();
            obj.Type = ReadByte(buffer);
            obj.RewardId = ReadUInt32(buffer);
            return obj;
        }
    }
}
