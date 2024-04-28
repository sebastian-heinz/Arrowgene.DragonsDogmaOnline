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
            Unk0 = 0;
        }

        public UInt32 ElementId {  get; set; }
        public JobId JobId { get; set; }
        public byte Unk0 { get; set; }
        public UInt32 RequireOrb { get; set; }
        public byte OrbRewardType { get; set; }
        public bool IsReleased { get; set; }
        public UInt32 ParamId { get; set; }
        public UInt32 ParamValue { get; set; }
        public byte Unk1 { get; set; }
        public UInt32 PosX { get; set; }
        public UInt32 PosY { get; set; }
        public bool DrawConnection { get; set; }

        public List<CDataCommonU32> RequiredElementIDList { get; set; }
        public List<CDataCommonU32> RequiredQuestList {  get; set; }

        public class Serializer : EntitySerializer<CDataJobOrbDevoteElement>
        {
            public override void Write(IBuffer buffer, CDataJobOrbDevoteElement obj)
            {
                WriteUInt32(buffer, obj.ElementId);
                WriteByte(buffer, (byte)obj.JobId);
                WriteByte(buffer, obj.Unk0);           // No idea what this does
                WriteUInt32(buffer, obj.RequireOrb);   // amount of BO required for the upgrade
                WriteByte(buffer, obj.OrbRewardType);  // Changes text between multiple effects in the UI
                WriteUInt32(buffer, obj.ParamId);      // No idea what this does
                WriteUInt32(buffer, obj.ParamValue);   // Shows next to OrbRewardType value
                WriteByte(buffer, 0xff);               // Doesn't impact Position
                WriteByte(buffer, 0xff);               // Doesn't Impact Position
                WriteByte(buffer, 0xff);               // Doesn't impact Position
                WriteByte(buffer, (byte) obj.PosX);    // This byte controls the x position
                WriteByte(buffer, 0x00);               // Client hangs if non-zero
                WriteByte(buffer, 0x00);               // No idea what this does; When set, client lags for a long time
                WriteBool(buffer, obj.DrawConnection); // Appears to be a bool which draws a line down
                WriteByte(buffer, 0x0);                // No idea what this does
                WriteBool(buffer, obj.IsReleased);     // Turn icon on/off
                WriteEntityList(buffer, obj.RequiredElementIDList); // Dependencies on unlocking other elements first?
                WriteEntityList(buffer, obj.RequiredQuestList);     // Quests required to be completed
            }

            public override CDataJobOrbDevoteElement Read(IBuffer buffer)
            {
                CDataJobOrbDevoteElement obj = new CDataJobOrbDevoteElement();
                obj.ElementId = ReadUInt32(buffer);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.RequireOrb = ReadUInt32(buffer);
                obj.OrbRewardType = ReadByte(buffer);
                obj.ParamId = ReadUInt32(buffer);
                obj.ParamValue = ReadUInt32(buffer);
                obj.PosX = ReadUInt32(buffer);
                obj.PosY = ReadUInt32(buffer);
                obj.IsReleased = ReadBool(buffer);
                obj.RequiredElementIDList = ReadEntityList<CDataCommonU32>(buffer);
                obj.RequiredQuestList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
