using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftStartAttachElementRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CRAFT_START_ATTACH_ELEMENT_RES;

        public S2CCraftStartAttachElementRes()
        {
            EquipElementParamList = new List<CDataEquipElementParam>();
            CurrentEquip = new CDataCurrentEquipInfo();
        }

        public uint Gold { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public CDataCurrentEquipInfo CurrentEquip { get; set;}

        public class Serializer : PacketEntitySerializer<S2CCraftStartAttachElementRes>
        {
            public override void Write(IBuffer buffer, S2CCraftStartAttachElementRes obj)
            {
                WriteUInt32(buffer, obj.Gold);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.EquipElementParamList);
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquip);
            }

            public override S2CCraftStartAttachElementRes Read(IBuffer buffer)
            {
                S2CCraftStartAttachElementRes obj = new S2CCraftStartAttachElementRes();
                obj.Gold = ReadUInt32(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.CurrentEquip = ReadEntity<CDataCurrentEquipInfo>(buffer);
                return obj;
            }
        }

    }
}