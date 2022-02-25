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
           // Logger.Error(Environment.NewLine + packet.PrintData());

            IBuffer requestBuffer = packet.AsBuffer();
            byte unk0 = requestBuffer.ReadByte();
            uint unk1 = requestBuffer.ReadUInt32(Endianness.Big);
            int unk2length = requestBuffer.ReadInt32(Endianness.Big);
            byte[] unk2content = requestBuffer.ReadBytes(unk2length);
            
           // Logger.Info($"LEN:{unk2length}");

            
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
                test.ReadUInt32();
                test.ReadUInt32();
                
                byte[] x = test.ReadBytes(8);
                byte[] y = test.ReadBytes(4);
                byte[] z = test.ReadBytes(8);
                Array.Reverse(x);
                Array.Reverse(y);
                Array.Reverse(z);
                // North = Y-
                // SOUTH = Y+
                // EAST = X+
                // WEST = X-
                //Logger.Info($"X:{BitConverter.ToDouble(x)} Y:{BitConverter.ToSingle(y)} Z:{BitConverter.ToDouble(z)}");

                client.X = BitConverter.ToDouble(x);
                client.Y = BitConverter.ToSingle(y);
                client.Z = BitConverter.ToDouble(z);
                
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                test.ReadUInt32();
                
             //   byte[] x1 = test.ReadBytes(8);
             //   byte[] z1 = test.ReadBytes(4);
             //   byte[] y1 = test.ReadBytes(8);
             //   Array.Reverse(x1);
             //   Array.Reverse(y1);
             //   Array.Reverse(z1);
             //   Logger.Info($"X1:{BitConverter.ToDouble(x1)} Y1:{BitConverter.ToDouble(y1)} Z1:{BitConverter.ToSingle(z1)}");

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
