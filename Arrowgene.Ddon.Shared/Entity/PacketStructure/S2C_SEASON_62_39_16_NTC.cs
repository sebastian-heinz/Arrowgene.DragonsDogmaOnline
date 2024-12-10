using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_SEASON_62_39_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SEASON_62_39_16_NTC;

        public S2C_SEASON_62_39_16_NTC()
        {
            BuffList = new List<CDataSeasonDungeonUnk0>();
        }

        public uint CharacterId { get; set; }
        public List<CDataSeasonDungeonUnk0> BuffList { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_SEASON_62_39_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_SEASON_62_39_16_NTC obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList(buffer, obj.BuffList);
            }

            public override S2C_SEASON_62_39_16_NTC Read(IBuffer buffer)
            {
                S2C_SEASON_62_39_16_NTC obj = new S2C_SEASON_62_39_16_NTC();
                obj.CharacterId = ReadUInt32(buffer);
                obj.BuffList = ReadEntityList<CDataSeasonDungeonUnk0>(buffer);
                return obj;
            }
        }
    }
}
