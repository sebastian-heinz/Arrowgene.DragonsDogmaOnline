using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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

            // Game alternates between 
            // S2CContextSetContextNtc and S2CContextSetContextBaseNtc
            // Not sure why it chooses what it does
            // In packet captures, these are mostly related to Quests and NPCs
            // but could it also be for others?
            // Or maybe if it doesn't exist it sends one, otherwise it sends the other?

            // We believe it may be telling the client to load a persistent context.
            // If it's not sent, it will load a new context.
            // Sending S2CInstance_13_42_16_Ntc resets it (Like its done in StageAreaChangeHandler)
            //  Send to all or just the host?

            var baseContext = packet.Structure.Base;
            var context = ContextManager.GetContext(client.Party, packet.Structure.Base.UniqueId);
            if (context == null)
            {
                client.Party.SendToAll(new S2CContextSetContextBaseNtc()
                {
                    Base = packet.Structure.Base
                });
            }
            else
            {
                client.Party.SendToAll(new S2CContextSetContextNtc()
                {
                    Base = context.Item1,
                    Additional = context.Item2
                });
            }

            Logger.Debug("===================================================================");
            Logger.Debug($"ContextGetSetContextHandler: ContextId: {baseContext.ContextId}, UniqueId: 0x{baseContext.UniqueId:x16}");
            Logger.Debug("===================================================================");
        }
    }
}
