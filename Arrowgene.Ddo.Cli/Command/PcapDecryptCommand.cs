using System;
using System.Text;
using Arrowgene.Ddo.Shared;
using Arrowgene.Ddo.Shared.Crypto;

namespace Arrowgene.Ddo.Cli.Command
{
    public class PcapDecryptCommand : ICommand
    {
        public string Key => "pcap";

        public string Description =>
            $"Decrypt Pcaps Data";

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };
        
        public CommandResultType Run(CommandParameter parameter)
        {
            Camellia c = new Camellia();
            byte[] data = Util.FromHexString(parameter.Arguments[0]);
            Console.WriteLine($"DataLength: {data.Length}");
            data = data.AsSpan(0, 64).ToArray();
            byte[] key = Encoding.UTF8.GetBytes(parameter.Arguments[1]);
            Console.WriteLine($"Key:{Environment.NewLine}{Util.HexDump(key)}");
            Console.WriteLine($"data:{Environment.NewLine}{Util.HexDump(data)}");
            c.Decrypt(data, out Span<byte> output, key,CamelliaIv);
            Console.WriteLine($"output:{Environment.NewLine}{Util.HexDump(output.ToArray())}");
            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}
