using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobOrbDevoteElement
    {
        public CDataJobOrbDevoteElement()
        {
            RequiredElementIDList = new List<CDataCommonU32>();
            RequiredQuestList = new List<CDataCommonU32>();
            Unk1List = new List<CDataCommonU32>();
        }

        public uint ElementId {  get; set; }
        public JobId JobId { get; set; }
        public byte Unk0 {  get; set; }
        public uint RequireOrb { get; set; } // Price in blood orbs
        public OrbGainParamType OrbParamType { get; set; } // ORB_GAIN_PARAM_*
        public uint ParamId { get; set; } // 0 unless using Augment or custom skill. Then this is the ID of those skills.
        public uint ParamValue { get; set; } // Shows up next to ORB_GAIN_PARAM reward
        public uint PosX {  get; set; }
        public uint PosY { get; set; }
        public bool IsReleased { get; set; }
        public List<CDataCommonU32> RequiredElementIDList { get; set; }
        public List<CDataCommonU32> RequiredQuestList { get; set; }
        public List<CDataCommonU32> Unk1List { get; set; }

        public class Serializer : EntitySerializer<CDataJobOrbDevoteElement>
        {
            public override void Write(IBuffer buffer, CDataJobOrbDevoteElement obj)
            {
                WriteUInt32(buffer, obj.ElementId);
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.Unk0);            // No idea what this does
                WriteUInt32(buffer, obj.RequireOrb);    // amount of BO required for the upgrade
                WriteByte(buffer, (byte) obj.OrbParamType);   // Changes text between multiple effects in the UI
                WriteUInt32(buffer, obj.ParamId);       // No idea what this does
                WriteUInt32(buffer, obj.ParamValue);    // Shows next to OrbRewardType value
                WriteUInt32(buffer, obj.PosX);          // Controls X position
                WriteUInt32(buffer, obj.PosY);          // Controls Y position
                WriteBool(buffer, obj.IsReleased);      // Appears to be a bool which draws a line down
                WriteEntityList(buffer, obj.RequiredElementIDList); // Dependencies on unlocking other elements first?
                WriteEntityList(buffer, obj.RequiredQuestList);     // Quests required to be completed
                WriteEntityList(buffer, obj.Unk1List);              // ???
            }

            public override CDataJobOrbDevoteElement Read(IBuffer buffer)
            {
                CDataJobOrbDevoteElement obj = new CDataJobOrbDevoteElement();
                obj.ElementId = ReadUInt32(buffer);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.RequireOrb = ReadUInt32(buffer);
                obj.OrbParamType = (OrbGainParamType) ReadByte(buffer);
                obj.ParamId = ReadUInt32(buffer);
                obj.ParamValue = ReadUInt32(buffer);
                obj.PosX = ReadUInt32(buffer);
                obj.PosY = ReadUInt32(buffer);
                obj.IsReleased = ReadBool(buffer);
                obj.RequiredElementIDList = ReadEntityList<CDataCommonU32>(buffer);
                obj.RequiredQuestList = ReadEntityList<CDataCommonU32>(buffer);
                obj.Unk1List = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
