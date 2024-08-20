using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentGetContentStatusFromOmReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_GET_CONTENT_STATUS_FROM_OM_REQ;

        public C2SBattleContentGetContentStatusFromOmReq()
        {
            StageLayoutId = new CDataStageLayoutId();
        }

        public CDataStageLayoutId StageLayoutId { get; set; }
        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentGetContentStatusFromOmReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentGetContentStatusFromOmReq obj)
            {
                WriteEntity(buffer, obj.StageLayoutId);
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SBattleContentGetContentStatusFromOmReq Read(IBuffer buffer)
            {
                C2SBattleContentGetContentStatusFromOmReq obj = new C2SBattleContentGetContentStatusFromOmReq();
                obj.StageLayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
