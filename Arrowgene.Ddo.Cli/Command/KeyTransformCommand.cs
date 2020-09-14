using System;
using Arrowgene.Buffers;
using Arrowgene.Ddo.Shared;

namespace Arrowgene.Ddo.Cli.Command
{
    public class KeyTransformCommand : ICommand
    {
        public string Key => "transform";
        public string Description => "Transforms Key";

        public CommandResultType Run(CommandParameter parameter)
        {
            string keyHex =
                "414141414141414141414141414141414141414141414141414141414141414141414141414141414141414141414141";
            byte[] key = Util.FromHexString(keyHex);

            Console.WriteLine("a:");
            Util.DumpBuffer(new StreamBuffer(a(key)));
            Console.WriteLine();
            Console.WriteLine("c:");
            Util.DumpBuffer(new StreamBuffer(c(key)));
            Console.WriteLine();
            Console.WriteLine("d:");
            Util.DumpBuffer(new StreamBuffer(d(key)));
            Console.WriteLine();

            byte[] output = new byte[0x210 * 3];


            output[0x210] = 2;
            ba(output);
            uint v = 0x8;
            uint v1 = v >> 0x5; // esp + c
            uint v2 = v & 0x1F; // 013EBFC0 | 83E1 1F | and ecx,1F   esp + 14
            uint v3 = 0x20 - v2;

            bb(output, v2, v3);

            uint v4 = (uint) (output[0] |
                              output[1] << 8 |
                              output[2] << 16 |
                              output[3] << 24
                );
            v4 = v4 << (byte) v2;
            output[0] = (byte) (v4 & 0xFF);
            output[1] = (byte) (v4 >> 8 & 0xFF);
            output[2] = (byte) (v4 >> 16 & 0xFF);
            output[3] = (byte) (v4 >> 24 & 0xFF);

            // output[1] = 2;
            // output[0x210] = 0xBB;

            // 013B5E10   | 56  | push esi
            byte[] data = new byte[]
            {
                0xF6, 0x47, 0x3E, 0x35, 0xD5, 0x7F, 0x88, 0xE7,
                0x09, 0x3F, 0x90, 0x2D, 0xA8, 0xB4, 0x65, 0x66
            };
            uint a1 = (uint) (data[0] | data[1] << 8 | data[2] << 16 | data[3] << 24);
            uint a2 = (uint) (data[4] | data[5] << 8 | data[6] << 16 | data[7] << 24);
            uint a3 = (uint) (data[8] | data[9] << 8 | data[10] << 16 | data[11] << 24);
            uint a4 = (uint) (data[12] | data[13] << 8 | data[14] << 16 | data[15] << 24);

            uint a11 = a1 << 0xF;
            uint a12 = a11 ^ a1;
            
            Console.WriteLine("output:");
            Util.DumpBuffer(new StreamBuffer(output));
            Console.WriteLine();

            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }



        private byte[] a(byte[] input)
        {
            uint c = 0x40AA9193; // 0415B3AB | 2305 A6769D00 | and eax,dword ptr ds:[9D76A6] 
            return x(input, c);
        }

        private byte[] c(byte[] input)
        {
            uint c = 0x194721D6; // 040A7904 | 2305 E960E600 | and eax,dword ptr ds:[E660E9] 
            return x(input, c);
        }

        private byte[] d(byte[] input)
        {
            uint c = 0xF6A44860; // 02F6E7E5 | 2305 49D05100 | and eax,dword ptr ds:[51D049] 
            return x(input, c);
        }

        private byte[] x(byte[] input, uint c)
        {
            byte[] output = new byte[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                byte current = input[i];
                uint j = (uint) (i % 4);
                uint j1 = j << 3;
                byte j2 = (byte) j1;
                uint j3 = (uint) (0xFF << j2);
                uint j4 = c & j3;
                uint j6 = j << 3;
                uint j7 = j4 >> (int) j6;
                byte res = (byte) (j7 ^ current);
                output[i] = res;
            }

            return output;
        }

        private void b(byte[] key, byte[] output)
        {
            for (int i = 0; i < key.Length; i++)
            {
                byte current = key[i];
                output[i + 0x210] = current;
                // bb(output);
            }
        }

        private void bb(byte[] output, uint eax, uint esp_p10)
        {
            uint edx = 0;
            uint edi = 0;
            uint esi = 0;
            //uint eax = 8;
            uint ebx = 0x84;
            uint ecx = 0x84;
            uint ebp = ecx * 4;
            uint esp_p_10 = edx;
            while (ebx > 1)
            {
                uint offset = ebx * 4 - 4;
                esi = (uint) (output[offset] |
                              output[offset + 1] << 8 |
                              output[offset + 2] << 16 |
                              output[offset + 3] << 24
                    );
                ebx--;
                ecx = eax;
                esi = esi << (byte) ecx;
                ecx = esp_p10;
                ebp = ebp - 4;

                // 013EC000 | 8975 00  | mov dword ptr ss:[ebp],esi 
                output[ebp] = (byte) (esi & 0xFF);
                output[ebp + 1] = (byte) (esi >> 8 & 0xFF);
                output[ebp + 2] = (byte) (esi >> 16 & 0xFF);
                output[ebp + 3] = (byte) (esi >> 24 & 0xFF);

                offset = ebx * 4 - 4;
                edx = (uint) (output[offset] |
                              output[offset + 1] << 8 |
                              output[offset + 2] << 16 |
                              output[offset + 3] << 24
                    );

                edx = edx >> (byte) ecx;
                edx = edx | esi;

                // 013EC00B | 8955 00 | mov dword ptr ss:[ebp],edx  
                output[ebp] = (byte) (edx & 0xFF);
                output[ebp + 1] = (byte) (edx >> 8 & 0xFF);
                output[ebp + 2] = (byte) (edx >> 16 & 0xFF);
                output[ebp + 3] = (byte) (edx >> 24 & 0xFF);
            }
        }

        private void ba(byte[] output)
        {
            uint edx = 0;
            uint edi = 0;
            uint esi = 0;
            uint ecx = 0;
            int offset = 0x210;
            for (int eax = 0; eax < 0x84; eax += 4)
            {
                edi = (uint) (output[offset + eax] |
                              output[offset + eax + 1] << 8 |
                              output[offset + eax + 2] << 16 |
                              output[offset + eax + 3] << 24
                    );
                esi = (byte) edx;
                edx = (uint) (output[eax] |
                              output[eax + 1] << 8 |
                              output[eax + 2] << 16 |
                              output[eax + 3] << 24
                    );
                edx = edx + esi;
                edx = edx + edi;
                ecx = 0;
                bool cf = false;
                bool zf = false;
                if (edx == esi)
                {
                    zf = true;
                    cf = false;
                }
                else if (edx < esi)
                {
                    zf = false;
                    cf = true;
                }
                else if (edx > esi)
                {
                    zf = false;
                    cf = false;
                }
                else
                {
                    // error
                    throw new Exception("ERROR");
                }

                output[eax] = (byte) (edx & 0xFF);
                output[eax + 1] = (byte) (edx >> 8 & 0xFF);
                output[eax + 2] = (byte) (edx >> 16 & 0xFF);
                output[eax + 3] = (byte) (edx >> 24 & 0xFF);
                if (zf)
                {
                    // 013EC0F2            | 0F44CE                | cmove ecx,esi 
                    // 0F 44	CMOVE r32, r/m32	Move if equal (ZF=1).
                    ecx = esi;
                }

                byte dl = 0;
                if (cf)
                {
                    // 013EC0F5            | 0F92C2                | setb dl   
                    //  0F 92	SETB r/m8	Set byte if below (CF=1)
                    dl = 1;
                }

                dl = (byte) (dl | (byte) ecx);
                byte[] b = new byte[4];
                b[0] = (byte) (edx & 0xFF);
                b[1] = (byte) (edx >> 8 & 0xFF);
                b[2] = (byte) (edx >> 16 & 0xFF);
                b[3] = (byte) (edx >> 24 & 0xFF);
                b[0] = dl;
                edx = (uint) (b[0] |
                              b[1] << 8 |
                              b[2] << 16 |
                              b[3] << 24
                    );
            }
        }
    }
}
