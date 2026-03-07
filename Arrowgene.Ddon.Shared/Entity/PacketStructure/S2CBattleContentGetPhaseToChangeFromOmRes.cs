using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentGetPhaseToChangeFromOmRes : ServerResponse
    {
        public S2CBattleContentGetPhaseToChangeFromOmRes()
        {
            BattleContentStage = new CDataBattleContentStage();
        }

        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_RES;

        // public int Unk0 { get; set; }
        // public int Unk1 { get; set; }
        public CDataBattleContentStage BattleContentStage { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentGetPhaseToChangeFromOmRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentGetPhaseToChangeFromOmRes obj)
            {
                WriteServerResponse(buffer, obj);
                // WriteInt32(buffer, obj.Unk0);
                // WriteInt32(buffer, obj.Unk1);
                WriteEntity(buffer, obj.BattleContentStage);
            }

            public override S2CBattleContentGetPhaseToChangeFromOmRes Read(IBuffer buffer)
            {
                S2CBattleContentGetPhaseToChangeFromOmRes obj = new();
                ReadServerResponse(buffer, obj);
                // obj.Unk0 = ReadInt32(buffer);
                // obj.Unk1 = ReadInt32(buffer);
                obj.BattleContentStage = ReadEntity<CDataBattleContentStage>(buffer);
                return obj;
            }
        }
    }
}
