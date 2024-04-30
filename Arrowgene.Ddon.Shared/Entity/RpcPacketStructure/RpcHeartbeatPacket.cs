using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;

namespace Arrowgene.Ddon.Shared.Entity.RpcPacketStructure
{
    public class RpcHeartbeatPacket : RpcPacketBase
    {
        public RpcHeartbeatPacket()
        {
        }

        public UInt64 Unk0 { get; set; }
        public bool IsEnemy { get; set; }
        public bool IsCharacter {  get; set; }
        public bool IsHuman { get; set; }
        public bool IsEnemyLarge {  get; set; }

        public double PosX { get; set; }
        public float PosY { get; set; }
        public double PosZ { get; set; }

        public UInt32 Unk1 { get; set; }
        public UInt32 Unk2 { get; set; }
        public UInt32 Unk3 { get; set; }

        public UInt16 GreenHP { get; set; }
        public UInt16 WhiteHP {  get; set; }
        public UInt16 Unk4 { get; set; }
        public UInt16 Stamina { get; set; }

        public override void Handle(Character character, IBuffer buffer)
        {
            RpcHeartbeatPacket obj = Read(buffer);

            if (obj.IsCharacter)
            {
                character.X = obj.PosX;
                character.Y = obj.PosY;
                character.Z = obj.PosZ;
                character.GreenHp = obj.GreenHP;
                character.WhiteHp = obj.WhiteHP;
            }
        }

        private RpcHeartbeatPacket Read(IBuffer buffer)
        {
            RpcHeartbeatPacket obj = new RpcHeartbeatPacket();
            obj.Unk0 = ReadUInt64(buffer); // nNetMsgData::CtrlBase::stMsgCtrlBaseData.mUniqueId ?
            obj.IsEnemy = ReadBool(buffer);
            obj.IsCharacter = ReadBool(buffer);
            obj.IsHuman = ReadBool(buffer);
            obj.IsEnemyLarge = ReadBool(buffer);
            obj.PosX = ReadDouble(buffer);
            obj.PosY = ReadFloat(buffer);
            obj.PosZ = ReadDouble(buffer);
            obj.Unk1 = ReadUInt32(buffer);
            obj.Unk2 = ReadUInt32(buffer);
            obj.Unk3 = ReadUInt32(buffer);
            obj.GreenHP = ReadUInt16(buffer);
            obj.WhiteHP = ReadUInt16(buffer);
            obj.Unk4 = ReadUInt16(buffer);
            obj.Stamina = ReadUInt16(buffer);
            return obj;
        }
    }
}
