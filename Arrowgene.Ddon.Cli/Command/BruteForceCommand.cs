using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using Arrowgene.Ddon.PacketLibrary;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;

namespace Arrowgene.Ddon.Cli.Command
{
    public class BruteForceCommand : ICommand
    {
        public string Key => "bf";

        public string Description => "Brute Force";

        private const int KeyLength = 32;
        private const int BlockSize = 16;
        private const int KeyLengthBits = KeyLength * 8;

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        private static readonly byte[] ExpectedLogin = new byte[]
        {
            0x01, 0x00, 0x00, 0x02, 0x34
        };

        private static readonly byte[] ExpectedGame = new byte[]
        {
            0x2C, 0x00, 0x00, 0x02, 0x34
        };

        private static readonly Camellia Camellia = new Camellia();

        private class BfMatch
        {
            public string Key;
            public uint Ms;
            public uint Depth;
            public byte[] BytesToCrack;
        }

        private BlockingCollection<BfMatch> _matches;

        public CommandResultType Run(CommandParameter parameter)
        {
            DdonRandom rng = new DdonRandom();
            rng.SetSeed(0x30F559EF,0x098DA670,0x10EEA283,0x49139877);
            uint a = rng.Next();
            uint b = rng.Next();
            uint c = rng.Next();
            uint e = rng.Next();
            
            
            _cts = new CancellationTokenSource();
            _matches = new BlockingCollection<BfMatch>();
            BruteForceJson(
                "C:\\Users\\railgun\\Downloads\\stream13-charcreation-marked.pcapng_3_login.json",
                8 * 100,
                0x520
            );

            return CommandResultType.Continue;
        }

        private void BruteForceJson(string jsonPath, int threadCount, int ms = -1)
        {
        //    String json = File.ReadAllText(jsonPath);
        //    PlPacketStream packetStream = JsonSerializer.Deserialize<PlPacketStream>(json);
        //    PlPacket packet = packetStream.Packets[2];
        //    if (packet.Data.Length != 98)
        //    {
        //        Console.WriteLine($"packet.Data.Length != 98 but:{packet.Data.Length}");
        //        return;
        //    }

          //  byte[] dataToCrack = packet.Data.AsSpan(2, 16).ToArray();
          byte[] dataToCrack = Util.FromHexString("AAD15D29238D9AC6CF8C696C99D3C781");
          
            Console.WriteLine($"DataToCrack:{Util.ToHexString(dataToCrack)}");
            byte[] expected;
            List<Thread> threads = new List<Thread>();
          //  if (packetStream.ServerType == "login")
          //  {
                expected = ExpectedLogin;
          // }
          // else
          // {
          //     expected = ExpectedGame;
          // }

            if (ms <= -1)
            {
               // uint min_ms = 0;
                uint startMs = 0;
                uint endMs = 10000;

                uint diff = endMs - startMs;
                Console.WriteLine(diff);

                // need to crack by finding MS
                uint depth = 15000;
                // uint startMs = 2000;
                // uint endMs = 10000;
                uint msStep = (uint) (endMs / threadCount);
                uint msOffset = startMs;
                for (int i = 0; i < threadCount; i++)
                {
                    uint start = msOffset;
                    uint end = start + msStep - 1;
                    Thread t = new Thread(() =>
                    {
                        CrackMs(
                            dataToCrack,
                            expected,
                            depth,
                            start,
                            end
                        );
                    });
                    threads.Add(t);
                    msOffset += msStep;
                }
            }
            else
            {
                UnlimitedDepth((uint) ms, threadCount, dataToCrack, expected);
                // crack by depth
                //  uint depth = 1000230900;
                //   int depthOffset = 0;
                //   int depthStep = (int) depth / threadCount;
                //   Span<byte> keyBuffer = GenerateKeyBuffer((uint) ms, depth);
                //   for (int i = 0; i < threadCount; i++)
                //   {
                //       Thread t = new Thread(() =>
                //       {
                //           byte[] keyBuffer = _keyBuffers.Take();
                //           Test((uint) ms, depth, keyBuffer, dataToCrack, expected);
                //       });
                //       threads.Add(t);
                //   }
            }

            int tCount = 0;
            foreach (Thread t in threads)
            {
                t.Name = "Thread-" + tCount;
                t.Start();
                tCount++;
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            foreach (BfMatch m in _matches)
            {
                Console.WriteLine($"K:{m.Key} Ms:{m.Ms} Depth:{m.Depth} Bytes:{Util.ToHexString(m.BytesToCrack)}");
            }

            Console.WriteLine($"Done");
        }

        private void CrackMs(byte[] dataToCrack, byte[] expected, uint depth, uint startMs, uint endMs)
        {
            for (uint ms = startMs; ms < endMs; ms++)
            {
                if (_matches.Count > 0)
                {
                    return;
                }

                Span<byte> keyBuffer = GenerateKeyBuffer(ms, depth);
                CrackDepth(dataToCrack, expected, keyBuffer, ms, depth);
            }
        }

        private void CrackDepth(byte[] dataToCrack, byte[] expected, Span<byte> keyBuffer, uint ms, uint depth)
        {
            Test(ms, depth, keyBuffer, dataToCrack, expected);
        }

        private void Test(uint ms, uint depthOffset, Span<byte> keyBuffer, byte[] data, byte[] expected)
        {
            Span<byte> t8 = new byte[8];
            Span<byte> output = new byte[BlockSize];
            for (int depth = 0; depth < keyBuffer.Length - KeyLength; depth++)
            {
                if (_matches.Count > 0)
                {
                    return;
                }

                Camellia.KeySchedule(
                    keyBuffer.Slice(depth, KeyLength),
                    out Memory<byte> subKey,
                    t8
                );
                Camellia.CryptBlock(
                    true,
                    KeyLengthBits,
                    data,
                    subKey.Span,
                    output,
                    t8
                );
                for (int i = 0; i < BlockSize; i++)
                {
                    output[i] = (byte) (output[i] ^ CamelliaIv[i]);
                }

                if (
                    output[0] == expected[0] &&
                    output[1] == expected[1] &&
                    output[2] == expected[2] &&
                    output[3] == expected[3] &&
                    output[4] == expected[4]
                )
                {
                    BfMatch match = new BfMatch();
                    match.Key = Encoding.UTF8.GetString(keyBuffer.Slice(depth, KeyLength));
                    match.Depth = (uint) (depthOffset + depth);
                    match.Ms = ms;
                    match.BytesToCrack = data;
                    _matches.Add(match);
                    _cts.Cancel();
                }
            }
        }

        private void TestCrypt(uint ms,
            uint depth,
            Span<byte> keyBuffer,
            Span<byte> data,
            byte[] expected,
            Span<byte> output,
            Span<byte> t8
        )
        {
            Camellia.KeySchedule(keyBuffer, out Memory<byte> subKey, t8);
            Camellia.CryptBlock(
                true,
                KeyLengthBits,
                data,
                subKey.Span,
                output,
                t8
            );
            for (int i = 0; i < BlockSize; i++)
            {
                output[i] = (byte) (output[i] ^ CamelliaIv[i]);
            }

            if (
                output[0] == expected[0] &&
                output[1] == expected[1] &&
                output[2] == expected[2] &&
                output[3] == expected[3] &&
                output[4] == expected[4]
            )
            {
                BfMatch match = new BfMatch();
                match.Key = Encoding.UTF8.GetString(keyBuffer);
                match.Depth = depth;
                match.Ms = ms;
                match.BytesToCrack = data.ToArray();
                _matches.Add(match);
                _cts.Cancel();
            }
        }


        public struct KBuff
        {
            public byte[] Key;
            public uint Depth;
        }

        private BlockingCollection<KBuff> _keyBuffers;
        private CancellationTokenSource _cts;

        private void UnlimitedDepth(uint ms, int threadCount, byte[] dataToCrack, byte[] expected)
        {
            _keyBuffers = new BlockingCollection<KBuff>(100000);
            _cts = new CancellationTokenSource();
            List<Thread> threads = new List<Thread>();
            DateTime startTime = DateTime.Now;
            ;

            Thread keyThread = new Thread(() => { PopulateKeyBuffers(ms); });
            threads.Add(keyThread);
            keyThread.Name = "KeyThread";
            keyThread.Start();

            for (int i = 0; i < threadCount; i++)
            {
                Thread t = new Thread(() =>
                {
                    Span<byte> t8 = new byte[8];
                    Span<byte> output = new byte[BlockSize];

                    while (!_cts.IsCancellationRequested)
                    {
                        KBuff kBuff;
                        try
                        {
                            kBuff = _keyBuffers.Take(_cts.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            continue;
                        }

                        TestCrypt(ms, kBuff.Depth, kBuff.Key, dataToCrack, expected, output, t8);
                        if (kBuff.Depth % 10000 == 0)
                        {
                            TimeSpan duration = DateTime.Now - startTime;
                            Console.WriteLine($"At Depth:{kBuff.Depth} Time:{duration}");
                        }
                    }
                });
                threads.Add(t);
                t.Name = "Bruter_" + i;
                t.Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }
        }

        private void PopulateKeyBuffers(uint ms)
        {
            DdonRandom rng = new DdonRandom();
            rng.SetSeed(ms);
            Span<byte> keyBuffer = new byte[KeyLength];
            uint depth = 0;
            for (int i = 0; i < KeyLength; i++)
            {
                keyBuffer[i] = rng.NextKeyByte();
            }

            _keyBuffers.Add(new KBuff()
            {
                Key = keyBuffer.ToArray(),
                Depth = depth
            }, _cts.Token);

            while (!_cts.IsCancellationRequested)
            {
                depth++;
                for (int i = 0; i < KeyLength - 1; i++)
                {
                    keyBuffer[i] = keyBuffer[i + 1];
                }

                keyBuffer[KeyLength - 1] = rng.NextKeyByte();
                KBuff kBuff = new KBuff()
                {
                    Key = keyBuffer.ToArray(),
                    Depth = depth
                };
                try
                {
                    _keyBuffers.Add(kBuff, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // ignore
                }
            }
        }

        private Span<byte> GenerateKeyBuffer(uint ms, uint depth)
        {
            DdonRandom rng = new DdonRandom();
            rng.SetSeed(ms);
            Span<byte> keyBuffer = new byte[depth];
            for (int i = 0; i < depth; i++)
            {
                keyBuffer[i] = rng.NextKeyByte();
            }

            return keyBuffer;
        }
        
        
        private void SkipDepthBrute(uint ms, int threadCount, byte[] dataToCrack, byte[] expected)
        {
            DdonRandom rng = new DdonRandom();
            rng.SetSeed(ms);
            Span<byte> keyBuffer = new byte[KeyLength];
            uint depth = 0;
            for (int i = 0; i < KeyLength; i++)
            {
                keyBuffer[i] = rng.NextKeyByte();
            }

            _keyBuffers.Add(new KBuff()
            {
                Key = keyBuffer.ToArray(),
                Depth = depth
            }, _cts.Token);

            while (!_cts.IsCancellationRequested)
            {
                depth++;
                for (int i = 0; i < KeyLength - 1; i++)
                {
                    keyBuffer[i] = keyBuffer[i + 1];
                }

                keyBuffer[KeyLength - 1] = rng.NextKeyByte();
                KBuff kBuff = new KBuff()
                {
                    Key = keyBuffer.ToArray(),
                    Depth = depth
                };
                try
                {
                    _keyBuffers.Add(kBuff, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // ignore
                }
            }
        }

        public void Shutdown()
        {
        }
    }
}
