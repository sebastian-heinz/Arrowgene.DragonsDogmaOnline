using System.Linq;
using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CShopGetShopGoodsListRes : ServerResponse, ICloneable
    {
        public override PacketId Id => PacketId.S2C_SHOP_GET_SHOP_GOODS_LIST_RES;

        public S2CShopGetShopGoodsListRes()
        {
            GoodsParamList = new List<CDataGoodsParam>();
        }

        public uint Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public WalletType WalletType { get; set; }
        public List<CDataGoodsParam> GoodsParamList { get; set; }

        public object Clone()
        {
            return new S2CShopGetShopGoodsListRes()
            {
                Unk0 = this.Unk0,
                Unk1 = this.Unk1,
                WalletType = this.WalletType,
                GoodsParamList = GoodsParamList.Select(gp => (CDataGoodsParam) gp.Clone()).ToList()
            };
        }

        public class Serializer : PacketEntitySerializer<S2CShopGetShopGoodsListRes>
        {
            public override void Write(IBuffer buffer, S2CShopGetShopGoodsListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteByte(buffer, (byte) obj.WalletType);
                WriteEntityList<CDataGoodsParam>(buffer, obj.GoodsParamList);
            }

            public override S2CShopGetShopGoodsListRes Read(IBuffer buffer)
            {
                S2CShopGetShopGoodsListRes obj = new S2CShopGetShopGoodsListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.WalletType = (WalletType) ReadByte(buffer);
                obj.GoodsParamList = ReadEntityList<CDataGoodsParam>(buffer);
                return obj;
            }
        }
    }
}