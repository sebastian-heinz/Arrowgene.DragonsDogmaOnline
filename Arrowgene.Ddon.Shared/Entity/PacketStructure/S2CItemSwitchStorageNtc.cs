using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemSwitchStorageNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SWITCH_STORAGE_NTC;

        public S2CItemSwitchStorageNtc()
        {
            ChangeList = new List<CDataSwitchStorage>();
        }

        public int Unk0 {  get; set; }
        public bool IsStart {  get; set; }
        public List<CDataSwitchStorage> ChangeList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemSwitchStorageNtc>
        {
            public override void Write(IBuffer buffer, S2CItemSwitchStorageNtc obj)
            {
                WriteInt32(buffer, obj.Unk0);
                WriteBool(buffer, obj.IsStart);
                WriteEntityList(buffer, obj.ChangeList);
            }

            public override S2CItemSwitchStorageNtc Read(IBuffer buffer)
            {
                S2CItemSwitchStorageNtc obj = new S2CItemSwitchStorageNtc();
                obj.Unk0 = ReadInt32(buffer);
                obj.IsStart = ReadBool(buffer);
                obj.ChangeList = ReadEntityList<CDataSwitchStorage>(buffer);
                return obj;
            }
        }
    }
}
