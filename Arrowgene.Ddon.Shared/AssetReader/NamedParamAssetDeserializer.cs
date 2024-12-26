using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class NamedParamAssetDeserializer : IAssetDeserializer<Dictionary<uint, NamedParam>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(NamedParamAssetDeserializer));

        public Dictionary<uint, NamedParam> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            Dictionary<uint, NamedParam> namedParams = new Dictionary<uint, NamedParam>();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);
            JsonElement namedParamListElement = document.RootElement.GetProperty("namedParamList");
            foreach (JsonElement namedParamListEntryElement in namedParamListElement.EnumerateArray())
            {
                NamedParam namedParam = new NamedParam
                {
                    Id = namedParamListEntryElement.GetProperty("ID").GetUInt32(),
                    Type = namedParamListEntryElement.GetProperty("Type").GetUInt32(),
                    HpRate = namedParamListEntryElement.GetProperty("HpRate").GetUInt32(),
                    Experience = namedParamListEntryElement.GetProperty("Experience").GetUInt32(),
                    AttackBasePhys = namedParamListEntryElement.GetProperty("AttackBasePhys").GetUInt32(),
                    AttackWepPhys = namedParamListEntryElement.GetProperty("AttackWepPhys").GetUInt32(),
                    DefenceBasePhys = namedParamListEntryElement.GetProperty("DefenceBasePhys").GetUInt32(),
                    DefenceWepPhys = namedParamListEntryElement.GetProperty("DefenceWepPhys").GetUInt32(),
                    AttackBaseMagic = namedParamListEntryElement.GetProperty("AttackBaseMagic").GetUInt32(),
                    AttackWepMagic = namedParamListEntryElement.GetProperty("AttackWepMagic").GetUInt32(),
                    DefenceBaseMagic = namedParamListEntryElement.GetProperty("DefenceBaseMagic").GetUInt32(),
                    DefenceWepMagic = namedParamListEntryElement.GetProperty("DefenceWepMagic").GetUInt32(),
                    Power = namedParamListEntryElement.GetProperty("Power").GetUInt32(),
                    GuardDefenceBase = namedParamListEntryElement.GetProperty("GuardDefenceBase").GetUInt32(),
                    GuardDefenceWep = namedParamListEntryElement.GetProperty("GuardDefenceWep").GetUInt32(),
                    ShrinkEnduranceMain = namedParamListEntryElement.GetProperty("ShrinkEnduranceMain").GetUInt32(),
                    BlowEnduranceMain = namedParamListEntryElement.GetProperty("BlowEnduranceMain").GetUInt32(),
                    DownEnduranceMain = namedParamListEntryElement.GetProperty("DownEnduranceMain").GetUInt32(),
                    ShakeEnduranceMain = namedParamListEntryElement.GetProperty("ShakeEnduranceMain").GetUInt32(),
                    HpSub = namedParamListEntryElement.GetProperty("HpSub").GetUInt32(),
                    ShrinkEnduranceSub = namedParamListEntryElement.GetProperty("ShrinkEnduranceSub").GetUInt32(),
                    BlowEnduranceSub = namedParamListEntryElement.GetProperty("BlowEnduranceSub").GetUInt32(),
                    OcdEndurance = namedParamListEntryElement.GetProperty("OcdEndurance").GetUInt32(),
                    AilmentDamage = namedParamListEntryElement.GetProperty("AilmentDamage").GetUInt32()
                };
                namedParams.Add(namedParam.Id, namedParam);
            }

            return namedParams;
        }
    }
}
