using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared
{
    public static class Util
    {
        public static readonly IBufferProvider Buffer = new StreamBuffer();

        public static byte[] Copy(byte[] src)
        {
            int srcLen = src.Length;
            byte[] dst = new byte[srcLen];
            System.Buffer.BlockCopy(src, 0, dst, 0, srcLen);
            return dst;
        }

        public static long GetUnixTime(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static byte[] FromHexString(string hexString)
        {
            if ((hexString.Length & 1) != 0)
            {
                throw new ArgumentException("Input must have even number of characters");
            }

            byte[] ret = new byte[hexString.Length / 2];
            for (int i = 0; i < ret.Length; i++)
            {
                int high = hexString[i * 2];
                int low = hexString[i * 2 + 1];
                high = (high & 0xf) + ((high & 0x40) >> 6) * 9;
                low = (low & 0xf) + ((low & 0x40) >> 6) * 9;

                ret[i] = (byte)((high << 4) | low);
            }

            return ret;
        }

        public static string ToHexString(byte[] data, char? seperator = null)
        {
            StringBuilder sb = new StringBuilder();
            int len = data.Length;
            for (int i = 0; i < len; i++)
            {
                sb.Append(data[i].ToString("X2"));
                if (seperator != null && i < len - 1)
                {
                    sb.Append(seperator);
                }
            }

            return sb.ToString();
        }

        public static string HexDump(byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null) return "<null>";
            int bytesLength = bytes.Length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                8 // 8 characters for the address
                + 3; // 3 spaces

            int firstCharColumn = firstHexColumn
                                  + bytesPerLine * 3 // - 2 digit for the hexadecimal value and 1 space
                                  + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                                  + 2; // 2 spaces 

            int lineLength = firstCharColumn
                             + bytesPerLine // - characters to show the ascii value
                             + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - Environment.NewLine.Length) + Environment.NewLine)
                .ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }

                    hexColumn += 3;
                    charColumn++;
                }

                result.Append(line);
            }

            string ret = result.ToString();
            ret = ret.TrimEnd(Environment.NewLine.ToCharArray());
            ret = ret += Environment.NewLine;
            return ret;
        }

        public static string UnrootPath(string path)
        {
            // https://stackoverflow.com/questions/53102/why-does-path-combine-not-properly-concatenate-filenames-that-start-with-path-di
            if (Path.IsPathRooted(path))
            {
                path = path.TrimStart(Path.DirectorySeparatorChar);
                path = path.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return path;
        }

        public static string ExecutingDirectory()
        {
            return AppContext.BaseDirectory;
        }

        public static List<Dictionary<string, string>> DebugGetVersions()
        {
            List<Dictionary<string, string>> assemblyInfo = new List<Dictionary<string, string>>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                AssemblyName assemblyName = assembly.GetName();
                string asmName = assemblyName.Name;
                if (asmName == null)
                {
                    continue;
                }

                if (!asmName.StartsWith("Arrowgene.Ddon"))
                {
                    continue;
                }

                Version asmVersion = assemblyName.Version;
                string asmVersionStr = "unknown";
                if (asmVersion != null)
                {
                    asmVersionStr = asmVersion.ToString();
                }
                // FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);


                AssemblyInformationalVersionAttribute asmInformalVersion =
                    assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                string asmInformalVersionStr = "unknown";
                if (asmInformalVersion != null)
                {
                    asmInformalVersionStr = asmInformalVersion.InformationalVersion;
                }

                if (asmInformalVersionStr.StartsWith(asmVersionStr))
                {
                    asmInformalVersionStr = asmInformalVersionStr.Substring(asmVersionStr.Length);
                }

                Dictionary<string, string> asmInfo = new Dictionary<string, string>();
                asmInfo.Add("name", asmName);
                asmInfo.Add("version", asmVersionStr);
                asmInfo.Add("informalVersion", asmInformalVersionStr);
                assemblyInfo.Add(asmInfo);
            }


            return assemblyInfo;
        }

        public static string GetVersion(string ddonComponent)
        {
            string gameServerVersion = "";
            List<Dictionary<string, string>> versions = DebugGetVersions();
            foreach (Dictionary<string, string> version in versions)
            {
                if (version["name"] != $"Arrowgene.Ddon.{ddonComponent}")
                {
                    continue;
                }

                gameServerVersion = $"{version["version"]}";
                if (!string.IsNullOrWhiteSpace(version["informalVersion"]))
                {
                    gameServerVersion += $":{version["informalVersion"]}";
                }
            }

            return gameServerVersion;
        }

        public static string[] DebugGetLocations()
        {
            string[] locations = new string[5];

            try
            {
                var path = Assembly.GetEntryAssembly().CodeBase;
                var uri = new Uri(path);
                var codeBase = Path.GetDirectoryName(uri.LocalPath);
                locations[0] = $"CodeBase: {codeBase}";
            }
            catch (Exception ex)
            {
                locations[0] = $"CodeBase: Caused Exception ({ex})";
            }

            locations[1] = $"Launched from {Environment.CurrentDirectory}";
            locations[2] = $"Physical location {AppDomain.CurrentDomain.BaseDirectory}";
            locations[3] = $"AppContext.BaseDir {AppContext.BaseDirectory}";

            try
            {
                locations[4] = $"Runtime Call {Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)}";
            }
            catch (Exception ex)
            {
                locations[4] = $"Runtime Call: Caused Exception ({ex})";
            }

            return locations;
        }

        public static string RelativeExecutingDirectory()
        {
            return RelativeDirectory(Environment.CurrentDirectory, ExecutingDirectory());
        }

        public static string RelativeDirectory(string fromDirectory, string toDirectory)
        {
            return RelativeDirectory(fromDirectory, toDirectory, toDirectory, Path.DirectorySeparatorChar);
        }

        public static string RelativeDirectory(string fromDirectory, string toDirectory, string defaultDirectory)
        {
            return RelativeDirectory(fromDirectory, toDirectory, defaultDirectory, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// Returns a directory that is relative.
        /// </summary>
        /// <param name="fromDirectory">The directory to navigate from.</param>
        /// <param name="toDirectory">The directory to reach.</param>
        /// <param name="defaultDirectory">A directory to return on failure.</param>
        /// <param name="directorySeparator">Directory Separator to use</param>
        /// <returns>The relative directory or the defaultDirectory on failure.</returns>
        public static string RelativeDirectory(string fromDirectory, string toDirectory, string defaultDirectory,
            char directorySeparator)
        {
            string result;

            if (fromDirectory.EndsWith("\\") || fromDirectory.EndsWith("/"))
            {
                fromDirectory = fromDirectory.Remove(fromDirectory.Length - 1);
            }

            if (toDirectory.EndsWith("\\") || toDirectory.EndsWith("/"))
            {
                toDirectory = toDirectory.Remove(toDirectory.Length - 1);
            }

            if (toDirectory.StartsWith(fromDirectory))
            {
                result = toDirectory.Substring(fromDirectory.Length);
                if (result.StartsWith("\\") || result.StartsWith("/"))
                {
                    result = result.Substring(1, result.Length - 1);
                }

                if (result != "")
                {
                    result += directorySeparator;
                }
            }
            else
            {
                string[] fromDirs = fromDirectory.Split(':', '\\', '/');
                string[] toDirs = toDirectory.Split(':', '\\', '/');
                if (fromDirs.Length <= 0 || toDirs.Length <= 0 || fromDirs[0] != toDirs[0])
                {
                    return defaultDirectory;
                }

                int offset = 1;
                for (; offset < fromDirs.Length; offset++)
                {
                    if (toDirs.Length <= offset)
                    {
                        break;
                    }

                    if (fromDirs[offset] != toDirs[offset])
                    {
                        break;
                    }
                }

                StringBuilder relativeBuilder = new StringBuilder();
                for (int i = 0; i < fromDirs.Length - offset; i++)
                {
                    relativeBuilder.Append("..");
                    relativeBuilder.Append(directorySeparator);
                }

                for (int i = offset; i < toDirs.Length - 1; i++)
                {
                    relativeBuilder.Append(toDirs[i]);
                    relativeBuilder.Append(directorySeparator);
                }

                result = relativeBuilder.ToString();
            }

            result = DirectorySeparator(result, directorySeparator);
            return result;
        }

        public static string DirectorySeparator(string path)
        {
            return DirectorySeparator(path, Path.DirectorySeparatorChar);
        }

        public static string DirectorySeparator(string path, char directorySeparator)
        {
            if (directorySeparator != '\\')
            {
                path = path.Replace('\\', directorySeparator);
            }

            if (directorySeparator != '/')
            {
                path = path.Replace('/', directorySeparator);
            }

            return path;
        }

        public static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }

                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }
            }

            return index;
        }

        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            byte[] temp = null;
            int index = FindBytes(src, search);
            while (index >= 0)
            {
                if (temp == null)
                    temp = src;
                else
                    temp = dst;

                dst = new byte[temp.Length - search.Length + repl.Length];

                // before found array
                System.Buffer.BlockCopy(temp, 0, dst, 0, index);
                // repl copy
                System.Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                System.Buffer.BlockCopy(
                    temp,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    temp.Length - (index + search.Length));


                index = FindBytes(dst, search);
            }

            if (dst == null)
            {
                dst = src;
            }

            return dst;
        }

        /// <summary>
        /// Path inside .arc files use `\` to separate directories.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToArcPath(string path)
        {
            path = path.Replace('/', '\\');
            path = path.Replace(Path.DirectorySeparatorChar, '\\');
            path = path.Replace(Path.AltDirectorySeparatorChar, '\\');
            return path;
        }

        private static bool IsFileLocked(FileInfo fileInfo)
        {
            FileStream stream = null;
            try
            {
                stream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch(IOException ex)
            {
                // The file is unavailable because it is:
                // Still being processed or does not exist
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return false;
        }

        /// <summary>
        /// Implements a safer way to read all lines in a file when there is contention on the file access.
        /// Note that, it is possible that sometimes when the file is read, due to file system quirkyness
        /// it might read back a zero length string.
        /// </summary>
        /// <param name="path">Path to a text based file to read</param>
        /// <returns>Returns the contents of the file read as a string</returns>
        public static string ReadAllText(string path)
        {
            var attempts = 0;
            const uint MaxAttempts = 30;
            FileInfo fileInfo = new FileInfo(path);
            while (IsFileLocked(fileInfo) && (attempts < MaxAttempts))
            {
                Thread.Sleep(100);
            }

            if (IsFileLocked(fileInfo))
            {
                throw new Exception($"Failed to read file contents of '{path}'");
            }

            string content;
            List<string> lines = new List<string>();
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(reader.ReadLine());
                }
                content = string.Join("\n", lines);
            }

            if (content.Length == 0)
            {
                throw new Exception($"EMPTY FILE ON HOT RELOAD : {path}");
            }

            return content;   
        }
    }
}
