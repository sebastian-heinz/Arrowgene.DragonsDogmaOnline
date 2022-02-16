using System.IO;

namespace Arrowgene.Ddon.Client
{
    public class ClientResourceRepository
    {
        public AreaStageListArc AreaStageListArc { get; }

        private readonly DirectoryInfo _directory;

        public ClientResourceRepository(string directoryPath)
        {
            _directory = new DirectoryInfo(directoryPath);
            AreaStageListArc = new AreaStageListArc();
        }

        public void Load()
        {
            AreaStageListArc.Open(GetPath("area_stage_list.71827B54"));
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
