using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextGetSetContextHandler : GameRequestPacketQueueHandler<C2SContextGetSetContextReq, S2CContextGetSetContextRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextGetSetContextHandler));


        public ContextGetSetContextHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SContextGetSetContextReq request)
        {
            PacketQueue queue = new();

            S2CContextGetSetContextRes res = new S2CContextGetSetContextRes();
            client.Enqueue(res, queue);

            // Game alternates between 
            // S2CContextSetContextNtc and S2CContextSetContextBaseNtc
            // Not sure why it chooses what it does
            // In packet captures, these are mostly related to Quests and NPCs
            // but could it also be for others?
            // Or maybe if it doesn't exist it sends one, otherwise it sends the other?

            // We believe it may be telling the client to load a persistent context.
            // If it's not sent, it will load a new context.
            // Sending S2CInstanceAreaResetNtc resets it (Like its done in StageAreaChangeHandler)
            // Send to all or just the host?

            CDataContextSetBase baseContext = request.Base;
            Tuple<CDataContextSetBase, CDataContextSetAdditional> context = ContextManager.GetContext(client.Party, baseContext.UniqueId);

            int clientIndex = Math.Max(client.Party.ClientIndex(client), 0);
            baseContext.MasterIndex = clientIndex; //Likely hacky.

            if (context == null)
            {
                var response = new S2CContextSetContextBaseNtc()
                {
                    Base = baseContext
                };

                client.Party.EnqueueToAll(response, queue);
            }
            else
            {
                var response = new S2CContextSetContextNtc()
                {
                    Base = context.Item1,
                    Additional = context.Item2
                };

                client.Party.EnqueueToAll(response, queue);
            }

            Logger.Debug($"ContextGetSetContextHandler: ContextId: {baseContext.ContextId}, UniqueId: 0x{baseContext.UniqueId:x16}");

            ContextManager.AssignMaster(client, baseContext.UniqueId, clientIndex);

            return queue;
        }
    }
}
