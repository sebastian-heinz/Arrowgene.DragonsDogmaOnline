using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arrowgene.Ddon.Cli.Command;
using Arrowgene.Ddon.Cli.Command.Packet;
using Xunit;

namespace Arrowgene.Ddon.Test.Cli.Command;

public class PacketCommandTest
{
    [Fact]
    public void TestUtf8DumpPcap1()
    {
        PacketCommand packetCommand = new PacketCommand();

        string testYaml = TestUtils.GetTestFileAsString("pcapng1-tcp-stream-33_reduced_test.yaml");
        List<PcapPacket> encryptedPackets = packetCommand.ReadYamlPcap(testYaml);
        List<PcapPacket> decryptedPackets = packetCommand.DecryptPackets(encryptedPackets, Encoding.ASCII.GetBytes("3jc6R11q__MGmP9YIn7fyiNVQgSUoiBc"));
        string annotatedPacketDump = packetCommand.GetAnnotatedPacketDump(decryptedPackets, new PacketCommandOptions(addUtf8StringDump: true));

        Assert.Contains("通信エラーが発生しました", annotatedPacketDump);
    }

    [Fact]
    public void TestDecryptPcap51()
    {
        string testYaml = TestUtils.GetTestFileAsString("stream51-marked.pcapng_tcp-stream-11.yaml");

        PacketCommand packetCommand = new PacketCommand();
        List<PcapPacket> encryptedPackets = packetCommand.ReadYamlPcap(testYaml);
        List<PcapPacket> decryptedPackets = packetCommand.DecryptPackets(encryptedPackets, Encoding.ASCII.GetBytes("_24ERLGmXGC0NTivXeXKZ24McjaaHJEC"));
        int count = decryptedPackets.Sum(decryptedPacket => decryptedPacket.ResolvedPackets?.Count ?? 0);

        Assert.Equal(762, encryptedPackets.Count);
        Assert.Equal(688, count);
    }

    [Fact]
    public void TestDecryptPcap85SplitPacket()
    {
        string testYaml = TestUtils.GetTestFileAsString("stream85-marked.pcapng_tcp-stream-11_edge_case.yaml");

        PacketCommand packetCommand = new PacketCommand();
        List<PcapPacket> encryptedPackets = packetCommand.ReadYamlPcap(testYaml);
        List<PcapPacket> decryptedPackets = packetCommand.DecryptPackets(encryptedPackets, Encoding.ASCII.GetBytes("hkNnBgyXz40de6kX3vkmAWGvatldB-Pp"));
        int count = decryptedPackets.Sum(decryptedPacket => decryptedPacket.ResolvedPackets?.Count ?? 0);

        Assert.Equal(4, encryptedPackets.Count);
        Assert.Equal(32, count);
    }
}
