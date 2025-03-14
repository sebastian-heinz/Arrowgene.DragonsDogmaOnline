using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonUpdateKeyPointDoorStatusHandler : GameRequestPacketHandler<C2SSeasonDungeonUpdateKeyPointDoorStatusReq, S2CSeasonDungeonUpdateKeyPointDoorStatusRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonUpdateKeyPointDoorStatusHandler));

        public SeasonDungeonUpdateKeyPointDoorStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonUpdateKeyPointDoorStatusRes Handle(GameClient client, C2SSeasonDungeonUpdateKeyPointDoorStatusReq request)
        {
            Logger.Info($"KeyPointDoor: StageId{request.LayoutId.AsStageLayoutId()}, PosId={request.PosId}");

            var doorState = Server.EpitaphRoadManager.GetMysteriousDoorState(client.Party, request.LayoutId.AsStageLayoutId(), request.PosId);

            string message = "";
            if (doorState.State == SoulOrdealOmState.DoorLocked)
            {
                message = "A mysterious power was scattered all around";
                Server.EpitaphRoadManager.SetMysteriousDoorState(client.Party, request.LayoutId.AsStageLayoutId(), request.PosId, SoulOrdealOmState.ScatterPowers);

                client.Party.SendToAll(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = request.LayoutId,
                    PosId = request.PosId,
                    State = SoulOrdealOmState.ScatterPowers
                });

                Server.EpitaphRoadManager.SpreadMysteriousPowers(client.Party, request.LayoutId.AsStageLayoutId(), request.PosId);
            }
            else if (doorState.State == SoulOrdealOmState.ScatterPowers)
            {
                message = "Collect scattered powers to unseal the door";
            }
            else if (doorState.State == SoulOrdealOmState.DoorUnlocked)
            {
                message = "The mysterious door has opened";
                client.Party.SendToAll(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = request.LayoutId,
                    PosId = request.PosId,
                    State = SoulOrdealOmState.DoorUnlocked
                });
            }

            // Send message to the rest of the party
            client.Party.SendToAllExcept(new S2C_SEASON_62_28_16_NTC() {Message = message}, client);

            return new S2CSeasonDungeonUpdateKeyPointDoorStatusRes()
            {
                Unk0 = 1, // This seems to be a bool?
                Unk1 = message
            };
        }
    }
}

