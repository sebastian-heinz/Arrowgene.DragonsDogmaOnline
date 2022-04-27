using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyListInfo
    {
        public CDataPartyListInfo() {
            ServerId = 0;
            PartyId = 0;
            MemberList = new List<CDataPartyMember>();
            Sequence = 0;
            ContentNumber = 0;
        }

        public uint ServerId { get; set; }
        public uint PartyId { get; set; }
        public List<CDataPartyMember> MemberList { get; set; }
        public uint Sequence { get; set; }
        public ulong ContentNumber { get; set; }

        public class Serializer : EntitySerializer<CDataPartyListInfo>
        {
            public override void Write(IBuffer buffer, CDataPartyListInfo obj)
            {
                WriteUInt32(buffer, obj.ServerId);
                WriteUInt32(buffer, obj.PartyId);
                WriteEntityList<CDataPartyMember>(buffer, obj.MemberList);
                WriteUInt32(buffer, obj.Sequence);
                WriteUInt64(buffer, obj.ContentNumber);
            }

            public override CDataPartyListInfo Read(IBuffer buffer)
            {
                CDataPartyListInfo obj = new CDataPartyListInfo();
                obj.ServerId = ReadUInt32(buffer);
                obj.PartyId = ReadUInt32(buffer);
                obj.MemberList = ReadEntityList<CDataPartyMember>(buffer);
                obj.Sequence = ReadUInt32(buffer);
                obj.ContentNumber = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}