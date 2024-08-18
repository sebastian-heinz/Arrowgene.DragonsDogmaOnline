using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentContentFirstPhaseChangeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_CONTENT_FIRST_PHASE_CHANGE_REQ;

        public uint StageId {  get; set; } // Comes as 602 at first door (Bitterblack Maze Cove)
        public uint Unk1 {  get; set; }

        public C2SBattleContentContentFirstPhaseChangeReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SBattleContentContentFirstPhaseChangeReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentContentFirstPhaseChangeReq obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override C2SBattleContentContentFirstPhaseChangeReq Read(IBuffer buffer)
            {
                C2SBattleContentContentFirstPhaseChangeReq obj = new C2SBattleContentContentFirstPhaseChangeReq();
                obj.StageId = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
