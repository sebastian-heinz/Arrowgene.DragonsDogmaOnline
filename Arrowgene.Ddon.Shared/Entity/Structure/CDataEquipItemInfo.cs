using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipItemInfo
    {
        public uint u0;
        public byte u1;
        public byte u2;
        public ushort u3;
        public byte u4;

        public byte u5;

        // length prefix
        public List<CDataEquipElementParam> u6;

        // length prefix
        public List<CDataEquipElementUnkType> u7;

        // length prefix
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
