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
    public class S2CDispelGetLockSettingRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_DISPEL_GET_LOCK_SETTING_RES;

        public List<CDataDispelLockPageData> PageData { get; set; } = [];
        public uint MaxSeals { get; set; }
        public List<CDataDispelLockCostData> ResetCostData { get; set; } = [];
        public List<CDataDispelLockCostData> AddSealCostData { get; set; } = [];
        public List<CDataDispelLockSealData> SealData { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CDispelGetLockSettingRes>
        {
            public override void Write(IBuffer buffer, S2CDispelGetLockSettingRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.PageData);
                WriteUInt32(buffer, obj.MaxSeals);
                WriteEntityList(buffer, obj.ResetCostData);
                WriteEntityList(buffer, obj.AddSealCostData);
                WriteEntityList(buffer, obj.SealData);
            }

            public override S2CDispelGetLockSettingRes Read(IBuffer buffer)
            {
                S2CDispelGetLockSettingRes obj = new S2CDispelGetLockSettingRes();
                ReadServerResponse(buffer, obj);
                obj.PageData = ReadEntityList<CDataDispelLockPageData>(buffer);
                obj.MaxSeals = ReadUInt32(buffer);
                obj.ResetCostData = ReadEntityList<CDataDispelLockCostData>(buffer);
                obj.AddSealCostData = ReadEntityList<CDataDispelLockCostData>(buffer);
                obj.SealData = ReadEntityList<CDataDispelLockSealData>(buffer);
                return obj;
            }
        }
    }
}
