using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftSettingHandler : GameRequestPacketHandler<C2SCraftGetCraftSettingReq, S2CCraftGetCraftSettingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftSettingHandler));

        /// <summary>
        /// TODO: Extract to asset
        /// </summary>
        private static readonly List<CDataCraftTimeSaveCost> TimeSaveCostList = new List<CDataCraftTimeSaveCost>
        {
            // TODO: Extract to asset, affects timesavehandler which requires this information
            new CDataCraftTimeSaveCost
            {
                ID = 1,
                Type = WalletType.GoldenGemstones,
                Price = 1,
                Sec = 0
            }
        };

        /// <summary>
        /// TODO: Extract to asset
        /// </summary>
        private static readonly List<CDataRefiningMaterialInfo> RefiningMaterialInfoList = new List<CDataRefiningMaterialInfo>
        {
            new CDataRefiningMaterialInfo
            {
                ItemId = 0,
                CraftCostItemRankMultiplier = 0,
                CraftCostMax = 0,
                CraftExpItemRankMultiplier = 0,
                CraftExpMax = 0,
                CanGreatSuccess = false
            },
            new CDataRefiningMaterialInfo
            {
                ItemId = 8035, // Attack Upgrade Rock
                CraftCostItemRankMultiplier = 300,
                CraftCostMax = 30000,
                CraftExpItemRankMultiplier = 2,
                CraftExpMax = 2,
                CanGreatSuccess = false
            },
            new CDataRefiningMaterialInfo
            {
                ItemId = 8067, // Defense Upgrade Rock
                CraftCostItemRankMultiplier = 300,
                CraftCostMax = 30000,
                CraftExpItemRankMultiplier = 2,
                CraftExpMax = 2,
                CanGreatSuccess = false
            },
            new CDataRefiningMaterialInfo
            {
                ItemId = 8036, // Quality Attack Upgrade Rock
                CraftCostItemRankMultiplier = 300,
                CraftCostMax = 30000,
                CraftExpItemRankMultiplier = 10,
                CraftExpMax = 100,
                CanGreatSuccess = true
            },
            new CDataRefiningMaterialInfo
            {
                ItemId = 8068, // Quality Defense Upgrade Rock"
                CraftCostItemRankMultiplier = 300,
                CraftCostMax = 30000,
                CraftExpItemRankMultiplier = 10,
                CraftExpMax = 100,
                CanGreatSuccess = true
            },
            new CDataRefiningMaterialInfo
            {
                ItemId = 8084, // White Dragon Defense Upgrade Rock
                CraftCostItemRankMultiplier = 300,
                CraftCostMax = 30000,
                CraftExpItemRankMultiplier = 10,
                CraftExpMax = 350,
                CanGreatSuccess = true
            },
            new CDataRefiningMaterialInfo
            {
                ItemId = 8052, // White Dragon Attack Upgrade Rock
                CraftCostItemRankMultiplier = 300,
                CraftCostMax = 30000,
                CraftExpItemRankMultiplier = 20,
                CraftExpMax = 350,
                CanGreatSuccess = true
            },
        };

        /// <summary>
        /// TODO: Extract to asset
        /// </summary>
        private static readonly List<CDataCommonU32> ColorRegulateItemList = new List<CDataCommonU32>
        {
            new(11234), new(11235), new(11236), new(11237), new(11238), new(11239), new(11240), new(11241), new(11242), new(11243), new(11244), new(11245), new(11246), new(11247),
            new(11248), new(11249), new(11250), new(11251), new(11252), new(11253), new(13536), new(13537), new(13538), new(13539), new(13541), new(13542), new(13543), new(13544),
            new(13546), new(13547), new(13548), new(13549), new(13551), new(13552), new(13553), new(13554), new(14164), new(14165), new(14166), new(14167), new(14169), new(14170),
            new(14171), new(14172), new(15673), new(15674), new(15675), new(15676), new(15677), new(15678), new(15679), new(15680), new(15681), new(15682), new(15713), new(15714),
            new(15715), new(15716), new(15717), new(15718), new(15719), new(15720), new(15721), new(15722), new(15733), new(15734), new(15735), new(15736), new(15737), new(15738),
            new(15739), new(15740), new(15741), new(15742), new(15773), new(15774), new(15775), new(15776), new(15777), new(15778), new(15779), new(15780), new(15781), new(15782),
            new(15793), new(15794), new(15795), new(15796), new(15797), new(15798), new(15799), new(15800), new(15801), new(15802), new(15813), new(15814), new(15815), new(15816),
            new(15817), new(15818), new(15819), new(15820), new(15821), new(15822), new(15833), new(15834), new(15835), new(15836), new(15837), new(15838), new(15839), new(15840),
            new(15841), new(15842), new(15853), new(15854), new(15855), new(15856), new(15857), new(15858), new(15859), new(15860), new(15861), new(15862), new(15873), new(15874),
            new(15875), new(15876), new(15877), new(15878), new(15879), new(15880), new(15881), new(15882), new(15893), new(15894), new(15895), new(15896), new(15897), new(15898),
            new(15899), new(15900), new(15901), new(15902), new(16145), new(16146), new(16147), new(16148), new(16150), new(16151), new(16152), new(16153), new(16155), new(16156),
            new(16157), new(16158), new(16160), new(16161), new(16162), new(16163), new(16523), new(16524), new(16525), new(16526), new(16527), new(16528), new(16529), new(16530),
            new(16531), new(16532), new(16536), new(16537), new(16538), new(16539), new(16540), new(16541), new(16542), new(16543), new(16544), new(16545), new(16546), new(16547),
            new(16548), new(16549), new(16550), new(16551), new(16552), new(16553), new(16554), new(16555), new(16556), new(16557), new(16558), new(16559), new(16560), new(16561),
            new(16562), new(16563), new(16564), new(16565), new(16566), new(16567), new(16568), new(16569), new(16570), new(16571), new(16572), new(16573), new(16574), new(16575),
            new(16576), new(16577), new(16578), new(16579), new(16580), new(16581), new(16582), new(16583), new(16584), new(16585), new(16586), new(16587), new(16588), new(16589),
            new(16590), new(16591), new(16592), new(16593), new(16594), new(16595), new(16596), new(16597), new(16598), new(16599), new(16600), new(16601), new(16602), new(16603),
            new(16604), new(16605), new(16606), new(16607), new(16608), new(16609), new(16610), new(16611), new(16612), new(16613), new(16614), new(16615), new(16616), new(16617),
            new(16618), new(16619), new(16620), new(16621), new(16622), new(16623), new(16624), new(16625), new(16764), new(16766), new(16768), new(16770), new(16772), new(16774),
            new(16776), new(16778), new(16780), new(16782), new(16784), new(16786), new(16793), new(16795), new(16796), new(16797), new(16798), new(16799), new(16859), new(16860),
            new(16861), new(16862), new(16863), new(16864), new(16865), new(16866), new(16867), new(16868), new(16869), new(16870), new(16871), new(16872), new(16873), new(16874),
            new(16875), new(16876), new(16877), new(16878), new(16879), new(16880), new(16881), new(16882), new(16883), new(16884), new(16885), new(16886), new(16887), new(16888),
            new(16889), new(16890), new(16891), new(16892), new(16893), new(16894), new(16895), new(16896), new(16897), new(16898), new(16899), new(16900), new(16901), new(16902),
            new(16903), new(16904), new(16905), new(16906), new(16907), new(16908), new(16909), new(16910), new(16911), new(16912), new(16913), new(16914), new(16915), new(16916),
            new(16917), new(16918), new(16919), new(16920), new(16921), new(16922), new(16923), new(16924), new(16925), new(16926), new(16927), new(16928), new(16929), new(16930),
            new(16931), new(16932), new(16933), new(16934), new(16935), new(16936), new(16937), new(16938), new(16939), new(16940), new(16941), new(16942), new(16943), new(16944),
            new(16945), new(16946), new(16947), new(16948), new(16949), new(16950), new(16951), new(16952), new(16953), new(16954), new(16955), new(16956), new(16957), new(16958),
            new(16959), new(16960), new(16961), new(16962), new(16963), new(16964), new(16965), new(16966), new(16967), new(16968), new(16969), new(16970), new(16971), new(16972),
            new(16973), new(16974), new(16975), new(16976), new(16977), new(16978), new(16979), new(16980), new(16981), new(16982), new(16983), new(16984), new(16985), new(16986),
            new(16987), new(16988), new(16989), new(16990), new(16991), new(16992), new(16993), new(16994), new(16995), new(16996), new(16997), new(16998), new(16999), new(17000),
            new(17001), new(17002), new(17003), new(17004), new(17005), new(17006), new(17007), new(17008), new(17009), new(17010), new(17011), new(17012), new(17013), new(17014),
            new(17015), new(17016), new(17017), new(17018), new(17019), new(17020), new(17021), new(17022), new(17023), new(17024), new(17025), new(17026), new(17027), new(17028),
            new(17029), new(17030), new(17031), new(17032), new(17033), new(17034), new(17035), new(17036), new(17037), new(17038), new(17039), new(17040), new(17041), new(17042),
            new(17043), new(17044), new(17045), new(17046), new(17047), new(17048), new(17049), new(17050), new(17051), new(17052), new(17053), new(17054), new(17055), new(17056),
            new(17057), new(17058), new(17941), new(17942), new(17943), new(17944), new(18620), new(18621), new(18622), new(18623), new(18624), new(18625), new(18626), new(18627),
            new(18628), new(18629), new(18630), new(18631), new(18632), new(18633), new(18634), new(18635), new(18900), new(18901), new(18902), new(18903), new(18904), new(18905),
            new(18906), new(18907), new(18908), new(18909), new(18910), new(18911), new(18912), new(18913), new(18914), new(18915), new(18916), new(18917), new(18918), new(18919),
            new(18920), new(18921), new(18922), new(18923), new(18924), new(18925), new(18926), new(18927), new(18928), new(18929), new(18930), new(18931), new(18932), new(18933),
            new(18934), new(18935), new(18936), new(18937), new(18938), new(18939), new(18940), new(18941), new(18942), new(18943), new(18944), new(18945), new(18946), new(18947),
            new(18948), new(18949), new(18950), new(18951), new(18952), new(18953), new(18954), new(18955), new(18956), new(18957), new(18958), new(18959), new(18960), new(18961),
            new(18962), new(18963), new(18964), new(18965), new(18966), new(18967), new(18968), new(18969), new(18970), new(18971), new(18972), new(18973), new(18974), new(18975),
            new(18976), new(18977), new(18978), new(18979), new(18980), new(18981), new(18982), new(18983), new(18984), new(18985), new(18986), new(18987), new(18988), new(18989),
            new(18990), new(18991), new(18992), new(18993), new(18994), new(18995), new(18996), new(18997), new(18998), new(18999), new(19000), new(19001), new(19002), new(19003),
            new(19004), new(19005), new(19006), new(19007), new(19008), new(19009), new(19010), new(19011), new(19012), new(19013), new(19014), new(19015), new(19016), new(19017),
            new(19018), new(19019), new(19020), new(19021), new(19022), new(19023), new(19024), new(19025), new(19026), new(19027), new(19028), new(19029), new(19030), new(19031),
            new(19032), new(19033), new(19034), new(19035), new(19036), new(19037), new(19038), new(19039), new(19040), new(19041), new(19042), new(19043), new(19044), new(19045),
            new(19046), new(19047), new(19048), new(19049), new(19050), new(19051), new(19052), new(19053), new(19054), new(19055), new(19056), new(19057), new(19058), new(19059),
            new(19060), new(19061), new(19062), new(19063), new(19064), new(19065), new(19066), new(19067), new(19068), new(19069), new(19070), new(19071), new(19072), new(19073),
            new(19074), new(19075), new(19076), new(19077), new(19078), new(19079), new(19080), new(19081), new(19082), new(19083), new(19084), new(19085), new(19086), new(19087),
            new(19088), new(19089), new(19090), new(19091), new(19092), new(19093), new(19094), new(19095), new(19096), new(19097), new(19098), new(19099), new(19100), new(19101),
            new(19102), new(19103), new(19104), new(19105), new(19106), new(19107), new(19108), new(19109), new(19110), new(19111), new(19112), new(19113), new(19114), new(19115),
            new(19116), new(19117), new(19118), new(19119), new(19120), new(19121), new(19122), new(19123), new(19124), new(19125), new(19126), new(19127), new(19128), new(19129),
            new(19130), new(19131), new(19132), new(19133), new(19134), new(19380), new(19381), new(19512), new(19513), new(19514), new(19517), new(19518), new(19568), new(19569),
            new(19570), new(19571), new(19572), new(19573), new(19574), new(19575), new(19576), new(19577), new(19578), new(19579), new(19580), new(19581), new(19582), new(19583),
            new(19584), new(19585), new(19586), new(19587), new(19588), new(19589), new(19590), new(19591), new(19592), new(19593), new(19595), new(19596), new(19597), new(19598),
            new(19599), new(19600), new(19601), new(19602), new(19603), new(19604), new(19605), new(19606), new(19607), new(19608), new(19609), new(19610), new(19611), new(19612),
            new(19613), new(19614), new(19615), new(19616), new(19618), new(19619), new(19620), new(19621), new(20443), new(20743), new(20744), new(20745), new(20746), new(20756),
            new(20757), new(20758), new(20759), new(20760), new(20761), new(21102), new(21103), new(21104), new(21105), new(21106), new(21107), new(21108), new(21109), new(21110),
            new(21111), new(21166), new(21167), new(21168), new(21169), new(21186), new(21187), new(21188), new(21189), new(21190), new(21323), new(21324), new(21325), new(21334),
            new(21335), new(21336), new(21337), new(21338), new(21339), new(21340), new(21341), new(21342), new(21343), new(21344), new(21345), new(21346), new(21347), new(21348),
            new(21349), new(21350), new(21351), new(21352), new(21353), new(21670), new(21671), new(21672), new(21673), new(21674), new(21675), new(21676), new(21677), new(21678),
            new(21679), new(21680), new(21681), new(21682), new(21683), new(21684), new(21685), new(21686), new(21687), new(21688), new(21689), new(21690), new(21691), new(21692),
            new(21693), new(21694), new(21695), new(21696), new(21697), new(21698), new(21699), new(21700), new(21701), new(21702), new(21703), new(21704), new(21705), new(21706),
            new(21707), new(21708), new(21709), new(21710), new(21711), new(21712), new(21781), new(21782), new(21783), new(21784), new(21785), new(21786), new(21812), new(21813),
            new(21814), new(21815), new(23434), new(23435), new(23436), new(23533), new(23534), new(23535), new(23536), new(23537), new(23538), new(23539), new(23540), new(23541),
            new(23542), new(23543), new(23544), new(24853), new(24854), new(24855), new(24856), new(24857), new(24858), new(24859), new(24860), new(24861), new(24862), new(24863),
            new(24864), new(24865), new(24866), new(24868), new(24870), new(24871), new(24872), new(24873), new(24874), new(24875), new(24876), new(24877), new(24878), new(24879),
            new(24880), new(24881), new(24882), new(24883), new(24884), new(24885), new(24886), new(24887), new(24888), new(24889), new(24890), new(24891), new(24892), new(24893),
            new(24894), new(24895), new(24896), new(24897), new(24898), new(24899), new(24900), new(24901), new(24902), new(24903), new(24904), new(24905), new(24906), new(24907),
            new(24908), new(24909), new(24910), new(24911), new(24912), new(24913), new(24914), new(24915), new(24916), new(24917), new(24918), new(24919), new(24920), new(24921),
            new(24922), new(24923), new(24924), new(24925), new(24926), new(24927), new(24928), new(24929), new(24930), new(24931), new(24932), new(24933), new(25019)
        };

        public CraftGetCraftSettingHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftGetCraftSettingRes Handle(GameClient client, C2SCraftGetCraftSettingReq request)
        {
            S2CCraftGetCraftSettingRes res = new S2CCraftGetCraftSettingRes
            {
                ColorRegulateItemList = ColorRegulateItemList,
                TimeSaveCostList = TimeSaveCostList,
                ReasonableCraftLv = CraftManager.ReasonableCraftLv,
                CraftItemLv = CraftManager.CraftItemLv,
                CreateCountMax = Server.GameSettings.GameServerSettings.CraftConsumableProductionTimesMax,
                CraftMasterLegendPawnInfoList = Server.AssetRepository.PawnCraftMasterLegendAsset,
                Unk1 = 49,
                Unk2 = 30,
                RefiningMaterialInfoList = RefiningMaterialInfoList
            };

            return res;
        }
    }
}
