using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

/// <summary>
///     This NTC has to be sent after an achievement's receivable reward has been clicked on in the achievement UI and the
///     player should unlock a new craft recipe.
/// </summary>
public class S2CItemAchievementRewardReceiveNtc : IPacketStructure
{
    public PacketId Id => PacketId.S2C_ITEM_ACHIEVEMENT_REWARD_RECEIVE_NTC;
    /// This could also be reward type..
    public ItemNoticeType UpdateType;
    public uint ItemNum;
    public StorageType StorageType;
    public uint ItemId;

    public class Serializer : PacketEntitySerializer<S2CItemAchievementRewardReceiveNtc>
    {
        public override void Write(IBuffer buffer, S2CItemAchievementRewardReceiveNtc obj)
        {
            WriteUInt16(buffer, (ushort)obj.UpdateType);
            WriteUInt32(buffer, obj.ItemNum);
            WriteByte(buffer, (byte)obj.StorageType);
            WriteUInt32(buffer, obj.ItemId);
        }

        public override S2CItemAchievementRewardReceiveNtc Read(IBuffer buffer)
        {
            S2CItemAchievementRewardReceiveNtc obj = new S2CItemAchievementRewardReceiveNtc();

            obj.UpdateType = (ItemNoticeType)ReadUInt16(buffer);
            obj.ItemNum = ReadUInt32(buffer);
            obj.StorageType = (StorageType)ReadByte(buffer);
            obj.ItemId = ReadUInt32(buffer);

            return obj;
        }
    }
}
