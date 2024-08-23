using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentResetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_RESET_INFO_REQ;

        public C2SBattleContentResetInfoReq()
        {
        }

        public GameMode GameMode { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentResetInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentResetInfoReq obj)
            {
                WriteUInt32(buffer, (uint)obj.GameMode);
            }

            public override C2SBattleContentResetInfoReq Read(IBuffer buffer)
            {
                C2SBattleContentResetInfoReq obj = new C2SBattleContentResetInfoReq();
                obj.GameMode = (GameMode)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

