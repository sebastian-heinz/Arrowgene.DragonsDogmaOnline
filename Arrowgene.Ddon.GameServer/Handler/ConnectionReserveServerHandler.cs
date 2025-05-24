using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionReserveServerHandler : GameRequestPacketHandler<C2SConnectionReserveServerReq, S2CConnectionReserveServerRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionReserveServerHandler));


        public ConnectionReserveServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionReserveServerRes Handle(GameClient client, C2SConnectionReserveServerReq request)
        {
            Connection reservedConnection = new Connection()
            {
                ServerId = request.GameServerUniqueID,
                AccountId = client.Account.Id,
                Type = ConnectionType.GameServer,
                Created = DateTime.UtcNow
            };
            
            if (!Server.RpcManager.DoesGameServerExist(request.GameServerUniqueID))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_NET_NOT_CONNECT_GAME_SERVER,
                    $"The requested server {request.GameServerUniqueID} does not exist.");
            }

            if (!Server.RpcManager.PingServer(request.GameServerUniqueID))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_NET_NOT_CONNECT_GAME_SERVER,
                    $"The requested server {request.GameServerUniqueID} did not respond when pinged.");
            }

            var targetServerInfo = Server.RpcManager.ServerListInfo(request.GameServerUniqueID);
            if (targetServerInfo.LoginNum >= targetServerInfo.MaxLoginNum)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_LOBBY_JOIN_NUM_OVER,
                    $"Attempting to join server {request.GameServerUniqueID} that is over capacity ({targetServerInfo.LoginNum}/{targetServerInfo.MaxLoginNum})");
            }

            if (!Server.Database.InsertConnection(reservedConnection))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_NET_NOT_CONNECT_GAME_SERVER, 
                    $"Failed to reserve connection on server {request.GameServerUniqueID} for account {client.Account.Id}");
            }

            return new S2CConnectionReserveServerRes()
            {
                GameServerUniqueID = request.GameServerUniqueID,
                ReserveInfoList = request.ReserveInfoList
            };
        }
    }
}
