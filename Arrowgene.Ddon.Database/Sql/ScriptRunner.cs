using System;
using System.IO;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql
{
    public class ScriptRunner
    {
        private const string DEFAULT_DELIMITER = ";";

        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ScriptRunner));

        private IDatabase _database;
        private string delimiter = DEFAULT_DELIMITER;
        private bool fullLineDelimiter = false;

        /**
         * Default constructor
         */
        public ScriptRunner(IDatabase database)
        {
            _database = database;
        }

        public void Run(string path)
        {
            int index = 0;
            try
            {
                string[] file = File.ReadAllLines(path);
                StringBuilder command = null;
                for (; index < file.Length; index++)
                {
                    string line = file[index];
                    if (command == null)
                    {
                        command = new StringBuilder();
                    }

                    string trimmedLine = line.Trim();

                    if (trimmedLine.Length < 1)
                    {
                        // Do nothing
                    }
                    else if (trimmedLine.StartsWith("//") || trimmedLine.StartsWith("--"))
                    {
                        // Print comment
                    }
                    else if (!fullLineDelimiter && trimmedLine.EndsWith(delimiter)
                             || fullLineDelimiter && trimmedLine == delimiter)
                    {
                        command.Append(
                            line.Substring(0, line.LastIndexOf(delimiter, StringComparison.InvariantCulture)));
                        command.Append(" ");
                        _database.Execute(command.ToString());
                        command = null;
                    }
                    else
                    {
                        command.Append(line);
                        command.Append("\n");
                    }
                }

                if (command != null)
                {
                    string cmd = command.ToString();
                    if (string.IsNullOrWhiteSpace(cmd))
                    {
                        //do nothing;
                    }
                    else
                    {
                        _database.Execute(cmd);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Error($"Sql error at Line: {index}");
                Logger.Exception(exception);
            }
        }
    }
}
