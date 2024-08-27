using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CbattleContentContentResetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_CONTENT_RESET_NTC;

        public S2CbattleContentContentResetNtc()
        {
            BattleContentStatusList = new List<CDataBattleContentStatus>();
        }

        public GameMode GameMode { get; set; }
        public List<CDataBattleContentStatus> BattleContentStatusList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CbattleContentContentResetNtc>
        {
            public override void Write(IBuffer buffer, S2CbattleContentContentResetNtc obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
                WriteEntityList(buffer, obj.BattleContentStatusList);
            }

            public override S2CbattleContentContentResetNtc Read(IBuffer buffer)
            {
                S2CbattleContentContentResetNtc obj = new S2CbattleContentContentResetNtc();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                obj.BattleContentStatusList = ReadEntityList<CDataBattleContentStatus>(buffer);
                return obj;
            }
        }
    }
}
