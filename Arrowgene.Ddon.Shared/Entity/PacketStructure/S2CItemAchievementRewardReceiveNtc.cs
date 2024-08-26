using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

/// <summary>
///     This NTC has to be sent after an achievement's receivable reward has been clicked on in the achievement UI and the
///     player should unlock a new craft recipe or potentially an arisen profile background.
/// </summary>
public class S2CItemAchievementRewardReceiveNtc : IPacketStructure
{
    public PacketId Id => PacketId.S2C_ITEM_ACHIEVEMENT_REWARD_RECEIVE_NTC;

    /// TODO: Seemingly has no effect
    public ushort Unk0;

    /// <summary>
    /// TODO: Seemingly has no effect
    /// Value must be 0, 1, 2 or 3, other values cause a disconnect
    /// </summary>
    public uint Unk1;

    /// <summary>
    /// TODO: Seemingly has no effect
    /// </summary>
    public byte Unk2;

    /// <summary>
    /// Any valid item ID works, but this is supposed to correspond to the expected reward item the user should receive.
    /// The item name is looked up by the client itself.
    /// </summary>
    public uint ItemId;

    public class Serializer : PacketEntitySerializer<S2CItemAchievementRewardReceiveNtc>
    {
        public override void Write(IBuffer buffer, S2CItemAchievementRewardReceiveNtc obj)
        {
            WriteUInt16(buffer, obj.Unk0);
            WriteUInt32(buffer, obj.Unk1);
            WriteByte(buffer, obj.Unk2);
            WriteUInt32(buffer, obj.ItemId);
        }

        public override S2CItemAchievementRewardReceiveNtc Read(IBuffer buffer)
        {
            S2CItemAchievementRewardReceiveNtc obj = new S2CItemAchievementRewardReceiveNtc();

            obj.Unk0 = ReadUInt16(buffer);
            obj.Unk1 = ReadUInt32(buffer);
            obj.Unk2 = ReadByte(buffer);
            obj.ItemId = ReadUInt32(buffer);

            return obj;
        }
    }
}
