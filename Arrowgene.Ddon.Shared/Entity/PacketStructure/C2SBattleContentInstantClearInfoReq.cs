using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentInstantClearInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_INSTANT_CLEAR_INFO_REQ;

        public C2SBattleContentInstantClearInfoReq()
        {
        }

        public GameMode GameMode { get; set; } // Game mode?

        public class Serializer : PacketEntitySerializer<C2SBattleContentInstantClearInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentInstantClearInfoReq obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
            }

            public override C2SBattleContentInstantClearInfoReq Read(IBuffer buffer)
            {
                C2SBattleContentInstantClearInfoReq obj = new C2SBattleContentInstantClearInfoReq();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

