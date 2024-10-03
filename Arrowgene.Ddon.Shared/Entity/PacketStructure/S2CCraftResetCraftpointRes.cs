using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftResetCraftpointRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_RESET_CRAFTPOINT_RES;

        public uint PawnID { get; set; }
        public List<CDataPawnCraftSkill> CraftSkillList { get; set; }
        public uint CraftPoint { get; set; }

        public S2CCraftResetCraftpointRes()
        {
            CraftSkillList = new List<CDataPawnCraftSkill>();
        }

        public class Serializer : PacketEntitySerializer<S2CCraftResetCraftpointRes>
        {
            public override void Write(IBuffer buffer, S2CCraftResetCraftpointRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteUInt32(buffer, obj.PawnID);
                WriteEntityList(buffer, obj.CraftSkillList);
                WriteUInt32(buffer, obj.CraftPoint);
            }

            public override S2CCraftResetCraftpointRes Read(IBuffer buffer)
            {
                S2CCraftResetCraftpointRes obj = new S2CCraftResetCraftpointRes();

                ReadServerResponse(buffer, obj);

                obj.PawnID = ReadUInt32(buffer);
                obj.CraftSkillList = ReadEntityList<CDataPawnCraftSkill>(buffer);
                obj.CraftPoint = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
