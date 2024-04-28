using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class SecretAbilityAsset
    {
        public SecretAbilityAsset()
        {
            DefaultSecretAbilities = new List<SecretAbility>();
        }

        public List<SecretAbility> DefaultSecretAbilities { get; set; }
    }
}
