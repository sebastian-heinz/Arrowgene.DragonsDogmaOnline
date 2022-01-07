using System;
using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipItemInfo
    {
        uint u0;
        byte u1;
        byte u2;
        ushort u3;
        byte u4;
        byte u5;
        // length prefix
        List<CDataEquipElementParam> u6;
        // length prefix
        List<CDataEquipElementUnkType> u7;
        // length prefix
        List<CDataEquipElementUnkType2> u8;
    }

    public class CDataEquipItemInfoSerializer : EntitySerializer<CDataEquipItemInfo>
    {
        public override void Write(IBuffer buffer, CDataEquipItemInfo obj)
        {
            throw new NotImplementedException();
        }

        public override CDataEquipItemInfo Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
