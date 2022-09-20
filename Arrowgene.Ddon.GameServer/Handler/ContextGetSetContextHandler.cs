using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextGetSetContextHandler : StructurePacketHandler<GameClient, C2SContextGetSetContextReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextGetSetContextHandler));


        public ContextGetSetContextHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SContextGetSetContextReq> packet)
        {
            S2CContextGetSetContextRes res = new S2CContextGetSetContextRes();
            client.Send(res);

            // Send to all or just the host?
            client.Party.SendToAll(new S2CContextMasterChangeNtc() {
                Unk0 = new List<CDataMasterInfo>() {
                    new CDataMasterInfo() {
                        UniqueId = packet.Structure.Base.UniqueId,
                        Unk0 = 0
                    }
                }
            });

            // We believe it may be telling the client to load a persistent context.
            // If it's not sent, it will load a new context.
            // Sending S2CInstance_13_42_16_Ntc resets it (Like its done in StageAreaChangeHandler)
            //  Send to all or just the host?
            client.Party.SendToAll(new S2CContextSetContextBaseNtc() {
                Base = packet.Structure.Base
            });

            if(client.Party.Contexts.ContainsKey(packet.Structure.Base.UniqueId))
            {
                Tuple<CDataContextSetBase, CDataContextSetAdditional> context = client.Party.Contexts[packet.Structure.Base.UniqueId];
                // Send to all or just the host?
                client.Party.SendToAll(new S2CContextSetContextNtc() {
                    Base = context.Item1,
                    Additional = context.Item2
                });
            }
        }
    }
}
