using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum EquipJobList
    {
        All = 0,
        Fighter = 1,
        Seeker = 2,
        Hunter = 3,
        Priest = 4,
        ShieldSage = 5,
        Sorcerer = 6,
        Warrior = 7,
        ElementArcher = 8,
        Alchemist = 9,
        SpiritLancer = 10,
        HighScepter = 11,
        GroupHeavy = 12, // Fighter, Warrior
        GroupPhysical = 13, // Fighter, Seeker, Hunter, Warrior, Spirit Lancer
        GroupLight = 14, // Seeker, Hunter, Spirit Lancer
        GroupMagickRanged = 15, // Priest, Sorcerer, Element Archer
        GroupMagickal = 16, //Priest, Shield Sage, Sorcerer, Element Archer, Alchemist, High Scepter
        GroupMagickMelee = 17, //Shield Sage, Alchemist, High Scepter
    }
}
