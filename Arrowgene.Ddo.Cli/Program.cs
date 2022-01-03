/*
 * This file is part of Arrowgene.Ddo.Cli
 *
 * Arrowgene.Ddo.Cli is a server implementation for the game "Dragons Dogma Online".
 * Copyright (C) 2019-2020 Ddo Team
 *
 * Github: https://github.com/sebastian-heinz/Ddo-server
 *
 * Arrowgene.Ddo.Cli is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Arrowgene.Ddo.Cli is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Arrowgene.Ddo.Cli. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Arrowgene.Ddo.Cli.Command;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.Cli
{
    internal class Program
    {
        private const char CliSeparator = ' ';
        private const char CliValueSeparator = '=';
        private static readonly ILogger Logger = LogProvider.Logger(typeof(Program));

        private static void Main(string[] args)
        {
            Console.WriteLine("Program started");
            Program program = new Program();
            program.RunArguments(args);
            Console.WriteLine("Program ended");
        }

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Dictionary<string, ICommand> _commands;
        private ICommand _lastCommand;
        private readonly object _consoleLock;

        private Program()
        {
            _lastCommand = null;
            _consoleLock = new object();
            _commands = new Dictionary<string, ICommand>();
            _cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;
            LogProvider.OnLogWrite += LogProviderOnOnLogWrite;
        }

        private void LoadCommands()
        {
            AddCommand(new ShowCommand());
            AddCommand(new ServerCommand());
            AddCommand(new HelpCommand(_commands));
            AddCommand(new TestCommand());
            AddCommand(new PcapDecryptCommand());
        }

        private void RunArguments(string[] arguments)
        {
            LogProvider.Start();
            Logger.Info("Argument Mode");
            if (arguments.Length <= 0)
            {
                Logger.Error("Invalid input");
                return;
            }

            LoadCommands();
            ShowCopyright();
            CommandResultType result = ProcessArguments(arguments);
            if (result != CommandResultType.Exit)
            {
                Logger.Info("Press `e'-key to exit.");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                while (keyInfo.Key != ConsoleKey.E)
                {
                    keyInfo = Console.ReadKey();
                }
            }

            if (_lastCommand != null)
            {
                _lastCommand.Shutdown();
            }

            LogProvider.Stop();
        }

        private void RunInteractive()
        {
            CommandResultType result = CommandResultType.Continue;
            while (result != CommandResultType.Exit)
            {
                string input = Console.ReadLine();
                if (input == null || input.Length <= 0)
                {
                    continue;
                }

                string[] arguments = input.Split(CliSeparator);
                result = ProcessArguments(arguments);
            }
        }

        private CommandResultType ProcessArguments(string[] arguments)
        {
            CommandParameter parameter = ParseParameter(arguments);
            if (!_commands.ContainsKey(parameter.Key))
            {
                Logger.Error(
                    $"Command: '{parameter.Key}' not available. Type `help' for a list of available commands.");
                return CommandResultType.Continue;
            }

            _lastCommand = _commands[parameter.Key];
            return _lastCommand.Run(parameter);
        }

        /// <summary>
        /// Parses the input text into arguments and switches.
        /// </summary>
        private CommandParameter ParseParameter(string[] args)
        {
            if (args.Length <= 0)
            {
                Logger.Error("Invalid input. Type 'help' for a list of available commands.");
                return null;
            }

            string paramKey = args[0];
            int cmdLength = args.Length - 1;
            string[] newArguments = new string[cmdLength];
            if (cmdLength > 0)
            {
                Array.Copy(args, 1, newArguments, 0, cmdLength);
            }

            args = newArguments;

            CommandParameter parameter = new CommandParameter(paramKey);
            foreach (string arg in args)
            {
                int count = CountChar(arg, CliValueSeparator);
                if (count == 1)
                {
                    string[] keyValue = arg.Split(CliValueSeparator);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0];
                        string value = keyValue[1];
                        if (key.StartsWith('-'))
                        {
                            if (key.Length <= 2 || parameter.SwitchMap.ContainsKey(key))
                            {
                                Logger.Error($"Invalid switch key: '{key}' is empty or duplicated.");
                                continue;
                            }

                            parameter.SwitchMap.Add(key, value);
                            continue;
                        }

                        if (key.Length <= 0 || parameter.ArgumentMap.ContainsKey(key))
                        {
                            Logger.Error($"Invalid argument key: '{key}' is empty or duplicated.");
                            continue;
                        }

                        parameter.ArgumentMap.Add(key, value);
                        continue;
                    }
                }

                if (arg.StartsWith('-'))
                {
                    string switchStr = arg;
                    if (switchStr.Length <= 2 || parameter.Switches.Contains(switchStr))
                    {
                        Logger.Error($"Invalid switch: '{switchStr}' is empty or duplicated.");
                        continue;
                    }

                    parameter.Switches.Add(switchStr);
                    continue;
                }

                if (arg.Length <= 0 || parameter.Switches.Contains(arg))
                {
                    Logger.Error($"Invalid argument: '{arg}' is empty or duplicated.");
                    continue;
                }

                parameter.Arguments.Add(arg);
            }

            return parameter;
        }

        private int CountChar(string str, char chr)
        {
            int count = 0;
            foreach (char c in str)
            {
                if (c == chr)
                {
                    count++;
                }
            }

            return count;
        }

        private void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        private void AddCommand(ICommand command)
        {
            _commands.Add(command.Key, command);
        }

        private void ShowCopyright()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append("Arrowgene.Ddo.Cli Copyright (C) 2019-2020 Ddo Team");
            sb.Append(Environment.NewLine);
            sb.Append("This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.");
            sb.Append(Environment.NewLine);
            sb.Append("This is free software, and you are welcome to redistribute it");
            sb.Append(Environment.NewLine);
            sb.Append("under certain conditions; type `show c' for details.");
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            Logger.Info(sb.ToString());
        }


        private void LogProviderOnOnLogWrite(object sender, LogWriteEventArgs e)
        {
            Log log = e.Log;
            LogLevel logLevel = log.LogLevel;

            ConsoleColor consoleColor;
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

            string text = log.ToString();
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
    }
}
