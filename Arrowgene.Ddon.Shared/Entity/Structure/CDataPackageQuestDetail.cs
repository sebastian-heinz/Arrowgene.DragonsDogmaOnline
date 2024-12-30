using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPackageQuestDetail
    {
        public CDataPackageQuestDetail()
        {
            Unk3 = new();
            Unk6 = new();
        }

        public uint Unk0 { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; }
        public bool Unk4 { get; set; }
        public bool Unk5 { get; set; }
        public List<CDataQuestList> Unk6 { get; set; }

        public class Serializer : EntitySerializer<CDataPackageQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataPackageQuestDetail obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteInt32(buffer, obj.Unk1);
                WriteInt32(buffer, obj.Unk2);
                WriteEntityList(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteBool(buffer, obj.Unk5);
                WriteEntityList(buffer, obj.Unk6);
            }

            public override CDataPackageQuestDetail Read(IBuffer buffer)
            {
                CDataPackageQuestDetail obj = new CDataPackageQuestDetail();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadInt32(buffer);
                obj.Unk2 = ReadInt32(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadBool(buffer);
                obj.Unk6 = ReadEntityList<CDataQuestList>(buffer);
                return obj;
            }
        }
    }
}
