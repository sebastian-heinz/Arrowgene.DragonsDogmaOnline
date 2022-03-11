using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CUserListLeaveNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_USER_LIST_LEAVE_NTC;

        public S2CUserListLeaveNtc()
        {
            CharacterList = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> CharacterList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CUserListLeaveNtc>
        {
            public override void Write(IBuffer buffer, S2CUserListLeaveNtc obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.CharacterList);
            }

            public override S2CUserListLeaveNtc Read(IBuffer buffer)
            {
                S2CUserListLeaveNtc obj = new S2CUserListLeaveNtc();
                obj.CharacterList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}