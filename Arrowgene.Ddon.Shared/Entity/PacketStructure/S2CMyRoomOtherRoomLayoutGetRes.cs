using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomOtherRoomLayoutGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_OTHER_ROOM_LAYOUT_GET_RES;

        public uint CharacterId { get; set; }
        public List<CDataFurnitureLayout> FurnitureList { get; set; } = new();
        public uint BgmAcquirementNo { get; set; }
        public uint ActivePlanetariumNo { get; set; }
        public uint PawnId { get; set; } // PawnId?
        public CDataNoraPawnInfo PawnInfo { get; set; } = new();
        public List<CDataMyMandragora> MandragoraList { get; set; } = new();
        public bool UnlockTerrace { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMyRoomOtherRoomLayoutGetRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomOtherRoomLayoutGetRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList(buffer, obj.FurnitureList);
                WriteUInt32(buffer, obj.BgmAcquirementNo);
                WriteUInt32(buffer, obj.ActivePlanetariumNo);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity(buffer, obj.PawnInfo);
                WriteEntityList(buffer, obj.MandragoraList);
                WriteBool(buffer, obj.UnlockTerrace);
            }

            public override S2CMyRoomOtherRoomLayoutGetRes Read(IBuffer buffer)
            {
                S2CMyRoomOtherRoomLayoutGetRes obj = new S2CMyRoomOtherRoomLayoutGetRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.FurnitureList = ReadEntityList<CDataFurnitureLayout>(buffer);
                obj.BgmAcquirementNo = ReadUInt32(buffer);
                obj.ActivePlanetariumNo = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnInfo = ReadEntity<CDataNoraPawnInfo>(buffer);
                obj.MandragoraList = ReadEntityList<CDataMyMandragora>(buffer);
                obj.UnlockTerrace = ReadBool(buffer);

                return obj;
            }
        }
    }
}
