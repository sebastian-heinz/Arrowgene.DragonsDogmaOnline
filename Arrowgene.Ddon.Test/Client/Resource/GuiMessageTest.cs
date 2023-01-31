using Arrowgene.Ddon.Client.Resource;
using Xunit;

namespace Arrowgene.Ddon.Test.Client.Resource;

public class GuiMessageTest
{
    [Fact]
    public void TestGmdSerialisationA()
    {
        // Keys and Messages
        GuiMessage gmdA = new GuiMessage();
        gmdA.Magic = "GMD ";
        gmdA.Str = "TextWeb";
        gmdA.Unknown = new byte[1024];
        gmdA.Version = 0x10302;
        gmdA.A = 0xFFFFFFFF;
        gmdA.B = 0xFFFFFFFF;
        gmdA.C = 0xFFFFFFFF;
        for (uint i = 0; i < 10; i++)
        {
            GuiMessage.Entry entry = new GuiMessage.Entry();
            entry.Index = i;
            entry.Key = $"KEY_{i}";
            entry.Msg = $"MSG_{i}";
            entry.a2 = 0xFFFFFFFF;
            entry.a3 = 0xFFFFFFFF;
            entry.a4 = 0xFFFFFFFF;
            entry.a5 = 0xFFFFFFFF;
            entry.KeyReadIndex = i;
            entry.MsgReadIndex = i;
            gmdA.Entries.Add(entry);
        }

        byte[] gmdSerialized = gmdA.Save();

        GuiMessage gmdDeserialized = new GuiMessage();
        gmdDeserialized.Open(gmdSerialized);

        Validate(gmdA, gmdDeserialized);
    }

    [Fact]
    public void TestGmdSerialisationB()
    {
        // Only Messages without key
        GuiMessage gmdB = new GuiMessage();
        gmdB.Magic = "GMD ";
        gmdB.Str = "TextWeb";
        gmdB.Unknown = null;
        gmdB.Version = 0x10302;
        gmdB.A = 0xFFFFFFFF;
        gmdB.B = 0xFFFFFFFF;
        gmdB.C = 0xFFFFFFFF;
        for (uint i = 0; i < 10; i++)
        {
            GuiMessage.Entry entry = new GuiMessage.Entry();
            entry.Index = 0;
            entry.Key = null;
            entry.Msg = "MSG";
            entry.a2 = 0;
            entry.a3 = 0;
            entry.a4 = 0;
            entry.a5 = 0;
            entry.KeyReadIndex = 0;
            entry.MsgReadIndex = i;
            gmdB.Entries.Add(entry);
        }

        byte[] gmdSerialized = gmdB.Save();

        GuiMessage gmdDeserialized = new GuiMessage();
        gmdDeserialized.Open(gmdSerialized);

        Validate(gmdB, gmdDeserialized);
    }


    [Fact]
    public void TestGmdSerialisationC()
    {
        // Only Key and Only Message mixed
        GuiMessage gmdC = new GuiMessage();
        gmdC.Magic = "GMD ";
        gmdC.Str = "TextWeb";
        gmdC.Unknown = new byte[1024];
        gmdC.Version = 0x10302;
        gmdC.A = 0xFFFFFFFF;
        gmdC.B = 0xFFFFFFFF;
        gmdC.C = 0xFFFFFFFF;

        GuiMessage.Entry entryA = new GuiMessage.Entry();
        entryA.Index = 0;
        entryA.Key = "KEY";
        entryA.Msg = "MSG";
        entryA.a2 = 0xFFFFFFFF;
        entryA.a3 = 0xFFFFFFFF;
        entryA.a4 = 0xFFFFFFFF;
        entryA.a5 = 0xFFFFFFFF;
        entryA.KeyReadIndex = 0;
        entryA.MsgReadIndex = 0;
        gmdC.Entries.Add(entryA);

        GuiMessage.Entry entryB = new GuiMessage.Entry();
        entryB.Index = 0;
        entryB.Key = null;
        entryB.Msg = "MSG";
        entryB.a2 = 0;
        entryB.a3 = 0;
        entryB.a4 = 0;
        entryB.a5 = 0;
        entryB.KeyReadIndex = 0;
        entryB.MsgReadIndex = 1;
        gmdC.Entries.Add(entryB);

        byte[] gmdSerialized = gmdC.Save();
        GuiMessage gmdDeserialized = new GuiMessage();
        gmdDeserialized.Open(gmdSerialized);

        Validate(gmdC, gmdDeserialized);
    }

    private void Validate(GuiMessage expected, GuiMessage actual)
    {
        Assert.Equal(expected.Str, actual.Str);
        Assert.Equal(expected.Version, actual.Version);
        Assert.Equal(expected.Unknown, actual.Unknown);
        Assert.Equal(expected.A, actual.A);
        Assert.Equal(expected.B, actual.B);
        Assert.Equal(expected.C, actual.C);
        for (int i = 0; i < expected.Entries.Count; i++)
        {
            GuiMessage.Entry entryExpected = expected.Entries[i];
            GuiMessage.Entry entryActual = actual.Entries[i];
            Assert.Equal(entryExpected.Index, entryActual.Index);
            Assert.Equal(entryExpected.Key, entryActual.Key);
            Assert.Equal(entryExpected.Msg, entryActual.Msg);
            Assert.Equal(entryExpected.a2, entryActual.a2);
            Assert.Equal(entryExpected.a3, entryActual.a3);
            Assert.Equal(entryExpected.a4, entryActual.a4);
            Assert.Equal(entryExpected.a5, entryActual.a5);
            Assert.Equal(entryExpected.KeyReadIndex, entryActual.KeyReadIndex);
            Assert.Equal(entryExpected.MsgReadIndex, entryActual.MsgReadIndex);
        }
    }
}
