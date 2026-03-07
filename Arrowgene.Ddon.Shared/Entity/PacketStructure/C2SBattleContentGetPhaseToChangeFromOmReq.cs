using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentGetPhaseToChangeFromOmReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_GET_PHASE_TO_CHANGE_FROM_OM_REQ;
        
        /// <summary>
        /// Uses the same data structures as C2SBattleContentGetContentStatusFromOmReq
        /// </summary>
        public C2SBattleContentGetPhaseToChangeFromOmReq()
        {
            StageLayoutId = new CDataStageLayoutId();
        }
        
        public CDataStageLayoutId StageLayoutId { get; set; }
        public uint Unk0 { get; set; }
        
        public class Serializer : PacketEntitySerializer<C2SBattleContentGetPhaseToChangeFromOmReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentGetPhaseToChangeFromOmReq obj)
            {
                WriteEntity(buffer, obj.StageLayoutId);
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SBattleContentGetPhaseToChangeFromOmReq Read(IBuffer buffer)
            {
                C2SBattleContentGetPhaseToChangeFromOmReq obj = new()
                {
                    StageLayoutId = ReadEntity<CDataStageLayoutId>(buffer),
                    Unk0 = ReadUInt32(buffer)
                };
                return obj;
            }
        }
    }
}
