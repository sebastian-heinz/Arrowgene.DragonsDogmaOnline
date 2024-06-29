using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPartyMemberInfoUpdateNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_PARTY_MEMBER_INFO_UPDATE_NTC;

        public S2CBattleContentPartyMemberInfoUpdateNtc()
        {
            Progress = new CDataBattleContentUnk0();
            Unk0 = new List<CDataBattleContentUnk2>();
        }

        public uint CharacterId { get; set; }
        public byte Job {  get; set; }
        public CDataBattleContentUnk0 Progress {  get; set; }
        public List<CDataBattleContentUnk2> Unk0 {  get; set; }
        public bool Status { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentPartyMemberInfoUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPartyMemberInfoUpdateNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.Job);
                WriteEntity(buffer, obj.Progress);
                WriteEntityList(buffer, obj.Unk0);
                WriteBool(buffer, obj.Status);
            }

            public override S2CBattleContentPartyMemberInfoUpdateNtc Read(IBuffer buffer)
            {
                S2CBattleContentPartyMemberInfoUpdateNtc obj = new S2CBattleContentPartyMemberInfoUpdateNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Job = ReadByte(buffer);
                obj.Progress = ReadEntity<CDataBattleContentUnk0>(buffer);
                obj.Unk0 = ReadEntityList<CDataBattleContentUnk2>(buffer);
                obj.Status = ReadBool(buffer);
                return obj;
            }
        }
    }
}

