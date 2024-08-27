using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataResetInfo
{
    public CDataResetInfo()
    {
        Unk0 = new CDataResetInfoUnk0();
        WalletPoints = new List<CDataWalletPoint>();
    }

    public CDataResetInfoUnk0 Unk0 { get; set; }
    public bool Unk1 { get; set; }
    public uint Unk2 { get; set; }
    public uint Unk3 { get; set; }
    public List<CDataWalletPoint> WalletPoints {  get; set; }

    public class Serializer : EntitySerializer<CDataResetInfo>
    {
        public override void Write(IBuffer buffer, CDataResetInfo obj)
        {
            WriteEntity(buffer, obj.Unk0);
            WriteBool(buffer, obj.Unk1);
            WriteUInt32(buffer, obj.Unk2);
            WriteUInt32(buffer, obj.Unk3);
            WriteEntityList(buffer, obj.WalletPoints);
        }

        public override CDataResetInfo Read(IBuffer buffer)
        {
            CDataResetInfo obj = new CDataResetInfo();
            obj.Unk0 = ReadEntity<CDataResetInfoUnk0>(buffer);
            obj.Unk1 = ReadBool(buffer);
            obj.Unk2 = ReadUInt32(buffer);
            obj.Unk3 = ReadUInt32(buffer);
            obj.WalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
            return obj;
        }
    }
}
