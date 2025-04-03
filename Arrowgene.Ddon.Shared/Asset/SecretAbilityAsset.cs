using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class SecretAbilityAsset
    {
        public SecretAbilityAsset()
        {
            DefaultSecretAbilities = new List<AbilityId>();
        }

        public List<AbilityId> DefaultSecretAbilities { get; set; }
    }
}
