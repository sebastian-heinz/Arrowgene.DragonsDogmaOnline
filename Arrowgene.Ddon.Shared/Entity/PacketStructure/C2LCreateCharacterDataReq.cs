using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2LCreateCharacterDataReq
    {

        public C2LCreateCharacterDataReq()
        {
            CharacterInfo = new CDataCharacterInfo();
            WaitNum = 0;
            RotationServerID = 0;
        }

        public CDataCharacterInfo CharacterInfo;
        public uint WaitNum;
        public byte RotationServerID;
    }

    public class C2LCreateCharacterDataReqSerializer : EntitySerializer<C2LCreateCharacterDataReq>
    {
        public override void Write(IBuffer buffer, C2LCreateCharacterDataReq obj)
        {
            WriteEntity(buffer, obj.CharacterInfo);
            WriteUInt32(buffer, obj.WaitNum);
            WriteByte(buffer, obj.RotationServerID);
        }

        public override C2LCreateCharacterDataReq Read(IBuffer buffer)
        {
            C2LCreateCharacterDataReq obj = new C2LCreateCharacterDataReq();
            obj.CharacterInfo = ReadEntity<CDataCharacterInfo>(buffer);
            obj.WaitNum = ReadUInt32(buffer);
            obj.RotationServerID = ReadByte(buffer);
            return obj;
        }
    }
}
