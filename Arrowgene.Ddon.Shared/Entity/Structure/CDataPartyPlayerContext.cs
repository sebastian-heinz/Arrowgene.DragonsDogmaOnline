using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyPlayerContext
    {
        // CDataPartyContextPlayer
        public CDataPartyPlayerContext()
        {
            Base = new CDataContextBase();
            PlayerInfo = new CDataContextPlayerInfo();
            ResistInfo = new CDataContextResist();
            EditInfo = new CDataEditInfo();
        }

        public CDataContextBase Base { get; set; }
        public CDataContextPlayerInfo PlayerInfo { get; set; }
        public CDataContextResist ResistInfo { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : EntitySerializer<CDataPartyPlayerContext>
        {
            public override void Write(IBuffer buffer, CDataPartyPlayerContext obj)
            {
                WriteEntity<CDataContextBase>(buffer, obj.Base);
                WriteEntity<CDataContextPlayerInfo>(buffer, obj.PlayerInfo);
                WriteEntity<CDataContextResist>(buffer, obj.ResistInfo);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
            }

            public override CDataPartyPlayerContext Read(IBuffer buffer)
            {
                CDataPartyPlayerContext obj = new CDataPartyPlayerContext();
                obj.Base = ReadEntity<CDataContextBase>(buffer);
                obj.PlayerInfo = ReadEntity<CDataContextPlayerInfo>(buffer);
                obj.ResistInfo = ReadEntity<CDataContextResist>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }
    }
}
