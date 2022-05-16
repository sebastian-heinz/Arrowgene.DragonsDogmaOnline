using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillChangeExSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_CHANGE_EX_SKILL_RES;

        public S2CSkillChangeExSkillRes()
        {
            SlotsToUpdate = new List<CDataCommonU8>();
        }

        // These are all guesses
        public byte Job { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }
        public uint Unk3 { get; set; }
        public List<CDataCommonU8> SlotsToUpdate { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillChangeExSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillChangeExSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.Job);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
                WriteUInt32(buffer, obj.Unk3);
                WriteEntityList<CDataCommonU8>(buffer, obj.SlotsToUpdate);
            }

            public override S2CSkillChangeExSkillRes Read(IBuffer buffer)
            {
                S2CSkillChangeExSkillRes obj = new S2CSkillChangeExSkillRes();
                ReadServerResponse(buffer, obj);
                obj.Job = ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.SlotsToUpdate = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }
    }
}