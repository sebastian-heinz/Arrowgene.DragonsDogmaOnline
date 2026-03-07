using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentPhaseEntryBeginRecruitmentReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_REQ;

        /// <summary>
        /// Uses the same data structures as C2SBattleContentGetContentStatusFromOmReq
        /// </summary>
        public C2SBattleContentPhaseEntryBeginRecruitmentReq()
        {
            StageLayoutId = new CDataStageLayoutId();
            BattleContentStageList = [];
            UnknownString = string.Empty;
        }

        public CDataStageLayoutId StageLayoutId { get; set; }
        public uint Unk3 { get; set; }
        public List<CDataBattleContentStage> BattleContentStageList { get; set; }
        public uint UnknownFlag { get; set; }
        public string UnknownString { get; set; }
        public bool Unk6 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentPhaseEntryBeginRecruitmentReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentPhaseEntryBeginRecruitmentReq obj)
            {
                WriteEntity(buffer, obj.StageLayoutId);
                WriteUInt32(buffer, obj.Unk3);
                WriteEntityList(buffer, obj.BattleContentStageList);
                WriteUInt32(buffer, obj.UnknownFlag);
                WriteMtString(buffer, obj.UnknownString);
                WriteBool(buffer, obj.Unk6);
            }

            public override C2SBattleContentPhaseEntryBeginRecruitmentReq Read(IBuffer buffer)
            {
                C2SBattleContentPhaseEntryBeginRecruitmentReq obj = new()
                {
                    StageLayoutId = ReadEntity<CDataStageLayoutId>(buffer),
                    Unk3 = ReadUInt32(buffer),
                    BattleContentStageList = ReadEntityList<CDataBattleContentStage>(buffer),
                    UnknownFlag = ReadUInt32(buffer),
                    UnknownString = ReadMtString(buffer),
                    Unk6 = ReadBool(buffer)
                };

                return obj;
            }
        }
    }
}
