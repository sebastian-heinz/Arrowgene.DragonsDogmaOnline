using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientDecideCharacterIdHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientDecideCharacterIdHandler));


        public ClientDecideCharacterIdHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_DECIDE_CHARACTER_ID_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {

            // Parse request -- C2L_DECIDE_CHARACTER_ID_REQ
            IBuffer recv = packet.AsBuffer();
            var characterID = recv.ReadUInt32(Endianness.Big);
            var clientVersion = recv.ReadUInt32(Endianness.Big);
            var type = recv.ReadByte();
            var rotationServerID = recv.ReadByte();
            var waitNum = recv.ReadUInt32(Endianness.Big);
            var counter = recv.ReadByte();

            Logger.Debug(client,
                $"C2L_DECIDE_CHARACTER_ID_REQ:\n" +
                $"    CharacterID: {characterID}\n" +
                $"    ClientVersion: {clientVersion}\n" +
                $"    Type: {type}\n" +
                $"    RotationServerID: {rotationServerID}\n" +
                $"    WaitNum: {waitNum}\n" +
                $"    Counter: {counter}\n" 
            );

            // Write L2C_DECIDE_CHARACTER_ID_RES packet.
            IBuffer buffer0 = new StreamBuffer();
            buffer0.WriteUInt32(0, Endianness.Big); // error
            buffer0.WriteUInt32(0, Endianness.Big); // result
            buffer0.WriteUInt32(2117592, Endianness.Big); // CharcterID
            buffer0.WriteUInt32(2, Endianness.Big); // Unknown -- (RotationServerID?)
            client.Send(new Packet(PacketId.L2C_DECIDE_CHARACTER_ID_RES, buffer0.GetAllBytes()));

            // Write L2C_LOGIN_WAIT_NUM_NTC packet. This is NOT required to get in game (can be commented out entirely).
            IBuffer buffer1 = new StreamBuffer();
            buffer1.WriteUInt32(100, Endianness.Big);
            client.Send(new Packet(PacketId.L2C_LOGIN_WAIT_NUM_NTC, buffer1.GetAllBytes()));
            //client.Send(LoginDump.Dump_31);

            // Write L2C_NEXT_CONNECT_SERVER_NTC
            IBuffer buffer2 = new StreamBuffer();
            buffer2.WriteUInt32(0, Endianness.Big); // Error
            CDataGameServerListInfoSerializer serializer = new CDataGameServerListInfoSerializer();
            serializer.Write(buffer2, new CDataGameServerListInfo
            {
                ID = 17,
                Name = "サーバー017",
                Brief = "",
                TrafficName = "少なめ",
                MaxLoginNum = 1000, // Player cap
                LoginNum = 0x1C, // Current players
                Addr = "127.0.0.1",
                Port = 52000,
                IsHide = false
            });
            buffer2.WriteByte(1); // "counter"
            client.Send(new Packet(PacketId.L2C_NEXT_CONNECT_SERVER_NTC, buffer2.GetAllBytes()));
        }
    }
}
