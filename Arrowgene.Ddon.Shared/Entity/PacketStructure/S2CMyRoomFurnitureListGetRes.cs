using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomFurnitureListGetRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_MY_ROOM_FURNITURE_LIST_GET_RES;

        public S2CMyRoomFurnitureListGetRes()
        {
        }

        public C2SMyRoomFurnitureListGetReq ReqData { get; set; }
        public List<MyRoomCsv> MyRoomCsv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMyRoomFurnitureListGetRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomFurnitureListGetRes obj)
            {
                MyRoomCsv myRoomCsv = obj.MyRoomCsv[0];
                WriteUInt64(buffer, 0);
                WriteUInt32(buffer, 96);
                WriteUInt32(buffer, myRoomCsv.TableType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 1);
                WriteUInt32(buffer, myRoomCsv.SideTable);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 2);
                WriteUInt32(buffer, myRoomCsv.CabinetType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 3);
                WriteUInt32(buffer, myRoomCsv.CarpetTypeReadingRoom);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 4);
                WriteUInt32(buffer, myRoomCsv.SingleChair);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 5);
                WriteUInt32(buffer, myRoomCsv.WallBookshelf);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 6);
                WriteUInt32(buffer, myRoomCsv.KitchenTableType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 7);
                WriteUInt32(buffer, myRoomCsv.FullKettleSet);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 8);
                WriteUInt32(buffer, myRoomCsv.Cuisine);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 9);
                WriteUInt32(buffer, myRoomCsv.PlushDollA);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 10);
                WriteUInt32(buffer, myRoomCsv.PlushDollB);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 11);
                WriteUInt32(buffer, myRoomCsv.LeftKitchenShelfFoodstuffs);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 12);
                WriteUInt32(buffer, myRoomCsv.CookwareSet);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 13);
                WriteUInt32(buffer, myRoomCsv.BedType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 14);
                WriteUInt32(buffer, myRoomCsv.PuppetType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 15);
                WriteUInt32(buffer, myRoomCsv.LampType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 17);
                WriteUInt32(buffer, myRoomCsv.LivingTableType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 18);
                WriteUInt32(buffer, myRoomCsv.CuisineType);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 19);
                WriteUInt32(buffer, myRoomCsv.Chest);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 20);
                WriteUInt32(buffer, myRoomCsv.ChestFigurine);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 21);
                WriteUInt32(buffer, myRoomCsv.Bath);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 25);
                WriteUInt32(buffer, myRoomCsv.ArisensRoomDecor);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 26);
                WriteUInt32(buffer, myRoomCsv.Object0);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 27);
                WriteUInt32(buffer, myRoomCsv.ObjectB);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 28);
                WriteUInt32(buffer, myRoomCsv.ArisensLoungewear);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 29);
                WriteUInt32(buffer, myRoomCsv.ServantsLoungewear);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 30);
                WriteUInt32(buffer, myRoomCsv.ServantsBathingClothes);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 31);
                WriteUInt32(buffer, myRoomCsv.ServantsSleepwear);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 32);
                WriteUInt32(buffer, myRoomCsv.ObjectA);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 34);
                WriteUInt32(buffer, myRoomCsv.ArisensRoomLighting);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 35);
                WriteUInt32(buffer, myRoomCsv.TableFlowerA);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 36);
                WriteUInt32(buffer, myRoomCsv.TableFlowerB);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 37);
                WriteUInt32(buffer, myRoomCsv.WeatherForecast);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 38);
                WriteUInt32(buffer, myRoomCsv.Wallpaper);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 39);
                WriteUInt32(buffer, myRoomCsv.Spout);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 40);
                WriteUInt32(buffer, myRoomCsv.CuckooClock);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 41);
                WriteUInt32(buffer, myRoomCsv.LeftWallFurniture);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 42);
                WriteUInt32(buffer, myRoomCsv.RightWallFurniture);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 43);
                WriteUInt32(buffer, myRoomCsv.CarpetTypeLivingRoom);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 44);
                WriteUInt32(buffer, myRoomCsv.Furniture);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 45);
                WriteUInt32(buffer, myRoomCsv.SummoningBell);
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 46);
                if (myRoomCsv.HideMandragora1 == false) { WriteUInt32(buffer, 17370); } else { WriteUInt32(buffer, 0); }
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 47);
                if (myRoomCsv.HideMandragora2 == false) { WriteUInt32(buffer, 17371); } else { WriteUInt32(buffer, 0); }
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 48);
                if (myRoomCsv.HideMandragora3 == false) { WriteUInt32(buffer, 17372); } else { WriteUInt32(buffer, 0); }
                WriteUInt32(buffer, 0);
                WriteByte(buffer, 49);
                WriteByteArray(buffer, Data0);
                WriteUInt32(buffer, 0);
                WriteUInt32(buffer, myRoomCsv.Music);
                WriteByte(buffer, 0);
                WriteByteArray(buffer, Data1);
            }

            public override S2CMyRoomFurnitureListGetRes Read(IBuffer buffer)
            {
                S2CMyRoomFurnitureListGetRes obj = new S2CMyRoomFurnitureListGetRes();
                return obj;
            }


            private readonly byte[] Data0 =
            {
                0x0, 0x0, 0x53, 0xE7, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x53, 0xE6, 0x0, 0xF, 0x42,
                0x3E, 0x0, 0x0, 0x0, 0x53, 0xE5, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x53, 0xE4, 0x0,
                0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x53, 0xE3, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x53,
                0xE2, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x53, 0xE0, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0,
                0x0, 0x53, 0xDF, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x53, 0xDE, 0x0, 0xF, 0x42, 0x3E,
                0x0, 0x0, 0x0, 0x53, 0xE1, 0x0, 0xF, 0x42, 0x3E, 0x0, 0x0, 0x0, 0x54, 0x35, 0x0, 0xF,
                0x42, 0x3E, 0x0, 0x0, 0x0, 0x5B, 0xE5, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5B, 0xE6,
                0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5B, 0xE7, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0,
                0x5B, 0xE8, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5B, 0xEA, 0x0, 0xF, 0x42, 0x3D, 0x0,
                0x0, 0x0, 0x5B, 0xEB, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5B, 0xEC, 0x0, 0xF, 0x42,
                0x3D, 0x0, 0x0, 0x0, 0x5B, 0xFF, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x0, 0x0,
                0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x1, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C,
                0x2, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x3, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0,
                0x0, 0x5C, 0x4, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x5, 0x0, 0xF, 0x42, 0x3D,
                0x0, 0x0, 0x0, 0x5C, 0x6, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x7, 0x0, 0xF,
                0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x8, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x9,
                0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0xA, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0,
                0x5C, 0xB, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0xC, 0x0, 0xF, 0x42, 0x3D, 0x0,
                0x0, 0x0, 0x5C, 0xD, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0xE, 0x0, 0xF, 0x42,
                0x3D, 0x0, 0x0, 0x0, 0x5C, 0xF, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x10, 0x0,
                0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x11, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C,
                0x12, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x13, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0,
                0x0, 0x5C, 0x14, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x15, 0x0, 0xF, 0x42, 0x3D,
                0x0, 0x0, 0x0, 0x5C, 0x16, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x17, 0x0, 0xF,
                0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x18, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x19,
                0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x1A, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0,
                0x5C, 0x1B, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x1C, 0x0, 0xF, 0x42, 0x3D, 0x0,
                0x0, 0x0, 0x5C, 0x1D, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5C, 0x1E, 0x0, 0xF, 0x42,
                0x3D, 0x0, 0x0, 0x0, 0x5C, 0x1F, 0x0, 0xF, 0x42, 0x3D, 0x0, 0x0, 0x0, 0x5B, 0xE9, 0x0,
                0xF, 0x42, 0x3D, 0x0
            };

            private readonly byte[] Data1 =
            {
                0x0, 0x0, 0x0, 0x0, 0x6, 0x0, 0x0, 0x0
            };
        }

    }
}
