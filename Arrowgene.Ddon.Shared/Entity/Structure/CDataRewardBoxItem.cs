using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardBoxItem
{
    public CDataRewardBoxItem()
    {
    }

    public UInt32 ItemId { get; set; }
    public UInt16 Num {  get; set; }
    public string UID {  get; set; }
    public byte Type { get; set; }
    public bool IsCharge {  get; set; }
    public bool IsHelp { get; set; }

    public class Serializer : EntitySerializer<CDataRewardBoxItem>
    {
        public override void Write(IBuffer buffer, CDataRewardBoxItem obj)
        {
            WriteUInt32(buffer, obj.ItemId);
            WriteUInt16(buffer, obj.Num);
            WriteMtString(buffer, obj.UID);
            WriteByte(buffer, obj.Type);
            WriteBool(buffer, obj.IsCharge);
            WriteBool(buffer, obj.IsHelp);
        }

        public override CDataRewardBoxItem Read(IBuffer buffer)
        {
            CDataRewardBoxItem obj = new CDataRewardBoxItem();
            obj.ItemId = ReadUInt32(buffer);
            obj.Num = ReadUInt16(buffer);
            obj.UID = ReadMtString(buffer);
            obj.Type = ReadByte(buffer);
            obj.IsCharge = ReadBool(buffer);
            obj.IsHelp = ReadBool(buffer);
            return obj;
        }
    }
}

