using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardBoxItem
{
    public CDataRewardBoxItem()
    {
        UID = "";
    }

    public ItemId ItemId { get; set; }
    public UInt16 Num {  get; set; }
    public string UID {  get; set; }
    public byte Type { get; set; }
    public bool IsCharge {  get; set; }
    public bool IsHelp { get; set; }

    public class Serializer : EntitySerializer<CDataRewardBoxItem>
    {
        public override void Write(IBuffer buffer, CDataRewardBoxItem obj)
        {
            WriteUInt32(buffer, (uint) obj.ItemId);
            WriteUInt16(buffer, obj.Num);
            WriteMtString(buffer, obj.UID);
            WriteByte(buffer, obj.Type);
            WriteBool(buffer, obj.IsCharge);
            WriteBool(buffer, obj.IsHelp);
        }

        public override CDataRewardBoxItem Read(IBuffer buffer)
        {
            CDataRewardBoxItem obj = new CDataRewardBoxItem();
            obj.ItemId = (ItemId) ReadUInt32(buffer);
            obj.Num = ReadUInt16(buffer);
            obj.UID = ReadMtString(buffer);
            obj.Type = ReadByte(buffer);
            obj.IsCharge = ReadBool(buffer);
            obj.IsHelp = ReadBool(buffer);
            return obj;
        }
    }
}

