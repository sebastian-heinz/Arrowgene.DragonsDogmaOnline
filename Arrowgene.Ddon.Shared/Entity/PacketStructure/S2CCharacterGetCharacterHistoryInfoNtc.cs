using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterGetCharacterHistoryInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_GET_CHARACTER_HISTORY_INFO_NTC;

        public uint CharacterId { get; set; }
        public List<CDataHistoryElement> HistoryElementList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CCharacterGetCharacterHistoryInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterGetCharacterHistoryInfoNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList(buffer, obj.HistoryElementList);
            }

            public override S2CCharacterGetCharacterHistoryInfoNtc Read(IBuffer buffer)
            {
                S2CCharacterGetCharacterHistoryInfoNtc obj = new S2CCharacterGetCharacterHistoryInfoNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.HistoryElementList = ReadEntityList<CDataHistoryElement>(buffer);
                return obj;
            }
        }
    }
}
