using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPartyMemberInfoUpdateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_NTC;

        public S2CBattleContentPartyMemberInfoUpdateNtc()
        {
            BattleContentSituationData = new CDataBattleContentSituationData();
            BattleContentAvailableRewardsList = new List<CDataBattleContentAvailableRewards>();
        }

        public uint CharacterId { get; set; }
        public byte Pos {  get; set; }
        public CDataBattleContentSituationData BattleContentSituationData {  get; set; }
        public List<CDataBattleContentAvailableRewards> BattleContentAvailableRewardsList {  get; set; }
        public bool Status { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentPartyMemberInfoUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPartyMemberInfoUpdateNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.Pos);
                WriteEntity(buffer, obj.BattleContentSituationData);
                WriteEntityList(buffer, obj.BattleContentAvailableRewardsList);
                WriteBool(buffer, obj.Status);
            }

            public override S2CBattleContentPartyMemberInfoUpdateNtc Read(IBuffer buffer)
            {
                S2CBattleContentPartyMemberInfoUpdateNtc obj = new S2CBattleContentPartyMemberInfoUpdateNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Pos = ReadByte(buffer);
                obj.BattleContentSituationData = ReadEntity<CDataBattleContentSituationData>(buffer);
                obj.BattleContentAvailableRewardsList = ReadEntityList<CDataBattleContentAvailableRewards>(buffer);
                obj.Status = ReadBool(buffer);
                return obj;
            }
        }
    }
}

