using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawn_8_37_16_Ntc : IPacketStructure
    {
        public S2CPawn_8_37_16_Ntc()
        {
            MyPawnCsvData = new List<MyPawnCsv>();
            Req = new C2SPawnJoinPartyMypawnReq();
        }

        public S2CPawn_8_37_16_Ntc(List<MyPawnCsv> myPawnCsvData, C2SPawnJoinPartyMypawnReq req)
        {
            MyPawnCsvData = myPawnCsvData;
            Req = req;
        }

        public List<MyPawnCsv> MyPawnCsvData { get; set; }
        public C2SPawnJoinPartyMypawnReq Req { get; set; }
        public PacketId Id => PacketId.S2C_PAWN_8_37_16_NTC;

        private class MyPawnMemberIndex
        {
            static int index;
            int id;

            static MyPawnMemberIndex()
            {
                index = 1;
            }

            public MyPawnMemberIndex()
            {
                id = index;
                index++;
            }

            public int ID
            {
                get { return id; }
            }
        
        }

        public class Serializer : PacketEntitySerializer<S2CPawn_8_37_16_Ntc>
        {
            public override void Write(IBuffer buffer, S2CPawn_8_37_16_Ntc obj)
            {

                C2SPawnJoinPartyMypawnReq req = obj.Req;
                int n = req.PawnNumber;
                n--;

                MyPawnMemberIndex myPawnMemberIndex = new MyPawnMemberIndex();
                MyPawnCsv myPawnCsvData = obj.MyPawnCsvData[n];
                WriteUInt32(buffer, myPawnCsvData.CharacterId);
                WriteMtString(buffer, myPawnCsvData.Name);
                WriteByteArray(buffer, obj.Pad7);
                WriteByte(buffer, myPawnCsvData.Job);
                WriteByte(buffer, myPawnCsvData.JobLv);
                WriteByteArray(buffer, obj.Pad5);
                WriteByte(buffer, 2);
                WriteInt32(buffer, myPawnMemberIndex.ID);
                //WriteUInt32(buffer, myPawnCsvData.MemberIndex);   //CSV
                WriteUInt32(buffer, myPawnCsvData.PawnId);
                WriteUInt16(buffer, 1);
                WriteUInt16(buffer, 2);
                WriteByteArray(buffer, obj.Array83716);
            }

            public override S2CPawn_8_37_16_Ntc Read(IBuffer buffer)
            {
                S2CPawn_8_37_16_Ntc obj = new S2CPawn_8_37_16_Ntc();
                MyPawnCsv myPawnCsvData = new MyPawnCsv();

                return obj;
            }

        }



        private readonly byte[] Pad5 = { 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Pad7 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Array83716 =
        {
            0x0, 0xDA, 0x5D, 0x4E, 0x0, 0x1, 0x0, 0x2, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
            0x0, 0xA0, 0x8C, 0x90, 0x2F, 0x0, 0x0, 0x0
        };

    }
}
