using System.Collections.Generic;

namespace Arrowgene.Ddon.Cli.Command.Packet;

public class PacketCommandOptions
{
    private static string ByteDumpSwitch => "--byte-dump";
    private static string ByteDumpHeaderSwitch => "--byte-dump-header";
    private static string ByteDumpPrefixSwitch => "--byte-dump-prefix";
    private static string Utf8StringDumpSwitch => "--utf8-dump";
    private static string StructureDumpSwitch => "--structure-dump";
    private static string PacketIncludeFilterSwitch => "--packet-include-filter";
    private static string ExportDecryptedPacketsSwitch => "--export-decrypted-packets";

    public bool AddByteDump { get; }
    public bool AddByteDumpHeader { get; }
    public string ByteDumpSeparator { get; }
    public string ByteDumpPrefix { get; }
    public bool AddUtf8StringDump { get; }
    public bool AddStructureDump { get; }
    public string StructureDumpFormat { get; }
    public string PacketIncludeFilter { get; }
    public bool ExportDecryptedPackets { get; }

    public PacketCommandOptions(bool addByteDump = false, bool addByteDumpHeader = false, string byteDumpSeparator = "", string byteDumpPrefix = "", bool addUtf8StringDump = false,
        bool addStructureDump = false, string structureDumpFormat = "", string packetIncludeFilter = "", bool exportDecryptedPackets = false)
    {
        AddByteDump = addByteDump;
        AddByteDumpHeader = addByteDumpHeader;
        ByteDumpSeparator = byteDumpSeparator;
        ByteDumpPrefix = byteDumpPrefix;
        AddUtf8StringDump = addUtf8StringDump;
        AddStructureDump = addStructureDump;
        StructureDumpFormat = structureDumpFormat;
        PacketIncludeFilter = packetIncludeFilter;
        ExportDecryptedPackets = exportDecryptedPackets;
    }

    public PacketCommandOptions(CommandParameter parameters) : this(parameters.Switches, parameters.SwitchMap)
    {
    }

    private PacketCommandOptions(List<string> parameterSwitches, Dictionary<string, string> parameterSwitchMap)
    {
        AddByteDump = parameterSwitches.Contains(ByteDumpSwitch) || parameterSwitchMap.ContainsKey(ByteDumpSwitch);
        AddByteDumpHeader = parameterSwitches.Contains(ByteDumpHeaderSwitch) || parameterSwitchMap.ContainsKey(ByteDumpHeaderSwitch);
        ByteDumpSeparator = parameterSwitchMap.GetValueOrDefault(ByteDumpSwitch, "");
        ByteDumpPrefix = parameterSwitchMap.GetValueOrDefault(ByteDumpPrefixSwitch, "");
        AddUtf8StringDump = parameterSwitches.Contains(Utf8StringDumpSwitch) || parameterSwitchMap.ContainsKey(Utf8StringDumpSwitch);
        AddStructureDump = parameterSwitches.Contains(StructureDumpSwitch) || parameterSwitchMap.ContainsKey(StructureDumpSwitch);
        StructureDumpFormat = parameterSwitchMap.GetValueOrDefault(StructureDumpSwitch, "JSON").ToLowerInvariant();
        PacketIncludeFilter = parameterSwitchMap.GetValueOrDefault(PacketIncludeFilterSwitch, "");
        ExportDecryptedPackets = parameterSwitches.Contains(ExportDecryptedPacketsSwitch) || parameterSwitchMap.ContainsKey(ExportDecryptedPacketsSwitch);
    }

    public static string GetUsage()
    {
        return
            $"[{ByteDumpSwitch}[=,]] [{ByteDumpHeaderSwitch}] [{ByteDumpPrefixSwitch}=0x] [{Utf8StringDumpSwitch}] [{StructureDumpSwitch}[=JSON|YAML]] [{PacketIncludeFilterSwitch}=11.21.2,S2C_QUEST_QUEST_PROGRESS_RES,...]";
    }
}
