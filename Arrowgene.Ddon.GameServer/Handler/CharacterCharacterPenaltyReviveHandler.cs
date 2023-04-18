using System.Threading.Tasks;
using System;
using System.Threading;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCharacterPenaltyReviveHandler : StructurePacketHandler<GameClient, C2SCharacterCharacterPenaltyReviveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCharacterPenaltyReviveHandler));

        // Same time as it was in the original server
        private static readonly TimeSpan WeaknessTimeSpan = TimeSpan.FromMinutes(20);

        public CharacterCharacterPenaltyReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCharacterPenaltyReviveReq> packet)
        {
            S2CCharacterCharacterPenaltyReviveRes res = new S2CCharacterCharacterPenaltyReviveRes();
            client.Send(res);

            // Weakness
            client.Send(new S2CCharacterStartDeathPenaltyNtc()
            {
                RemainTime = (uint) WeaknessTimeSpan.Seconds
            });

            // Restore after time passes
            Task.Delay(WeaknessTimeSpan).ContinueWith(_ => client.Send(new S2CCharacterFinishDeathPenaltyNtc()));
        }
    }
}