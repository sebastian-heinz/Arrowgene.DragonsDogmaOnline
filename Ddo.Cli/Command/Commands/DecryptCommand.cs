using System;
using Arrowgene.Services.Buffers;
using Ddo.Cli.Argument;
using Ddo.Server.Common;
using Ddo.Server.Crypto;
using Buffer = Arrowgene.Services.Buffers.Buffer;

namespace Ddo.Cli.Command.Commands
{
    public class DecryptCommand : ConsoleCommand
    {
        private static void DumpBuffer(IBuffer buffer)
        {
            int pos = buffer.Position;
            buffer.SetPositionStart();
            Console.WriteLine(Util.ToAsciiString(buffer.GetAllBytes(), true));
            while (buffer.Size > buffer.Position)
            {
                byte[] row = buffer.ReadBytes(16);
                Console.WriteLine(Util.ToHexString(row, ' '));
            }

            buffer.Position = pos;
        }

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            string data =
                "0130C0FA76B46338A97AAD8469AF8926D93232CEA2FBB37C7A807BC9B91028CDFA74B553790754BB160A838C371630EA75347AE3BAEEE746F28D9809F669FF0F4D516DBE7FE00656B918C9AEA6CE794FE5EA6BB16E1D58223DCD87DA0B3B531DA3719DF7200624FC2EC5196F7676EBB7D4FC89587E84433526D9C9F11E7EAE3D44B5276516BAC887BB932D6E0442F1C46C0E2BDE1F07745E1D490AD0FEBFAED94B5ADEF93AADE151C09D3E999684B98230B4088B0407638E1A39BD14676D85AAD612496DC1852CB674C5647BC68DF1FC62FECEEAAFA55E728E723268BE988AB1EA33885CCD5FDAE7B9B3F867FEE4BD62464DECC56E2215AF88C6464B355080A348A471DF15E660CFA35C0A5A1106D0A47E057FE0857D3DC7A02EBE976AA9C1B7F45BBBC31C87106488A34B33832B7BC237A7";
            string data1 =
                "0130b71a9b05ff4c4f2cbfc63bbf8b955587b1d42764a984c71c5b710fd4b351e98ebc90d3be3dc9d49ebfb981b7c4f01b0b3944f294f0a114b35b44bb24084ee16471d1f4d9d13c784f434af0ef35f17505557a4a0a5c1a25b8013cee0bb55d4645effa115785c5e480e84ffc32f82c9c1f2f3e658723ba2794c238cd5f51655c5d64ba2f3ccf2fb7ea43546f9aa87122e9c6e9e85598e0c8926d13f5ef1481a47a5fbeb9f34978337b0c475d2a730f3370306275b02d1c456633e3180a6c3734338b1dbdc68c21a9039e3c8c8d2634147641f6a7aacf88f3df1bd439517d82c9d53f6ac1fd69549357963e0f4762390c64674009c10dee3f2fca1b415bef5f0bec821f794e9a6a6917e61d600977e8945cf899a803c975048b3faf20021a839e163169ad17d3270b7e2986bd237fb53209";
            string data2 =
                "00603b440b4e0e65f4d73322e9f37c0d73adb4b72750bc9e7a45d14bf59e1031576fdb9dce65b0ce1743c69ce4a1dafd8eb5175f0ec9372ed50d7b59f68ce1b87a13d4472c8478240b3c37dd2229d254337bede3f8f1f60a0d263634a994b6abbd97";
            byte[] decrypted = Decrypt(Util.FromHexString(data1));


            return CommandResultType.Exit;
        }

        public override string Key => "decrypt";
        public override string Description => "Decrypt packet data";

        private byte[] Decrypt(byte[] input)
        {
            DdoPacketCrypto crypto = new DdoPacketCrypto();

            byte[] decrypted = crypto.Decrypt(input);
            IBuffer output = new StreamBuffer(decrypted);

            Console.WriteLine("Decrypted:");
            DumpBuffer(output);
            Console.WriteLine();

            // Inflate output for hash calculation
            IBuffer inflated = Inflate(output);
            Console.WriteLine("Inflated:");
            DumpBuffer(inflated);
            Console.WriteLine();

            // Parse expected Hash
            IBuffer hashBytes = output.Clone(272, 20);
            byte[] expectedHash = ReadHash(hashBytes.GetAllBytes());
            Console.WriteLine("Expected Hash:");
            DumpBuffer(new StreamBuffer(expectedHash));
            Console.WriteLine();
            
            // Calculate hash
            DdoPacketHash ddoPacketHash = new DdoPacketHash();
            byte[] calculatedHash = ddoPacketHash.ComputeHash(inflated.GetAllBytes());
            Console.WriteLine("Calculated Hash:");
            DumpBuffer(new StreamBuffer(calculatedHash));
            Console.WriteLine();
            
            // Reverse decrypted
            Console.WriteLine("Reversed:");
            byte[] reversed = output.GetBytes(0,256);
            Array.Reverse(reversed);
            DumpBuffer(new StreamBuffer(reversed));
            Console.WriteLine();
            
            // Transform reversed
            byte[] transformed = new byte[reversed.Length];
            uint c = 0x204D276C;
            for (int i = 0; i < reversed.Length; i++)
            {
                uint current = reversed[i];
                uint j = (uint) (i % 4);
                uint j1 = j << 3; // 0369164A | C1E2 03 | shl edx,3 
                byte j2 = (byte)j1;
                uint j3 = (uint)(0xFF << j2);
                uint j4 = c & j3;
                uint j6 = j << 3;
                uint j7 = j4 >> (int)j6;
                byte res = (byte)(j7 ^ current);
                transformed[i] = res;
            }
            Console.WriteLine("Transformed:");
            DumpBuffer(new StreamBuffer(transformed));
            Console.WriteLine();
            
            return output.GetAllBytes();
        }

        private IBuffer Inflate(IBuffer output)
        {
    
            IBuffer processedA = Process64(output.Clone(0, 64));
            IBuffer processedB = Process64(output.Clone(64, 64));
            IBuffer processedC = Process64(output.Clone(128, 64));
            IBuffer processedD = Process64(output.Clone(192, 64));
            
            // Inflate last bytes
            IBuffer lastRow = output.Clone(256, 16);
            lastRow.WriteBytes(new byte[48]);
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

            uint a = 0 & 0xFF00;
            uint b = 0 << 0x10;
            uint result = a & b; // 025F34AD | 03C8 | add ecx,eax
            uint r1 = result << 0x8;


            // lastRow.WriteByte(0x80);
            // lastRow.WriteBytes(new byte[15]);
            // lastRow.WriteBytes(new byte[16]);
            // lastRow.WriteBytes(new byte[14]);
            // lastRow.WriteByte(0x08);
            // lastRow.WriteByte(0x80);
            IBuffer processedE = Process64(lastRow);


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
            input.SetPositionStart();
            IBuffer output = new StreamBuffer();
            for (int i = 0; i < 4; i++)
            {
                uint a1 = input.ReadUInt32();
                uint a2 = input.ReadUInt32();
                uint a3 = input.ReadUInt32();
                uint a4 = input.ReadUInt32();
                uint a11 = Buffer.SwapBytes(a1);
                uint a22 = Buffer.SwapBytes(a2);
                uint a33 = Buffer.SwapBytes(a3);
                uint a44 = Buffer.SwapBytes(a4);
                output.WriteInt32(a11);
                output.WriteInt32(a22);
                output.WriteInt32(a33);
                output.WriteInt32(a44);
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
                output.WriteInt32(rol);
            }

            return output;
        }

        public static uint RotateLeft(uint x, int n)
        {
            return (((x) << (n)) | ((x) >> (32 - (n))));
        }



    }
}
