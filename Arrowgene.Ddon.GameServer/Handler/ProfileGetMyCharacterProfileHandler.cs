using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Dynamic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileGetMyCharacterProfileHandler : StructurePacketHandler<GameClient, C2SProfileGetMyCharacterProfileReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileGetMyCharacterProfileHandler));

        private OrbUnlockManager _OrbUnlockManager;
        private CharacterManager _CharacterManager;

        public ProfileGetMyCharacterProfileHandler(DdonGameServer server) : base(server)
        {
            _OrbUnlockManager = server.OrbUnlockManager;
            _CharacterManager = server.CharacterManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SProfileGetMyCharacterProfileReq> packet)
        {
            client.EquipPresetIndex = 0;

            S2CProfileGetMyCharacterProfileRes Result = new S2CProfileGetMyCharacterProfileRes();
            Result.OrbStatusList = _OrbUnlockManager.GetOrbPageStatus(client.Character);
            Result.AbilityCostMax = _CharacterManager.GetMaxAugmentAllocation(client.Character);
            client.Send(Result);
        }
    }
}
