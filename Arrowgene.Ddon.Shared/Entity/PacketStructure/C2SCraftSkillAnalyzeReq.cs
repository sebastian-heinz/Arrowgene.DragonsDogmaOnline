using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftSkillAnalyzeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_CRAFT_SKILL_ANALYZE_REQ;

        public C2SCraftSkillAnalyzeReq()
        {
            AssistPawnIds = new List<CDataCommonU32>();
        }

        public CraftType CraftType { get; set; }
        public uint RecipeId { get; set; }
        public uint ItemId { get; set; }
        public uint PawnId { get; set; }
        /// This will never contain Master Craft / Legend Pawn IDs, even though they might have an effect on the analysis.
        public List<CDataCommonU32> AssistPawnIds { get; set; }
        public uint CreateCount { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftSkillAnalyzeReq>
        {
            public override void Write(IBuffer buffer, C2SCraftSkillAnalyzeReq obj)
            {
                WriteByte(buffer, (byte)obj.CraftType);
                WriteUInt32(buffer, obj.RecipeId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList(buffer, obj.AssistPawnIds);
                WriteUInt32(buffer, obj.CreateCount);
            }

            public override C2SCraftSkillAnalyzeReq Read(IBuffer buffer)
            {
                C2SCraftSkillAnalyzeReq obj = new C2SCraftSkillAnalyzeReq();
                obj.CraftType = (CraftType)ReadByte(buffer);
                obj.RecipeId = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.AssistPawnIds = ReadEntityList<CDataCommonU32>(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
