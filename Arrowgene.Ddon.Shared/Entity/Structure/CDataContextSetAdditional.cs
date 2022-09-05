using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextSetAdditional
    {
        public CDataContextSetAdditional()
        {
            Hp = new ulong[10];
            OcdActiveList = new List<CDataOcdActive>();
            ImmuneLvList = new List<CDataOcdImmuneLv>();
            EncountAreaBuff = new int[3];
            EnemyStatusGroupNo = new uint[16];
            EnemyStatusSubGroupNo = new uint[16];
            Unk0 = new uint[4];
        }

        public int MasterIndex { get; set; }
        public bool MasterChangeFlag { get; set; }
        public double PosX { get; set; }
        public float PosY { get; set; }
        public double PosZ { get; set; }
        public float MoveSpeed { get; set; }
        public float MoveAngle { get; set; }
        public float AngleY { get; set; }
        public uint ActNo { get; set; }
        public byte ActReqPrio { get; set; }
        public ushort ActAtkAdjustUniqueId { get; set; }
        public ulong ActFreeWork { get; set; }
        public ulong[] Hp { get; set; } // Array of 10 ulongs
        public ushort UseRegionBit { get; set; }
        public ulong HpWhite { get; set; }
        public ushort CommonWork { get; set; }
        public ushort CustomWork { get; set; }
        public List<CDataOcdActive> OcdActiveList { get; set; }
        public List<CDataOcdImmuneLv> ImmuneLvList { get; set; }
        public float EnchantRate { get; set; }
        public float AilmentDamage { get; set; }
        public double CliffPosX { get; set; }
        public float CliffPosY { get; set; }
        public double CliffPosZ { get; set; }
        public float CliffNormalX { get; set; }
        public float CliffNormalY { get; set; }
        public float CliffNormalZ { get; set; }
        public double CliffStartPosX { get; set; }
        public float CliffStartPosY { get; set; }
        public double CliffStartPosZ { get; set; }
        public double CliffStartOldPosX { get; set; }
        public float CliffStartOldPosY { get; set; }
        public double CliffStartOldPosZ { get; set; }
        public ulong ClimbEnemyID { get; set; }
        public int JointNo { get; set; }
        public float JointOffsetX { get; set; }
        public float JointOffsetY { get; set; }
        public float JointOffsetZ { get; set; }
        public ushort NodeIndex { get; set; }
        public ushort GeomIndex { get; set; }
        public ulong TargetUID { get; set; }
        public double TargetPosX { get; set; }
        public float TargetPosY { get; set; }
        public double TargetPosZ { get; set; }
        public int[] EncountAreaBuff { get; set; } // Array of 3 ints
        public uint EnemyStatusGroupNoNum { get; set; } // Not 100% sure of this one
        public uint[] EnemyStatusGroupNo { get; set; } // Array of 16 uints
        public uint[] EnemyStatusSubGroupNo { get; set; } // Array of 16 uints
        public ulong EnemyBitCtrlCollision { get; set; }
        public byte EnemyCtrlRegionSelectNo { get; set; }
        public ulong EnemyBitCtrlParts { get; set; }
        public ulong EnemyBitCtrlWorkRate { get; set; }
        public ulong EnemyBitCtrlScale { get; set; }
        public ulong EnemyBitCtrlMontage { get; set; }
        public uint EnemyBitCtrlSyncBit { get; set; }
        public bool IsEnemyWaiting { get; set; }
        public bool IsEnemyStartWait { get; set; }
        public ulong AttackParamUIDForDmVec { get; set; }
        public double WallClimbPosX { get; set; }
        public float WallClimbPosY { get; set; }
        public double WallClimbPosZ { get; set; }
        public float WallClimbNormalX { get; set; }
        public float WallClimbNormalY { get; set; }
        public float WallClimbNormalZ { get; set; }
        public double WallClimbLandPosX { get; set; }
        public float WallClimbLandPosY { get; set; }
        public double WallClimbLandPosZ { get; set; }
        public uint StateLive { get; set; }
        public byte CatchType { get; set; }
        public byte CatchJointNo { get; set; }
        public ulong CatchTargetUID { get; set; }
        public uint QuestId { get; set; }
        public uint[] Unk0 { get; set; } // Array of 4 uints

        public class Serializer : EntitySerializer<CDataContextSetAdditional> {
            public override void Write(IBuffer buffer, CDataContextSetAdditional obj)
            {
                WriteInt32(buffer, obj.MasterIndex);
                WriteBool(buffer, obj.MasterChangeFlag);
                WriteDouble(buffer, obj.PosX);
                WriteFloat(buffer, obj.PosY);
                WriteDouble(buffer, obj.PosZ);
                WriteFloat(buffer, obj.MoveSpeed);
                WriteFloat(buffer, obj.MoveAngle);
                WriteFloat(buffer, obj.AngleY);
                WriteUInt32(buffer, obj.ActNo);
                WriteByte(buffer, obj.ActReqPrio);
                WriteUInt16(buffer, obj.ActAtkAdjustUniqueId);
                WriteUInt64(buffer, obj.ActFreeWork);
                WriteUInt64Array(buffer, obj.Hp);
                WriteUInt16(buffer, obj.UseRegionBit);
                WriteUInt64(buffer, obj.HpWhite);
                WriteUInt16(buffer, obj.CommonWork);
                WriteUInt16(buffer, obj.CustomWork);
                WriteEntityList<CDataOcdActive>(buffer, obj.OcdActiveList);
                WriteEntityList<CDataOcdImmuneLv>(buffer, obj.ImmuneLvList);
                WriteFloat(buffer, obj.EnchantRate);
                WriteFloat(buffer, obj.AilmentDamage);
                WriteDouble(buffer, obj.CliffPosX);
                WriteFloat(buffer, obj.CliffPosY);
                WriteDouble(buffer, obj.CliffPosZ);
                WriteFloat(buffer, obj.CliffNormalX);
                WriteFloat(buffer, obj.CliffNormalY);
                WriteFloat(buffer, obj.CliffNormalZ);
                WriteDouble(buffer, obj.CliffStartPosX);
                WriteFloat(buffer, obj.CliffStartPosY);
                WriteDouble(buffer, obj.CliffStartPosZ);
                WriteDouble(buffer, obj.CliffStartOldPosX);
                WriteFloat(buffer, obj.CliffStartOldPosY);
                WriteDouble(buffer, obj.CliffStartOldPosZ);
                WriteUInt64(buffer, obj.ClimbEnemyID);
                WriteInt32(buffer, obj.JointNo);
                WriteFloat(buffer, obj.JointOffsetX);
                WriteFloat(buffer, obj.JointOffsetY);
                WriteFloat(buffer, obj.JointOffsetZ);
                WriteUInt16(buffer, obj.NodeIndex);
                WriteUInt16(buffer, obj.GeomIndex);
                WriteUInt64(buffer, obj.TargetUID);
                WriteDouble(buffer, obj.TargetPosX);
                WriteFloat(buffer, obj.TargetPosY);
                WriteDouble(buffer, obj.TargetPosZ);
                WriteInt32Array(buffer, obj.EncountAreaBuff);
                WriteUInt32(buffer, obj.EnemyStatusGroupNoNum);
                WriteUInt32Array(buffer, obj.EnemyStatusGroupNo);
                WriteUInt32Array(buffer, obj.EnemyStatusSubGroupNo);
                WriteUInt64(buffer, obj.EnemyBitCtrlCollision);
                WriteByte(buffer, obj.EnemyCtrlRegionSelectNo);
                WriteUInt64(buffer, obj.EnemyBitCtrlParts);
                WriteUInt64(buffer, obj.EnemyBitCtrlWorkRate);
                WriteUInt64(buffer, obj.EnemyBitCtrlScale);
                WriteUInt64(buffer, obj.EnemyBitCtrlMontage);
                WriteUInt32(buffer, obj.EnemyBitCtrlSyncBit);
                WriteBool(buffer, obj.IsEnemyWaiting);
                WriteBool(buffer, obj.IsEnemyStartWait);
                WriteUInt64(buffer, obj.AttackParamUIDForDmVec);
                WriteDouble(buffer, obj.WallClimbPosX);
                WriteFloat(buffer, obj.WallClimbPosY);
                WriteDouble(buffer, obj.WallClimbPosZ);
                WriteFloat(buffer, obj.WallClimbNormalX);
                WriteFloat(buffer, obj.WallClimbNormalY);
                WriteFloat(buffer, obj.WallClimbNormalZ);
                WriteDouble(buffer, obj.WallClimbLandPosX);
                WriteFloat(buffer, obj.WallClimbLandPosY);
                WriteDouble(buffer, obj.WallClimbLandPosZ);
                WriteUInt32(buffer, obj.StateLive);
                WriteByte(buffer, obj.CatchType);
                WriteByte(buffer, obj.CatchJointNo);
                WriteUInt64(buffer, obj.CatchTargetUID);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32Array(buffer, obj.Unk0);
            }

            public override CDataContextSetAdditional Read(IBuffer buffer)
            {
                CDataContextSetAdditional obj = new CDataContextSetAdditional();
                obj.MasterIndex = ReadInt32(buffer);
                obj.MasterChangeFlag = ReadBool(buffer);
                obj.PosX = ReadDouble(buffer);
                obj.PosY = ReadFloat(buffer);
                obj.PosZ = ReadDouble(buffer);
                obj.MoveSpeed = ReadFloat(buffer);
                obj.MoveAngle = ReadFloat(buffer);
                obj.AngleY = ReadFloat(buffer);
                obj.ActNo = ReadUInt32(buffer);
                obj.ActReqPrio = ReadByte(buffer);
                obj.ActAtkAdjustUniqueId = ReadUInt16(buffer);
                obj.ActFreeWork = ReadUInt64(buffer);
                obj.Hp = ReadUInt64Array(buffer, 10);
                obj.UseRegionBit = ReadUInt16(buffer);
                obj.HpWhite = ReadUInt64(buffer);
                obj.CommonWork = ReadUInt16(buffer);
                obj.CustomWork = ReadUInt16(buffer);
                obj.OcdActiveList = ReadEntityList<CDataOcdActive>(buffer);
                obj.ImmuneLvList = ReadEntityList<CDataOcdImmuneLv>(buffer);
                obj.EnchantRate = ReadFloat(buffer);
                obj.AilmentDamage = ReadFloat(buffer);
                obj.CliffPosX = ReadDouble(buffer);
                obj.CliffPosY = ReadFloat(buffer);
                obj.CliffPosZ = ReadDouble(buffer);
                obj.CliffNormalX = ReadFloat(buffer);
                obj.CliffNormalY = ReadFloat(buffer);
                obj.CliffNormalZ = ReadFloat(buffer);
                obj.CliffStartPosX = ReadDouble(buffer);
                obj.CliffStartPosY = ReadFloat(buffer);
                obj.CliffStartPosZ = ReadDouble(buffer);
                obj.CliffStartOldPosX = ReadDouble(buffer);
                obj.CliffStartOldPosY = ReadFloat(buffer);
                obj.CliffStartOldPosZ = ReadDouble(buffer);
                obj.ClimbEnemyID = ReadUInt64(buffer);
                obj.JointNo = ReadInt32(buffer);
                obj.JointOffsetX = ReadFloat(buffer);
                obj.JointOffsetY = ReadFloat(buffer);
                obj.JointOffsetZ = ReadFloat(buffer);
                obj.NodeIndex = ReadUInt16(buffer);
                obj.GeomIndex = ReadUInt16(buffer);
                obj.TargetUID = ReadUInt64(buffer);
                obj.TargetPosX = ReadDouble(buffer);
                obj.TargetPosY = ReadFloat(buffer);
                obj.TargetPosZ = ReadDouble(buffer);
                obj.EncountAreaBuff = ReadInt32Array(buffer, 3);
                obj.EnemyStatusGroupNoNum = ReadUInt32(buffer);
                obj.EnemyStatusGroupNo = ReadUInt32Array(buffer, 16);
                obj.EnemyStatusSubGroupNo = ReadUInt32Array(buffer, 16);
                obj.EnemyBitCtrlCollision = ReadUInt64(buffer);
                obj.EnemyCtrlRegionSelectNo = ReadByte(buffer);
                obj.EnemyBitCtrlParts = ReadUInt64(buffer);
                obj.EnemyBitCtrlWorkRate = ReadUInt64(buffer);
                obj.EnemyBitCtrlScale = ReadUInt64(buffer);
                obj.EnemyBitCtrlMontage = ReadUInt64(buffer);
                obj.EnemyBitCtrlSyncBit = ReadUInt32(buffer);
                obj.IsEnemyWaiting = ReadBool(buffer);
                obj.IsEnemyStartWait = ReadBool(buffer);
                obj.AttackParamUIDForDmVec = ReadUInt64(buffer);
                obj.WallClimbPosX = ReadDouble(buffer);
                obj.WallClimbPosY = ReadFloat(buffer);
                obj.WallClimbPosZ = ReadDouble(buffer);
                obj.WallClimbNormalX = ReadFloat(buffer);
                obj.WallClimbNormalY = ReadFloat(buffer);
                obj.WallClimbNormalZ = ReadFloat(buffer);
                obj.WallClimbLandPosX = ReadDouble(buffer);
                obj.WallClimbLandPosY = ReadFloat(buffer);
                obj.WallClimbLandPosZ = ReadDouble(buffer);
                obj.StateLive = ReadUInt32(buffer);
                obj.CatchType = ReadByte(buffer);
                obj.CatchJointNo = ReadByte(buffer);
                obj.CatchTargetUID = ReadUInt64(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt32Array(buffer, 4);
                return obj;
            }
        }
    }
}