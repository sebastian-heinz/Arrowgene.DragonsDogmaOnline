using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;

namespace Arrowgene.Ddon.Cli.Command
{
    public class BruteForceCommand : ICommand
    {
        public string Key => "bf";

        public string Description =>
            $"Brute Force";

        private const int KeyLength = 32;
        private const int BlockSize = 16;
        private const int KeyLengthBits = KeyLength * 8;

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        private static byte[] Data;

        public CommandResultType Run(CommandParameter parameter)
        {
            string dataStr = parameter.Arguments[0];

            Data = Util.FromHexString(dataStr);
            int depth = 1024 * 100;
            uint start_s = 0;
            uint end_s = 60;
            uint start_ms = start_s * 1000;
            uint end_ms = end_s * 1000;
            
            
   

            List<Thread> threads = new List<Thread>();
            for (uint ms = start_ms; ms < end_ms; ms++)
            {
                Thread t = new Thread(() => { ProcessMs(ms, depth); });
                threads.Add(t);
                t.Priority = ThreadPriority.AboveNormal;
                t.Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            return CommandResultType.Continue;
        }

        private void ProcessMs(uint ms, int depth)
        {
            Console.WriteLine($"Ms{ms} start");
            DdonRandom rng = new DdonRandom();
            rng.SetSeed(ms);
            Span<byte> keyBuffer = new byte[depth];
            for (int i = 0; i < depth; i++)
            {
                keyBuffer[i] = rng.NextKeyByte();
            }

            Camellia c = new Camellia();
            byte[] output = new byte[BlockSize];
            for (int j = 0; j < keyBuffer.Length - KeyLength; j++)
            {
                c.KeySchedule( keyBuffer.Slice(j, KeyLength), out Memory<byte> subKey);
                c.CryptBlock(
                    true,
                    KeyLength * 8,
                    Data,
                    subKey.Span,
                    output
                );
                for (int i = 0; i < BlockSize; i++)
                {
                    output[i] = (byte) (output[i] ^ CamelliaIv[i]);
                }

                if (
                    output[0] == 0x00 &&
                    output[1] == 0x00
                    && output[2] == 0x05
                    && output[3] == 0x01
                    //  && output[4] == 0x34
                )
                {
                    try
                    {
                        string foundKey = Encoding.UTF8.GetString(keyBuffer.Slice(j, KeyLength));
                        string base64 = Convert.ToBase64String(output);
                        string outputDump = Util.HexDump(output);
                        Console.WriteLine($"Key:{foundKey}{Environment.NewLine}{Util.HexDump(output)}");
                        File.WriteAllText("F://key_" + ms + "_" + base64 + ".txt", foundKey + Environment.NewLine + outputDump);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            Console.WriteLine($"Ms{ms} done");
        }


        public void Shutdown()
        {
        }
    }
}
