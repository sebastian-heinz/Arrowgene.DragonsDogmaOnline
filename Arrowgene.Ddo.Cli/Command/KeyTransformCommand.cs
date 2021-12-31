using System;
using System.Numerics;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddo.Shared;
using Arrowgene.Ddo.Shared.Crypto;
using Buffer = Arrowgene.Buffers.Buffer;

namespace Arrowgene.Ddo.Cli.Command
{
    public class KeyTransformCommand : ICommand
    {
        public string Key => "transform";
        public string Description => "Transforms Key";

        private static byte[] keyData_D1_E8;
        private static byte[] keyData_9C_AC;
        private static byte[] keyData_BD_CF;
        private static byte[] keyData_2F_17;
        private static byte[] keyData_52_2E;
        private static byte[] keyData_A2_D1;


        public CommandResultType Run(CommandParameter parameter)
        {
            string seedHex = "8A 92 D7 7F 2A C6 57 06 D9 A2 1B 27 F0 92 A0 F7".Replace(" ", "");
            string serverKeyHex =
                "9CAC5E77F860F58C59BB1F1F907AD06ADAA97E40AC439F83A15B8282CAB6EC4A45165EBBD0A7B5FA731AFFDD9A2591F2F7CEF497E4D4972AB098955C28440DBF08876898A8BCC4ED4F98997DFA776225988CB445E78D0B0A011102B80B09F9F2B3B54551EA5DB6831AD38E71630CBE21E0934FE7EE55DDA5AB81406EB7564DD97270B6F3C7B038CF871EFE78E68C9032800761A9FC9FEC93A8FF092E717DEE5E1C5287DD7E2A8B0C32F41F4CF53F838F6F56189D57488C1A0B69AEC8F621BB4E5D2BE2C78154A4DBC226346BA4C09AD65DEB90BB09F3967ABA1F3485B1DEC2169C7161BAC2D0C14839FEEE3AAA58D95DF75F51A6228F48566C8FFA699D68E8D1";
            byte[] rngSeed = Util.FromHexString(seedHex);
            byte[] serverKey = Util.FromHexString(serverKeyHex);


            //transform_server();
            transform_bi(rngSeed, serverKey);
            transform_client();
            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }

        private void transform_bi(byte[] rngSeedData, byte[] serverKeyData)
        {
            // === server ===
            BigInteger serverKey = new BigInteger(serverKeyData, true, true);
            byte[] dst = new byte[0x210];
            dst[0x104] = 1;
            BigInteger constant = new BigInteger(dst, true, false);
            BigInteger s1 = serverKey * BigInteger.Pow(2, 8 * 4);
            BigInteger s2 = constant - s1; // 013EBC60 
            // s2 = 2F 17 97 62 __ 88 A1 53 63
            Console.WriteLine($"transform_bi-s1:{Environment.NewLine}{Util.HexDump(s1.ToByteArray())}");

            BigInteger s3 = s2 / BigInteger.Pow(2, 8 * 4);
            BigInteger s4 = s3 * BigInteger.Pow(2, 1); // 013EBFB0 (x*2^y)
            BigInteger s5 = serverKey * BigInteger.Pow(2, 1); // 013EBFB0

            Console.WriteLine($"transform_bi-s4:{Environment.NewLine}{Util.HexDump(s4.ToByteArray())}");
            Console.WriteLine($"transform_bi-s5:{Environment.NewLine}{Util.HexDump(s5.ToByteArray())}");

            // === client ===
            // 1) Construct BigInteger with RandomData + New Cammelia Key
            DdoRandom rng = new DdoRandom();
            rng.SetSeed(rngSeedData);
            string newKey = rng.GenerateKey();
            byte[] newKeyBytes = Encoding.UTF8.GetBytes(newKey);
            BigInteger c1 = new BigInteger(0);
            BigInteger c2 = new BigInteger(2);

            c1 = c1 + c2; // add first byte
            c1 = c1 * BigInteger.Pow(2, 8); // shift left, to make space

            // add random data
            for (int i = 0; i < 0xCD; i++)
            {
                byte rnd = (byte) rng.Next();
                c1 = c1 + new BigInteger(rnd); // add random byte
                c1 = c1 * BigInteger.Pow(2, 8); // shift left, to make space for next
            }

            // add new camellia key
            for (int i = 0; i < newKeyBytes.Length; i++)
            {
                c1 = c1 * BigInteger.Pow(2, 8); // shift left, to make space for next
                byte newKeyByte = newKeyBytes[i];
                c1 = c1 + new BigInteger(newKeyByte); // add new key byte
            }


            Console.WriteLine($"transform_bi-c1:{Environment.NewLine}{Util.HexDump(c1.ToByteArray())}");


            bool breakPoint = true;
        }

        private void transform_client_bi()
        {
            string keyDataHex =
                "9CAC5E77F860F58C59BB1F1F907AD06ADAA97E40AC439F83A15B8282CAB6EC4A45165EBBD0A7B5FA731AFFDD9A2591F2F7CEF497E4D4972AB098955C28440DBF08876898A8BCC4ED4F98997DFA776225988CB445E78D0B0A011102B80B09F9F2B3B54551EA5DB6831AD38E71630CBE21E0934FE7EE55DDA5AB81406EB7564DD97270B6F3C7B038CF871EFE78E68C9032800761A9FC9FEC93A8FF092E717DEE5E1C5287DD7E2A8B0C32F41F4CF53F838F6F56189D57488C1A0B69AEC8F621BB4E5D2BE2C78154A4DBC226346BA4C09AD65DEB90BB09F3967ABA1F3485B1DEC2169C7161BAC2D0C14839FEEE3AAA58D95DF75F51A6228F48566C8FFA699D68E8D1";
            keyData_9C_AC = Util.FromHexString(keyDataHex);


            // generate random
            BigInteger c1 = new BigInteger(0);
            BigInteger c2 = new BigInteger(2);
        }

        private void transform_server()
        {
            // server decrypted response, 256 bytes
            string keyDataHex =
                "9CAC5E77F860F58C59BB1F1F907AD06ADAA97E40AC439F83A15B8282CAB6EC4A45165EBBD0A7B5FA731AFFDD9A2591F2F7CEF497E4D4972AB098955C28440DBF08876898A8BCC4ED4F98997DFA776225988CB445E78D0B0A011102B80B09F9F2B3B54551EA5DB6831AD38E71630CBE21E0934FE7EE55DDA5AB81406EB7564DD97270B6F3C7B038CF871EFE78E68C9032800761A9FC9FEC93A8FF092E717DEE5E1C5287DD7E2A8B0C32F41F4CF53F838F6F56189D57488C1A0B69AEC8F621BB4E5D2BE2C78154A4DBC226346BA4C09AD65DEB90BB09F3967ABA1F3485B1DEC2169C7161BAC2D0C14839FEEE3AAA58D95DF75F51A6228F48566C8FFA699D68E8D1";
            keyData_9C_AC = Util.FromHexString(keyDataHex);
            // Console.WriteLine("Server Data:");
            //  Util.ConsoleDumpBuffer(new StreamBuffer(keyData_9C_AC));
            //  Console.WriteLine();

            keyData_D1_E8 = Util.FromHexString(keyDataHex);
            Array.Reverse(keyData_D1_E8);
            //  Console.WriteLine("Reversed:");
            //  Util.ConsoleDumpBuffer(new StreamBuffer(keyData_D1_E8));
            //  Console.WriteLine();

            // keyData_BD_CF = x(keyData_D1_E8, 0x204D276C);
            // Console.WriteLine("Transformed:");
            // Util.ConsoleDumpBuffer(new StreamBuffer(keyData_BD_CF));
            // Console.WriteLine()

            // constant value
            byte[] dst = new byte[0x210];
            dst[0x104] = 1;

            // 
            byte[] src = new byte[0x210];
            for (int i = 0; i < keyData_D1_E8.Length; i++)
            {
                src[i + 4] = keyData_D1_E8[i];
            }

            //  Console.WriteLine($"src:{Environment.NewLine}{Util.HexDump(src)}");

            t4(src, dst); // x=b-a
            //   Console.WriteLine($"t4:{Environment.NewLine}{Util.HexDump(dst)}");

            keyData_2F_17 = new byte[0x100];
            for (int i = 0; i < keyData_2F_17.Length; i++)
            {
                keyData_2F_17[i] = dst[i + 4];
            }

            // Console.WriteLine($"keyData_2F_17:{Environment.NewLine}{Util.HexDump(keyData_2F_17)}");

            dst = new byte[0x210 * 2];
            for (int i = 0; i < keyData_2F_17.Length; i++)
            {
                dst[i] = keyData_2F_17[i];
            }

            //  Console.WriteLine($"dst:{Environment.NewLine}{Util.HexDump(dst)}");

            bb(dst, 0x1); // x*2^y 
            //   Console.WriteLine($"bb:{Environment.NewLine}{Util.HexDump(dst)}");

            // resize? can be ignored?
            keyData_52_2E = new byte[0x100];
            for (int i = 0; i < keyData_52_2E.Length; i++)
            {
                keyData_52_2E[i] = dst[i];
            }

            //  Console.WriteLine($"s4:{Environment.NewLine}{Util.HexDump(keyData_52_2E)}");

            dst = new byte[0x210 * 2];
            for (int i = 0; i < keyData_D1_E8.Length; i++)
            {
                dst[i] = keyData_D1_E8[i];
            }

            bb(dst, 0x1);
            keyData_A2_D1 = new byte[0x100];
            for (int i = 0; i < keyData_A2_D1.Length; i++)
            {
                keyData_A2_D1[i] = dst[i];
            }

            // Console.WriteLine("bb:");
            //  Util.ConsoleDumpBuffer(new StreamBuffer(keyData_A2_D1));
            //   Console.WriteLine();


            // 07599CB8  88 46 47 EB 4C D3 7F 64 B3 42 7A 14 31 8D FA BA  .FGëLÓ.d³Bz.1.úº  
// 013EBA00
        }

        private void transform_client()
        {
            string seedHex = "8A 92 D7 7F 2A C6 57 06 D9 A2 1B 27 F0 92 A0 F7".Replace(" ", "");

            DdoRandom rng = new DdoRandom();
            rng.SetSeed(Util.FromHexString(seedHex));
            string key = rng.GenerateKey();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            //  Util.ConsoleDumpBuffer(new StreamBuffer(keyBytes));
            //  Console.WriteLine($"Key: {key}");
            //  Console.WriteLine();


            int count = 0xCD;
            // 0332DF66 | 0FBE18 | movsx ebx,byte ptr ds:[eax] | proc - 1
            byte[] proc1_in = new byte[]
            {
                0xD7, 0xE4, 0x88, 0xB1
            };
            byte[] proc1_out = x(proc1_in, 0xB188E5D7);
            // 00B010E7 | 8D6424 04 | lea esp,dword ptr ss:[esp+4] | proc 1 - end


            //   Buffer bss = new StreamBuffer();
            //   bss.WriteBytes(c1.ToByteArray());
            //   bss.WriteBytes(new byte[520]);
            //   bss.WriteBytes(c2.ToByteArray());
            //   bss.WriteBytes(new byte[527]);
            //   byte[] test = bss.GetAllBytes();
            // Console.WriteLine($"output:{Environment.NewLine}{Util.HexDump(t.ToByteArray())}");

            // init two big ints
            BigInteger c1 = new BigInteger(0);
            BigInteger c2 = new BigInteger(2);
            byte[] output = new byte[0x210 * 2];
            b(output, 0x02); // set 2nd big int to INT32 value and null
            //   BigInteger k1 = new BigInteger(test.AsSpan(0, 0x210));
            //   BigInteger k2 = new BigInteger(test.AsSpan(0x210, 0x210));
            //   Console.WriteLine($"output:{Environment.NewLine}{Util.HexDump(k2.ToByteArray())}");
            //   Console.WriteLine($"output:{Environment.NewLine}{Util.HexDump(c2.ToByteArray())}");

            BigInteger c3 = c1 + c2;
            ba(output); // addition of two BigInts -> result in first parts
            //   Console.WriteLine($"output:{Environment.NewLine}{Util.HexDump(output)}");


            // 1st Buffer * 2 ^ esp_14 
            BigInteger c4 = c1 * BigInteger.Pow(2, 8);
            // BigInteger c4 = c3 * BigInteger.Pow(2, 8 * 0xCD);
            bb(output, 0x8);
            //   Console.WriteLine($"bb(output, 0x8):{Environment.NewLine}{Util.HexDump(output)}");
            //    Console.WriteLine($"c3 * BigInteger.Pow(c2, 8):{Environment.NewLine}{Util.HexDump(c4.ToByteArray())}");

            byte[] rnd = new byte[0xCD];
            for (int i = 0; i < rnd.Length; i++)
            {
                rnd[i] = (byte) rng.Next();
            }

            BigInteger r = new BigInteger(rnd, true, true);
            BigInteger t = c4 + r;

            BigInteger c1x = c4;
            BigInteger c2x = c3;
            while (count > 0)
            {
                // 040DD0A3 | 0FB6C8 | movzx ecx,al 
                uint eax = rnd[rnd.Length - count]; //rng.Next();
                uint ecx = rnd[rnd.Length - count]; //(byte) eax;
                uint ebp = 0x1;
                if ((byte) eax == 0)
                {
                    // 040DD0A6 | 84C0 | test al,al
                    ecx = ebp;
                }

                eax = ecx;
                uint edx = 0; // 040DD0AE | 99 | cdq
                // eax = 0745F63C
                c2x = c2x.DdoSetInteger(eax);
                b(output, eax); // updatekeyandIV_Step
                c1x = c1x + c2x;
                ba(output);
                c1x = c1x * BigInteger.Pow(2, 8);
                bb(output, 0x8);
                count--;
            }

            //     Console.WriteLine($"while (count > 0):{Environment.NewLine}{Util.HexDump(output)}");
            //    Console.WriteLine($"c1x:{Environment.NewLine}{Util.HexDump(c1x.ToByteArray())}");
            //   Console.WriteLine($"r:{Environment.NewLine}{Util.HexDump(r.ToByteArray())}");
            //   Console.WriteLine($"t:{Environment.NewLine}{Util.HexDump(t.ToByteArray())}");


            // 013EDFF8 | 5D  | pop ebp  

            for (int kindex = 0; kindex < keyBytes.Length; kindex++)
            {
                bb(output, 0x8);
                // todo maybe clear from 0x210 - end ?
                b(output, keyBytes[kindex]);
                ba(output);
            }

            // 007B9EBB | E9 7841C300 | jmp ddo_dump_fix.13EE038 

            // 013EBCBF | F3:A5 | rep movsd | decryption - access 7 (copy) (D1 E8 68 9D)

            // Console.WriteLine("after new key bytes:");
            // Util.ConsoleDumpBuffer(new StreamBuffer(output));
            //  Console.WriteLine();

            // 013EBA34            | 8B043A                | mov eax,dword ptr ds:[edx+edi]  
            // byte[] dst = new byte[0x210];
            // dst[0] = 1;
            // byte[] srcA = new byte[0x210];
            // for (int i = 0; i < keyData_D1_E8.Length; i++)
            // {
            //     srcA[i + 4] = keyData_D1_E8[i];
            // }
            // byte[] srcB = new byte[0x210];
            // combine(srcA, srcB, dst);
            // 
            // Console.WriteLine("output:");
            // Util.DumpBuffer(new StreamBuffer(srcB));
            // Console.WriteLine();
        }

        /// <summary>
        /// 013EBC60 | 53 | push ebx
        /// </summary>
        private void t4(byte[] src, byte[] dst)
        {
            uint eax = 0;
            for (int i = 0; i < 0x84; i++)
            {
                int offset = i * 4;
                uint d = (uint) (dst[offset] | dst[offset + 1] << 8 | dst[offset + 2] << 16 | dst[offset + 3] << 24);
                uint s = (uint) (src[offset] | src[offset + 1] << 8 | src[offset + 2] << 16 | src[offset + 3] << 24);
                uint r = d - s;
                r = r - eax;

                if (d == 1)
                {
                    int sda = 1;
                }

                uint edx = 0;
                if (d == r)
                {
                    edx = eax;
                }

                if (d < r)
                {
                    eax = 1;
                }
                else
                {
                    eax = 0;
                }

                eax = eax | edx;

                dst[offset] = (byte) (r & 0xFF);
                dst[offset + 1] = (byte) (r >> 8 & 0xFF);
                dst[offset + 2] = (byte) (r >> 16 & 0xFF);
                dst[offset + 3] = (byte) (r >> 24 & 0xFF);
            }
        }


        private void combine(byte[] srcA, byte[] srcB, byte[] dst)
        {
            // 013EBA00 | 51 | push ecx 
            uint esi = 0;
            for (uint i = 0; i < 0x84; i++)
            {
                uint offset = i * 4;
                esi = 0;

                uint offset_esi = esi * 4;
                uint s0 = (uint) (srcA[offset] |
                                  srcA[offset + 1] << 8 |
                                  srcA[offset + 2] << 16 |
                                  srcA[offset + 3] << 24
                    );
                uint s1 = (uint) (dst[offset_esi] |
                                  dst[offset_esi + 1] << 8 |
                                  dst[offset_esi + 2] << 16 |
                                  dst[offset_esi + 3] << 24
                    );
                uint s2 = (uint) (srcB[offset] |
                                  srcB[offset + 1] << 8 |
                                  srcB[offset + 2] << 16 |
                                  srcB[offset + 3] << 24
                    );
                uint r = s1 * s0;
                uint r1 = r + s2;
                uint oflow = 0;
                if (Uint32OverFlow(r, s2))
                {
                    // todo calculate overflow value, add CF
                    //  edx += 1;
                }

                uint r2 = r1 + oflow;
                if (Uint32OverFlow(r1, oflow))
                {
                    // todo calculate overflow value, add CF
                    //  edx += 1;
                }

                srcB[offset] = (byte) (r2 & 0xFF);
                srcB[offset + 1] = (byte) (r2 >> 8 & 0xFF);
                srcB[offset + 2] = (byte) (r2 >> 16 & 0xFF);
                srcB[offset + 3] = (byte) (r2 >> 24 & 0xFF);
            }
        }

        private bool Uint32OverFlow(ulong a, ulong b)
        {
            return a + b > uint.MaxValue;
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

        private void b(byte[] output, uint edi)
        {
            uint edx = 0;

            // 013EBD2C | 893E | mov dword ptr ds:[esi],edi 
            uint esi = 0x210;
            output[esi] = (byte) (edi & 0xFF);
            output[esi + 1] = (byte) (edi >> 8 & 0xFF);
            output[esi + 2] = (byte) (edi >> 16 & 0xFF);
            output[esi + 3] = (byte) (edi >> 24 & 0xFF);

            uint eax = edx;
            eax = eax >> 0x1F;

            // 013EBD34 | 8956 04 | mov dword ptr ds:[esi+4],edx
            output[esi + 4] = (byte) (edx & 0xFF);
            output[esi + 4 + 1] = (byte) (edx >> 8 & 0xFF);
            output[esi + 4 + 2] = (byte) (edx >> 16 & 0xFF);
            output[esi + 4 + 3] = (byte) (edx >> 24 & 0xFF);
        }

        /// <summary>
        /// 013EBFB0 | 83EC 08 | sub esp,8 
        /// </summary>
        private void bb(byte[] output, uint esp_14)
        {
            uint offset;
            uint ecx = esp_14;
            uint eax = ecx;
            eax = eax >> 0x5;
            ecx = ecx & 0x1F;
            uint mem_a = eax;
            uint mem_b = ecx;
            uint ebx = 0x84;
            ebx = ebx - eax;
            if (ebx >= 1)
            {
                // 013EBFD5 | 83FB 01 | cmp ebx,1
            }

            uint edx = 0x20;
            edx = edx - ecx;
            ecx = ebx + eax;
            eax = mem_b;
            uint mem_c = edx;
            uint ebp = ecx * 4;
            while (ebx > 1)
            {
                offset = ebx * 4 - 4;
                // 013EBFF0 | 8B749F FC | mov esi,dword ptr ds:[edi+ebx*4-4] 
                uint esi = (uint) (output[offset] |
                                   output[offset + 1] << 8 |
                                   output[offset + 2] << 16 |
                                   output[offset + 3] << 24
                    );
                ebx--;
                ecx = eax;
                esi = esi << (byte) ecx;
                ecx = mem_c;
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

            eax = mem_a;
            ecx = mem_b;
            edx = (uint) (output[0] |
                          output[1] << 8 |
                          output[2] << 16 |
                          output[3] << 24
                );
            edx = edx << (byte) ecx;

            offset = eax * 4;
            output[offset] = (byte) (edx & 0xFF);
            output[offset + 1] = (byte) (edx >> 8 & 0xFF);
            output[offset + 2] = (byte) (edx >> 16 & 0xFF);
            output[offset + 3] = (byte) (edx >> 24 & 0xFF);

            // test eax, eax9
            while (eax > 0)
            {
                eax--;
                offset = eax * 4;
                output[offset] = 0;
                output[offset + 1] = 0;
                output[offset + 2] = 0;
                output[offset + 3] = 0;
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
