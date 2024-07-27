using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusGetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAMP_BONUS_GET_LIST_RES;

        public S2CStampBonusGetListRes()
        {
            Unk0 = 1;
            Unk1 = 1;
            Unk2 = 77;
            
            StampBonusDaily = new List<CDataStampBonusDaily> { };

            Unk3 = 252;

            StampBonusTotal = new List<CDataStampBonusTotal> { };

            Unk4 = 50;

            Unk5 = 0;
        }

        public ushort TotalStampNum { get; set; }
        public ushort Unk0 {  get; set; } // 1
        public uint Unk1 { get; set; } // 1
        public uint Unk2 { get; set; } // 77
        public List<CDataStampBonusDaily> StampBonusDaily { get; set; }
        public ushort Unk3 { get; set; } // 252
        public List<CDataStampBonusTotal> StampBonusTotal { get; set; }
        public ushort Unk4 { get; set; } // 50;
        public byte Unk5 { get; set; } // 0;

        public class Serializer : PacketEntitySerializer<S2CStampBonusGetListRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusGetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt16(buffer, obj.TotalStampNum);
                WriteUInt16(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteEntityList<CDataStampBonusDaily>(buffer, obj.StampBonusDaily);
                WriteUInt16(buffer, obj.Unk3);
                WriteEntityList<CDataStampBonusTotal>(buffer, obj.StampBonusTotal);
                WriteUInt16(buffer, obj.Unk4);
                WriteByte(buffer, obj.Unk5);
            }

            public override S2CStampBonusGetListRes Read(IBuffer buffer)
            {
                S2CStampBonusGetListRes obj = new S2CStampBonusGetListRes();
                ReadServerResponse(buffer, obj);
                obj.TotalStampNum = ReadUInt16(buffer);
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.StampBonusDaily = ReadEntityList<CDataStampBonusDaily>(buffer);
                obj.Unk3 = ReadUInt16(buffer);
                obj.StampBonusTotal = ReadEntityList<CDataStampBonusTotal>(buffer);
                obj.Unk4 = ReadUInt16(buffer);
                obj.Unk5 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
