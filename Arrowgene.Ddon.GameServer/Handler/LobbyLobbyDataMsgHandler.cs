using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class LobbyLobbyDataMsgHandler : StructurePacketHandler<GameClient, C2SLobbyLobbyDataMsgReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(LobbyLobbyDataMsgHandler));

        // List of lobby areas, where you're supposed to see all other players.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> LobbyStageIds = new HashSet<uint>(){
            2, // White Dragon Temple
            341, // Dana Centrum
            486, // Fortress City Megado: Residential Level
            487, // Fortress City Megado: Residential Level
            488, // Fortress City Megado: Royal Palace Level
        };

        public LobbyLobbyDataMsgHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SLobbyLobbyDataMsgReq> packet)
        {
            // In the pcaps ive only seen 3.4.16 packets whose RpcPacket length is either of these
            S2CLobbyLobbyDataMsgNotice res = new S2CLobbyLobbyDataMsgNotice();
            res.Type = packet.Structure.Type; // I haven't seen any values other than 0x02
            res.CharacterId = client.Character.CharacterId; // Has to be overwritten since the request has the id set to 0
            res.RpcPacket = packet.Structure.RpcPacket;
            res.OnlineStatus = client.Character.OnlineStatus;

            if (client.Party != null)
            {
                if(!LobbyStageIds.Contains(client.Character.Stage.Id))
                {
                    client.Party.SendToAllExcept(res, client);
                }
                else
                {
                    foreach (GameClient otherClient in Server.ClientLookup.GetAll())
                    {
                        if (otherClient != client && (client.Character.Stage.Id == otherClient.Character.Stage.Id || client.Party.Id == otherClient.Party.Id))
                        {
                            otherClient.Send(res);
                        }
                    }
                }
            }

            if(packet.Structure.RpcPacket.Length > 52)
            {
                IBuffer rpcPacketBuffer = new StreamBuffer(packet.Structure.RpcPacket);
                rpcPacketBuffer.SetPositionStart();

                // nNetMsgData::Head::deserialize
                uint sessionId = rpcPacketBuffer.ReadUInt32(Endianness.Big); // NetMsgData.Head.SessionId
                ulong rpcId = rpcPacketBuffer.ReadUInt64(Endianness.Big); // NetMsgData.Head.RpcId
                uint msgIdFull = rpcPacketBuffer.ReadUInt32(Endianness.Big); // NetMsgData.Head.MsgIdFull
                uint searchId = rpcPacketBuffer.ReadUInt32(Endianness.Big); // NetMsgData.Head.SearchId, seems to either a PawnId or 0

                // There is theoretically a nNetMsgData::Base, but i cant even see the code that deserializes it in the client

                // nNetMsgData::CtrlBase::deserialize
                rpcPacketBuffer.ReadUInt64(Endianness.Big); // nNetMsgData::CtrlBase::stMsgCtrlBaseData.mUniqueId ?

                // Apparently all received 3.4.16s:
                //  - Have searchID of 0 (aren't related to pawns)
                //  - Have CtrlBaseData.mUniqueId of 0x8000000000000001

                // nNetMsgData::CtrlAction::deserialize
                bool isEnemy = rpcPacketBuffer.ReadBool(); // NetMsgData.CtrlAction.IsEnemy
                bool isCharacter = rpcPacketBuffer.ReadBool(); // NetMsgData.CtrlAction.IsCharacter
                bool isHuman = rpcPacketBuffer.ReadBool(); // NetMsgData.CtrlAction.IsHuman
                bool isEnemyLarge = rpcPacketBuffer.ReadBool(); // NetMsgData.CtrlAction.IsEnemyLarge

                // From now on, the contents (subpackets) depend on MsgId, a set of flags
                if((msgIdFull & 1) != 0) { // This is incomplete, msgIdFull is converted through a function first, (0x5FE8D0) but should work for most cases
                    client.Character.X = rpcPacketBuffer.ReadDouble(Endianness.Big);
                    client.Character.Y = rpcPacketBuffer.ReadFloat(Endianness.Big);
                    client.Character.Z = rpcPacketBuffer.ReadDouble(Endianness.Big);
                }
            }
        }
    }
}
