using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftSkillAnalyzeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_CRAFT_SKILL_ANALYZE_REQ;

        public C2SCraftSkillAnalyzeReq()
        {
            AssistPawnIds = new List<CDataCommonU32>();
        }

        public byte CraftType { get; set; }
        public UInt32 RecipeId { get; set; }
        public UInt32 ItemId { get; set; }
        public UInt32 PawnId { get; set; }
        public List<CDataCommonU32> AssistPawnIds { get; set; }
        public UInt32 CreateCount { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftSkillAnalyzeReq>
        {
            public override void Write(IBuffer buffer, C2SCraftSkillAnalyzeReq obj)
            {
                WriteByte(buffer, obj.CraftType);
                WriteUInt32(buffer, obj.RecipeId);
                WriteUInt32(buffer, obj.ItemId);
                WriteEntityList<CDataCommonU32>(buffer, obj.AssistPawnIds);
                WriteUInt32(buffer, obj.CreateCount);
            }

            public override C2SCraftSkillAnalyzeReq Read(IBuffer buffer)
            {
                C2SCraftSkillAnalyzeReq obj = new C2SCraftSkillAnalyzeReq();
                obj.CraftType = ReadByte(buffer);
                obj.RecipeId = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.AssistPawnIds = ReadEntityList<CDataCommonU32>(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}
