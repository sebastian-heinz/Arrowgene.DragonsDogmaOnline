using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataResetInfo
{
    public CDataResetInfo()
    {
        Unk0 = new CDataResetInfoUnk0();
        WalletPoints = new List<CDataWalletPoint>();
    }

    public CDataResetInfoUnk0 Unk0 { get; set; }
    public bool TrackUses { get; set; }
    public uint MaxUses { get; set; }
    public uint CurrentUses { get; set; }
    public List<CDataWalletPoint> WalletPoints {  get; set; }

    public class Serializer : EntitySerializer<CDataResetInfo>
    {
        public override void Write(IBuffer buffer, CDataResetInfo obj)
        {
            WriteEntity(buffer, obj.Unk0);
            WriteBool(buffer, obj.TrackUses);
            WriteUInt32(buffer, obj.MaxUses);
            WriteUInt32(buffer, obj.CurrentUses);
            WriteEntityList(buffer, obj.WalletPoints);
        }

        public override CDataResetInfo Read(IBuffer buffer)
        {
            CDataResetInfo obj = new CDataResetInfo();
            obj.Unk0 = ReadEntity<CDataResetInfoUnk0>(buffer);
            obj.TrackUses = ReadBool(buffer);
            obj.MaxUses = ReadUInt32(buffer);
            obj.CurrentUses = ReadUInt32(buffer);
            obj.WalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
            return obj;
        }
    }
}
