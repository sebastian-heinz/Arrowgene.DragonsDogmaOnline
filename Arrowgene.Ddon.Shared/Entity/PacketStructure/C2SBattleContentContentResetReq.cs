using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentContentResetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_CONTENT_RESET_REQ;

        public C2SBattleContentContentResetReq()
        {
        }

        public GameMode GameMode {  get; set; }
        public uint ResetIndex {  get; set; } // Value of Unk0 from CDataResetInfoUnk0

        public class Serializer : PacketEntitySerializer<C2SBattleContentContentResetReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentContentResetReq obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
                WriteUInt32(buffer, obj.ResetIndex);
            }

            public override C2SBattleContentContentResetReq Read(IBuffer buffer)
            {
                C2SBattleContentContentResetReq obj = new C2SBattleContentContentResetReq();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                obj.ResetIndex = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
