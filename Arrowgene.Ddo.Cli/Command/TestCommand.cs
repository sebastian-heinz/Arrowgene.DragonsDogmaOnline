using System;
using System.Text;
using Arrowgene.Ddo.Shared;
using Arrowgene.Ddo.Shared.Crypto;

namespace Arrowgene.Ddo.Cli.Command
{
    public class TestCommand : ICommand
    {
        public string Key => "test";

        public string Description =>
            $"TEST";

        public CommandResultType Run(CommandParameter parameter)
        {

            string bKey4 = "ABB(DF2I8[{Y-oS_CCMy(@<}qR}WYX11M)w[5V.~CbjwM5q<F1Iab+-";
            
            string bKey3 = "mofumofu capcom(^-^)";
            string dataS = "0123456789abcdef";
            
            byte[] k1 = Encoding.UTF8.GetBytes(bKey3);
            byte[] data = Encoding.UTF8.GetBytes(dataS);
            Console.WriteLine($"k1:{Environment.NewLine}{Util.HexDump(k1)}");

            BlowFish bf = new BlowFish(k1);
            bf.CompatMode = true;
            byte[] enc = bf.Encrypt_ECB(data);
            Console.WriteLine($"k1:{Environment.NewLine}{Util.HexDump(enc)}");

            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}
