using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class Lobby_3_3_16_Handler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Lobby_3_3_16_Handler));

        public Lobby_3_3_16_Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_LOBBY_3_3_16_NTC;

        public override void Handle(GameClient client, IPacket packet)
        {
            Logger.Error(packet.PrintData());

            IBuffer requestBuffer = packet.AsBuffer();
            byte unk0 = requestBuffer.ReadByte();
            uint unk1 = requestBuffer.ReadUInt32(Endianness.Big);
            int unk2length = requestBuffer.ReadInt32(Endianness.Big);
            byte[] unk2content = requestBuffer.ReadBytes(unk2length);

            IBuffer test = new StreamBuffer(unk2content);
            test.SetPositionStart();
            if (test.Size > 56)
            {
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32(); //16
                test.ReadUInt32();
                test.ReadUInt32();
                //   uint asdaa = test.ReadUInt32();
                //  uint asda = test.ReadUInt32(); //32

                byte[] b0 = test.ReadBytes(8);
                byte[] x = test.ReadBytes(8);
                byte[] b2 = test.ReadBytes(8);
                byte[] b3 = test.ReadBytes(8);
                byte[] b4 = test.ReadBytes(8);
                Array.Reverse(b0);
                Array.Reverse(x);
                Array.Reverse(b2);
                Array.Reverse(b3);
                Array.Reverse(b4);
                Logger.Info(
                    $"a0:{BitConverter.ToDouble(b0)} x:{BitConverter.ToDouble(x)} a2:{BitConverter.ToDouble(b2)} a3:{BitConverter.ToDouble(b3)} a4:{BitConverter.ToDouble(b4)}");
            }


            foreach (GameClient otherClient in Server.Clients)
            {
                if (otherClient != client)
                {
                    IBuffer responseBuffer = new StreamBuffer();
                    responseBuffer.WriteByte(unk0);
                    responseBuffer.WriteUInt32(client.Character.Id, Endianness.Big);
                    responseBuffer.WriteInt32(unk2length, Endianness.Big);
                    responseBuffer.WriteBytes(unk2content);
                    responseBuffer.WriteByte(0); // idk either

                    Packet response = new Packet(PacketId.S2C_LOBBY_3_4_16_NTC, responseBuffer.GetAllBytes());
                    client.Send(response);
                }
            }
        }
    }
}
