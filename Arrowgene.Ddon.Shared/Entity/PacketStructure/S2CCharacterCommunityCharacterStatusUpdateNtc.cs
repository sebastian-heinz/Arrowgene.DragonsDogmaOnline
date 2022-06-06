using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCommunityCharacterStatusUpdateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_COMMUNITY_CHARACTER_STATUS_UPDATE_NTC;

        public S2CCharacterCommunityCharacterStatusUpdateNtc()
        {
            UpdateCharacterList = new List<CDataCharacterListElement>();
            UpdateMatchingProfileList = new List<CDataUpdateMatchingProfileInfo>();
        }

        public List<CDataCharacterListElement> UpdateCharacterList { get; set; }
        public List<CDataUpdateMatchingProfileInfo> UpdateMatchingProfileList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterCommunityCharacterStatusUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterCommunityCharacterStatusUpdateNtc obj)
            {
                WriteEntityList<CDataCharacterListElement>(buffer, obj.UpdateCharacterList);
                WriteEntityList<CDataUpdateMatchingProfileInfo>(buffer, obj.UpdateMatchingProfileList);
            }

            public override S2CCharacterCommunityCharacterStatusUpdateNtc Read(IBuffer buffer)
            {
                S2CCharacterCommunityCharacterStatusUpdateNtc obj = new S2CCharacterCommunityCharacterStatusUpdateNtc();
                obj.UpdateCharacterList = ReadEntityList<CDataCharacterListElement>(buffer);
                obj.UpdateMatchingProfileList = ReadEntityList<CDataUpdateMatchingProfileInfo>(buffer);
                return obj;
            }
        }
    }
}