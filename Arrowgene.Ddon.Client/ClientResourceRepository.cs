using System.IO;
using Arrowgene.Ddon.Client.Resource;

namespace Arrowgene.Ddon.Client
{
    /// <summary>
    /// https://raw.githubusercontent.com/Andoryuuta/DDON_RE/master/old_data/dti/dti_prop_dump.h
    /// </summary>
    public class ClientResourceRepository
    {
        public FieldAreaListFal FieldAreaListFal { get; }
        public AreaStageListArs AreaStageListArs { get; }
        public AreaListAri AreaListAri { get; }
        public StageListSlt StageListSlt { get; }
        public LandListLal LandListLal { get; }

        private readonly DirectoryInfo _directory;

        public ClientResourceRepository(string directoryPath)
        {
            _directory = new DirectoryInfo(directoryPath);
            AreaStageListArs = new AreaStageListArs();
            AreaListAri = new AreaListAri();
            StageListSlt = new StageListSlt();
            FieldAreaListFal = new FieldAreaListFal();
            LandListLal = new LandListLal();
        }

        public void Load()
        {
            LandListLal.Open(GetPath("land_list.00E36E23"));
            FieldAreaListFal.Open(GetPath("field_area_list.4114AC4C"));
            AreaStageListArs.Open(GetPath("area_stage_list.71827B54"));
            AreaListAri.Open(GetPath("area_list.6FF78212"));
            StageListSlt.Open(GetPath("stage_list.21502841"));
        }

        private string GetPath(string filePath)
        {
            string path = Path.Combine(_directory.FullName, filePath);
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                // Logger.Error($"Could not load '{fileName}', file does not exist");
            }

            return path;
        }
    }
}
