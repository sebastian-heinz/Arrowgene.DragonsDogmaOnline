using System;
using System.Xml.Linq;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffAbilityHandler : StructurePacketHandler<GameClient, C2SSkillSetOffAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffAbilityHandler));

        public SkillSetOffAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffAbilityReq> packet)
        {
            // TODO: Performance
            List<Ability> newAbilities = new List<Ability>();
            lock(client.Character.Abilities)
            {
                byte removedAbilitySlotNo = Byte.MaxValue;
                for(int i=0; i<client.Character.Abilities.Count; i++)
                {
                    Ability ability = client.Character.Abilities[i];
                    if(ability.EquippedToJob == client.Character.Job && ability.SlotNo == packet.Structure.SlotNo)
                    {
                        client.Character.Abilities.RemoveAt(i);
                        removedAbilitySlotNo = ability.SlotNo;
                        break;
                    }
                }

                for(int i=0; i<client.Character.Abilities.Count; i++)
                {
                    Ability ability = client.Character.Abilities[i];
                    if(ability.EquippedToJob == client.Character.Job)
                    {
                        if(ability.SlotNo > removedAbilitySlotNo)
                        {
                            ability.SlotNo--;
                        }
                        newAbilities.Add(ability);
                    }
                }
            }

            Database.ReplaceEquippedAbilities(client.Character.CommonId, client.Character.Job, newAbilities);

            client.Send(new S2CSkillSetOffAbilityRes() {
                SlotNo = packet.Structure.SlotNo
            });

            // Same as skills, i haven't found an Ability off NTC. It may not be required
        }
    }
}