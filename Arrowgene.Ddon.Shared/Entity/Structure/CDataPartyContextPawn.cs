using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyContextPawn
    {
        public CDataPartyContextPawn()
        {
            Base = new CDataContextBase();
            PlayerInfo = new CDataContextPlayerInfo();
            PawnReactionList = new List<CDataPawnReaction>();
            Unk0 = new byte[64];
            SpSkillList = new List<CDataSpSkill>();
            ResistInfo = new CDataContextResist();
            EditInfo = new CDataEditInfo();
        }

        public CDataContextBase Base { get; set; }
        public CDataContextPlayerInfo PlayerInfo { get; set; }
        public List<CDataPawnReaction> PawnReactionList { get; set; }
        public byte[] Unk0 { get; set; }
        public List<CDataSpSkill> SpSkillList { get; set; }
        public CDataContextResist ResistInfo { get; set; }
        public CDataEditInfo EditInfo { get; set; }

        public class Serializer : EntitySerializer<CDataPartyContextPawn>
        {
            public override void Write(IBuffer buffer, CDataPartyContextPawn obj)
            {
                WriteEntity<CDataContextBase>(buffer, obj.Base);
                WriteEntity<CDataContextPlayerInfo>(buffer, obj.PlayerInfo);
                WriteEntityList<CDataPawnReaction>(buffer, obj.PawnReactionList);
                WriteByteArray(buffer, obj.Unk0);
                WriteEntityList<CDataSpSkill>(buffer, obj.SpSkillList);
                WriteEntity<CDataContextResist>(buffer, obj.ResistInfo);
                WriteEntity<CDataEditInfo>(buffer, obj.EditInfo);
            }

            public override CDataPartyContextPawn Read(IBuffer buffer)
            {
                CDataPartyContextPawn obj = new CDataPartyContextPawn();
                obj.Base = ReadEntity<CDataContextBase>(buffer);
                obj.PlayerInfo = ReadEntity<CDataContextPlayerInfo>(buffer);
                obj.PawnReactionList = ReadEntityList<CDataPawnReaction>(buffer);
                obj.Unk0 = ReadByteArray(buffer, 64);
                obj.SpSkillList = ReadEntityList<CDataSpSkill>(buffer);
                obj.ResistInfo = ReadEntity<CDataContextResist>(buffer);
                obj.EditInfo = ReadEntity<CDataEditInfo>(buffer);
                return obj;
            }
        }
    }
}
