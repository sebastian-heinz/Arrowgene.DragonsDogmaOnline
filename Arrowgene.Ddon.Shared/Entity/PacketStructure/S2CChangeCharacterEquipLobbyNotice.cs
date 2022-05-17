using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CChangeCharacterEquipLobbyNotice : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_EQUIP_29_17_16_NTC;
        
        public S2CChangeCharacterEquipLobbyNotice()
        {
            CharacterId=0;
            Job=0;
            EquipItemList=new List<CDataEquipItemInfo>();
            VisualEquipItemList=new List<CDataEquipItemInfo>();
        }

        public uint CharacterId { get; set; }
        public JobId Job { get; set; }
        public List<CDataEquipItemInfo> EquipItemList { get; set; }
        public List<CDataEquipItemInfo> VisualEquipItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CChangeCharacterEquipLobbyNotice>
        {
            public override void Write(IBuffer buffer, S2CChangeCharacterEquipLobbyNotice obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, (byte) obj.Job);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemList);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.VisualEquipItemList);
            }

            public override S2CChangeCharacterEquipLobbyNotice Read(IBuffer buffer)
            {
                S2CChangeCharacterEquipLobbyNotice obj = new S2CChangeCharacterEquipLobbyNotice();
                obj.CharacterId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.EquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.VisualEquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                return obj;
            }
        }
    }
}
