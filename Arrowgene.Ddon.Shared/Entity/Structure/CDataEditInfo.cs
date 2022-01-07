using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEditInfo
    {
        byte Sex;
        byte Voice;
        ushort VoicePitch;
        byte Personality;
        byte SpeechFreq;
        byte BodyType;
        byte Hair;
        byte Beard;
        byte Makeup;
        byte Scar;
        byte EyePresetNo;
        byte NosePresetNo;
        byte MouthPresetNo;
        byte EyebrowTexNo;
        byte ColorSkin;
        byte ColorHair;
        byte ColorBeard;
        byte ColorEyebrow;
        byte ColorREye;
        byte ColorLEye;
        byte ColorMakeup;
        ushort Sokutobu;
        ushort Hitai;
        ushort MimiJyouge;
        ushort Kannkaku;
        ushort MabisasiJyouge;
        ushort HanakuchiJyouge;
        ushort AgoSakiHaba;
        ushort AgoZengo;
        ushort AgoSakiJyouge;
        ushort HitomiOokisa;
        ushort MeOokisa;
        ushort MeKaiten;
        ushort MayuKaiten;
        ushort MimiOokisa;
        ushort MimiMuki;
        ushort ElfMimi;
        ushort MikenTakasa;
        ushort MikenHaba;
        ushort HohoboneRyou;
        ushort HohoboneJyouge;
        ushort Hohoniku;
        ushort ErahoneJyouge;
        ushort ErahoneHaba;
        ushort HanaJyouge;
        ushort HanaHaba;
        ushort HanaTakasa;
        ushort HanaKakudo;
        ushort KuchiHaba;
        ushort KuchiAtsusa;
        ushort EyebrowUVOffsetX;
        ushort EyebrowUVOffsetY;
        ushort Wrinkle;
        ushort WrinkleAlbedoBlendRate;
        ushort WrinkleDetailNormalPower;
        ushort MuscleAlbedoBlendRate;
        ushort MuscleDetailNormalPower;
        ushort Height;
        ushort HeadSize;
        ushort NeckOffset;
        ushort NeckScale;
        ushort UpperBodyScaleX;
        ushort BellySize;
        ushort TeatScale;
        ushort TekubiSize;
        ushort KoshiOffset;
        ushort KoshiSize;
        ushort AnkleOffset;
        ushort Fat;
        ushort Muscle;
        ushort MotionFilter;
    }

    public class CDataEditInfoSerializer : EntitySerializer<CDataEditInfo>
    {
        public override void Write(IBuffer buffer, CDataEditInfo obj)
        {
            throw new NotImplementedException();
        }

        public override CDataEditInfo Read(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
