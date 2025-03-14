using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MandragoraBeginCraftHandler : GameRequestPacketHandler<C2SMandragoraBeginCraftReq, S2CMandragoraBeginCraftRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MandragoraBeginCraftHandler));

        public MandragoraBeginCraftHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMandragoraBeginCraftRes Handle(GameClient client, C2SMandragoraBeginCraftReq request)
        {
            S2CMandragoraBeginCraftRes res = new S2CMandragoraBeginCraftRes();

            // TODO:
            res.Unk0 = new CDataMyMandragoraBeginCraftResUnk0
            {
                SpeciesIndex = 101,
                Unk1 = 0,
                MandragoraId = 1,
                Unk3 = "Test",
                Unk4 = 0,
                Unk5 = 0,
                Unk6 = 0,
                Unk7 = new CDataMyMandragoraBeginCraftResUnk0Unk7
                {
                    Unk0 = 0,
                    Unk1 = 0,
                    Unk2 = new List<CDataMyMandragoraBeginCraftResUnk0Unk7Unk2>
                    {
                        new CDataMyMandragoraBeginCraftResUnk0Unk7Unk2
                        {
                            Unk0 = 0,
                            Unk1 = 0
                        }
                    },
                    Unk3 = 0
                }
            };
            
            return res;
        }
    }
}
