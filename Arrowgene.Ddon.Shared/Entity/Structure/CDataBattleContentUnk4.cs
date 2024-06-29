using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBattleContentUnk4
    {
        public CDataBattleContentUnk4()
        {
            UnknownString = "";
            WalletPoints = new List<CDataWalletPoint>();
            Unk3 = new List<CDataBattleContentUnk5>();
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public String UnknownString { get; set; }
        public List<CDataWalletPoint> WalletPoints {  get; set; }
        public List<CDataBattleContentUnk5> Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataBattleContentUnk4>
        {
            public override void Write(IBuffer buffer, CDataBattleContentUnk4 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteMtString(buffer, obj.UnknownString);
                WriteEntityList(buffer, obj.WalletPoints);
                WriteEntityList(buffer, obj.Unk3);
            }

            public override CDataBattleContentUnk4 Read(IBuffer buffer)
            {
                CDataBattleContentUnk4 obj = new CDataBattleContentUnk4();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.UnknownString = ReadMtString(buffer);
                obj.WalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk3 = ReadEntityList<CDataBattleContentUnk5>(buffer);
                return obj;
            }
        }
    }
}



