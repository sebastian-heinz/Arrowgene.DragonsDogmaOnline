using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;


namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemUseBagItemRes : IPacketStructure
    {
        public S2CItemUseBagItemRes()
        {
            //MyPawnCsvData = new List<MyPawnCsv>();
            Req = new C2SItemUseBagItemReq();
        }

        public S2CItemUseBagItemRes(C2SItemUseBagItemReq req)
        {
            Req = req;
        }

        public C2SItemUseBagItemReq Req { get; set; }
        public PacketId Id => PacketId.S2C_ITEM_USE_BAG_ITEM_RES;


        public class Serializer : PacketEntitySerializer<S2CItemUseBagItemRes>
        {

            public override void Write(IBuffer buffer, S2CItemUseBagItemRes obj)
            {
                C2SItemUseBagItemReq req = obj.Req;
                string text = req.ItemUID;
                byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(text);
                WriteByteArray(buffer, obj.Pad6);
                WriteUInt16(buffer, 0);
                WriteByteArray(buffer, utf8);
                
                //Temp
                uint i = 0;
                if (req.ItemUID == "12345678") { i = 13807; }
                if (req.ItemUID == "12345679") { i = 11407; }
                if (req.ItemUID == "12345680") { i = 9378; }
                if (req.ItemUID == "12345681") { i = 13801; }
                if (req.ItemUID == "12345682") { i = 55; }
                if (req.ItemUID == "12345683") { i = 9387; }
                if (req.ItemUID == "12345684") { i = 9389; }
                if (req.ItemUID == "12345685") { i = 9429; }
                if (req.ItemUID == "12345686") { i = 47; }
                if (req.ItemUID == "12345687") { i = 9404; }
                if (req.ItemUID == "12345688") { i = 9405; }
                if (req.ItemUID == "12345689") { i = 9406; }
                WriteUInt32(buffer, i);
                WriteByteArray(buffer, obj.Pad3);
            }

            public override S2CItemUseBagItemRes Read(IBuffer buffer)
            {
                S2CItemUseBagItemRes obj = new S2CItemUseBagItemRes();
                return obj;
            }

        }

        private readonly byte[] Pad6 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Pad3 = { 0x0, 0x0, 0x0 };

    }
}
