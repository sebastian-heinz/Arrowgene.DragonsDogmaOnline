using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Server.Handler
{
    public class FallbackHandler<TClient> : PacketHandler<TClient> where TClient : Client
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FallbackHandler<TClient>));

        public FallbackHandler(DdonServer<TClient> server) : base(server)
        {
        }

        public override PacketId Id => PacketId.UNKNOWN;

        public override void Handle(TClient client, IPacket request)
        {
            if(request.Id.HandlerSubId == 1)
            {
                PacketId responsePacketId = PacketId.GetPacketId(Server.Type, request.Id.GroupId, request.Id.HandlerId, 2);
                IBuffer responseBuffer = new StreamBuffer();
                responseBuffer.WriteUInt32((uint) ErrorCode.ERROR_CODE_NOT_IMPLEMENTED, Endianness.Big);
                Packet response = new Packet(responsePacketId, responseBuffer.GetAllBytes());
                client.Send(response);
            }
        }
    }
}
