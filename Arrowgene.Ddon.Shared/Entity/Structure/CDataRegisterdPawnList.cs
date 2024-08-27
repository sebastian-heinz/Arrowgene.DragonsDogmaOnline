using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRegisterdPawnList
{
    public CDataRegisterdPawnList()
    {
        Name = string.Empty;
        PawnListData = new CDataPawnListData();
    }

    public uint PawnId { get; set; }
    public string Name { get; set; }
    public byte Sex { get; set; }
    public uint RentalCost {  get; set; }
    public ulong Updated {  get; set; }
    public CDataPawnListData PawnListData { get; set; }

    public class Serializer : EntitySerializer<CDataRegisterdPawnList>
    {
        public override void Write(IBuffer buffer, CDataRegisterdPawnList obj)
        {
            WriteUInt32(buffer, obj.PawnId);
            WriteMtString(buffer, obj.Name);
            WriteByte(buffer, obj.Sex);
            WriteUInt32(buffer, obj.RentalCost);
            WriteUInt64(buffer, obj.Updated);
            WriteEntity(buffer, obj.PawnListData);
        }

        public override CDataRegisterdPawnList Read(IBuffer buffer)
        {
            CDataRegisterdPawnList obj = new CDataRegisterdPawnList();
            obj.PawnId = ReadUInt32(buffer);
            obj.Name = ReadMtString(buffer);
            obj.Sex = ReadByte(buffer);
            obj.RentalCost = ReadUInt32(buffer);
            obj.Updated = ReadUInt64(buffer);
            obj.PawnListData = ReadEntity<CDataPawnListData>(buffer);
            return obj;
        }
    }
}
