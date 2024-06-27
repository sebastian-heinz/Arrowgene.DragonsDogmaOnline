using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentInstantClearInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_INSTANT_CLEAR_INFO_REQ;

        public C2SBattleContentInstantClearInfoReq()
        {
        }

        public uint Unk0 { get; set; } // Game mode?

        public class Serializer : PacketEntitySerializer<C2SBattleContentInstantClearInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentInstantClearInfoReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SBattleContentInstantClearInfoReq Read(IBuffer buffer)
            {
                C2SBattleContentInstantClearInfoReq obj = new C2SBattleContentInstantClearInfoReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}

