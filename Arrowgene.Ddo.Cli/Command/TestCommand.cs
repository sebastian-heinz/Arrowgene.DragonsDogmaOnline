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
            string bKey1 = "f23e98HafJdSoaj80QBjhh23oajgklSadrhogh2IJnwJEF58";
            string bKey2 = "nGIzy3qJo2fqLOgZI3Bv4UwZZ3LqKCUW";
            string bKey3 = "mofumofu capcom(^-^)";
            string bKey4 = "ABB(DF2I8[{Y-oS_CCMy(@<}qR}WYX11M)w[5V.~CbjwM5q<F1Iab+-";
            string dataS = "0123456789abcdef";
            
            byte[] k1 = Encoding.UTF8.GetBytes(bKey1);
            byte[] k2 = Encoding.UTF8.GetBytes(bKey2);
            byte[] k3 = Encoding.UTF8.GetBytes(bKey3);
            byte[] k4 = Encoding.UTF8.GetBytes(bKey4);
            byte[] data = Encoding.UTF8.GetBytes(dataS);

            BlowFish bf = new BlowFish(k1);
            byte[] enc = bf.Encrypt_ECB(data);
            Console.WriteLine($"k1:{Environment.NewLine}{Util.HexDump(enc)}");
            
            bf = new BlowFish(k2);
            enc = bf.Encrypt_ECB(data);
            Console.WriteLine($"k2:{Environment.NewLine}{Util.HexDump(enc)}");
            
            bf = new BlowFish(k3);
            enc = bf.Encrypt_ECB(data);
            Console.WriteLine($"k3:{Environment.NewLine}{Util.HexDump(enc)}");
            
            bf = new BlowFish(k4);
            enc = bf.Encrypt_ECB(data);
            Console.WriteLine($"k4:{Environment.NewLine}{Util.HexDump(enc)}");
            
            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}
