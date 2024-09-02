using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMandragoraGetMyMandragoraRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MANDRAGORA_GET_MY_MANDRAGORA_RES;

        public List<CDataMyMandragoraFurnitureItem> MandragoraFurnitureItemList { get; set; }
        public List<CDataMyMandragora> MandragoraList { get; set; }
        public List<CDataMyMandragoraCraftCategory> MandragoraCraftCategoriesMaybe { get; set; }
        public List<CDataMyMandragoraUnk3> Unk3 { get; set; }
        public List<CDataMyMandragoraFertilizerItem> MandragoraFertilizerItemList { get; set; }
        public uint MandragoraCultivationMaterialMaxMaybe;
        public List<CDataMyMandragoraSpeciesCategory> MandragoraSpeciesCategoryList { get; set; }
        public List<CDataMyMandragoraRarityLevel> RarityLevelList { get; set; }
        public List<CDataCommonU8> FreeMandragoraIdListMaybe { get; set; }
        public uint Unk9;

        public S2CMandragoraGetMyMandragoraRes()
        {
            MandragoraFurnitureItemList = new List<CDataMyMandragoraFurnitureItem>();
            MandragoraList = new List<CDataMyMandragora>();
            MandragoraCraftCategoriesMaybe = new List<CDataMyMandragoraCraftCategory>();
            Unk3 = new List<CDataMyMandragoraUnk3>();
            MandragoraFertilizerItemList = new List<CDataMyMandragoraFertilizerItem>();
            MandragoraSpeciesCategoryList = new List<CDataMyMandragoraSpeciesCategory>();
            RarityLevelList = new List<CDataMyMandragoraRarityLevel>();
            FreeMandragoraIdListMaybe = new List<CDataCommonU8>();
        }

        public class Serializer : PacketEntitySerializer<S2CMandragoraGetMyMandragoraRes>
        {
            public override void Write(IBuffer buffer, S2CMandragoraGetMyMandragoraRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataMyMandragoraFurnitureItem>(buffer, obj.MandragoraFurnitureItemList);
                WriteEntityList<CDataMyMandragora>(buffer, obj.MandragoraList);
                WriteEntityList<CDataMyMandragoraCraftCategory>(buffer, obj.MandragoraCraftCategoriesMaybe);
                WriteEntityList<CDataMyMandragoraUnk3>(buffer, obj.Unk3);
                WriteEntityList<CDataMyMandragoraFertilizerItem>(buffer, obj.MandragoraFertilizerItemList);
                WriteUInt32(buffer, obj.MandragoraCultivationMaterialMaxMaybe);
                WriteEntityList<CDataMyMandragoraSpeciesCategory>(buffer, obj.MandragoraSpeciesCategoryList);
                WriteEntityList<CDataMyMandragoraRarityLevel>(buffer, obj.RarityLevelList);
                WriteEntityList<CDataCommonU8>(buffer, obj.FreeMandragoraIdListMaybe);
                WriteUInt32(buffer, obj.Unk9);
            }

            public override S2CMandragoraGetMyMandragoraRes Read(IBuffer buffer)
            {
                S2CMandragoraGetMyMandragoraRes obj = new S2CMandragoraGetMyMandragoraRes();

                ReadServerResponse(buffer, obj);

                obj.MandragoraFurnitureItemList = ReadEntityList<CDataMyMandragoraFurnitureItem>(buffer);
                obj.MandragoraList = ReadEntityList<CDataMyMandragora>(buffer);
                obj.MandragoraCraftCategoriesMaybe = ReadEntityList<CDataMyMandragoraCraftCategory>(buffer);
                obj.Unk3 = ReadEntityList<CDataMyMandragoraUnk3>(buffer);
                obj.MandragoraFertilizerItemList = ReadEntityList<CDataMyMandragoraFertilizerItem>(buffer);
                obj.MandragoraCultivationMaterialMaxMaybe = ReadUInt32(buffer);
                obj.MandragoraSpeciesCategoryList = ReadEntityList<CDataMyMandragoraSpeciesCategory>(buffer);
                obj.RarityLevelList = ReadEntityList<CDataMyMandragoraRarityLevel>(buffer);
                obj.FreeMandragoraIdListMaybe = ReadEntityList<CDataCommonU8>(buffer);
                obj.Unk9 = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
