using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraSpecies
    {
        /// <summary>
        /// Normal: 1-48 (3.2: 1-37)
        /// Chilli: 101-131 (3.2: 101-126)
        /// Albino: 201-239 (3.2: 201-234)
        /// Charcoal: 301-322 (3.2: 301-317)
        /// Veggie: 401-420 (3.4)
        /// Armored: 501-549, 561-571, 591,592 (3.2: 501-592)
        /// Clothed: 601-613, 621-633, 641-653, 661-666 (3.2: 601-611; 621-631; 641-666) 
        /// Flowering: 701-720 (3.2: 701-715)
        /// Scroll: 751-772 (3.4)
        /// Barbarian: 801-810 (3.2: 801-810)
        /// Helmet: 851-865 (3.4)
        /// Special: 990-998 (3.2: 990-996)
        /// </summary>
        public uint Index { get; set; }

        public uint Unk1 { get; set; }
        public MandragoraRarity Rarity { get; set; }
        public byte Unk3 { get; set; }
        public bool Visible { get; set; }
        public bool Unk5 { get; set; }

        /// <summary>
        /// This could be any character on the server that managed to figure out the correct recipe
        /// </summary>
        public string FirstDiscovery { get; set; } = string.Empty;

        public long DiscoveredDate { get; set; }

        /// <summary>
        /// Whether the entry should show up with the "NEW" icon next to the name in the observation journal
        /// </summary>
        public bool New { get; set; }

        public class Serializer : EntitySerializer<CDataMyMandragoraSpecies>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraSpecies obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, (byte)obj.Rarity);
                WriteByte(buffer, obj.Unk3);
                WriteBool(buffer, obj.Visible);
                WriteBool(buffer, obj.Unk5);
                WriteMtString(buffer, obj.FirstDiscovery);
                WriteInt64(buffer, obj.DiscoveredDate);
                WriteBool(buffer, obj.New);
            }

            public override CDataMyMandragoraSpecies Read(IBuffer buffer)
            {
                CDataMyMandragoraSpecies obj = new CDataMyMandragoraSpecies();
                obj.Index = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Rarity = (MandragoraRarity)ReadByte(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.Visible = ReadBool(buffer);
                obj.Unk5 = ReadBool(buffer);
                obj.FirstDiscovery = ReadMtString(buffer);
                obj.DiscoveredDate = ReadInt64(buffer);
                obj.New = ReadBool(buffer);
                return obj;
            }
        }
    }
}
