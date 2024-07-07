using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class BitterBlackManager
    {
        public static uint RollBraceletCrest()
        {
            Random rnd = new Random();
            // [21816, 23112]
            return (uint)rnd.Next(21816, 23112 + 1);
        }

        public static uint RollEarringCrest(JobId jobId)
        {
            Random rnd = new Random();
            var jobCrests = gEarringCrestStats[jobId];
            return jobCrests[rnd.Next(0, jobCrests.Count)];
        }

        public static ushort RollEarringPercent(JobId job)
        {
            Random rnd = new Random();
            if (job == JobId.Warrior || job == JobId.ShieldSage)
            {
                // [8, 20]
                return (ushort) rnd.Next(8, 20 + 1);
            }
            // [1, 13]
            return (ushort)rnd.Next(1, 13 + 1);
        }

        private static readonly Dictionary<JobId, List<uint>> gEarringCrestStats = new Dictionary<JobId, List<uint>>()
        {
            [JobId.Fighter] = new List<uint>()
            {
                23606, // ファイターの斬属性ダメージUP, Slash damage for fighters UP,
                23607, // ファイターの打属性ダメージUP, Fighter Strike Damage UP,
                23608, // ファイターの射属性ダメージUP, Fighter's shooting attribute damage UP,
                23609, // ファイターの無属性ダメージUP, Fighter's Non-attribute damage UP,
                23610, // ファイターの炎属性ダメージUP, Fighter's flame damage UP,
                23611, // ファイターの氷属性ダメージUP, Fighter's ice damage UP,
                23612, // ファイターの雷属性ダメージUP, Fighter's Lightning damage UP,
                23613, // ファイターの聖属性ダメージUP, Holy attribute damage UP for fighters,
                23614, // ファイターの闇属性ダメージUP, Fighter's dark attribute damage UP,
            },
            [JobId.Hunter] = new List<uint>()
            {
                23615, // ハンターの斬属性ダメージUP, Hunter's slash damage UP,
                23616, // ハンターの打属性ダメージUP, Hunter's strike damage UP,
                23617, // ハンターの射属性ダメージUP, Hunter's shooting attribute damage UP,
                23618, // ハンターの無属性ダメージUP, Hunter's nonattribute damage UP,
                23619, // ハンターの炎属性ダメージUP, Hunter's flame damage UP,
                23620, // ハンターの氷属性ダメージUP, Hunter's ice damage UP,
                23621, // ハンターの雷属性ダメージUP, Hunter lightning damage UP,
                23622, // ハンターの聖属性ダメージUP, Hunter's Holy Attribute Damage UP,
                23623, // ハンターの闇属性ダメージUP, Hunter's dark attribute damage UP,
            },
            [JobId.Priest] = new List<uint>()
            {
                23624, // プリーストの斬属性ダメージUP, Priest's slash damage UP,
                23625, // プリーストの打属性ダメージUP, Priest's strike damage UP,
                23626, // プリーストの射属性ダメージUP, Priest's pierce damage UP,
                23627, // プリーストの無属性ダメージUP, Priest's nonattribute damage UP,
                23628, // プリーストの炎属性ダメージUP, Priest's flame damage UP,
                23629, // プリーストの氷属性ダメージUP, Priest's ice damage UP,
                23630, // プリーストの雷属性ダメージUP, Priest lightning damage UP,
                23631, // プリーストの聖属性ダメージUP, Priest's Holy Attribute Damage UP,
                23632, // プリーストの闇属性ダメージUP, Priest's Dark Attribute Damage UP,
            },
            [JobId.ShieldSage] = new List<uint>()
            {
                23633, // シールドセージの斬属性ダメージUP, Shield Sage slash damage UP,
                23634, // シールドセージの打属性ダメージUP, Shield Sage Strike Damage UP,
                23635, // シールドセージの射属性ダメージUP, Shield Sage's pierce attribute damage UP,
                23636, // シールドセージの無属性ダメージUP, Shield Sage Non - Attribute Damage UP,
                23637, // シールドセージの炎属性ダメージUP, Shield Sage Flame Damage UP,
                23638, // シールドセージの氷属性ダメージUP, Shield Sage's ice attribute damage UP,
                23639, // シールドセージの雷属性ダメージUP, Shield Sage lightning damage UP,
                23640, // シールドセージの聖属性ダメージUP, Shield Sage Holy Attribute Damage UP,
                23641, // シールドセージの闇属性ダメージUP, Shield Sage's Dark Attribute Damage UP,
            },
            [JobId.Seeker] = new List<uint>()
            {
                23642, // シーカーの斬属性ダメージUP, Seeker's slash damage UP,
                23643, // シーカーの打属性ダメージUP, Seeker strike damage UP,
                23644, // シーカーの射属性ダメージUP, Seeker's shooting attribute damage UP,
                23645, // シーカーの無属性ダメージUP, Seeker's nonattribute damage UP,
                23646, // シーカーの炎属性ダメージUP, Seeker's flame damage UP,
                23647, // シーカーの氷属性ダメージUP, Seeker's ice damage UP,
                23648, // シーカーの雷属性ダメージUP, Seeker lightning damage UP,
                23649, // シーカーの聖属性ダメージUP, Seeker's Holy Attribute Damage UP,
                23650, // シーカーの闇属性ダメージUP, Seeker's Dark Attribute Damage UP,
            },
            [JobId.Sorcerer] = new List<uint>()
            {
                23651, // ソーサラーの斬属性ダメージUP, Sorcerer's slash damage UP,
                23652, // ソーサラーの打属性ダメージUP, Sorcerer's strike damage UP,
                23653, // ソーサラーの射属性ダメージUP, Sorcerer's pierce attribute UP,
                23654, // ソーサラーの無属性ダメージUP, Sorcerer's nonattribute damage UP,
                23655, // ソーサラーの炎属性ダメージUP, Sorcerer's flame damage UP,
                23656, // ソーサラーの氷属性ダメージUP, Sorcerer's ice damage UP,
                23657, // ソーサラーの雷属性ダメージUP, Sorcerer's lightning damage UP,
                23658, // ソーサラーの聖属性ダメージUP, Sorcerer's Holy Attribute Damage UP,
                23659, // ソーサラーの闇属性ダメージUP, Sorcerer's Dark Attribute Damage UP,
            },
            [JobId.ElementArcher] = new List<uint>()
            {
                23660, // エレメントアーチャーの斬属性ダメージUP, Element Archer's slash damage UP,
                23661, // エレメントアーチャーの打属性ダメージUP, Element Archer's Strike Damage UP,
                23662, // エレメントアーチャーの射属性ダメージUP, Element Archer's pierce damage UP,
                23663, // エレメントアーチャーの無属性ダメージUP, Element Archer's Non-Attribute Damage UP,
                23664, // エレメントアーチャーの炎属性ダメージUP, Element Archer's flame damage UP,
                23665, // エレメントアーチャーの氷属性ダメージUP, Element Archer's ice damage UP,
                23666, // エレメントアーチャーの雷属性ダメージUP, Element Archer's Lightning damage UP,
                23667, // エレメントアーチャーの聖属性ダメージUP, Element Archer's Holy Attribute Damage UP,
                23668, // エレメントアーチャーの闇属性ダメージUP, Element Archer's Darkness Damage UP,
            },
            [JobId.Warrior] = new List<uint>()
            {
                23669, // ウォリアーの斬属性ダメージUP, Warrior's slash damage UP,
                23670, // ウォリアーの打属性ダメージUP, Warrior Strike Damage UP,
                23671, // ウォリアーの射属性ダメージUP, Warrior's pierce damage UP,
                23672, // ウォリアーの無属性ダメージUP, Warrior's nonattribute damage UP,
                23673, // ウォリアーの炎属性ダメージUP, Warrior's flame damage UP,
                23674, // ウォリアーの氷属性ダメージUP, Warrior's ice damage UP,
                23675, // ウォリアーの雷属性ダメージUP, Warrior lightning damage UP,
                23676, // ウォリアーの聖属性ダメージUP, Warrior's Holy Attribute Damage UP,
                23677, // ウォリアーの闇属性ダメージUP, Warrior's Dark Attribute Damage UP,
            },
            [JobId.Alchemist] = new List<uint>()
            {
                23678, // アルケミストの斬属性ダメージUP, Alchemist's slash damage UP,
                23679, // アルケミストの打属性ダメージUP, Alchemist strike damage UP,
                23680, // アルケミストの射属性ダメージUP, Alchemist's shooting attribute damage UP,
                23681, // アルケミストの無属性ダメージUP, Alchemist's nonattribute damage UP,
                23682, // アルケミストの炎属性ダメージUP, Alchemist's flame damage UP,
                23683, // アルケミストの氷属性ダメージUP, Alchemist's ice damage UP,
                23684, // アルケミストの雷属性ダメージUP, Alchemist lightning damage UP,
                23685, // アルケミストの聖属性ダメージUP, Alchemist's Holy Attribute Damage UP,
                23686, // アルケミストの闇属性ダメージUP, Alchemist's Dark Attribute Damage UP,
            },
            [JobId.SpiritLancer] = new List<uint>()
            {
                23687, // スピリットランサーの斬属性ダメージUP, Spirit Lancer's slash damage UP,
                23688, // スピリットランサーの打属性ダメージUP, Spirit Lancer's percussion damage UP,
                23689, // スピリットランサーの射属性ダメージUP, Spirit Lancer's shooting attribute damage UP,
                23690, // スピリットランサーの無属性ダメージUP, Spirit Lancer's nonattribute damage UP,
                23691, // スピリットランサーの炎属性ダメージUP, Spirit Lancer's flame damage UP,
                23692, // スピリットランサーの氷属性ダメージUP, Spirit Lancer's ice damage UP,
                23693, // スピリットランサーの雷属性ダメージUP, Spirit Lancer's Lightning damage UP,
                23694, // スピリットランサーの聖属性ダメージUP, Spirit Lancer's Holy Attribute Damage UP,
                23695, // スピリットランサーの闇属性ダメージUP, Spirit Lancer's Dark Attribute Damage UP,
            },
            [JobId.HighScepter] = new List<uint>()
            {
                23696, // ハイセプターの斬属性ダメージUP, High scepter slash damage UP,
                23697, // ハイセプターの打属性ダメージUP, High scepter strike damage UP,
                23698, // ハイセプターの射属性ダメージUP, High scepter shooting attribute damage UP,
                23699, // ハイセプターの無属性ダメージUP, High scepter nonattribute damage UP,
                23700, // ハイセプターの炎属性ダメージUP, High scepter flame damage UP,
                23701, // ハイセプターの氷属性ダメージUP, High scepter ice damage UP,
                23702, // ハイセプターの雷属性ダメージUP, High scepter lightning damage UP,
                23703, // ハイセプターの聖属性ダメージUP, High scepter holy attribute damage UP,
                23704, // ハイセプターの闇属性ダメージUP, High scepter's dark attribute damage UP,
            }
        };
    }
}
