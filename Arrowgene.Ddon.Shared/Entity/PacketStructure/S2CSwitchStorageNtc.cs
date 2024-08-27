using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSwitchStorageNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_USER_LIST_JOIN_NTC;

        public S2CSwitchStorageNtc()
        {
            ChangeList = new List<CDataSwitchStorage>();
        }

        public bool IsStart { get; set; }
        public List<CDataSwitchStorage> ChangeList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSwitchStorageNtc>
        {
            public override void Write(IBuffer buffer, S2CSwitchStorageNtc obj)
            {
                WriteBool(buffer, obj.IsStart);
                WriteEntityList(buffer, obj.ChangeList);
            }

            public override S2CSwitchStorageNtc Read(IBuffer buffer)
            {
                S2CSwitchStorageNtc obj = new S2CSwitchStorageNtc();
                obj.IsStart = ReadBool(buffer);
                obj.ChangeList = ReadEntityList<CDataSwitchStorage>(buffer);
                return obj;
            }
        }
    }
}
