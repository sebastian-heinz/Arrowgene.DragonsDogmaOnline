using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Buffer = System.Buffer;

namespace Arrowgene.Ddon.Client
{
    public class ArcArchive : ClientFile
    {
        private const string Key = "ABB(DF2I8[{Y-oS_CCMy(@<}qR}WYX11M)w[5V.~CbjwM5q<F1Iab+-";
        private const int FileIndexSize = 80;
        private const int FileNameSize = 64;
        private const int FileIndexOffset = 8;
        private const int FileDataOffset = 0x8000;

        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ResourceFile));
        private static readonly BlowFish BlowFish = new BlowFish(Encoding.UTF8.GetBytes(Key), true);
        private static readonly JamCrc32 JamCrc32 = new JamCrc32();
        private static readonly Dictionary<uint, ArcExt> JamCrcLookup = new Dictionary<uint, ArcExt>();

        private static void Register(string className, string extension)
        {
            ArcExt arcExt = new ArcExt();
            byte[] classBytes = Encoding.UTF8.GetBytes(className);
            arcExt.Class = className;
            arcExt.Extension = extension;
            arcExt.JamCrcBytes = JamCrc32.ComputeHash(classBytes);
            arcExt.JamCrc = BitConverter.ToUInt32(arcExt.JamCrcBytes);
            arcExt.JamCrcStr = $"0x{arcExt.JamCrc:X8}";
            JamCrcLookup.Add(arcExt.JamCrc, arcExt);
        }

        public static T GetFile<T>(DirectoryInfo romDir, string arcPath, string filePath, string ext = null)
            where T : ClientFile, new()
        {
            ArcFile arcFile = GetArcFile(romDir, arcPath, filePath, ext, true);
            if (arcFile == null)
            {
                return null;
            }

            T file = new T();
            file.Open(arcFile.Data);
            return file;
        }

        public static T GetResource<T>(DirectoryInfo romDir, string arcPath, string filePath, string ext = null)
            where T : ResourceFile, new()
        {
            ArcFile arcFile = GetArcFile(romDir, arcPath, filePath, ext, true);
            if (arcFile == null)
            {
                return null;
            }

            T resource = new T();
            resource.Open(arcFile.Data);
            return resource;
        }

        public static T GetResource_NoLog<T>(DirectoryInfo romDir, string arcPath, string filePath, string ext = null)
            where T : ResourceFile, new()
        {
            ArcFile arcFile = GetArcFile(romDir, arcPath, filePath, ext, false);
            if (arcFile == null)
            {
                return null;
            }

            T resource = new T();
            resource.Open(arcFile.Data);
            return resource;
        }

        public static ArcFile GetArcFile(DirectoryInfo romDir, string arcPath, string filePath, string ext, bool log)
        {
            string path = Path.Combine(romDir.FullName, Util.UnrootPath(arcPath));
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                if (log)
                {
                    Logger.Error($"File does not exist. ({path})");
                }

                return null;
            }

            ArcArchive archive = new ArcArchive();
            archive.Open(file.FullName);
            FileIndexSearch search = Search()
                .ByArcPath(filePath)
                .ByExtension(ext);
            ArcFile arcFile = archive.GetFile(search);
            if (arcFile == null)
            {
                if (log)
                {
                    Logger.Error($"File:{filePath} could not be located in archive:{path}");
                }

                return null;
            }

            return arcFile;
        }

        /// <summary>
        /// Search Stream for specific indices in .arc files.
        /// Intended for FileStream/FileSystem to avoid reading whole .arc file into memory.
        /// </summary>
        public static List<FileIndex> IndexProbeStream(Stream stream, FileIndexSearch search)
        {
            List<FileIndex> indices = new List<FileIndex>();
            using BinaryReader r = new BinaryReader(stream, Encoding.UTF8, true);
            byte[] magicTagBytes = r.ReadBytes(4);
            string magicTag = Encoding.UTF8.GetString(magicTagBytes);
            ushort magicId = r.ReadUInt16();
            if ((magicTag != "ARC\0" && magicTag != "ARCC") || magicId != 0x07)
            {
                throw new Exception($"Invalid .arc File. Invalid MagicTag ({magicTag}) or MagicId ({magicId})");
            }

            int entries = r.ReadUInt16();
            for (int i = 0; i < entries; i++)
            {
                uint indexOffset = (uint)stream.Position;
                byte[] indexData = r.ReadBytes(FileIndexSize);
                FileIndex index = DeserializeFileIndex(IsCypheredArc(magicTag), indexData, indexOffset);
                if (search.Match(index))
                {
                    indices.Add(index);
                }
            }

            return indices;
        }

        /// <summary>
        /// Read ArcFiles from Stream based on indices.
        /// Intended for FileStream/FileSystem to avoid reading whole .arc file into memory.
        /// </summary>
        public static List<ArcFile> ReadFileStream(Stream stream, List<FileIndex> indices)
        {
            List<ArcFile> files = new List<ArcFile>();
            foreach (FileIndex index in indices)
            {
                byte[] fileData = new byte[index.CompressedSize];
                stream.Position = index.Offset;
                int read = stream.Read(fileData, 0, fileData.Length);
                if (read != fileData.Length)
                {
                    Logger.Error(
                        $"Stream read less than required: (Read Length:{read} != File Length:{fileData.Length})");
                    continue;
                }

                fileData = DeserializeFileData(index.IsCyphered, fileData);
                if (fileData.Length != index.Size)
                {
                    Logger.Error(
                        $"File Length does not match expected: (Data Length:{fileData.Length} != Index Length:{index.Size})");
                    continue;
                }

                ArcFile file = new ArcFile();
                file.Data = fileData;
                file.Index = index;
                files.Add(file);
            }

            return files;
        }

        public static FileIndexSearch Search()
        {
            return new FileIndexSearch();
        }

        public static bool IsCypheredArc(string MagicTag)
        {
            return MagicTag[3] == 'C';
        }

        private readonly List<ArcFile> _files;

        public ArcArchive()
        {
            _files = new List<ArcFile>();
        }

        public string MagicTag { get; set; }
        public ushort MagicId { get; set; }

        /// <summary>
        /// Returns all file indices from this archive.
        /// </summary>
        public List<ArcFile> GetFiles()
        {
            return new List<ArcFile>(_files);
        }

        /// <summary>
        /// Returns a list of all matching FileIndices.
        /// </summary>
        public List<ArcFile> GetFiles(FileIndexSearch search)
        {
            List<ArcFile> arcFiles = new List<ArcFile>();
            if (search != null)
            {
                foreach (ArcFile arcFile in _files)
                {
                    if (search.Match(arcFile.Index))
                    {
                        arcFiles.Add(arcFile);
                    }
                }
            }

            return arcFiles;
        }

        /// <summary>
        /// Returns the only file that matches the search criteria, raises an exception if none or more than one file is found.
        /// </summary>
        public ArcFile GetFile(FileIndexSearch search)
        {
            return GetFiles(search).Single();
        }

        public ArcFile PutFile(string arcPath, string ext, byte[] fileData)
        {
            FileIndex newFileIndex = new FileIndex();
            newFileIndex.IsCyphered = IsCypheredArc(MagicTag);

            newFileIndex.ArcPath = arcPath;

            foreach (uint jamCrc in JamCrcLookup.Keys)
            {
                ArcExt arcExt = JamCrcLookup[jamCrc];
                if (arcExt.Extension == ext)
                {
                    newFileIndex.ArcExt = arcExt;
                    newFileIndex.JamCrc = arcExt.JamCrc;
                    newFileIndex.Extension = arcExt.Extension;
                    break;
                }
            }

            if (string.IsNullOrEmpty(newFileIndex.Extension))
            {
                // todo calculate jamcrc?
                newFileIndex.Extension = $"{newFileIndex.JamCrc:X8}";
            }

            newFileIndex.Path = arcPath+"."+ newFileIndex.Extension;

            newFileIndex.Directory = Path.GetDirectoryName(newFileIndex.ArcPath);
            if (newFileIndex.Directory == null)
            {
                newFileIndex.Directory = "";
            }

            newFileIndex.Name = $"{Path.GetFileName(newFileIndex.ArcPath)}.{newFileIndex.Extension}";
            newFileIndex.Size = (uint)fileData.Length;

            ArcFile newArcFile = new ArcFile();
            newArcFile.Index = newFileIndex;
            newArcFile.Data = fileData;
            _files.Add(newArcFile);
            return newArcFile;
        }

        public void DeleteFile(ArcFile file)
        {
            _files.Remove(file);
        }

        /// <summary>
        /// Extract all files into the given rootPath.
        /// Preserving the arc directory structure.
        /// Directories that do not exists will be created.
        /// </summary>
        public void ExtractArchive(string rootPath)
        {
            foreach (ArcFile file in _files)
            {
                ExtractFile(rootPath, file);
            }
        }

        /// <summary>
        /// Extract a single file to the given root path.
        /// Preserving the arc directory structure.
        /// Directories that do not exists will be created.
        /// </summary>
        public void ExtractFile(string rootPath, ArcFile file)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine(rootPath, file.Index.Path));
            if (fileInfo.DirectoryName == null)
            {
                Logger.Error("Failed to extract file (fileInfo.DirectoryName == null)");
                return;
            }

            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            File.WriteAllBytes(fileInfo.FullName, file.Data);
        }

        public override void Read(IBuffer buffer)
        {
            _files.Clear();

            if (buffer.Size < 6)
            {
                Logger.Error($"Not enough data to parse ArcArchive (Size:{buffer.Size} < 8)");
                return;
            }

            byte[] magicTag = buffer.ReadBytes(4);
            MagicTag = Encoding.UTF8.GetString(magicTag);
            MagicId = ReadUInt16(buffer);
            if ((MagicTag != "ARC\0" && MagicTag != "ARCC") || MagicId != 0x07)
            {
                throw new Exception($"Invalid .arc File. Invalid MagicTag ({MagicTag}) or MagicId ({MagicId})");
            }
            
            bool cypheredArc = IsCypheredArc(MagicTag);

            int entries = ReadInt16(buffer);
            List<FileIndex> indices = new List<FileIndex>();
            for (int i = 0; i < entries; i++)
            {
                uint indexOffset = (uint)buffer.Position;
                byte[] indexData = buffer.ReadBytes(FileIndexSize);
                FileIndex index = DeserializeFileIndex(cypheredArc, indexData, indexOffset);
                indices.Add(index);
            }

            foreach (FileIndex fileIndex in indices)
            {
                byte[] fileData = buffer.GetBytes((int)fileIndex.Offset, (int)fileIndex.CompressedSize);

                fileData = DeserializeFileData(cypheredArc, fileData);
                if (fileData.Length != fileIndex.Size)
                {
                    Logger.Error(
                        $"File Length does not match expected: (Data Length:{fileData.Length} != Index Length:{fileIndex.Size})");
                    continue;
                }

                ArcFile file = new ArcFile();
                file.Data = fileData;
                file.Index = fileIndex;
                _files.Add(file);
            }
        }

        public override void Write(IBuffer buffer)
        {
            byte[] magicTag = Encoding.UTF8.GetBytes(MagicTag);
            buffer.WriteBytes(magicTag);
            buffer.WriteUInt16(MagicId);
            buffer.WriteUInt16((ushort)_files.Count);

            uint requiredIndexSize = (uint)(FileIndexSize * _files.Count);
            uint factor = (requiredIndexSize / FileDataOffset) + 1;

            uint fileDataOffset = factor * FileDataOffset;
            uint currentDataOffset = fileDataOffset;
            uint currentIndexOffset = FileIndexOffset;
            for (int i = 0; i < _files.Count; i++)
            {
                if (currentIndexOffset >= fileDataOffset)
                {
                    Exception ex = new Exception("Index to big, can't store more files");
                    Logger.Exception(ex);
                    throw ex;
                }

                ArcFile file = _files[i];
                byte[] fileData = SerializeFileData(file.Data, file.Index.Compression);

                file.Index.Offset = currentDataOffset;
                file.Index.IndexOffset = currentIndexOffset;
                file.Index.Size = (uint)file.Data.Length;
                file.Index.CompressedSize = (uint)fileData.Length;

                byte[] fileIndex = SerializeFileIndex(file.Index);
                buffer.WriteBytes(fileIndex, 0, (int)file.Index.IndexOffset, fileIndex.Length);
                buffer.WriteBytes(fileData, 0, (int)file.Index.Offset, fileData.Length);

                currentIndexOffset += FileIndexSize;
                currentDataOffset += file.Index.CompressedSize;
            }
        }


        private byte[] SerializeFileIndex(FileIndex fileIndex)
        {
            IBuffer fileIndexBuffer = new StreamBuffer();
            fileIndexBuffer.WriteFixedString(fileIndex.ArcPath, FileNameSize);
            fileIndexBuffer.WriteUInt32(fileIndex.JamCrc);
            fileIndexBuffer.WriteUInt32(fileIndex.CompressedSize);
            uint sizeBits = fileIndex.Size;
            uint compressionBits = (uint)fileIndex.Compression;
            uint flags = sizeBits
                         | compressionBits << 29;
            fileIndexBuffer.WriteUInt32(flags);
            fileIndexBuffer.WriteUInt32(fileIndex.Offset);
            byte[] buffer = fileIndexBuffer.GetAllBytes();
            if (fileIndex.IsCyphered)
            {
                buffer = BlowFish.Encrypt_ECB(buffer);
            }
            return buffer;
        }

        private static FileIndex DeserializeFileIndex(bool cypheredArc, byte[] fileIndexData, uint indexOffset = 0)
        {
            FileIndex fileIndex = new FileIndex();
            fileIndex.IsCyphered = cypheredArc;
            fileIndex.IndexOffset = indexOffset;
            byte[] entry = fileIndexData;
            if (cypheredArc)
            {
                entry = BlowFish.Decrypt_ECB(entry);
            }
            IBuffer entryBuffer = new StreamBuffer(entry);
            entryBuffer.Position = 0;
            fileIndex.ArcPath = entryBuffer.ReadFixedString(FileNameSize);
            fileIndex.Directory = Path.GetDirectoryName(fileIndex.ArcPath);
            if (fileIndex.Directory == null)
            {
                fileIndex.Directory = "";
            }

            fileIndex.JamCrc = entryBuffer.ReadUInt32();
            if (JamCrcLookup.ContainsKey(fileIndex.JamCrc))
            {
                fileIndex.ArcExt = JamCrcLookup[fileIndex.JamCrc];
                fileIndex.Extension = fileIndex.ArcExt.Extension;
            }
            else
            {
                fileIndex.Extension = $"{fileIndex.JamCrc:X8}";
            }

            fileIndex.Name = $"{Path.GetFileName(fileIndex.ArcPath)}.{fileIndex.Extension}";
            fileIndex.Path = Path.Combine(fileIndex.Directory, fileIndex.Name);
            fileIndex.CompressedSize = entryBuffer.ReadUInt32();
            uint flags = entryBuffer.ReadUInt32();
            uint sizeBits = flags & ((1 << 29) - 1);
            uint compressionBits = (flags >> 29) & ((1 << 3) - 1);
            fileIndex.Compression = (ArcCompression)compressionBits;
            fileIndex.Size = sizeBits;
            fileIndex.Offset = entryBuffer.ReadUInt32();
            return fileIndex;
        }

        private byte[] SerializeFileData(byte[] fileData, ArcCompression compression)
        {
            int dataLen = fileData.Length;
            byte[] copy = new byte[dataLen];
            Buffer.BlockCopy(fileData, 0, copy, 0, dataLen);
            copy = Compress(copy, (int)compression);
            if(IsCypheredArc(MagicTag))
            {
                copy = BlowFish.Encrypt_ECB(copy);
            }
            return copy;
        }

        private static byte[] DeserializeFileData(bool cypheredArc, byte[] fileData)
        {
            int dataLen = fileData.Length;
            byte[] copy = new byte[dataLen];
            Buffer.BlockCopy(fileData, 0, copy, 0, dataLen);
            if (cypheredArc)
            {
                copy = BlowFish.Decrypt_ECB(copy);
            }
            copy = Decompress(copy);
            return copy;
        }

        private byte[] Compress(byte[] input, int level = Deflater.BEST_COMPRESSION)
        {
            Deflater compressor = new Deflater();
            compressor.SetLevel(level);
            compressor.SetInput(input);
            compressor.Finish();
            using MemoryStream bos = new MemoryStream(input.Length);
            byte[] buf = new byte[1024];
            while (!compressor.IsFinished)
            {
                int count = compressor.Deflate(buf);
                bos.Write(buf, 0, count);
            }

            return bos.ToArray();
        }

        private static byte[] Decompress(byte[] input)
        {
            Inflater decompressor = new Inflater();
            decompressor.SetInput(input);
            using MemoryStream bos = new MemoryStream(input.Length);
            byte[] buf = new byte[1024];
            while (!decompressor.IsFinished)
            {
                int count = decompressor.Inflate(buf);
                bos.Write(buf, 0, count);
                if (count == 0 && !decompressor.IsFinished)
                {
                    if (decompressor.IsNeedingDictionary)
                    {
                        throw new Exception("Corrupted Arc file. decompressor.IsNeedingDictionary");
                    }
                    else if (decompressor.IsNeedingInput)
                    {
                        throw new Exception("Corrupted Arc file. decompressor.IsNeedingInput");
                    }
                    else
                    {
                        throw new Exception("Corrupted Arc file. Unknown Decompression Error");
                    }
                }
            }

            return bos.ToArray();
        }

        [Flags]
        public enum SearchOption : ushort
        {
            None = 0,
            Class = 1 << 0,
            Extension = 1 << 1,
            JamCrc = 1 << 2,
            JamCrcStr = 1 << 3,
            Name = 1 << 4,
            ArcPath = 1 << 5
        }

        public class FileIndexSearch
        {
            private string _arcPath;

            public FileIndexSearch()
            {
                Option = SearchOption.None;
            }

            public FileIndexSearch(FileIndexSearch fis)
            {
                Option = fis.Option;
                Class = fis.Class;
                Extension = fis.Extension;
                JamCrc = fis.JamCrc;
                JamCrcStr = fis.JamCrcStr;
                Name = fis.Name;
            }

            public SearchOption Option { get; private set; }
            public string Class { get; set; }
            public string Extension { get; set; }
            public uint JamCrc { get; set; }
            public string JamCrcStr { get; set; }
            public string Name { get; set; }

            public string ArcPath
            {
                get => _arcPath;
                set => _arcPath = Util.ToArcPath(value);
            }

            public bool Match(FileIndex fileIndex)
            {
                if (fileIndex == null)
                {
                    return false;
                }

                if (Option == SearchOption.None)
                {
                    return true;
                }

                if (Option.HasFlag(SearchOption.ArcPath))
                {
                    if (fileIndex.ArcPath != null
                        && fileIndex.ArcPath == ArcPath)
                    {
                        return Resolve(SearchOption.ArcPath).Match(fileIndex);
                    }

                    return false;
                }

                if (Option.HasFlag(SearchOption.JamCrc))
                {
                    if (fileIndex.JamCrc == JamCrc)
                    {
                        return Resolve(SearchOption.JamCrc).Match(fileIndex);
                    }

                    return false;
                }

                if (Option.HasFlag(SearchOption.Name))
                {
                    if (fileIndex.Name == Name)
                    {
                        return Resolve(SearchOption.Name).Match(fileIndex);
                    }

                    return false;
                }

                if (Option.HasFlag(SearchOption.Class))
                {
                    if (fileIndex.ArcExt.Class == Class)
                    {
                        return Resolve(SearchOption.Class).Match(fileIndex);
                    }

                    return false;
                }

                if (Option.HasFlag(SearchOption.Extension))
                {
                    if (fileIndex.ArcExt.Extension == Extension)
                    {
                        return Resolve(SearchOption.Extension).Match(fileIndex);
                    }

                    return false;
                }

                if (Option.HasFlag(SearchOption.JamCrcStr))
                {
                    if (fileIndex.ArcExt.JamCrcStr == JamCrcStr)
                    {
                        return Resolve(SearchOption.JamCrcStr).Match(fileIndex);
                    }

                    return false;
                }

                return false;
            }

            public FileIndexSearch Resolve(SearchOption option)
            {
                FileIndexSearch fis = new FileIndexSearch(this);
                return fis.RemoveCriteria(option);
            }

            public FileIndexSearch AddCriteria(SearchOption option)
            {
                Option |= option;
                return this;
            }

            public FileIndexSearch RemoveCriteria(SearchOption option)
            {
                Option &= ~option;
                return this;
            }

            public FileIndexSearch ByArcPath(string arcPath)
            {
                if (arcPath == null)
                {
                    return this;
                }

                ArcPath = arcPath;
                return AddCriteria(SearchOption.ArcPath);
            }

            public FileIndexSearch ByExtension(string extension)
            {
                if (extension == null)
                {
                    return this;
                }

                Extension = extension;
                return AddCriteria(SearchOption.Extension);
            }
        }

        public class FileIndex
        {
            public bool IsCyphered { get; set; }
            public uint IndexOffset { get; set; }
            public string Name { get; set; }
            public string Directory { get; set; }
            public string ArcPath { get; set; }
            public string Path { get; set; }
            public string Extension { get; set; }
            public uint JamCrc { get; set; }
            public uint Offset { get; set; }
            public uint CompressedSize { get; set; }
            public uint Size { get; set; }
            public ArcCompression Compression { get; set; }
            public ArcExt ArcExt { get; set; }
        }

        public enum ArcCompression
        {
            Lowest = 0,
            Low = 1,
            Normal = 2,
            High = 3,
            Highest = 4,
            StreamLow = 5,
            StreamHigh = 6,
            Invalid = 7,
        }

        public class ArcFile
        {
            public FileIndex Index { get; set; }
            public byte[] Data { get; set; }
        }

        public struct ArcExt
        {
            public string Class;
            public string Extension;
            public uint JamCrc;
            public byte[] JamCrcBytes;
            public string JamCrcStr;

            public ArcExt()
            {
                Class = "";
                Extension = "";
                JamCrcStr = "";
                JamCrc = 0;
                JamCrcBytes = null;
            }
        }

        static ArcArchive()
        {
            Register("rAI", "ais");
            Register("rAIConditionTree", "cdt");
            Register("rAIDynamicLayout", "dpth");
            Register("rAIFSM", "fsm");
            Register("rAIFSMList", "fsl");
            Register("rAIPathBase", "are");
            Register("rAIPathBaseXml", "are.xml");
            Register("rAIPawnActNoSwitch", "pas");
            Register("rAIPawnAutoMotionTbl", "pam");
            Register("rAIPawnAutoWordTbl", "paw");
            Register("rAIPawnCulPrioThinkCategory", "pc_ptkc");
            Register("rAIPawnEmParam", "pep");
            Register("rAIPawnOrder", "pao");
            Register("rAIPawnSkillParamTbl", "aps");
            Register("rAIPawnSpecialityInfo", "ps_info");
            Register("rAISensor", "sn2");
            Register("rAIWayPoint", "way");
            Register("rAIWayPointGraph", "gway");
            Register("rAbilityList", "abl");
            Register("rAchievement", "acv");
            Register("rAchievementHeader", "ach");
            Register("rAcquirement::rAbilityAddData", "aad");
            Register("rAcquirement::rAbilityData", "abd");
            Register("rAcquirement::rCustomSkillData", "csd");
            Register("rAcquirement::rNormalSkillData", "nsd");
            Register("rActionParamList", "acp");
            Register("rActivateDragonSkill", "ads");
            Register("rActorLight", "ali");
            Register("rAdjLimitParam", "alp");
            Register("rAdjustParam", "ajp");
            Register("rAnimalData", "aml");
            Register("rArchive", "arc");
            Register("rArchiveImport", "aimp");
            Register("rArchiveListArray", "ala");
            Register("rAreaHitShape", "ahs");
            Register("rAreaInfo", "ari");
            Register("rAreaInfoJointArea", "arj");
            Register("rAreaInfoStage", "ars");
            Register("rAreaMasterRankData", "amr");
            Register("rAreaMasterSpotData", "ams");
            Register("rAreaMasterSpotDetailData", "amsd");
            Register("rArmedEnemyInfo", "aeminfo");
            Register("rAtDfRateRaid", "atdf_raid");
            Register("rAttackParam", "atk");
            Register("rBakeJoint", "bjt");
            Register("rBitTable", "btb");
            Register("rBlazeEnemyInfo", "beminfo");
            Register("rBlowSaveEmLvParam", "blow_save");
            Register("rBowActParamList", "bap");
            Register("rBrowserFont", "bft");
            Register("rBrowserUITableData", "but");
            Register("rCalcDamageAtdmAdj", "cda");
            Register("rCalcDamageAtdmAdjRate", "cdarate");
            Register("rCalcDamageLvAdj", "cdl");
            Register("rCameraList", "lcm");
            Register("rCameraParamList", "cpl");
            Register("rCameraQuakeList", "cql");
            Register("rCatchInfoParam", "cip");
            Register("rCaughtDamageRateRefTbl", "cdrr");
            Register("rCaughtDamageRateTbl", "cdrt");
            Register("rCaughtInfoParam", "caip");
            Register("rCharParamEnemy", "cpe");
            Register("rCharacterEdit", "edt");
            Register("rCharacterEditCameraParam", "cecp");
            Register("rCharacterEditColorDef", "edt_color_def");
            Register("rCharacterEditModelPalette", "edt_mod_pal");
            Register("rCharacterEditMuscle", "edt_muscle");
            Register("rCharacterEditPersonalityPalette", "edt_personality_pal");
            Register("rCharacterEditPresetPalette", "edt_pset_pal");
            Register("rCharacterEditTalkLvPalette", "edt_talk_pal");
            Register("rCharacterEditTexturePalette", "edt_tex_pal");
            Register("rCharacterEditVoicePalette", "edt_voice_pal");
            Register("rChildRegionStatusParam", "crs");
            Register("rChildRegionStatusParamList", "rsl");
            Register("rCnsIK", "ik");
            Register("rCnsJointOffset", "jof");
            Register("rCnsLookAt", "lat");
            Register("rCnsMatrix", "mtx");
            Register("rCnsTinyChain", "ctc");
            Register("rCnsTinyIK", "tik");
            Register("rCollGeom", "coll_geom");
            Register("rCollIndex", "coll_idx");
            Register("rCollNode", "coll_node");
            Register("rCollision", "sbc");
            Register("rCollisionHeightField", "sbch");
            Register("rCollisionObj", "obc");
            Register("rConstModelParam", "cmp");
            Register("rConvexHull", "hul");
            Register("rCraftCapPass", "ccp");
            Register("rCraftElementExp", "cee");
            Register("rCraftQuality", "cqr");
            Register("rCraftRecipe", "");
            Register("rCraftSkillCost", "ckc");
            Register("rCraftSkillSpd", "cks");
            Register("rCraftUpGradeExp", "cuex");
            Register("rCustimShlLimit", "csl");
            Register("rCycleContentsSortieInfo", "csi");
            Register("rCycleQuestInfo", "cqi");
            Register("rDDOBenchmark", "bmk");
            Register("rDDOModelMontage", "dmt");
            Register("rDDOModelMontageEm", "dme");
            Register("rDamageCounterInfo", "counter_Adj");
            Register("rDamageSaveEmLvParam", "damage_save");
            Register("rDamageSpecialAdj", "damage_spAdj");
            Register("rDarkSkyParam", "dsp");
            Register("rDeformWeightMap", "dwm");
            Register("rDmJobAdjParam", "dja");
            Register("rDmJobPawnAdjParam", "dja_pawn");
            Register("rDmLvPawnAdjParam", "cdl_pawn");
            Register("rDmVecWeightParam", "dvw");
            Register("rDragonSkillColorParam", "dscp");
            Register("rDragonSkillEnhanceParam", "dse");
            Register("rDragonSkillLevelParam", "dsl");
            Register("rDragonSkillParam", "dsd");
            Register("rDungeonMarker", "dmi");
            Register("rDynamicSbc", "dsc");
            Register("rEditConvert", "edc");
            Register("rEditStageParam", "esp");
            Register("rEffect2D", "e2d");
            Register("rEffectAnim", "ean");
            Register("rEffectList", "efl");
            Register("rEffectProvider", "epv");
            Register("rEffectStrip", "efs");
            Register("rEmBaseInfoSv", "ebi_sv");
            Register("rEmCategory", "ecg");
            Register("rEmDamageDirInfo", "edv");
            Register("rEmDmgTimerTbl", "dtt");
            Register("rEmEffectTable", "eef");
            Register("rEmLvUpParam", "lup");
            Register("rEmMsgTable", "emt");
            Register("rEmScaleTable", "esl");
            Register("rEmScrAdjust", "em_scr_adj");
            Register("rEmSoundTable", "esn");
            Register("rEmStatusAdj", "esa");
            Register("rEmWarpParam", "ewp");
            Register("rEmWeakSafe", "wallmaria");
            Register("rEmWorkRateTable", "ewk");
            Register("rEmblemColorTable", "ect");
            Register("rEmoteGroup", "peg");
            Register("rEmparam", "emparam");
            Register("rEndContentsSortieInfo", "esi");
            Register("rEnemyBloodStain", "ebs");
            Register("rEnemyGroup", "emg");
            Register("rEnemyLocalEst", "ele");
            Register("rEnemyLocalShelTable", "esh");
            Register("rEnemyMaterialTable", "ema");
            Register("rEnemyReactResEx", "era");
            Register("rEnemyStatusChange", "est");
            Register("rEnhancedParamList", "epl");
            Register("rEnumDef", "edf");
            Register("rEquipCaptureList", "ecl");
            Register("rEquipPartsInfo", "epi");
            Register("rEquipPreset", "equip_preset");
            Register("rEquipPresetPalette", "epp");
            Register("rErosionInfoRes", "reg_info");
            Register("rErosionRegion", "reg_ersion");
            Register("rErosionRegionScaleChange", "scl_change");
            Register("rErosionShakeConvert", "ero_addTime");
            Register("rErosionSmallInfoRes", "eroSmall_info");
            Register("rErosionSuperInfoRes", "eroSuper_info");
            Register("rEvaluationTable", "evl");
            Register("rEventParam", "evp");
            Register("rEventResTable", "evtr");
            Register("rEventViewerList", "evlst");
            Register("rEventViewerSetInfo", "evsi");
            Register("rEvidenceList", "evd");
            Register("rFacialEditJointPreset", "fedt_jntpreset");
            Register("rFatAdjust", "fat_adjust");
            Register("rFieldAreaAdjoinList", "faa");
            Register("rFieldAreaList", "fal");
            Register("rFieldAreaMarkerInfo", "fmi");
            Register("rFieldMapData", "fmd");
            Register("rFreeF32Tbl", "f2p");
            Register("rFullbodyIKHuman2", "fbik_human2");
            Register("rFunctionList", "ftl");
            Register("rFurnitureAccessories", "fad");
            Register("rFurnitureData", "fnd");
            Register("rFurnitureGroup", "fng");
            Register("rFurnitureItem", "fni");
            Register("rFurnitureLayout", "fnl");
            Register("rGUI", "gui");
            Register("rGUIDogmaOrb", "dgm");
            Register("rGUIFont", "gfd");
            Register("rGUIIconInfo", "gii");
            Register("rGUIMapSetting", "gmp");
            Register("rGUIMessage", "gmd");
            Register("rGUIPhotoFrame", "pho");
            Register("rGatheringItem", "gat");
            Register("rGeometry2", "geo2");
            Register("rGeometry2Group", "geog");
            Register("rGeometry3", "geo3");
            Register("rGraphPatch", "gpt");
            Register("rGrass", "grs");
            Register("rGrass2", "gr2");
            Register("rGrass2Setting", "gr2s");
            Register("rGrassWind", "grw");
            Register("rHeadCtrl", "head_ctrl");
            Register("rHideNpcNameInfo", "hni");
            Register("rHumanEnemyCustomSkill", "hmcs");
            Register("rHumanEnemyEquip", "hmeq");
            Register("rHumanEnemyParam", "hmeparam");
            Register("rHumanEnemyPreset", "hmpre");
            Register("rIKCtrl", "ikctrl");
            Register("rISC", "isc");
            Register("rImplicitSurface", "is");
            Register("rIniLocal", "ini");
            Register("rIsEquipOneOfSeveral", "ieo");
            Register("rItemEquipJobInfoList", "eir");
            Register("rItemList", "ipa");
            Register("rJobBaseParam", "jobbase");
            Register("rJobCustomParam", "jcp");
            Register("rJobLevelUpTbl2", "jlt2");
            Register("rJobMasterCtrl", "jmc");
            Register("rJobTutorialQuestList", "jtq");
            Register("rJointEx2", "jex2");
            Register("rJointInfo", "jnt_info");
            Register("rJointOrder", "jnt_order");
            Register("rJukeBoxItem", "jbi");
            Register("rJumpParamTbl", "jmp");
            Register("rKeyCommand", "kcm");
            Register("rKeyConfigTextTable", "kctt");
            Register("rKeyCustomParam", "kcp");
            Register("rLandInfo", "lai");
            Register("rLanguageResIDConverter", "lrc");
            Register("rLargeCameraParam", "lcp");
            Register("rLayout", "lot");
            Register("rLayoutGroupParam", "lgp");
            Register("rLayoutGroupParamList", "gpl");
            Register("rLayoutPreset", "lop");
            Register("rLegCtrl", "leg_ctrl");
            Register("rLineBuilder", "mlb");
            Register("rLinkageEnemy", "lae");
            Register("rLinkageEnemyXml", "lae.xml");
            Register("rLoadingParam", "ldp");
            Register("rLocationData", "lcd");
            Register("rMagicChantParam", "chant");
            Register("rMagicCommandList", "mgcc");
            Register("rMagicCommandWord", "mcw");
            Register("rMandraActionParam", "map");
            Register("rMandraCharaMake", "mcm");
            Register("rMandraMotCombine", "mmc");
            Register("rMandraReaction", "mdr");
            Register("rMapSpotData", "msd");
            Register("rMapSpotStageList", "msl");
            Register("rMaterial", "mrl");
            Register("rModel", "mod");
            Register("rMotionFilter", "mot_filter");
            Register("rMotionList", "lmt");
            Register("rMotionParam", "motparam");
            Register("rMovieOnDisk", "wmv");
            Register("rMovieOnDiskInterMediate", "wmv");
            Register("rMovieOnMemory", "mem.wmv");
            Register("rMovieOnMemoryInterMediate", "mem.wmv");
            Register("rMsgSet", "mss");
            Register("rMyRoomActParam", "mra");
            Register("rNPCEmoMyRoom", "nem");
            Register("rNPCMotMyRoom", "nmm");
            Register("rNPCMotionSet", "nms");
            Register("rNamedParam", "ndp");
            Register("rNavConnect", "nvc");
            Register("rNavigationMesh", "nav");
            Register("rNpcConstItem", "nci");
            Register("rNpcCustomSkill", "ncs");
            Register("rNpcEditData", "ned");
            Register("rNpcIsNoSetPS3", "nsp");
            Register("rNpcIsUseJobParamEx", "ujp");
            Register("rNpcLedgerList", "nll");
            Register("rNpcMeetingPlace", "nmp");
            Register("rNulls", "nls");
            Register("rObjCollision", "col");
            Register("rOccluder", "occ");
            Register("rOccluderEx", "oce");
            Register("rOcdElectricParam", "eoc");
            Register("rOcdImmuneParamRes", "oIp");
            Register("rOcdIrAdj", "ir_adj");
            Register("rOcdIrAdjPL", "ir_adj_pl");
            Register("rOcdPriorityParam", "opp");
            Register("rOcdStatusParamRes", "osp");
            Register("rOmKey", "omk");
            Register("rOmLoadList", "oll");
            Register("rOmParam", "omp");
            Register("rOmParamEx", "ompe");
            Register("rOmParamPart", "ompp");
            Register("rOutfitInfo", "ofi");
            Register("rOutlineParamList", "olp");
            Register("rPCSimpleDebuggerTarget", "pdd");
            Register("rPackageQuestInfo", "pqi");
            Register("rParentRegionStatusParam", "prs");
            Register("rPartnerPawnTalk", "ppt");
            Register("rPartnerReactParam", "ppr");
            Register("rPartsCtrlTable", "ptc");
            Register("rPawnAIAction", "paa");
            Register("rPawnQuestTalk", "pqt");
            Register("rPawnSpSkillCategoryUI", "pssc");
            Register("rPawnSpSkillLevelUI", "pssl");
            Register("rPawnThinkControl", "ptc");
            Register("rPawnThinkLevelUp", "plu");
            Register("rPhoteNGItem", "pni");
            Register("rPlPartsInfo", "psi");
            Register("rPlanetariumItem", "planet");
            Register("rPlantTree", "plt");
            Register("rPriorityThink", "ptk");
            Register("rPrologueHmStatus", "phs");
            Register("rPushRate", "push_rate");
            Register("rQuestHistoryData", "qhd");
            Register("rQuestList", "qst");
            Register("rQuestMarkerInfo", "qmi");
            Register("rQuestSequenceList", "qsq");
            Register("rQuestTextData", "qtd");
            Register("rRagdoll", "rdd");
            Register("rRageTable", "rag");
            Register("rReaction", "rac");
            Register("rRecommendDragonSkill", "rds");
            Register("rRegionBreakInfo", "erb");
            Register("rRegionStatusCtrlTable", "rsc");
            Register("rRenderTargetTexture", "rtex");
            Register("rReplaceWardGmdList", "repgmdlist");
            Register("rRigidBody", "rbd");
            Register("rRoomWearParam", "rwr");
            Register("rScenario", "sce");
            Register("rSceneTexture", "stex");
            Register("rScheduler", "sdl");
            Register("rShader2", "mfx");
            Register("rShaderCache", "sch");
            Register("rShaderPackage", "spkg");
            Register("rShakeCtrl", "shake_ctrl");
            Register("rShlLimit", "slm");
            Register("rShlParamList", "shl");
            Register("rShopGoods", "spg_tbl");
            Register("rShotReqInfo", "sri");
            Register("rShotReqInfo2", "sri2");
            Register("rShrinkBlowValue", "sbv");
            Register("rSimpleCom::rChatComData", "ccd");
            Register("rSitePack", "sit");
            Register("rSituationMsgCtrl", "smc");
            Register("rSky", "sky");
            Register("rSndPitchLimit", "spl");
            Register("rSoundAreaInfo", "sar");
            Register("rSoundAttributeSe", "aser");
            Register("rSoundBank", "sbkr");
            Register("rSoundBossBgm", "sbb");
            Register("rSoundCurveSet", "scsr");
            Register("rSoundCurveXml", "scvr.xml");
            Register("rSoundDirectionalCurveXml", "sdcr.xml");
            Register("rSoundDirectionalSet", "sdsr");
            Register("rSoundEQ", "equr");
            Register("rSoundHitInfo", "shi");
            Register("rSoundMotionSe", "mser");
            Register("rSoundOptData", "sot");
            Register("rSoundParamOfs", "spo");
            Register("rSoundPhysicsJoint", "spjr");
            Register("rSoundPhysicsList", "splr");
            Register("rSoundPhysicsRigidBody", "sprr");
            Register("rSoundPhysicsSoftBody", "spsr");
            Register("rSoundRangeEqSet", "sreq");
            Register("rSoundRequest", "srqr");
            Register("rSoundReverb", "revr");
            Register("rSoundSequenceSe", "ssqr");
            Register("rSoundSimpleCurve", "sscr");
            Register("rSoundSourceMSADPCM", "xsew");
            Register("rSoundSourceOggVorbis", "sngw");
            Register("rSoundSourcePC", "");
            Register("rSoundSpeakerSetXml", "sssr.xml");
            Register("rSoundStreamRequest", "stqr");
            Register("rSoundSubMixer", "smxr");
            Register("rSoundSubMixerSet", "sms");
            Register("rStageAdjoinList", "sal");
            Register("rStageAdjoinList2", "sal2");
            Register("rStageConnect", "scc");
            Register("rStageCustom", "sca");
            Register("rStageCustomParts", "scp");
            Register("rStageCustomPartsEx", "scpx");
            Register("rStageInfo", "sti");
            Register("rStageJoint", "sja");
            Register("rStageList", "slt");
            Register("rStageMap", "smp");
            Register("rStageToSpot", "sts");
            Register("rStaminaDecTbl", "sdt");
            Register("rStarCatalog", "stc");
            Register("rStartPos", "stp");
            Register("rStartPosArea", "sta");
            Register("rStatusCheck", "sck");
            Register("rStatusGainTable", "sg_tbl");
            Register("rSwingModel", "swm");
            Register("rTable", "");
            Register("rTargetCursorOffset", "tco");
            Register("rTbl2Base", "");
            Register("rTbl2ChatMacro", "tcm");
            Register("rTbl2ClanEmblemTextureId", "ceti_tbl");
            Register("rTbl2ItemIconId", "tii");
            Register("rTblMenuComm", "tmc");
            Register("rTblMenuOption", "tmo");
            Register("rTexDetailEdit", "tde");
            Register("rTexture", "tex");
            Register("rTextureJpeg", "tex");
            Register("rTextureMemory", "tex");
            Register("rTexturePNG", "tex");
            Register("rThinkParamRange", "thp_range");
            Register("rThinkParamTimer", "thp_timer");
            Register("rTutorialDialogMessage", "tdm");
            Register("rTutorialList", "tlt");
            Register("rTutorialQuestGroup", "tqg");
            Register("rTutorialTargetList", "ttl");
            Register("rVertices", "vts");
            Register("rVfxLightInfluence", "eli");
            Register("rVibration", "vib");
            Register("rWarpLocation", "wal");
            Register("rWaypoint", "wpt");
            Register("rWaypoint2", "wp2");
            Register("rWeaponOffset", "wpn_ofs");
            Register("rWeaponResTable", "wrt");
            Register("rWeatherEffectParam", "wep");
            Register("rWeatherFogInfo", "wtf");
            Register("rWeatherInfoTbl", "wta");
            Register("rWeatherParamEfcInfo", "wte");
            Register("rWeatherParamInfoTbl", "wtl");
            Register("rWeatherStageInfo", "wsi");
            Register("rWepCateResTbl", "wcrt");
            Register("rZone", "zon");
            Register("rkThinkData", "pen");
            Register("uSoundSubMixer::CurrentSubMixer", "smxr");
        }
    }
}
