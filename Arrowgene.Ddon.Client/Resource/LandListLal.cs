using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Client.Resource
{
    public class LandListLal : ClientResourceFile
    {
        public class Info
        {

        }

        public List<Info> Infos { get; }

        public LandListLal()
        {
            Infos = new List<Info>();
        }

        protected override void Read(IBuffer buffer)
        {
            Infos.Clear();
            List<Info> infos = ReadMtArray(buffer, ReadEntry);
            Infos.AddRange(infos);
        }

        private Info ReadEntry(IBuffer buffer)
        {
            Info entry = new Info();
            return entry;
        }
    }
}
