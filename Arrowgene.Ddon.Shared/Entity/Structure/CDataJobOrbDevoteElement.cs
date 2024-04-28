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
            Unk1 = 0;
            Unk2 = 0;
            Unk3 = 0;
            Unk4 = 0;
        }

        public UInt32 ElementId {  get; set; }
        public JobId JobId { get; set; }
        public UInt32 RequireOrb { get; set; }
        public byte OrbRewardType { get; set; }
        public bool IsReleased { get; set; }
        public UInt32 ParamId { get; set; }
        public UInt32 ParamValue { get; set; }
        public byte PosX { get; set; }
        // public UInt32 PosY { get; set; }
        public bool DrawConnection { get; set; }

        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public byte Unk4 { get; set; }
        public byte Unk5 { get; set; }
        public byte Unk6 { get; set; }

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
                WriteByte(buffer, obj.Unk1);           // Doesn't impact Position
                WriteByte(buffer, obj.Unk2);           // Doesn't Impact Position
                WriteByte(buffer, obj.Unk3);           // Doesn't impact Position
                WriteByte(buffer, (byte) obj.PosX);    // This byte controls the x position
                WriteByte(buffer, obj.Unk4);           // Client hangs if non-zero
                WriteByte(buffer, obj.Unk5);           // No idea what this does; When set, client lags for a long time
                WriteBool(buffer, obj.DrawConnection); // Appears to be a bool which draws a line down
                WriteByte(buffer, obj.Unk6);           // No idea what this does
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
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.PosX = ReadByte(buffer);
                obj.Unk4 = ReadByte(buffer);
                obj.Unk5 = ReadByte(buffer);
                obj.DrawConnection = ReadBool(buffer);
                obj.Unk6 = ReadByte(buffer);
                obj.IsReleased = ReadBool(buffer);
                obj.RequiredElementIDList = ReadEntityList<CDataCommonU32>(buffer);
                obj.RequiredQuestList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
