using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCreateModeCharacterEditHandler : GameRequestPacketHandler<C2SCharacterCreateModeCharacterEditParamReq, S2CCharacterCreateModeCharacterEditParamRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCreateModeCharacterEditHandler));

        public CharacterCreateModeCharacterEditHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCreateModeCharacterEditParamRes Handle(GameClient client, C2SCharacterCreateModeCharacterEditParamReq request)
        {
            client.Character.EditInfo = request.EditInfo;
            Server.Database.UpdateEditInfo(client.Character);

            foreach (Client other in Server.ClientLookup.GetAll())
            {
                other.Send(new S2CCharacterEditUpdateEditParamExNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = 0,
                    EditInfo = client.Character.EditInfo,
                    Name = client.Character.FirstName
                });
            }

            return new S2CCharacterCreateModeCharacterEditParamRes();
        }
    }
}
