using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSeasonDungeonBuffEffectParam
    {
        public CDataSeasonDungeonBuffEffectParam()
        {
        }

        public uint Level { get; set; }
        public CDataSeasonDungeonUnk2 Unk1 { get; set; } = new();

        public class Serializer : EntitySerializer<CDataSeasonDungeonBuffEffectParam>
        {
            public override void Write(IBuffer buffer, CDataSeasonDungeonBuffEffectParam obj)
            {
                WriteUInt32(buffer, obj.Level);
                WriteEntity(buffer, obj.Unk1);
            }

            public override CDataSeasonDungeonBuffEffectParam Read(IBuffer buffer)
            {
                CDataSeasonDungeonBuffEffectParam obj = new CDataSeasonDungeonBuffEffectParam();
                obj.Level = ReadUInt32(buffer);
                obj.Unk1 = ReadEntity<CDataSeasonDungeonUnk2>(buffer);
                return obj;
            }
        }
    }
}


