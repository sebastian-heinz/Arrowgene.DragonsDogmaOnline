using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipItemInfo
    {
        public CDataEquipItemInfo()
        {
            u0 = 0;
            u1 = 0;
            u2 = 0;
            u3 = 0;
            u4 = 0;
            u5 = 0;
            u6 = new List<CDataEquipElementParam>();
            u7 = new List<CDataEquipElementUnkType>();
            u8 = new List<CDataEquipElementUnkType2>();
        }

        public uint u0;
        public byte u1;
        public byte u2;
        public ushort u3;
        public byte u4;
        public byte u5;
        public List<CDataEquipElementParam> u6;
        public List<CDataEquipElementUnkType> u7;
        public List<CDataEquipElementUnkType2> u8;
    }

    public class CDataEquipItemInfoSerializer : EntitySerializer<CDataEquipItemInfo>
    {
        public override void Write(IBuffer buffer, CDataEquipItemInfo obj)
        {
            WriteUInt32(buffer, obj.u0);
            WriteByte(buffer, obj.u1);
            WriteByte(buffer, obj.u2);
            WriteUInt16(buffer, obj.u3);
            WriteByte(buffer, obj.u4);
            WriteByte(buffer, obj.u5);
            WriteEntityList(buffer, obj.u6);
            WriteEntityList(buffer, obj.u7);
            WriteEntityList(buffer, obj.u8);
        }

        public override CDataEquipItemInfo Read(IBuffer buffer)
        {
            CDataEquipItemInfo obj = new CDataEquipItemInfo();
            obj.u0 = ReadUInt32(buffer);
            obj.u1 = ReadByte(buffer);
            obj.u2 = ReadByte(buffer);
            obj.u3 = ReadUInt16(buffer);
            obj.u4 = ReadByte(buffer);
            obj.u5 = ReadByte(buffer);
            obj.u6 = ReadEntityList<CDataEquipElementParam>(buffer);
            obj.u7 = ReadEntityList<CDataEquipElementUnkType>(buffer);
            obj.u8 = ReadEntityList<CDataEquipElementUnkType2>(buffer);
            return obj;
        }
    }
}
