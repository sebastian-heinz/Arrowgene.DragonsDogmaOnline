using System;
using System.Security.Cryptography;
using Arrowgene.Buffers;
using Arrowgene.Ddo.Shared;
using Arrowgene.Ddo.Shared.Crypto;

namespace Arrowgene.Ddo.Cli.Command
{
    public class DecryptCommand : ICommand
    {
        public string Key => "decrypt";
        public string Description => "Decrypt packet data";

        public CommandResultType Run(CommandParameter parameter)
        {
            // 0130
            string server_1 =
                "b71a9b05ff4c4f2cbfc63bbf8b955587b1d42764a984c71c5b710fd4b351e98ebc90d3be3dc9d49ebfb981b7c4f01b0b3944f294f0a114b35b44bb24084ee16471d1f4d9d13c784f434af0ef35f17505557a4a0a5c1a25b8013cee0bb55d4645effa115785c5e480e84ffc32f82c9c1f2f3e658723ba2794c238cd5f51655c5d64ba2f3ccf2fb7ea43546f9aa87122e9c6e9e85598e0c8926d13f5ef1481a47a5fbeb9f34978337b0c475d2a730f3370306275b02d1c456633e3180a6c3734338b1dbdc68c21a9039e3c8c8d2634147641f6a7aacf88f3df1bd439517d82c9d53f6ac1fd69549357963e0f4762390c64674009c10dee3f2fca1b415bef5f0bec821f794e9a6a6917e61d600977e8945cf899a803c975048b3faf20021a839e163169ad17d3270b7e2986bd237fb53209";
            // 0150
            string client_1 =
                "066b608643985002409e7be9541a39c4658cde84ce0f290dd91ebc1c7d4054864bfcaa80279c06d06b149f350c320029f8329c6e837b07608d4bd7aa13ecea46b77f3673e05da5a4b6877f27922701d333c89f10171c3b5f9482924f38b572ef68a598323f5091d0b1d572678437bd10f1dad2b112decf9c801f193188c0cd7c881918220bd851d8372c73f87b66e78462792e53d2cc5868e768941b6011fe9d65300b8841153afb06e73e9419f057656c9f30e836c17e0489537cdf91d17bf6a8d24e889a6b8c414a456d06410bca38a15aba22109841d523911b716ee7eca07970e1392857dc646d9adfecb26d0f877cd19cd27857aaf098cd73d0cf18597286eb6b06c1e4a9934046e369d8c619d3af94867aa106789d43ef76427dbaaf728d06d668b7e252ca95bfa6f99c5f515d7334218a51a761eb7b25977c574f8ce878daeac7be1945b1c00f954b5cab9810";

            DecryptServer1(Util.FromHexString(server_1));
            DecryptClient1(Util.FromHexString(client_1));
            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }

        private static byte[] InitialKey = new byte[]
        {
            0x66, 0x32, 0x33, 0x65, 0x39, 0x38, 0x48, 0x61, 0x66, 0x4A, 0x64, 0x53, 0x6F, 0x61, 0x6A, 0x38,
            0x30, 0x51, 0x42, 0x6A, 0x68, 0x68, 0x32, 0x33, 0x6F, 0x61, 0x6A, 0x67, 0x6B, 0x6C, 0x53, 0x61,
        };

        private static byte[] InitialPrev = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        private void DecryptServer1(byte[] input)
        {
            Console.WriteLine("--DecryptServer1--");
            Console.WriteLine();

            Console.WriteLine("Camellia Key:");
            Util.DumpBuffer(new StreamBuffer(InitialKey));
            Console.WriteLine();

            byte[] decrypted = new byte[input.Length];
            Camellia camellia = new Camellia();
            camellia.Decrypt(input, decrypted, InitialKey, InitialPrev);
            IBuffer decBuffer = new StreamBuffer(decrypted);
            decBuffer.SetPositionStart();
            byte[] hashData = decBuffer.ReadBytes(272);
            decBuffer.SetPositionStart();
            byte[] keyData = decBuffer.ReadBytes(256);
            byte[] unknown0 = decBuffer.ReadBytes(16);
            byte[] expectedHash = decBuffer.ReadBytes(20);
            byte[] unknown1 = decBuffer.ReadBytes(12);

            Console.WriteLine("Decrypted:");
            Util.DumpBuffer(new StreamBuffer(decrypted));
            Console.WriteLine();

            Console.WriteLine("Expected Hash:");
            Util.DumpBuffer(new StreamBuffer(expectedHash));
            Console.WriteLine();

            SHA1Managed sha1 = new SHA1Managed();
            byte[] calculatedHash = sha1.ComputeHash(hashData);
            Console.WriteLine("Calculated Hash:");
            Util.DumpBuffer(new StreamBuffer(calculatedHash));
            Console.WriteLine();
        }

        private void DecryptClient1(byte[] input)
        {
            Console.WriteLine("--DecryptClient1--");
            Console.WriteLine();

            Console.WriteLine("Camellia Key:");
            Util.DumpBuffer(new StreamBuffer(InitialKey));
            Console.WriteLine();

            byte[] decrypted = new byte[input.Length];
            Camellia camellia = new Camellia();
            camellia.Decrypt(input, decrypted, InitialKey, InitialPrev);
            IBuffer decBuffer = new StreamBuffer(decrypted);
            decBuffer.SetPositionStart();
            byte[] keyData = decBuffer.ReadBytes(256);

            Console.WriteLine("Decrypted:");
            Util.DumpBuffer(new StreamBuffer(decrypted));
            Console.WriteLine();
        }
    }
}
