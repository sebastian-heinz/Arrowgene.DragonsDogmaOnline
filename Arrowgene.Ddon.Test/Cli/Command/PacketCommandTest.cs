using System.Collections.Generic;
using System.Text;
using Arrowgene.Ddon.Cli.Command;
using Arrowgene.Ddon.Cli.Command.Packet;
using Xunit;

namespace Arrowgene.Ddon.Test.Cli.Command;

public class PacketCommandTest
{
    [Fact]
    public void TestUtf8Dump()
    {
        PacketCommand packetCommand = new PacketCommand();

        string testYaml = TestUtils.GetTestFileAsString("pcapng1-tcp-stream-33_reduced_test.yaml");
        List<PcapPacket> encryptedPackets = packetCommand.ReadYamlPcap(testYaml);
        List<PcapPacket> decryptedPackets = packetCommand.DecryptPackets(encryptedPackets, Encoding.ASCII.GetBytes("3jc6R11q__MGmP9YIn7fyiNVQgSUoiBc"));
        string annotatedPacketDump = packetCommand.GetAnnotatedPacketDump(decryptedPackets, true);

        Assert.Contains("通信エラーが発生しました", annotatedPacketDump);
    }
}
