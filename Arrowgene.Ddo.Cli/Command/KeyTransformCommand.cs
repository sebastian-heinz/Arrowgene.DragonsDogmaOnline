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
            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }

        private void Scramble()
        {
            string keyHex =
                "414141414141414141414141414141414141414141414141414141414141414141414141414141414141414141414141";
            byte[] key = Util.FromHexString(keyHex);


            byte[] output = new byte[0x210 * 2];
            // output[1] = 2;
            //  output[0x210] = 0xBB;
            ba(output);


            Console.WriteLine("output:");
            Util.DumpBuffer(new StreamBuffer(output));
            Console.WriteLine();
        }

        private byte[] a(byte[] input)
        {
            byte[] output = new byte[input.Length];
            uint c = 0x40AA9193; // 0415B3AB | 2305 A6769D00 | and eax,dword ptr ds:[9D76A6] 
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

        private byte[] b(byte[] input)
        {
            byte[] output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                byte current = input[i];
            }

            return output;
        }


        private void bb(byte[] output)
        {
            uint edx = 0;
            uint edi = 0;
            uint esi = 0;
            uint eax = 8;
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
