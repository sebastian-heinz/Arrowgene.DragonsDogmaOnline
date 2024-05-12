using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLobbyContextPlayer
    {
        public CDataLobbyContextPlayer()
        {
            Base=new CDataContextBase();
            PlayerInfo=new CDataContextPlayerInfo();
            EditInfo=new CDataEditInfo();
        }

        public CDataContextBase Base { get; set; }
        public CDataContextPlayerInfo PlayerInfo { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : EntitySerializer<CDataLobbyContextPlayer>
        {
            public override void Write(IBuffer buffer, CDataLobbyContextPlayer obj)
            {
                WriteEntity<CDataContextBase>(buffer, obj.Base);
                WriteEntity<CDataContextPlayerInfo>(buffer, obj.PlayerInfo);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
            }

            public override CDataLobbyContextPlayer Read(IBuffer buffer)
            {
                CDataLobbyContextPlayer obj = new CDataLobbyContextPlayer();
                obj.Base = ReadEntity<CDataContextBase>(buffer);
                obj.PlayerInfo = ReadEntity<CDataContextPlayerInfo>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }
    }
}