using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CRecycleGetInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_RECYCLE_GET_INFO_RES;

        public S2CRecycleGetInfoRes()
        {
            ResetCostList = new List<CDataRecycleWalletCost>();
            Unk3List = new List<CDataCommonU32>();
        }

        public List<CDataRecycleWalletCost> ResetCostList { get; set; }
        public byte MaxAttempts { get; set; }
        public byte AttemptsTaken { get; set; }
        public ulong UnkTimestamp0 { get; set; }
        public ulong UnkTimestamp1 { get; set; }
        public List<CDataCommonU32> Unk3List { get; set; }

        public class Serializer : PacketEntitySerializer<S2CRecycleGetInfoRes>
        {
            public override void Write(IBuffer buffer, S2CRecycleGetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ResetCostList);
                WriteByte(buffer, obj.MaxAttempts);
                WriteByte(buffer, obj.AttemptsTaken);
                WriteUInt64(buffer, obj.UnkTimestamp0);
                WriteUInt64(buffer, obj.UnkTimestamp1);
                WriteEntityList(buffer, obj.Unk3List);
            }

            public override S2CRecycleGetInfoRes Read(IBuffer buffer)
            {
                S2CRecycleGetInfoRes obj = new S2CRecycleGetInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ResetCostList = ReadEntityList<CDataRecycleWalletCost>(buffer);
                obj.MaxAttempts = ReadByte(buffer);
                obj.AttemptsTaken = ReadByte(buffer);
                obj.UnkTimestamp0 = ReadUInt64(buffer);
                obj.UnkTimestamp1 = ReadUInt64(buffer);
                obj.Unk3List = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}

