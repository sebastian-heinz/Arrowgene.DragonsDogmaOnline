using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragora
    {
        /// <summary>
        /// Determines the species, i.e. which mandragora is rendered and shown as "Current" in the journal
        /// </summary>
        public uint SpeciesIndex { get; set; }
        /// <summary>
        /// When changing appearance, the journal UI jumps to the correct category
        /// </summary>
        public MandragoraSpeciesCategory SpeciesCategory { get; set; }
        public uint MandragoraId { get; set; }
        public string MandragoraName { get; set; } = string.Empty;
        public uint Unk4 { get; set; }
        public long Unk5 { get; set; }
        public uint Unk6 { get; set; }
        public CDataMyMandragoraUnk1Unk7 Unk7 { get; set; } = new();

        public class Serializer : EntitySerializer<CDataMyMandragora>
        {
            public override void Write(IBuffer buffer, CDataMyMandragora obj)
            {
                WriteUInt32(buffer, obj.SpeciesIndex);
                WriteByte(buffer, (byte)obj.SpeciesCategory);
                WriteUInt32(buffer, obj.MandragoraId);
                WriteMtString(buffer, obj.MandragoraName);
                WriteUInt32(buffer, obj.Unk4);
                WriteInt64(buffer, obj.Unk5);
                WriteUInt32(buffer, obj.Unk6);
                WriteEntity<CDataMyMandragoraUnk1Unk7>(buffer, obj.Unk7);
            }

            public override CDataMyMandragora Read(IBuffer buffer)
            {
                CDataMyMandragora obj = new CDataMyMandragora();
                obj.SpeciesIndex = ReadUInt32(buffer);
                obj.SpeciesCategory = (MandragoraSpeciesCategory)ReadByte(buffer);
                obj.MandragoraId = ReadUInt32(buffer);
                obj.MandragoraName = ReadMtString(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadInt64(buffer);
                obj.Unk6 = ReadUInt32(buffer);
                obj.Unk7 = ReadEntity<CDataMyMandragoraUnk1Unk7>(buffer);
                return obj;
            }
        }
    }
}
