using Arrowgene.Ddon.GameServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;

namespace Arrowgene.Ddon.Shared
{
    public class Bitfield
    {
        public Bitfield(uint msb, uint lsb, string Name)
        {
            this.msb = msb;
            this.lsb = lsb;
            this.Name = Name;
        }

        public readonly uint msb;
        public readonly uint lsb;
        public readonly string Name;

        public static ulong Mask(uint msb, uint lsb)
        {
            return ((1UL << (int)(msb - lsb + 1UL)) - 1UL);
        }

        public static ulong ShiftedMask(uint msb, uint lsb)
        {
            return Bitfield.Mask(msb, lsb) << (int) lsb;
        }

        public static ulong Set(uint msb, uint lsb, ulong data, ulong value)
        {
            ulong maskedValue = Mask(msb, lsb) & value;
            ulong maskedData = data & ShiftedMask(msb, lsb);
            return (maskedValue << (int)lsb) | maskedData;
        }

        public static ulong Get(uint msb, uint lsb, ulong data)
        {
            return ((data >> (int)lsb) & Bitfield.Mask(msb, lsb));
        }

        public static ulong Value(uint msb, uint lsb, ulong value)
        {
            return (Bitfield.Mask(msb, lsb) & value) << (int) lsb;
        }

        public ulong Mask()
        {
            return Bitfield.Mask(msb, lsb);
        }

        public ulong ShiftedMask() 
        {
            return Bitfield.ShiftedMask(lsb, msb);
        }

        public ulong Get(ulong data)
        {
            return Bitfield.Get(msb, lsb, data);
        }

        public T Get<T>(ulong data) where T : struct
        {
            return NumericUtils.DowncastWithTruncation<T>(Bitfield.Get(msb, lsb, data));
        }

        public ulong Set(ulong data, ulong value)
        {
            return Bitfield.Set(msb, lsb, data, value);
        }

        public ulong Value(ulong value)
        {
            return Bitfield.Value(msb, lsb, value);
        }

        public T Value<T>(ulong value) where T : struct
        {
            return NumericUtils.DowncastWithTruncation<T>(Bitfield.Value(msb, lsb, value));
        }

        public ulong Width()
        {
            return msb - lsb + 1;
        }
    }
}
