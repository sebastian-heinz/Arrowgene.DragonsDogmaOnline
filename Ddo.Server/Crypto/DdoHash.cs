using System;
using System.Security.Cryptography;

namespace Ddo.Server.Crypto
{
    public sealed class DdoHash : HashAlgorithm
    {
        public const string HashAlgorithmName = "DdoHash";

        public static void Register()
        {
            CryptoConfig.AddAlgorithm(typeof(DdoHash), HashAlgorithmName);
        }

        private const uint InitA = 0xEFCDAB89;
        private const uint InitB = 0x10325476;
        private const uint InitC = 0x98BADCFE;
        private const uint InitD = 0x67452301;
        private const uint InitE = 0xC3D2E1F0;
        private const uint ConstantA = 0x512F8357;
        private const uint ConstantB = 0x6574116F;
        private const uint ConstantC = 0x84B64612;
        private const uint ConstantE = 0xC1CF3B18;

        private const int CycleSize = 80;

        private byte[] _memory;
        private uint _memoryA;
        private uint _memoryB;
        private uint _memoryC;
        private uint _memoryD;
        private uint _memoryE;
        private uint _prevMemoryA;
        private uint _prevMemoryB;
        private uint _prevMemoryC;
        private uint _prevMemoryD;
        private uint _prevMemoryE;

        public DdoHash()
        {
            Initialize();
        }

        public override int HashSize => 160;
        public int HashSizeBytes => HashSize / 8;

        public override void Initialize()
        {
            _memory = null;
            _prevMemoryA = _memoryA = InitA;
            _prevMemoryB = _memoryB = InitB;
            _prevMemoryC = _memoryC = InitC;
            _prevMemoryD = _memoryD = InitD;
            _prevMemoryE = _memoryE = InitE;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            if (_memory == null)
            {
                _memory = array;
            }
            else
            {
                _memory = ExtendBuffer(_memory, array);
            }

            if (_memory.Length < CycleSize)
            {
                // Not enough data to complete cycle
                return;
            }

            int index = 0;
            int memorySize = _memory.Length;
            while (index < memorySize)
            {
                int cycle = 0;
                while (cycle < CycleSize)
                {
                    uint eax;
                    uint ecx;
                    if (cycle > 0x3b)
                    {
                        uint tmp = _memoryB ^ _memoryC;
                        tmp = tmp ^ _memoryA;
                        eax = tmp;
                        ecx = ConstantE;
                    }
                    else if (cycle > 0x27)
                    {
                        uint tmp = _memoryC | _memoryA;
                        tmp = _memoryB & tmp;
                        tmp = tmp | (_memoryC & _memoryA);
                        eax = tmp;
                        ecx = ConstantC;
                    }
                    else if (cycle > 0x13)
                    {
                        uint tmp = _memoryB ^ _memoryC;
                        tmp = tmp ^ _memoryA;
                        eax = tmp;
                        ecx = ConstantB;
                    }
                    else
                    {
                        uint tmp = ~_memoryA;
                        tmp = tmp & _memoryB;
                        tmp = (_memoryA & _memoryC) | tmp;
                        eax = tmp;
                        ecx = ConstantA;
                    }

                    uint valA = RotateLeft(_memoryD, 5);
                    uint valB = ReadUInt(_memory, index, true);
                    uint resultC = RotateRight(_memoryA, 2);
                    uint resultD = _memoryE + eax + valA + valB + (ecx ^ 0xBADFACE);
                    _memoryE = _memoryB;
                    _memoryA = _memoryD;
                    _memoryB = _memoryC;
                    _memoryC = resultC;
                    _memoryD = resultD;
                    index += 4;
                    cycle++;
                    if (cycle == CycleSize)
                    {
                        break;
                    }
                }

                _memoryA = _prevMemoryA + _memoryA;
                _memoryB = _prevMemoryB + _memoryB;
                _memoryC = _prevMemoryC + _memoryC;
                _memoryD = _prevMemoryD + _memoryD;
                _memoryE = _prevMemoryE + _memoryE;

                _prevMemoryA = _memoryA;
                _prevMemoryB = _memoryB;
                _prevMemoryC = _memoryC;
                _prevMemoryD = _memoryD;
                _prevMemoryE = _memoryE;
            }

            _memory = CutBuffer(_memory, 0, CycleSize);
        }

        protected override byte[] HashFinal()
        {
            byte[] result = new byte[HashSizeBytes];
            WriteUInt(_memoryD, result, 0, true);
            WriteUInt(_memoryA, result, 4, true);
            WriteUInt(_memoryC, result, 8, true);
            WriteUInt(_memoryB, result, 12, true);
            WriteUInt(_memoryE, result, 16, true);
            return result;
        }

        private uint RotateLeft(uint x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }

        private uint RotateRight(uint x, int n)
        {
            return (x >> n) | (x << (32 - n));
        }

        private uint ReadUInt(byte[] data, int offset, bool isLittleEndian)
        {
            return (uint) ReadInt(data, offset, isLittleEndian);
        }

        private int ReadInt(byte[] data, int offset, bool isLittleEndian)
        {
            if (isLittleEndian)
            {
                return data[offset] | (data[offset + 1] << 8) | (data[offset + 2] << 16) | (data[offset + 3] << 24);
            }

            return (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
        }

        private void WriteUInt(uint number, byte[] data, int offset, bool isLittleEndian)
        {
            if (isLittleEndian)
            {
                data[offset] = (byte) (number & 0xFF);
                data[offset + 1] = (byte) (number >> 8 & 0xFF);
                data[offset + 2] = (byte) (number >> 16 & 0xFF);
                data[offset + 3] = (byte) (number >> 24 & 0xFF);
            }
            else
            {
                data[offset] = (byte) (number >> 24 & 0xFF);
                data[offset + 1] = (byte) (number >> 16 & 0xFF);
                data[offset + 2] = (byte) (number >> 8 & 0xFF);
                data[offset + 3] = (byte) (number & 0xFF);
            }
        }

        private byte[] ExtendBuffer(byte[] buffer, byte[] data)
        {
            int bufferSize = buffer.Length;
            int dataSize = data.Length;
            int combinedSize = bufferSize + dataSize;
            byte[] combined = new byte[combinedSize];
            Buffer.BlockCopy(buffer, 0, combined, 0, bufferSize);
            Buffer.BlockCopy(data, 0, combined, bufferSize, dataSize);
            return combined;
        }

        private byte[] CutBuffer(byte[] buffer, int offset, int size)
        {
            byte[] cut = new byte[size];
            Buffer.BlockCopy(buffer, offset, cut, 0, size);
            return cut;
        }
    }
}
