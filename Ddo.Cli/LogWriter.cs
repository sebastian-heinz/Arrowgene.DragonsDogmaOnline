using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Ddo.Cli.Argument;
using Ddo.Server.Logging;

namespace Ddo.Cli
{
    public class LogWriter : ISwitchConsumer
    {
        private readonly object _consoleLock;
        private readonly HashSet<ushort> _packetIdWhitelist;
        private readonly HashSet<ushort> _packetIdBlacklist;
        private readonly ILogger _logger;
        private readonly Queue<Log> _logQueue;
        private bool _paused;
        private bool _continueing;

        public LogWriter()
        {
            _logger = LogProvider.Logger(this);
            _packetIdWhitelist = new HashSet<ushort>();
            _packetIdBlacklist = new HashSet<ushort>();
            _logQueue = new Queue<Log>();
            _consoleLock = new object();
            Switches = new List<ISwitchProperty>();
            _paused = false;
            _continueing = false;
            Reset();
            LoadSwitches();
            LogProvider.GlobalLogWrite += LogProviderOnGlobalLogWrite;
        }

        public List<ISwitchProperty> Switches { get; }

        /// <summary>
        /// --max-packet-size=64
        /// </summary>
        public int MaxPacketSize { get; set; }

        /// <summary>
        /// --no-data=true
        /// </summary>
        public bool NoData { get; set; }

        /// <summary>
        /// --log-level=2
        /// </summary>
        public int MinLogLevel { get; set; }

        public void Reset()
        {
            MaxPacketSize = -1;
            NoData = false;
            MinLogLevel = (int) LogLevel.Debug;
            _packetIdWhitelist.Clear();
            _packetIdBlacklist.Clear();
        }

        public void WhitelistPacket(ushort packetId)
        {
            if (_packetIdWhitelist.Contains(packetId))
            {
                _logger.Error($"PacketId:{packetId} is already whitelisted");
                return;
            }

            _packetIdWhitelist.Add(packetId);
        }

        public void BlacklistPacket(ushort packetId)
        {
            if (_packetIdBlacklist.Contains(packetId))
            {
                _logger.Error($"PacketId:{packetId} is already blacklisted");
                return;
            }

            _packetIdBlacklist.Add(packetId);
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Continue()
        {
            _continueing = true;
            while (_logQueue.TryDequeue(out Log log))
            {
                WriteLog(log);
            }

            _paused = false;
            _continueing = false;
        }

        private void LoadSwitches()
        {
            Switches.Add(
                new SwitchProperty<bool>(
                    "--no-data",
                    "--no-data=true (true|false)",
                    "Don't display packet data",
                    bool.TryParse,
                    (result => NoData = result)
                )
            );
            Switches.Add(
                new SwitchProperty<int>(
                    "--max-packet-size",
                    "--max-packet-size=64 (integer)",
                    "Don't display packet data",
                    int.TryParse,
                    (result => MaxPacketSize = result)
                )
            );
            Switches.Add(
                new SwitchProperty<int>(
                    "--log-level",
                    "--log-level=20 (integer) [Debug=10, Info=20, Error=30]",
                    "Only display logs of the same level or above",
                    int.TryParse,
                    (result => MinLogLevel = result)
                )
            );
            Switches.Add(
                new SwitchProperty<object>(
                    "--clear",
                    "--clear",
                    "Resets all switches to default",
                    SwitchProperty<object>.NoOp,
                    result => Reset()
                )
            );
            Switches.Add(
                new SwitchProperty<List<ushort>>(
                    "--b-list",
                    "--b-list=1000,2000,0xAA (PacketId[0xA|10])",
                    "A blacklist that does not logs packets specified",
                    TryParsePacketIdList,
                    results => { AssinPacketIdList(results, BlacklistPacket); }
                )
            );
            Switches.Add(
                new SwitchProperty<List<ushort>>(
                    "--w-list",
                    "--w-list=1000,2000,0xAA (PacketId[0xA|10])",
                    "A whitelist that only logs packets specified",
                    TryParsePacketIdList,
                    results => { AssinPacketIdList(results, WhitelistPacket); }
                )
            );
        }

        /// <summary>
        /// parses strings like "1:1000,2000,3:4000" into ServerType and PacketId
        /// </summary>
        private bool TryParsePacketIdList(string value, out List<ushort> result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = null;
                return false;
            }

            string[] values = value.Split(",");

            result = new List<ushort>();
            foreach (string entry in values)
            {
                NumberStyles numberStyles;
                String entryStr;
                if (entry.StartsWith("0x"))
                {
                    entryStr = entry.Substring(2);
                    numberStyles = NumberStyles.HexNumber;
                }
                else
                {
                    entryStr = entry;
                    numberStyles = NumberStyles.Integer;
                }


                if (!ushort.TryParse(entryStr, numberStyles, null, out ushort val))
                {
                    return false;
                }

                result.Add(val);
            }

            return true;
        }

        private void AssinPacketIdList(List<ushort> results, Action<ushort> addToPacketList)
        {
            foreach (ushort entry in results)
            {
                addToPacketList(entry);
            }
        }

        private void LogProviderOnGlobalLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            while (_continueing)
            {
                Thread.Sleep(10000);
            }

            if (_paused)
            {
                _logQueue.Enqueue(logWriteEventArgs.Log);
                return;
            }

            WriteLog(logWriteEventArgs.Log);
        }

        private void WriteLog(Log log)
        {
            ConsoleColor consoleColor;
            string text;

            object tag = log.Tag;
            if (tag is DdoLogPacket logPacket)
            {
                switch (logPacket.LogType)
                {
                    case DdoLogType.PacketIn:
                        consoleColor = ConsoleColor.Green;
                        break;
                    case DdoLogType.PacketOut:
                        consoleColor = ConsoleColor.Blue;
                        break;
                    case DdoLogType.PacketUnhandled:
                        consoleColor = ConsoleColor.Red;
                        break;
                    default:
                        consoleColor = ConsoleColor.Gray;
                        break;
                }

                text = CreatePacketLog(logPacket);
            }
            else
            {
                LogLevel logLevel = log.LogLevel;
                if ((int) logLevel < MinLogLevel)
                {
                    return;
                }

                switch (logLevel)
                {
                    case LogLevel.Debug:
                        consoleColor = ConsoleColor.DarkCyan;
                        break;
                    case LogLevel.Info:
                        consoleColor = ConsoleColor.Cyan;
                        break;
                    case LogLevel.Error:
                        consoleColor = ConsoleColor.Red;
                        break;
                    default:
                        consoleColor = ConsoleColor.Gray;
                        break;
                }

                text = log.ToString();
            }

            if (text == null)
            {
                return;
            }

            lock (_consoleLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        private bool ExcludeLog(ushort packetId)
        {
            bool useWhitelist = _packetIdWhitelist.Count > 0;
            bool whitelisted = _packetIdWhitelist.Contains(packetId);
            bool blacklisted = _packetIdBlacklist.Contains(packetId);

            if (useWhitelist && whitelisted)
            {
                return false;
            }

            if (useWhitelist)
            {
                return true;
            }

            if (blacklisted)
            {
                return true;
            }

            return false;
        }

        private string CreatePacketLog(DdoLogPacket logPacket)
        {
            ushort packetId = logPacket.Id;

            if (ExcludeLog(packetId))
            {
                return null;
            }

            int dataSize = logPacket.Data.Size;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{logPacket.ClientIdentity} Packet Log");
            sb.Append(Environment.NewLine);
            sb.Append("----------");
            sb.Append(Environment.NewLine);
            sb.Append($"[{logPacket.TimeStamp:HH:mm:ss}][Typ:{logPacket.LogType}]");
            sb.Append(Environment.NewLine);
            sb.Append(
                $"[Id:0x{logPacket.Id:X2}|{logPacket.Id}][BodyLen:{logPacket.Data.Size}][{logPacket.PacketIdName}]");
            sb.Append(Environment.NewLine);
            sb.Append(logPacket.Header.ToLogText());
            sb.Append(Environment.NewLine);

            if (!NoData)
            {
                IBuffer data = logPacket.Data;
                int maxPacketSize = MaxPacketSize;
                if (maxPacketSize > 0 && dataSize > maxPacketSize)
                {
                    data = data.Clone(0, maxPacketSize);

                    sb.Append($"- Truncated Data showing {maxPacketSize} of {dataSize} bytes");
                    sb.Append(Environment.NewLine);
                }

                sb.Append("ASCII:");
                sb.Append(Environment.NewLine);
                sb.Append(data.ToAsciiString(true));
                sb.Append(Environment.NewLine);
                sb.Append("HEX:");
                sb.Append(Environment.NewLine);
                sb.Append(data.ToHexString(' '));
                sb.Append(Environment.NewLine);
            }

            sb.Append("----------");

            return sb.ToString();
        }
    }
}
