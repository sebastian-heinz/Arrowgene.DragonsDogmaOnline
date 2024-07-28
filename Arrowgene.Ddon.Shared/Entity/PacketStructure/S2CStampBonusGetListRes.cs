using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStampBonusGetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAMP_BONUS_GET_LIST_RES;

        public S2CStampBonusGetListRes()
        {
            Unk1 = 1;
            Unk2 = 1;

            Unk3 = 77;
            
            StampBonusDaily = new List<CDataStampBonusDaily> { };

            TotalStampNum = 252;

            StampBonusTotal = new List<CDataStampBonusTotal> { };

            Unk5 = 50;

            Unk6 = 0;
        }

        public uint Unk1 {  get; set; } // 1
        public uint Unk2 { get; set; } // 1
        public uint Unk3 { get; set; } // 77
        public List<CDataStampBonusDaily> StampBonusDaily { get; set; }
        public ushort TotalStampNum { get; set; } // 252
        public List<CDataStampBonusTotal> StampBonusTotal { get; set; }
        public ushort Unk5 { get; set; } // 50;
        public byte Unk6 { get; set; } // 0;

        public class Serializer : PacketEntitySerializer<S2CStampBonusGetListRes>
        {
            public override void Write(IBuffer buffer, S2CStampBonusGetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteEntityList<CDataStampBonusDaily>(buffer, obj.StampBonusDaily);
                WriteUInt16(buffer, obj.TotalStampNum);
                WriteEntityList<CDataStampBonusTotal>(buffer, obj.StampBonusTotal);
                WriteUInt16(buffer, obj.Unk5);
                WriteByte(buffer, obj.Unk6);
            }

            public override S2CStampBonusGetListRes Read(IBuffer buffer)
            {
                S2CStampBonusGetListRes obj = new S2CStampBonusGetListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.StampBonusDaily = ReadEntityList<CDataStampBonusDaily>(buffer);
                obj.TotalStampNum = ReadUInt16(buffer);
                obj.StampBonusTotal = ReadEntityList<CDataStampBonusTotal>(buffer);
                obj.Unk5 = ReadUInt16(buffer);
                obj.Unk6 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
