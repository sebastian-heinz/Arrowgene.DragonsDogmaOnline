using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CServerGetGameSettingRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SERVER_GET_GAME_SETTING_RES;

        public S2CServerGetGameSettingRes()
        {
            GameSetting = new CDataGameSetting();
        }

        public CDataGameSetting GameSetting { get; set; }

        public class Serializer : PacketEntitySerializer<S2CServerGetGameSettingRes>
        {
            public override void Write(IBuffer buffer, S2CServerGetGameSettingRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataGameSetting>(buffer, obj.GameSetting);
            }

            public override S2CServerGetGameSettingRes Read(IBuffer buffer)
            {
                S2CServerGetGameSettingRes obj = new S2CServerGetGameSettingRes();
                ReadServerResponse(buffer, obj);
                obj.GameSetting = ReadEntity<CDataGameSetting>(buffer);
                return obj;
            }
        }
    }
}
