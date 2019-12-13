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
            string data2 =
                "0150066b608643985002409e7be9541a39c4658cde84ce0f290dd91ebc1c7d4054864bfcaa80279c06d06b149f350c320029f8329c6e837b07608d4bd7aa13ecea46b77f3673e05da5a4b6877f27922701d333c89f10171c3b5f9482924f38b572ef68a598323f5091d0b1d572678437bd10f1dad2b112decf9c801f193188c0cd7c881918220bd851d8372c73f87b66e78462792e53d2cc5868e768941b6011fe9d65300b8841153afb06e73e9419f057656c9f30e836c17e0489537cdf91d17bf6a8d24e889a6b8c414a456d06410bca38a15aba22109841d523911b716ee7eca07970e1392857dc646d9adfecb26d0f877cd19cd27857aaf098cd73d0cf18597286eb6b06c1e4a9934046e369d8c619d3af94867aa106789d43ef76427dbaaf728d06d668b7e252ca95bfa6f99c5f515d7334218a51a761eb7b25977c574f8ce878daeac7be1945b1c00f954b5cab9810";
            byte[] decrypted = Decrypt(Util.FromHexString(data));


            return CommandResultType.Exit;
        }

        public override string Key => "decrypt";
        public override string Description => "Decrypt packet data";

        private byte[] Decrypt(byte[] input)
        {
            IBuffer buffer = new StreamBuffer(input);
            buffer.SetPositionStart();

            // Read data length
            ushort dataLen = buffer.ReadUInt16();

            // Decrypt Data
            IBuffer output = new StreamBuffer();
            byte[] lastData = init;
            while (buffer.Size > buffer.Position)
            {
                byte[] data = buffer.ReadBytes(16);
                byte[] result = Process16Byte(data, lastData);
                output.WriteBytes(result);
                lastData = data;
            }

            Console.WriteLine("Decrypted:");
            DumpBuffer(output);
            Console.WriteLine();


            IBuffer processedA = Process64(output.Clone(0, 64));
            IBuffer processedB = Process64(output.Clone(64, 64));
            IBuffer processedC = Process64(output.Clone(128, 64));
            IBuffer processedD = Process64(output.Clone(192, 64));
            
            IBuffer lastRow = output.Clone(256, 16);
            lastRow.WriteByte(0x80);
            lastRow.WriteBytes(new byte[15]);
            lastRow.WriteBytes(new byte[16]);
            lastRow.WriteBytes(new byte[14]);
            lastRow.WriteByte(0x08);
            lastRow.WriteByte(0x80);
            IBuffer processedE = Process64(lastRow);

            
            IBuffer processed = new StreamBuffer();
            processed.WriteBuffer(processedA);
            processed.WriteBuffer(processedB);
            processed.WriteBuffer(processedC);
            processed.WriteBuffer(processedD);
            processed.WriteBuffer(processedE);
            Console.WriteLine("Processed:");
            DumpBuffer(processed);
            Console.WriteLine();

            // Parse expected Hash
            IBuffer hashBytes = output.Clone(272, 20);
            byte[] expectedHash = ReadHash(hashBytes.GetAllBytes());
            Console.WriteLine("Expected Hash:");
            DumpBuffer(new StreamBuffer(expectedHash));
            Console.WriteLine();
            
            Unknown();

            // Calculate hash
            DdoHash ddoHash = new DdoHash();
            byte[] calculatedHash = ddoHash.ComputeHash(processed.GetAllBytes());
            Console.WriteLine("Calculated Hash:");
            DumpBuffer(new StreamBuffer(calculatedHash));
            Console.WriteLine();

            return output.GetAllBytes();
        }

        private void Unknown()
        {
            // possible hash init
            uint c1 = 0xD9AAD30C;
            uint c2 = 0x51225B84;
            uint c3 = 0x26552CF3;
            uint c4 = 0xAEDDA47B;
            uint c5 = 0x7D3D11FD;

            uint a = c1 ^ 0xBEEFF00D;
            uint a1 = c2 ^ 0xBEEFF00D;
            uint a3 = c3 ^ 0xBEEFF00D;
            uint a4 = c4 ^ 0xBEEFF00D;
            uint a5 = c5 ^ 0xBEEFF00D;


            int z = 0;
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

        private byte[] Process16Byte(byte[] data, byte[] lastData)
        {
            byte[] memoryIn = new byte[16];
            byte[] memoryOut = new byte[16];

            uint valueA = (uint) (data[3] << 24 | data[2] << 16 | data[1] << 8 | data[0]);
            uint keyA = GetKey(256);
            uint resultA = valueA ^ keyA; // 00B18539 | 3301 | xor eax,dword ptr ds:[ecx]
            memoryIn[0] = (byte) (resultA & 0xFF);
            memoryIn[1] = (byte) (resultA >> 8 & 0xFF);
            memoryIn[2] = (byte) (resultA >> 16 & 0xFF);
            memoryIn[3] = (byte) (resultA >> 24 & 0xFF);

            uint valueB = (uint) (data[7] << 24 | data[6] << 16 | data[5] << 8 | data[4]);
            uint keyB = GetKey(260);
            uint resultB = valueB ^ keyB; // 019927A3 | 3341 04 | xor eax,dword ptr ds:[ecx+4]
            memoryIn[4] = (byte) (resultB & 0xFF);
            memoryIn[5] = (byte) (resultB >> 8 & 0xFF);
            memoryIn[6] = (byte) (resultB >> 16 & 0xFF);
            memoryIn[7] = (byte) (resultB >> 24 & 0xFF);


            uint valueC = (uint) (data[11] << 24 | data[10] << 16 | data[9] << 8 | data[8]);
            uint keyC = GetKey(264);
            uint resultC = valueC ^ keyC; // 019927B7 | 3341 08 | xor eax,dword ptr ds:[ecx+8] 
            memoryIn[8] = (byte) (resultC & 0xFF);
            memoryIn[9] = (byte) (resultC >> 8 & 0xFF);
            memoryIn[10] = (byte) (resultC >> 16 & 0xFF);
            memoryIn[11] = (byte) (resultC >> 24 & 0xFF);

            uint valueD = (uint) (data[15] << 24 | data[14] << 16 | data[13] << 8 | data[12]);
            uint keyD = GetKey(268);
            uint resultD = valueD ^ keyD; // 019927C8 | 3341 0C | xor eax,dword ptr ds:[ecx+C]
            memoryIn[12] = (byte) (resultD & 0xFF);
            memoryIn[13] = (byte) (resultD >> 8 & 0xFF);
            memoryIn[14] = (byte) (resultD >> 16 & 0xFF);
            memoryIn[15] = (byte) (resultD >> 24 & 0xFF);

            uint keyIdx = 248;
            uint count = 3; // 00971F52 | C74424 20 02000000 | mov dword ptr ss:[esp+20],2 // use 3 as test is -1
            uint jmp = 0x19927DD; // 00D8D2E6 | 68 DD279901 | push ddo.19927DD 

            return Loop(memoryIn, memoryOut, jmp, keyIdx, count, lastData);
        }

        private byte[] Loop(byte[] memoryIn, byte[] memoryOut, uint jmp, uint keyIdx, uint count, byte[] lastData)
        {
            byte resultA4 = memoryIn[0]; // 025DBB73 | 0FB60B | movzx ecx,byte ptr ds:[ebx]
            byte keyA4 = CryptoKey[keyIdx]; // 03B52400 | 0FB607 | movzx eax,byte ptr ds:[edi]
            byte xorA4 = (byte) (resultA4 ^ keyA4); // 03B52403 | 33C8 | xor ecx,eax   
            byte keyH4 = CryptoKey[keyIdx + 3]; // 023C7253 | 0FB677 03 | movzx esi,byte ptr ds:[edi+3] 
            byte lookA4 = K_2179DD0[xorA4]; // 023C7257 | 8A81 D09D1702  | mov al,byte ptr ds:[ecx+2179DD0] 

            byte resultA3 = memoryIn[1]; // 034FEB74 | 0FB64B 01 | movzx ecx,byte ptr ds:[ebx+1] 
            byte keyA3 = CryptoKey[keyIdx + 1]; // 0256E9D1 | 0FB647 01 | movzx eax,byte ptr ds:[edi+1] 
            byte xorA3 = (byte) (resultA3 ^ keyA3); //0256E9D5 | 33C8 | xor ecx,eax   
            byte lookA3 = K_2179DD0[xorA3]; // 0256E9D7 | 8A81 D09D1702 | mov al,byte ptr ds:[ecx+2179DD0]     

            byte shRxorAFaRes = (byte) (lookA3 >> 7); //0365AF1F | C0E9 07 | shr cl,7  
            byte andand = (byte) (lookA3 + lookA3); //0365AF22 | 02C0 | add al,al  
            byte xorAFas = (byte) (shRxorAFaRes ^ andand); //0365AF24 | 32C8 | xor cl,al     

            byte resultA2 = memoryIn[2]; // 024B03CD | 0FB64B 02 | movzx ecx,byte ptr ds:[ebx+2]   
            byte keyA2 = CryptoKey[keyIdx + 2]; // 024B03C5 | 0FB647 02 | movzx eax,byte ptr ds:[edi+2]  
            byte xorA2 = (byte) (resultA2 ^ keyA2); //024639F2 | 33C8  | xor ecx,eax     
            byte lookA2 = K_2179DD0[xorA2]; // 024639F4 | 8A81 D09D1702 | mov al,byte ptr ds:[ecx+2179DD0]    

            byte shrA2 = (byte) (lookA2 >> 1); // 024ABFF5 | D0E8 | shr al,1  
            byte shlA2 = (byte) (lookA2 << 7); // 024ABFF7 | C0E1 07 | shl cl,7 
            byte shXorA2 = (byte) (shrA2 ^ shlA2); // 024ABFFA | 32C8 | xor cl,al 

            byte resultA1 = memoryIn[3]; // 03C03A36 | 0FB64B 03 | movzx ecx,byte ptr ds:[ebx+3]   
            byte shrA1 = (byte) (resultA1 >> 7); // 03636888 | C1EA 07 | shr edx,7 


            byte shrKeyH4 = (byte) (keyH4 >> 7); // 0365E78E | C1F8 07 | sar eax,7   
            byte shKeyA1 = (byte) (shrA1 ^ shrKeyH4); // 024B16BC | 33D0 | xor edx,eax  
            ushort andResultA1 = (ushort) (resultA1 + resultA1); // 025F0F2C | 8D0409 | lea eax,dword ptr ds:[ecx+ecx]  

            byte resultB4 = memoryIn[4]; // 025F0F2F | 0FB64B 04 | movzx ecx,byte ptr ds:[ebx+4]   
            ushort andXorA1 = (ushort) (shKeyA1 ^ andResultA1); // 025F0F33 | 33D0 | xor edx,eax
            ushort bAndXorA1 = (ushort) (andXorA1 & 0xFF); // 03AD24DB | 81E2 FF000000 | and edx,FF

            byte andKeyH4 = (byte) (keyH4 & 0x7F); //03AD24E1 | 83E6 7F | and esi,7F
            ushort addKeyH4 = (ushort) (andKeyH4 + andKeyH4); // 03AD24E4 | 03F6 | add esi,esi
            byte bAndXorXorA1 = (byte) (bAndXorA1 ^ addKeyH4); // 03B4C9E4 | 33D6 | xor edx,esi

            byte cryptoLookB2 = CryptoKey[keyIdx + 6]; // 03B4C9E6 | 0FB677 06 | movzx esi,byte ptr ds:[edi+6] 
            byte look1 = K_2179DD0[bAndXorXorA1]; // 02330E88 | 8A82 D09D1702 | mov al,byte ptr ds:[edx+2179DD0]   

            byte cryptoLookB4 = CryptoKey[keyIdx + 4]; //02330E92 | 0FB647 04 | movzx eax,byte ptr ds:[edi+4]
            byte xorB4 = (byte) (resultB4 ^ cryptoLookB4); // 0415AC34 | 33C8   | xor ecx,eax
            byte lookB4 = K_2179DD0[xorB4]; // 025E854F | 8A81 D09D1702 | mov al,byte ptr ds:[ecx+2179DD0]    
            byte shrLookB4 = (byte) (lookB4 >> 7); //025E8557 | C0E9 07 | shr cl,7   
            byte addB4 = (byte) (lookB4 + lookB4); // 025E855A | 02C0 | add al,al
            byte rB4 = (byte) (shrLookB4 ^ addB4); // 035082F2 | 32C8 | xor cl,al 

            byte cryptoLookB3 = CryptoKey[keyIdx + 5]; // 035082F4 | 0FB647 05 | movzx eax,byte ptr ds:[edi+5] 
            byte resultB3 = memoryIn[5]; // 02500A1E | 0FB64B 05 | movzx ecx,byte ptr ds:[ebx+5]  
            byte xorB3 = (byte) (cryptoLookB3 ^ resultB3); // 033DE217 | 33C8 | xor ecx,eax 
            byte lookB3 = K_2179DD0[xorB3]; // 033DE219 | 8A81 D09D1702 | mov al,byte ptr ds:[ecx+2179DD0]
            byte shrB3 = (byte) (lookB3 >> 1); // 033DE221 | D0E8 | shr al,1 
            byte shlB3 = (byte) (lookB3 << 7); // 03B24EAD | C0E1 07 | shl cl,7                             
            byte shXorB3 = (byte) (shrB3 ^ shlB3); // 03B24EB0 | 32C8 | xor cl,al

            byte resultB2 = memoryIn[6]; // 034DE9A8 | 0FB64B 06 | movzx ecx,byte ptr ds:[ebx+6]
            byte shrB2 = (byte) (resultB2 >> 7); // 03D1EEF0 | C1EA 07 | shr edx,7
            byte sarCoLookB2 = (byte) (cryptoLookB2 >> 7); // 03D1EEF5 | C1F8 07 | sar eax,7                
            byte shXorB2 = (byte) (shrB2 ^ sarCoLookB2); // 03EA7024 | 33D0 | xor edx,eax
            ushort addB2 = (ushort) (resultB2 + resultB2); // 03EA7026 | 8D0409 | lea eax,dword ptr ds:[ecx+ecx]

            byte resultB1 = memoryIn[7]; // 033D919F | 0FB64B 07 | movzx ecx,byte ptr ds:[ebx+7]
            ushort shXorB1 = (ushort) (shXorB2 ^ addB2); // 0246C0A8 | 33D0 | xor edx,eax
            byte cryptoLookB1 = CryptoKey[keyIdx + 7]; // 0246C0AA | 0FB647 07 | movzx eax,byte ptr ds:[edi+7]

            byte andCryptoLookB2 = (byte) (cryptoLookB2 & 0x7f); // 0246C0AE | 83E6 7F | and esi,7F 
            byte andAddB2 = (byte) (shXorB1 & 0xFF); // 0246C0B1 | 81E2 FF000000 | and edx,FF  
            ushort addAndCryptoLookB2 = (ushort) (andCryptoLookB2 + andCryptoLookB2); // 034B69DE | 03F6 | add esi,esi
            ushort xorAndAnd = (ushort) (andAddB2 ^ addAndCryptoLookB2); // 034B69E0 | 33D6 | xor edx,esi

            byte lookC4 = K_2179DD0[xorAndAnd]; //024BB6D2 | 8A92 D09D1702 | mov dl,byte ptr ds:[edx+2179DD0]
            byte xorB1 = (byte) (resultB1 ^ cryptoLookB1); //03A1E770 | 33C8 | xor ecx,eax
            byte resultC4 = memoryIn[8]; //034AAC47 | 8A06 | mov al,byte ptr ds:[esi]  
            byte lookB1 = K_2179DD0[xorB1]; //034AAC49 | 8A89 D09D1702 | mov cl,byte ptr ds:[ecx+2179DD0] 

            byte xorc4b1 = (byte) (resultC4 ^ lookB1); // 03A16B82 | 32C1 | xor al,cl
            byte xorc4b1c4 = (byte) (xorc4b1 ^ lookC4); // 03A16B84 | 32C2 | xor al,dl

            byte xor1 = (byte) (shXorB3 ^ xorc4b1c4); // 03A16B8A | 32C3 | xor al,bl
            byte xor2 = (byte) (xor1 ^ look1); // 041313B3 | 32C5 | xor al,ch
            byte xor3 = (byte) (xor2 ^ shXorA2); // 041313B9 | 32C6 | xor al,dh
            byte xor4 = (byte) (xor3 ^ lookA4); // 041313BB | 324424 14 | xor al,byte ptr ss:[esp+14]
            memoryOut[0] = xor4;

            byte xor5 = (byte) (lookB1 ^ lookC4); // 02560FB0 | 32C2 | xor al,dl  
            byte xor6 = (byte) (xor5 ^ rB4); // 02560FB2 | 32C4  | xor al,ah 
            byte xor7 = (byte) (xor6 ^ look1); // 02E93BC0 | 32C5 | xor al,ch
            byte xor8 = (byte) (xor7 ^ xorAFas); // 02E93BC2 | 32C7 | xor al,bh 
            byte xor9 = (byte) (xor8 ^ lookA4); // 02E93BC4 | 324424 14 | xor al,byte ptr ss:[esp+14]
            byte resultC3 = memoryIn[9]; // esi+1
            byte xor10 = (byte) (xor9 ^ resultC3); // 037526A4 | 3046 01 | xor byte ptr ds:[esi+1],al
            memoryOut[1] = xor10;

            byte resultC2 = memoryIn[10]; // 037526A7 | 8A46 02 | mov al,byte ptr ds:[esi+2]
            byte xor11 = (byte) (resultC2 ^ lookB1); // 033D32FD | 32C1 | xor al,cl
            byte xor12 = (byte) (xor11 ^ shXorB3); // 033D32FF | 32C3 | xor al,bl
            byte xor13 = (byte) (xor12 ^ rB4); // 033D3301 | 32C4  xor al,ah
            byte xor14 = (byte) (xor13 ^ shXorA2); // 03D752D0 | 32C6 | xor al,dh
            byte xor15 = (byte) (xor14 ^ xorAFas); // 03D752D2 | 32C7 | xor al,bh
            byte xor16 = (byte) (xor15 ^ lookA4); // 03D752D4 | 324424 10 | xor al,byte ptr ss:[esp+10]
            memoryOut[2] = xor16;

            byte resultC1 = memoryIn[11]; // 0333995C | 8A46 03 | mov al,byte ptr ds:[esi+3]
            byte xor17 = (byte) (resultC1 ^ lookC4); // 0333995F | 32C2 | xor al,dl 
            byte xor18 = (byte) (xor17 ^ shXorB3); // 03339961 | 32C3 | xor al,bl 
            byte xor19 = (byte) (xor18 ^ rB4); //03339963 | 32C4 | xor al,ah
            byte xor20 = (byte) (xor19 ^ look1); // 0331F03C | 32C5 | xor al,ch 
            byte xor21 = (byte) (xor20 ^ shXorA2); // 0331F03E | 32C6 | xor al,dh 
            byte xor22 = (byte) (xor21 ^ xorAFas); // 0331F040 | 32C7 | xor al,bh 
            memoryOut[3] = xor22;

            byte resultD4 = memoryIn[12]; // 03D4A9C1 | 8A46 04 | mov al,byte ptr ds:[esi+4]
            byte xor23 = (byte) (resultD4 ^ lookB1); // 03D8670B | 32C1 | xor al,cl 
            byte xor24 = (byte) (xor23 ^ lookC4); // 03D8670D | 32C2 | xor al,dl
            byte xor25 = (byte) (xor24 ^ shXorB3); // 03D8670F | 32C3 | xor al,bl
            byte xor26 = (byte) (xor25 ^ xorAFas); // 0246C339 | 32C7 | xor al,bh 
            byte xor27 = (byte) (xor26 ^ lookA4); // 03790E3C | 324424 10 | xor al,byte ptr ss:[esp+10]
            memoryOut[4] = xor27;

            byte resultD3 = memoryIn[13]; // 041D53E7 | 8A46 05 | mov al,byte ptr ds:[esi+5]
            byte xor28 = (byte) (resultD3 ^ lookB1); // 041D53EA | 32C1 | xor al,cl
            byte xor29 = (byte) (xor28 ^ lookC4); // 03C01209 | 32C2 | xor al,dl
            byte xor30 = (byte) (xor29 ^ rB4); // 03EA6431 | 32C4 | xor al,ah
            byte xor31 = (byte) (xor30 ^ shXorA2); // 03EA6433 | 32C6 | xor al,dh
            byte xor32 = (byte) (xor31 ^ xorAFas); // 03EA6435 | 32C7 | xor al,bh
            memoryOut[5] = xor32;

            byte resultD2 = memoryIn[14]; // 0265E78B | 8A46 06 | mov al,byte ptr ds:[esi+6]
            byte xor33 = (byte) (resultD2 ^ lookB1); // 0265E78E | 32C1 | xor al,cl
            byte xor34 = (byte) (xor33 ^ shXorB3); //0265E790 | 32C3 | xor al,bl
            byte xor35 = (byte) (xor34 ^ rB4); // 03C00FF7 | 32C4 | xor al,ah
            byte xor36 = (byte) (xor35 ^ look1); // 025CEBB0 | 32C5 | xor al,ch
            byte xor37 = (byte) (xor36 ^ shXorA2); // 025CEBB2 | 32C6 | xor al,dh
            memoryOut[6] = xor37;

            byte resultD1 = memoryIn[15]; // 032D83F3 | 8A46 07 | mov al,byte ptr ds:[esi+7] 
            byte xor38 = (byte) (resultD1 ^ lookC4); // 032D83F6 | 32C2 | xor al,dl
            byte xor39 = (byte) (xor38 ^ shXorB3); // 025C352A | 32C3 | xor al,bl
            byte xor40 = (byte) (xor39 ^ rB4); // 03616B81 | 32C4 | xor al,ah 
            byte xor41 = (byte) (xor40 ^ look1); // 03616B83 | 32C5 | xor al,ch
            byte xor42 = (byte) (xor41 ^ lookA4); // 03616B85 | 324424 10 | xor al,byte ptr ss:[esp+10]
            memoryOut[7] = xor42;

            if (jmp == 0x19927DD)
            {
                memoryOut[8] = memoryIn[0];
                memoryOut[9] = memoryIn[1];
                memoryOut[10] = memoryIn[2];
                memoryOut[11] = memoryIn[3];
                memoryOut[12] = memoryIn[4];
                memoryOut[13] = memoryIn[5];
                memoryOut[14] = memoryIn[6];
                memoryOut[15] = memoryIn[7];
                jmp = 0x19927E7; // 00AC1F06 | 68 E7279901 | push ddo.19927E7 
                keyIdx -= 8;
                Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
            }
            else if (jmp == 0x19927E7)
            {
                count -= 1; //019927EB | 48 | dec eax
                if (count > 0)
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    jmp = 0x19927DD; // 00D8D2E6 | 68 DD279901 | push ddo.19927DD
                    keyIdx -= 8;
                    Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
                }
                else
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    LoopC(memoryOut, memoryIn, 192);

                    // resC == esp+1c // 019929F3 | 897C24 1C | mov dword ptr ss:[esp+1C],edi 

                    count = 3; // 005A8803 | C74424 20 02000000 | mov dword ptr ss:[esp+20],2
                    byte edi = CryptoKey[176]; // 00CE09E5 | 8DB9 B0000000 | lea edi,dword ptr ds:[ecx+B0]  
                    byte eax = CryptoKey[184]; // 01992A11 | 8D47 08 | lea eax,dword ptr ds:[edi+8]   

                    keyIdx = 184;
                    jmp = 0x1992A1D; // 00531E46 | 68 1D2A9901 | push ddo.1992A1D 

                    Loop(memoryIn, memoryOut, jmp, keyIdx, count, lastData);
                }
            }
            else if (jmp == 0x1992A1D)
            {
                memoryOut[8] = memoryIn[0];
                memoryOut[9] = memoryIn[1];
                memoryOut[10] = memoryIn[2];
                memoryOut[11] = memoryIn[3];
                memoryOut[12] = memoryIn[4];
                memoryOut[13] = memoryIn[5];
                memoryOut[14] = memoryIn[6];
                memoryOut[15] = memoryIn[7];
                keyIdx -= 8;

                jmp = 0x1992A27; // 004FD496 | 68 272A9901 | push ddo.1992A27 
                Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
            }
            else if (jmp == 0x1992A27)
            {
                count -= 1; //019927EB | 48 | dec eax
                if (count > 0)
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    jmp = 0x1992A1D; // 00531E46 | 68 1D2A9901 | push ddo.1992A1D 
                    keyIdx -= 8;
                    Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
                }
                else
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    LoopC(memoryOut, memoryIn, 128);

                    count = 3;
                    keyIdx = 120; // 01992C45 | 83C5 70 | add ebp,70 
                    jmp = 0x1992C55; // 00492BE6 | 68 552C9901 | push ddo.1992C55 
                    Loop(memoryIn, memoryOut, jmp, keyIdx, count, lastData);
                }
            }
            else if (jmp == 0x1992C55)
            {
                memoryOut[8] = memoryIn[0];
                memoryOut[9] = memoryIn[1];
                memoryOut[10] = memoryIn[2];
                memoryOut[11] = memoryIn[3];
                memoryOut[12] = memoryIn[4];
                memoryOut[13] = memoryIn[5];
                memoryOut[14] = memoryIn[6];
                memoryOut[15] = memoryIn[7];
                keyIdx -= 8;

                jmp = 0x1992C5F; // 00492BE6 | 68 552C9901 | push ddo.1992C55 
                Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
            }
            else if (jmp == 0x1992C5F)
            {
                count -= 1;
                if (count > 0)
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    jmp = 0x1992C55; // 00492BE6 | 68 552C9901 | push ddo.1992C55
                    keyIdx -= 8;
                    Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
                }
                else
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    LoopC(memoryOut, memoryIn, 64);

                    count = 3;
                    keyIdx = 56; // 01992E51 | 83C0 30 | add eax,30
                    jmp = 0x1992E63; // 0051C636 | 68 632E9901 | push ddo.1992E63
                    Loop(memoryIn, memoryOut, jmp, keyIdx, count, lastData);
                }
            }
            else if (jmp == 0x1992E63)
            {
                memoryOut[8] = memoryIn[0];
                memoryOut[9] = memoryIn[1];
                memoryOut[10] = memoryIn[2];
                memoryOut[11] = memoryIn[3];
                memoryOut[12] = memoryIn[4];
                memoryOut[13] = memoryIn[5];
                memoryOut[14] = memoryIn[6];
                memoryOut[15] = memoryIn[7];
                keyIdx -= 8;

                jmp = 0x1992E6D; // 004B6F86 | 68 6D2E9901 | push ddo.1992E6D
                Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
            }
            else if (jmp == 0x1992E6D)
            {
                count -= 1;
                if (count > 0)
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];
                    jmp = 0x1992E63;
                    keyIdx -= 8;
                    Loop(memoryOut, memoryIn, jmp, keyIdx, count, lastData);
                }
                else
                {
                    memoryOut[8] = memoryIn[0];
                    memoryOut[9] = memoryIn[1];
                    memoryOut[10] = memoryIn[2];
                    memoryOut[11] = memoryIn[3];
                    memoryOut[12] = memoryIn[4];
                    memoryOut[13] = memoryIn[5];
                    memoryOut[14] = memoryIn[6];
                    memoryOut[15] = memoryIn[7];


                    uint resultA =
                        (uint) (memoryOut[11] << 24 | memoryOut[10] << 16 | memoryOut[9] << 8 | memoryOut[8]);
                    uint keyA = (uint) (CryptoKey[3] << 24 | CryptoKey[2] << 16 | CryptoKey[1] << 8 | CryptoKey[0]);
                    uint xorA = resultA ^ keyA;

                    uint resultB = (uint) (memoryOut[15] << 24 | memoryOut[14] << 16 | memoryOut[13] << 8 |
                                           memoryOut[12]);
                    uint keyB = (uint) (CryptoKey[7] << 24 | CryptoKey[6] << 16 | CryptoKey[5] << 8 | CryptoKey[4]);
                    uint xorB = resultB ^ keyB;

                    uint resultC = (uint) (memoryOut[3] << 24 | memoryOut[2] << 16 | memoryOut[1] << 8 | memoryOut[0]);
                    uint keyC = (uint) (CryptoKey[11] << 24 | CryptoKey[10] << 16 | CryptoKey[9] << 8 | CryptoKey[8]);
                    uint xorC = resultC ^ keyC;

                    uint resultD = (uint) (memoryOut[7] << 24 | memoryOut[6] << 16 | memoryOut[5] << 8 | memoryOut[4]);
                    uint keyD = (uint) (CryptoKey[15] << 24 | CryptoKey[14] << 16 | CryptoKey[13] << 8 | CryptoKey[12]);
                    uint xorD = resultD ^ keyD;

                    memoryIn[0] = (byte) (xorA & 0xFF);
                    memoryIn[1] = (byte) (xorA >> 8 & 0xFF);
                    memoryIn[2] = (byte) (xorA >> 16 & 0xFF);
                    memoryIn[3] = (byte) (xorA >> 24 & 0xFF);

                    memoryIn[4] = (byte) (xorB & 0xFF);
                    memoryIn[5] = (byte) (xorB >> 8 & 0xFF);
                    memoryIn[6] = (byte) (xorB >> 16 & 0xFF);
                    memoryIn[7] = (byte) (xorB >> 24 & 0xFF);

                    memoryIn[8] = (byte) (xorC & 0xFF);
                    memoryIn[9] = (byte) (xorC >> 8 & 0xFF);
                    memoryIn[10] = (byte) (xorC >> 16 & 0xFF);
                    memoryIn[11] = (byte) (xorC >> 24 & 0xFF);

                    memoryIn[12] = (byte) (xorD & 0xFF);
                    memoryIn[13] = (byte) (xorD >> 8 & 0xFF);
                    memoryIn[14] = (byte) (xorD >> 16 & 0xFF);
                    memoryIn[15] = (byte) (xorD >> 24 & 0xFF);

                    for (int i = 0; i < 16; i++)
                    {
                        memoryOut[i] = (byte) (memoryIn[i] ^ lastData[i]);
                    }
                }
            }

            return memoryOut;
        }

        private void LoopC(byte[] memoryIn, byte[] memoryOut, uint loKeyIdx)
        {
            uint resultA = (uint) (memoryIn[0] << 24 | memoryIn[1] << 16 | memoryIn[2] << 8 | memoryIn[3]);
            uint resultB = (uint) (memoryIn[4] << 24 | memoryIn[5] << 16 | memoryIn[6] << 8 | memoryIn[7]);
            uint resultC = (uint) (memoryIn[8] << 24 | memoryIn[9] << 16 | memoryIn[10] << 8 | memoryIn[11]);
            uint resultD = (uint) (memoryIn[12] << 24 | memoryIn[13] << 16 | memoryIn[14] << 8 | memoryIn[15]);


            uint keyA = (uint) (CryptoKey[loKeyIdx + 8] << 24 | CryptoKey[loKeyIdx + 9] << 16 |
                                CryptoKey[loKeyIdx + 10] << 8 | CryptoKey[loKeyIdx + 11]);
            uint keyB = (uint) (CryptoKey[loKeyIdx + 12] << 24 | CryptoKey[loKeyIdx + 13] << 16 |
                                CryptoKey[loKeyIdx + 14] << 8 | CryptoKey[loKeyIdx + 15]);
            uint keyC = (uint) (CryptoKey[loKeyIdx] << 24 | CryptoKey[loKeyIdx + 1] << 16 |
                                CryptoKey[loKeyIdx + 2] << 8 | CryptoKey[loKeyIdx + 3]);
            uint keyD = (uint) (CryptoKey[loKeyIdx + 4] << 24 | CryptoKey[loKeyIdx + 5] << 16 |
                                CryptoKey[loKeyIdx + 6] << 8 | CryptoKey[loKeyIdx + 7]);

            uint resA = resultA >> 0x1F;
            uint resD = resultD | keyD;
            uint rkA = keyA & resultA;
            uint shrKeyA = keyA >> 0x1F; // 01992942 | C1E9 1F | shr ecx,1F 
            uint rkAs = shrKeyA & resA; // 01992945 | 23C8 | and ecx,eax  
            uint rka2 = rkA + rkA; // 0199294B | 03ED | add ebp,ebp
            uint xrka2 = rkAs ^ rka2; // 0199294D | 33CD | xor ecx,ebp
            uint xrka3 = xrka2 ^ resultB; // 0199294F | 33C1 | xor eax,ecx
            uint rkak4 = keyB | xrka3; // 01992951 | 0BF8 | or edi,eax
            uint resiltA1 = resultA ^ rkak4; // 01992953 | 317C24 20 | xor dword ptr ss:[esp+20],edi 

            uint resC = resultC ^ resD; //0199295F | 33FA | xor edi,edx 
            uint keyC1 = keyC >> 0x1F; //01992967 | C1E9 1F | shr ecx,1F 
            uint keyC2 = resC >> 0x1F; //0199296C | C1E8 1F | shr eax,1F 
            uint andKeyC = keyC1 & keyC2; // 0199296F | 23C8 | and ecx,eax 
            uint keyCa = keyC & resC; //01992971 | 23DF | and ebx,edi 
            uint keyCa2 = keyCa + keyCa; // 01992973 | 03DB | add ebx,ebx 
            uint keyCa3 = andKeyC ^ keyCa2; // 01992975 | 33CB  | xor ecx,ebx 
            uint keyCa4 = resultD ^ keyCa3; // 01992977 | 33D1 | xor edx,ecx 

            uint res3 = resiltA1 >> 0x18; // 0199297F | C1E8 18 | shr eax,18
            memoryOut[0] = (byte) res3; // 01992982 | 8806 | mov byte ptr ds:[esi],al
            uint res4 = resiltA1 >> 0x10; // 01992986 | C1E8 10 | shr eax,10
            memoryOut[1] = (byte) res4; // 01992989 | 8846 01 | mov byte ptr ds:[esi+1],al
            uint res5 = resiltA1 >> 0x8; // 0199298E | C1E8 08 | shr eax,8
            memoryOut[2] = (byte) res5; // 01992991 | 8846 02 | mov byte ptr ds:[esi+2],al
            memoryOut[3] = (byte) resiltA1; // 01992994 | 884E 03 | mov byte ptr ds:[esi+3],cl

            uint re4 = xrka3 >> 0x18;
            memoryOut[4] = (byte) re4;
            uint re5 = xrka3 >> 0x10;
            memoryOut[5] = (byte) re5;
            uint re6 = xrka3 >> 0x8;
            memoryOut[6] = (byte) re6;
            memoryOut[7] = (byte) xrka3;

            uint re7 = resC >> 0x18;
            memoryOut[8] = (byte) re7;
            uint re8 = resC >> 0x10;
            memoryOut[9] = (byte) re8;
            uint re9 = resC >> 0x8;
            memoryOut[10] = (byte) re9;
            memoryOut[11] = (byte) resC;

            uint re10 = keyCa4 >> 0x18;
            memoryOut[12] = (byte) re10;
            uint re11 = keyCa4 >> 0x10;
            memoryOut[13] = (byte) re11;
            uint re12 = keyCa4 >> 0x8;
            memoryOut[14] = (byte) re12;
            memoryOut[15] = (byte) keyCa4;
        }

        private static uint GetKey(uint index)
        {
            return (uint) (CryptoKey[index + 3] << 24 | CryptoKey[index + 2] << 16 | CryptoKey[index + 1] << 8 |
                           CryptoKey[index + 0]);
        }


        private static byte[] init = new byte[]
            {0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41};

        private static byte[] CryptoKey = new byte[]
        {
            0x66, 0x32, 0x33, 0x65, 0x39, 0x38, 0x48, 0x61, 0x66, 0x4A, 0x64, 0x53, 0x6F, 0x61, 0x6A, 0x38,
            0x89, 0xE6, 0x62, 0xC9, 0xB2, 0x80, 0x3F, 0xDB, 0x3F, 0x43, 0xD1, 0x08, 0x03, 0x44, 0xFC, 0x27,
            0xA1, 0x35, 0x34, 0x34, 0x19, 0x19, 0xB7, 0xB0, 0xB5, 0x33, 0xB5, 0xB6, 0x29, 0xB0, 0x98, 0x28,
            0xEA, 0x98, 0x5C, 0x73, 0x77, 0xA0, 0x1C, 0xCF, 0xB9, 0xD8, 0xE9, 0xAF, 0xB9, 0x54, 0xE2, 0xA3,
            0x9A, 0x1A, 0x0C, 0x8C, 0xDB, 0xD8, 0x5A, 0x99, 0xDA, 0xDB, 0x14, 0xD8, 0x4C, 0x14, 0x50, 0x9A,
            0x6C, 0xA0, 0x0F, 0xF6, 0xCF, 0xD0, 0xF4, 0x42, 0x00, 0xD1, 0x3F, 0x09, 0xE2, 0x79, 0x98, 0xB2,
            0x09, 0x0C, 0x2C, 0xC9, 0x4C, 0x8A, 0x6D, 0xEC, 0x2D, 0x47, 0x0C, 0xC6, 0x46, 0x6C, 0xA7, 0x27,
            0xDD, 0xE8, 0x07, 0x33, 0xEE, 0x76, 0x3A, 0x6B, 0xEE, 0x55, 0x38, 0xA8, 0xFA, 0xA6, 0x17, 0x1C,
            0x16, 0x64, 0xA6, 0x45, 0x36, 0xF6, 0x16, 0xA3, 0x86, 0x63, 0x23, 0x36, 0x53, 0x93, 0x84, 0x86,
            0x36, 0xF6, 0x16, 0xA6, 0x76, 0xB6, 0xC5, 0x36, 0x13, 0x05, 0x14, 0x26, 0xA6, 0x86, 0x83, 0x23,
            0xB3, 0xF4, 0x3D, 0x10, 0x80, 0x34, 0x4F, 0xC2, 0x78, 0x9E, 0x66, 0x2C, 0x9B, 0x28, 0x03, 0xFD,
            0x4C, 0x8A, 0x6D, 0xEC, 0x2D, 0x47, 0x0C, 0xC6, 0x46, 0x6C, 0xA7, 0x27, 0x09, 0x0C, 0x2C, 0xC9,
            0xEE, 0x76, 0x3A, 0x6B, 0xEE, 0x55, 0x38, 0xA8, 0xFA, 0xA6, 0x17, 0x1C, 0xDD, 0xE8, 0x07, 0x33,
            0xDA, 0xDB, 0x14, 0xD8, 0x4C, 0x14, 0x50, 0x9A, 0x9A, 0x1A, 0x0C, 0x8C, 0xDB, 0xD8, 0x5A, 0x99,
            0x74, 0xD7, 0xDC, 0xAA, 0x71, 0x51, 0xF5, 0x4C, 0x2E, 0x39, 0xBB, 0xD0, 0x0E, 0x67, 0xDC, 0xEC,
            0xB5, 0x1C, 0x33, 0x19, 0x19, 0xB2, 0x9C, 0x9C, 0x24, 0x30, 0xB3, 0x25, 0x32, 0x29, 0xB7, 0xB0,
            0x7E, 0x13, 0xC4, 0xF3, 0x31, 0x64, 0xD9, 0x40, 0x1F, 0xED, 0x9F, 0xA1, 0xE8, 0x84, 0x01, 0xA2,
        };

        private static byte[] K_2179DD0 = new byte[]
        {
            0x70, 0x82, 0x2C, 0xEC, 0xB3, 0x27, 0xC0, 0xE5, 0xE4, 0x85, 0x57, 0x35, 0xEA, 0x0C, 0xAE, 0x41,
            0x23, 0xEF, 0x6B, 0x93, 0x45, 0x19, 0xA5, 0x21, 0xED, 0x0E, 0x4F, 0x4E, 0x1D, 0x65, 0x92, 0xBD,
            0x86, 0xB8, 0xAF, 0x8F, 0x7C, 0xEB, 0x1F, 0xCE, 0x3E, 0x30, 0xDC, 0x5F, 0x5E, 0xC5, 0x0B, 0x1A,
            0xA6, 0xE1, 0x39, 0xCA, 0xD5, 0x47, 0x5D, 0x3D, 0xD9, 0x01, 0x5A, 0xD6, 0x51, 0x56, 0x6C, 0x4D,
            0x8B, 0x0D, 0x9A, 0x66, 0xFB, 0xCC, 0xB0, 0x2D, 0x74, 0x12, 0x2B, 0x20, 0xF0, 0xB1, 0x84, 0x99,
            0xDF, 0x4C, 0xCB, 0xC2, 0x34, 0x7E, 0x76, 0x05, 0x6D, 0xB7, 0xA9, 0x31, 0xD1, 0x17, 0x04, 0xD7,
            0x14, 0x58, 0x3A, 0x61, 0xDE, 0x1B, 0x11, 0x1C, 0x32, 0x0F, 0x9C, 0x16, 0x53, 0x18, 0xF2, 0x22,
            0xFE, 0x44, 0xCF, 0xB2, 0xC3, 0xB5, 0x7A, 0x91, 0x24, 0x08, 0xE8, 0xA8, 0x60, 0xFC, 0x69, 0x50,
            0xAA, 0xD0, 0xA0, 0x7D, 0xA1, 0x89, 0x62, 0x97, 0x54, 0x5B, 0x1E, 0x95, 0xE0, 0xFF, 0x64, 0xD2,
            0x10, 0xC4, 0x00, 0x48, 0xA3, 0xF7, 0x75, 0xDB, 0x8A, 0x03, 0xE6, 0xDA, 0x09, 0x3F, 0xDD, 0x94,
            0x87, 0x5C, 0x83, 0x02, 0xCD, 0x4A, 0x90, 0x33, 0x73, 0x67, 0xF6, 0xF3, 0x9D, 0x7F, 0xBF, 0xE2,
            0x52, 0x9B, 0xD8, 0x26, 0xC8, 0x37, 0xC6, 0x3B, 0x81, 0x96, 0x6F, 0x4B, 0x13, 0xBE, 0x63, 0x2E,
            0xE9, 0x79, 0xA7, 0x8C, 0x9F, 0x6E, 0xBC, 0x8E, 0x29, 0xF5, 0xF9, 0xB6, 0x2F, 0xFD, 0xB4, 0x59,
            0x78, 0x98, 0x06, 0x6A, 0xE7, 0x46, 0x71, 0xBA, 0xD4, 0x25, 0xAB, 0x42, 0x88, 0xA2, 0x8D, 0xFA,
            0x72, 0x07, 0xB9, 0x55, 0xF8, 0xEE, 0xAC, 0x0A, 0x36, 0x49, 0x2A, 0x68, 0x3C, 0x38, 0xF1, 0xA4,
            0x40, 0x28, 0xD3, 0x7B, 0xBB, 0xC9, 0x43, 0xC1, 0x15, 0xE3, 0xAD, 0xF4, 0x77, 0xC7, 0x80, 0x9E
        };

        public static uint RotateLeft(uint x, int n)
        {
            return (((x) << (n)) | ((x) >> (32 - (n))));
        }

        public static uint RotateRight(uint x, int n)
        {
            return (x >> n) | (x << (32 - n));
        }
    }
}
