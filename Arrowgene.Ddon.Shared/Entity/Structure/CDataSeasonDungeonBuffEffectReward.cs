using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonBuffEffectReward
    {
        public CDataSeasonDungeonBuffEffectReward()
        {
            BuffName = string.Empty;
        }

        public uint BuffEffect { get; set; }
        public uint BuffId { get; set; }
        public string BuffName { get; set; }
        public uint Level { get; set; } // Seems like Level 4 is max

        public class Serializer : EntitySerializer<CDataSeasonDungeonBuffEffectReward>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonBuffEffectReward obj)
            {
                WriteUInt32(buffer, obj.BuffEffect);
                WriteUInt32(buffer, obj.BuffId);
                WriteMtString(buffer, obj.BuffName);
                WriteUInt32(buffer, obj.BuffId);
            }

            public override CDataSeasonDungeonBuffEffectReward Read(IBuffer buffer)
            {
                CDataSeasonDungeonBuffEffectReward obj = new CDataSeasonDungeonBuffEffectReward();
                obj.BuffEffect = ReadUInt32(buffer);
                obj.BuffId = ReadUInt32(buffer);
                obj.BuffName = ReadMtString(buffer);
                obj.Level = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


