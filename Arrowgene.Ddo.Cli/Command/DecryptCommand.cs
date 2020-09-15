using System;
using Arrowgene.Buffers;
using Arrowgene.Ddo.Shared;
using Arrowgene.Ddo.Shared.Crypto;

namespace Arrowgene.Ddo.Cli.Command
{
    public class DecryptCommand : ICommand
    {
        public string Key => "decrypt";
        public string Description => "Decrypt packet data";

        public CommandResultType Run(CommandParameter parameter)
        {
            // 0130
            string data1 =
                "b71a9b05ff4c4f2cbfc63bbf8b955587b1d42764a984c71c5b710fd4b351e98ebc90d3be3dc9d49ebfb981b7c4f01b0b3944f294f0a114b35b44bb24084ee16471d1f4d9d13c784f434af0ef35f17505557a4a0a5c1a25b8013cee0bb55d4645effa115785c5e480e84ffc32f82c9c1f2f3e658723ba2794c238cd5f51655c5d64ba2f3ccf2fb7ea43546f9aa87122e9c6e9e85598e0c8926d13f5ef1481a47a5fbeb9f34978337b0c475d2a730f3370306275b02d1c456633e3180a6c3734338b1dbdc68c21a9039e3c8c8d2634147641f6a7aacf88f3df1bd439517d82c9d53f6ac1fd69549357963e0f4762390c64674009c10dee3f2fca1b415bef5f0bec821f794e9a6a6917e61d600977e8945cf899a803c975048b3faf20021a839e163169ad17d3270b7e2986bd237fb53209";
            string data2 =
                "0150066b608643985002409e7be9541a39c4658cde84ce0f290dd91ebc1c7d4054864bfcaa80279c06d06b149f350c320029f8329c6e837b07608d4bd7aa13ecea46b77f3673e05da5a4b6877f27922701d333c89f10171c3b5f9482924f38b572ef68a598323f5091d0b1d572678437bd10f1dad2b112decf9c801f193188c0cd7c881918220bd851d8372c73f87b66e78462792e53d2cc5868e768941b6011fe9d65300b8841153afb06e73e9419f057656c9f30e836c17e0489537cdf91d17bf6a8d24e889a6b8c414a456d06410bca38a15aba22109841d523911b716ee7eca07970e1392857dc646d9adfecb26d0f877cd19cd27857aaf098cd73d0cf18597286eb6b06c1e4a9934046e369d8c619d3af94867aa106789d43ef76427dbaaf728d06d668b7e252ca95bfa6f99c5f515d7334218a51a761eb7b25977c574f8ce878daeac7be1945b1c00f954b5cab9810";
            string data3 =
                "0150D2D0C06F4C7EE6D68546DA5E10F98DF8E1D20688803E7B26167C77F45B906DAEA6A19404B7C9243E7EC4A37946D4BDC39F05A38AEE425B0AC932AD33A0326A5F9E9467BC3851C362E4754E0D8EFF9C7A89A7A442D920C42967087CD3F1C4B4C09271058F88989F4B642D8FB128C06D54A322BF2553EA57D89666C09FCC3B92E66268D04FCADAB90DE72D42ADF8085D551F4EFBCAA83EEC299254FDEE82B7A48D87225EF737191C54400EB0B0F9196118580A8E80E0494BDC5863A326618BC0FB62C0B3463257DBACE19CEA65E876BE196B6C04B71B8457FE5A9ECE9BA5BD85CA9CD5AEC210952BA3AF9C9508F9CDF77ECD53BA610011DC339BDCCBE3909A112D8DFBADC8080742D4AD036E0B21C2B8D15CF73EBACD965E75F6CC3E383DAB74F226C88D39BFB201D7C6B20C2A283B4099F1D334F80105E26C87EC82948366C0060FDC8BF90F6BCB84A6BFABBD40E56D22";

            byte[] decrypted = Decrypt(Util.FromHexString(data1));

            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }

        private static byte[] InitialKey = new byte[]
        {
            0x66, 0x32, 0x33, 0x65, 0x39, 0x38, 0x48, 0x61, 0x66, 0x4A, 0x64, 0x53, 0x6F, 0x61, 0x6A, 0x38,
            0x30, 0x51, 0x42, 0x6A, 0x68, 0x68, 0x32, 0x33, 0x6F, 0x61, 0x6A, 0x67, 0x6B, 0x6C, 0x53, 0x61,
        };

        private static byte[] InitialPrev = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        private byte[] Decrypt(byte[] input)
        {
            Console.WriteLine("Key:");
            Util.DumpBuffer(new StreamBuffer(InitialKey));

            //camellia.KeySchedule();
            //Console.WriteLine("Subkey:");
            // Util.DumpBuffer(new StreamBuffer(key));

            byte[] decrypted = new byte[input.Length];
            Camellia camellia = new Camellia();
            camellia.Decrypt(input, decrypted, InitialKey, InitialPrev);

            Console.WriteLine("Decrypted:");
            Util.DumpBuffer(new StreamBuffer(decrypted));
            Console.WriteLine();


            IBuffer output = new StreamBuffer(decrypted);
            // Inflate output for hash calculation
            IBuffer inflated = Inflate(output);
            Console.WriteLine("Inflated:");
            Util.DumpBuffer(inflated);
            Console.WriteLine();

            // Parse expected Hash
            IBuffer hashBytes = output.Clone(272, 20);
            byte[] expectedHash = ReadHash(hashBytes.GetAllBytes());
            Console.WriteLine("Expected Hash:");
            Util.DumpBuffer(new StreamBuffer(expectedHash));
            Console.WriteLine();

            // Calculate hash
            DdoPacketHash ddoPacketHash = new DdoPacketHash();
            byte[] calculatedHash = ddoPacketHash.ComputeHash(inflated.GetAllBytes());
            Console.WriteLine("Calculated Hash:");
            Util.DumpBuffer(new StreamBuffer(calculatedHash));
            Console.WriteLine();

            return decrypted;
        }

        private IBuffer Inflate(IBuffer output)
        {
            IBuffer processedA = Process64(output.Clone(0, 64));
            IBuffer processedB = Process64(output.Clone(64, 64));
            IBuffer processedC = Process64(output.Clone(128, 64));
            IBuffer processedD = Process64(output.Clone(192, 64));

            // Inflate last bytes
            IBuffer lastRow = output.Clone(256, 16);
            lastRow.WriteBytes(new byte[48]
                {
                    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                    0x17, 0x18, 0x19, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x30,
                    0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46,
                    0x47, 0x48,
                }
            );
            byte[] final = lastRow.GetAllBytes();


            // possible hash init
            uint c1 = 0xD9AAD30C;
            uint c2 = 0x51225B84;
            uint c3 = 0x26552CF3;
            uint c4 = 0xAEDDA47B;
            uint c5 = 0x7D3D11FD;

            uint ad = c1 ^ 0xBEEFF00D;
            uint a1 = c2 ^ 0xBEEFF00D;
            uint a3 = c3 ^ 0xBEEFF00D;
            uint a4 = c4 ^ 0xBEEFF00D;
            uint a5 = c5 ^ 0xBEEFF00D;


            //TODO extract how the last bytes are inflated.
            final[17] = (byte) (final[16] | 0x80); //025E0593 | 804C2C 3C 80 | or byte ptr ss:[esp+ebp+3C],80

            uint a00 = 0;
            uint a01 = 0;
            uint
                a = a00 & 0xFF00; // 0333166B | 81E1 00FF0000            | and ecx,FF00                                                            |
            uint
                b = a01 << 0x10; // 025F34AA | C1E0 10                  | shl eax,10                                                              |
            uint result = a & b; // 025F34AD | 03C8 | add ecx,eax
            uint r1 = result << 0x8;
            uint
                r2 = b >> 0x8; // 02EC44EC | C1E8 08                  | shr eax,8                                                               |
            uint r3 = r2 & 0xFF00;


            // IBuffer processedE = Process64(lastRow);


            // TODO REMOVE THIS BLOCK ONCE ALGO COMPLETED
            IBuffer lastRowStatic = output.Clone(256, 16);
            lastRowStatic.WriteByte(0x80);
            lastRowStatic.WriteBytes(new byte[15]);
            lastRowStatic.WriteBytes(new byte[16]);
            lastRowStatic.WriteBytes(new byte[14]);
            lastRowStatic.WriteByte(0x08);
            lastRowStatic.WriteByte(0x80);
            IBuffer processedE = Process64(lastRowStatic);
            // TODO REMOVE THIS BLOCK ONCE ALGO COMPLETED

            IBuffer processed = new StreamBuffer();
            processed.WriteBuffer(processedA);
            processed.WriteBuffer(processedB);
            processed.WriteBuffer(processedC);
            processed.WriteBuffer(processedD);
            processed.WriteBuffer(processedE);

            return processed;
        }

        private byte[] ReadHash(byte[] input)
        {
            int hashSize = 20;
            byte[] output = new byte[hashSize];
            int srcIndex = -1;
            int srcOffset = 0;
            for (int dstIndex = 0; dstIndex < 20; dstIndex++)
            {
                if (srcIndex < srcOffset)
                {
                    srcIndex = dstIndex + 3;
                    srcOffset = dstIndex;
                }

                output[dstIndex] = input[srcIndex];
                srcIndex--;
            }

            return output;
        }

        private IBuffer Process64(IBuffer input)
        {
            EndiannessSwapper swapper = new EndiannessSwapper(Endianness.Little);

            input.SetPositionStart();
            IBuffer output = new StreamBuffer();
            for (int i = 0; i < 4; i++)
            {
                uint a1 = input.ReadUInt32();
                uint a2 = input.ReadUInt32();
                uint a3 = input.ReadUInt32();
                uint a4 = input.ReadUInt32();
                uint a11 = swapper.SwapBytes(a1);
                uint a22 = swapper.SwapBytes(a2);
                uint a33 = swapper.SwapBytes(a3);
                uint a44 = swapper.SwapBytes(a4);
                output.WriteUInt32(a11);
                output.WriteUInt32(a22);
                output.WriteUInt32(a33);
                output.WriteUInt32(a44);
            }

            for (int i = 0; i < 64; i++)
            {
                output.Position = 0x34 + (i * 4);
                uint e = output.ReadUInt32();
                output.Position = 0x20 + (i * 4);
                uint f = output.ReadUInt32();

                uint ef = e ^ f; // 013ED0B3 | 3341 18 | xor eax,dword ptr ds:[ecx+18]

                output.Position = 0 + (i * 4);
                uint g = output.ReadUInt32();
                uint efg = ef ^ g;

                output.Position = 0x8 + (i * 4);
                uint h = output.ReadUInt32();
                uint efgh = efg ^ h;

                uint rol = RotateLeft(efgh, 1);
                output.Position = 0x40 + (i * 4);
                output.WriteUInt32(rol);
            }

            return output;
        }

        public static uint RotateLeft(uint x, int n)
        {
            return (((x) << (n)) | ((x) >> (32 - (n))));
        }
    }
}
