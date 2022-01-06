using System;
using System.Collections;

namespace Arrowgene.Ddo.PacketLibrary
{
    /*
     * Data offset (4 bits)
     * Reserved (3 bits) For future use and should be set to zero.
     * NS (1 bit): ECN-nonce - concealment protection
     * CWR (1 bit): Congestion window reduced
     * ECE (1 bit): ECN-Echo has a dual role, depending on the value of the SYN flag.
     * URG (1 bit): Indicates that the Urgent pointer field is significant
     * ACK (1 bit): Indicates that the Acknowledgment field is significant.
     * PSH (1 bit): Push function. Asks to push the buffered data to the receiving application.
     * RST (1 bit): Reset the connection
     * SYN (1 bit): Synchronize sequence numbers. Only the first packet sent from each end should have this flag set. Some other flags and fields change meaning based on this flag, and some are only valid when it is set, and others when it is clear.
     * FIN (1 bit): Last packet from sender
     */

    public static class TcpFlags
    {
        public static TcpFlag ParseFlags(byte b12, byte b13)
        {
            TcpFlag flags = TcpFlag.no;
            BitArray a12 = new BitArray(new byte[] {b12});
            Reverse(a12); // byte order
            flags |= a12.Get(7) ? TcpFlag.ns : TcpFlag.no;
            BitArray a13 = new BitArray(new byte[] {b13});
            Reverse(a13); // byte order
            flags |= a13.Get(0) ? TcpFlag.cwr : TcpFlag.no;
            flags |= a13.Get(1) ? TcpFlag.ece : TcpFlag.no;
            flags |= a13.Get(2) ? TcpFlag.urg : TcpFlag.no;
            flags |= a13.Get(3) ? TcpFlag.ack : TcpFlag.no;
            flags |= a13.Get(4) ? TcpFlag.psh : TcpFlag.no;
            flags |= a13.Get(5) ? TcpFlag.rst : TcpFlag.no;
            flags |= a13.Get(6) ? TcpFlag.syn : TcpFlag.no;
            flags |= a13.Get(7) ? TcpFlag.fin : TcpFlag.no;
            return flags;
        }

        private static void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }
    }

    [Flags]
    public enum TcpFlag : ushort
    {
        no = 0,
        off1 = 1 << 0,
        off2 = 1 << 1,
        off3 = 1 << 2,
        off4 = 1 << 3,
        res1 = 1 << 4,
        res2 = 1 << 5,
        res3 = 1 << 6,
        ns = 1 << 7,
        cwr = 1 << 8,
        ece = 1 << 9,
        urg = 1 << 10,
        ack = 1 << 11,
        psh = 1 << 12,
        rst = 1 << 13,
        syn = 1 << 14,
        fin = 1 << 15,
    }
}
