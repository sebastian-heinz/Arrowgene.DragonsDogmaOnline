using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterDecideCharacterIdHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterDecideCharacterIdHandler));


        public CharacterDecideCharacterIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_DECIDE_CHARACTER_ID_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CCharacterDecideCharacterIdRes res = new S2CCharacterDecideCharacterIdRes();
            res.CharacterId = client.Character.Id;
            res.FirstName = client.Character.FirstName;
            res.LastName = client.Character.LastName;
            res.Sex = client.Character.Visual.Sex;
            res.Voice = client.Character.Visual.Voice;
            res.VoicePitch = client.Character.Visual.VoicePitch;
            res.Personality = client.Character.Visual.Personality;
            res.SpeechFreq = client.Character.Visual.SpeechFreq;
            res.BodyType = client.Character.Visual.BodyType;
            res.Hair = client.Character.Visual.Hair;
            res.Beard = client.Character.Visual.Beard;
            res.Makeup = client.Character.Visual.Makeup;
            res.Scar = client.Character.Visual.Scar;
            res.EyePresetNo = client.Character.Visual.EyePresetNo;
            res.NosePresetNo = client.Character.Visual.NosePresetNo;
            res.MouthPresetNo = client.Character.Visual.MouthPresetNo;
            res.EyebrowTexNo = client.Character.Visual.EyebrowTexNo;
            res.ColorSkin = client.Character.Visual.ColorSkin;
            res.ColorHair = client.Character.Visual.ColorHair;
            res.ColorBeard = client.Character.Visual.ColorBeard;
            res.ColorEyebrow = client.Character.Visual.ColorEyebrow;
            res.ColorREye = client.Character.Visual.ColorREye;
            res.ColorLEye = client.Character.Visual.ColorLEye;
            res.ColorMakeup = client.Character.Visual.ColorMakeup;
            res.Sokutobu = client.Character.Visual.Sokutobu;
            res.Hitai = client.Character.Visual.Hitai;
            res.MimiJyouge = client.Character.Visual.MimiJyouge;
            res.Kannkaku = client.Character.Visual.Kannkaku;
            res.MabisasiJyouge = client.Character.Visual.MabisasiJyouge;
            res.HanakuchiJyouge = client.Character.Visual.HanakuchiJyouge;
            res.AgoSakiHaba = client.Character.Visual.AgoSakiHaba;
            res.AgoZengo = client.Character.Visual.AgoZengo;
            res.AgoSakiJyouge = client.Character.Visual.AgoSakiJyouge;
            res.HitomiOokisa = client.Character.Visual.HitomiOokisa;
            res.MeOokisa = client.Character.Visual.MeOokisa;
            res.MeKaiten = client.Character.Visual.MeKaiten;
            res.MayuKaiten = client.Character.Visual.MayuKaiten;
            res.MimiOokisa = client.Character.Visual.MimiOokisa;
            res.MimiMuki = client.Character.Visual.MimiMuki;
            res.ElfMimi = client.Character.Visual.ElfMimi;
            res.MikenTakasa = client.Character.Visual.MikenTakasa;
            res.MikenHaba = client.Character.Visual.MikenHaba;
            res.HohoboneRyou = client.Character.Visual.HohoboneRyou;
            res.HohoboneJyouge = client.Character.Visual.HohoboneJyouge;
            res.Hohoniku = client.Character.Visual.Hohoniku;
            res.ErahoneJyouge = client.Character.Visual.ErahoneJyouge;
            res.ErahoneHaba = client.Character.Visual.ErahoneHaba;
            res.HanaJyouge = client.Character.Visual.HanaJyouge;
            res.HanaHaba = client.Character.Visual.HanaHaba;
            res.HanaTakasa = client.Character.Visual.HanaTakasa;
            res.HanaKakudo = client.Character.Visual.HanaKakudo;
            res.KuchiHaba = client.Character.Visual.KuchiHaba;
            res.KuchiAtsusa = client.Character.Visual.KuchiAtsusa;
            res.EyebrowUVOffsetX = client.Character.Visual.EyebrowUVOffsetX;
            res.EyebrowUVOffsetY = client.Character.Visual.EyebrowUVOffsetY;
            res.Wrinkle = client.Character.Visual.Wrinkle;
            res.WrinkleAlbedoBlendRate = client.Character.Visual.WrinkleAlbedoBlendRate;
            res.WrinkleDetailNormalPower = client.Character.Visual.WrinkleDetailNormalPower;
            res.MuscleAlbedoBlendRate = client.Character.Visual.MuscleAlbedoBlendRate;
            res.MuscleDetailNormalPower = client.Character.Visual.MuscleDetailNormalPower;
            res.Height = client.Character.Visual.Height;
            res.HeadSize = client.Character.Visual.HeadSize;
            res.NeckOffset = client.Character.Visual.NeckOffset;
            res.NeckScale = client.Character.Visual.NeckScale;
            res.UpperBodyScaleX = client.Character.Visual.UpperBodyScaleX;
            res.BellySize = client.Character.Visual.BellySize;
            res.TeatScale = client.Character.Visual.TeatScale;
            res.TekubiSize = client.Character.Visual.TekubiSize;
            res.KoshiOffset = client.Character.Visual.KoshiOffset;
            res.KoshiSize = client.Character.Visual.KoshiSize;
            res.AnkleOffset = client.Character.Visual.AnkleOffset;
            res.Fat = client.Character.Visual.Fat;
            res.Muscle = client.Character.Visual.Muscle;
            res.MotionFilter = client.Character.Visual.MotionFilter;
            client.Send(res);
            
            // client.Send(GameDump.Dump_13);

            S2CCharacterContentsReleaseElementNotice contentsReleaseElementNotice = EntitySerializer.Get<S2CCharacterContentsReleaseElementNotice>().Read(GameFull.data_Dump_20);
            client.Send(contentsReleaseElementNotice);
        }
    }
}
