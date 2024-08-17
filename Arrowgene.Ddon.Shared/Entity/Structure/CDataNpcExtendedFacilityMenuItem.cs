using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataNpcExtendedFacilityMenuItem
    {

        public CDataNpcExtendedFacilityMenuItem()
        {
            CustomLabel = string.Empty;
        }

        public NpcFunction FunctionClass { get; set; }
        public NpcFunction FunctionSelect { get; set; } // FUNC_SELECT_NAME_
        public uint Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint ShopId { get; set; } // Shows up as ShopId in S2CShopGetGoodsListRes
        public uint Unk5 { get; set; }
        public bool Unk6 { get; set; }
        public string CustomLabel {  get; set; }
        public byte Unk7 { get; set; }

        public class Serializer : EntitySerializer<CDataNpcExtendedFacilityMenuItem>
        {
            public override void Write(IBuffer buffer, CDataNpcExtendedFacilityMenuItem obj)
            {
                WriteUInt32(buffer, (uint) obj.FunctionClass);
                WriteUInt32(buffer, (uint) obj.FunctionSelect);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.ShopId);
                WriteUInt32(buffer, obj.Unk5);
                WriteBool(buffer, obj.Unk6);
                WriteMtString(buffer, obj.CustomLabel);
                WriteByte(buffer, obj.Unk7);
            }

            public override CDataNpcExtendedFacilityMenuItem Read(IBuffer buffer)
            {
                CDataNpcExtendedFacilityMenuItem obj = new CDataNpcExtendedFacilityMenuItem();
                obj.FunctionClass = (NpcFunction) ReadUInt32(buffer);
                obj.FunctionSelect = (NpcFunction) ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.ShopId = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt32(buffer);
                obj.Unk6 = ReadBool(buffer);
                obj.CustomLabel = ReadMtString(buffer);
                obj.Unk7 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
