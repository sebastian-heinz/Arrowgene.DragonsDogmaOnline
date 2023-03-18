using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAllPlayerContext
    {
        public CDataAllPlayerContext() {
            Base = new CDataContextBase();
            PlayerInfo = new CDataContextPlayerInfo();
            ResistInfo = new CDataContextResist();
            EditInfo = new CDataEditInfo();
        }

        public CDataContextBase Base { get; set; }
        public CDataContextPlayerInfo PlayerInfo { get; set; }
        public CDataContextResist ResistInfo { get; set; }
        public CDataEditInfo EditInfo { get; set; }
    
        public class Serializer : EntitySerializer<CDataAllPlayerContext>
        {
            public override void Write(IBuffer buffer, CDataAllPlayerContext obj)
            {
                WriteEntity<CDataContextBase>(buffer, obj.Base);
                WriteEntity<CDataContextPlayerInfo>(buffer, obj.PlayerInfo);
                WriteEntity<CDataContextResist>(buffer, obj.ResistInfo);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
            }
        
            public override CDataAllPlayerContext Read(IBuffer buffer)
            {
                CDataAllPlayerContext obj = new CDataAllPlayerContext();
                obj.Base = ReadEntity<CDataContextBase>(buffer);
                obj.PlayerInfo = ReadEntity<CDataContextPlayerInfo>(buffer);
                obj.ResistInfo = ReadEntity<CDataContextResist>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }
    }
}