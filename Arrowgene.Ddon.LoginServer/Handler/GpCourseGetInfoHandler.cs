using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GpCourseGetInfoHandler : LoginRequestPacketHandler<C2LGpCourseGetInfoReq, L2CGpCourseGetInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpCourseGetInfoHandler));

        public GpCourseGetInfoHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CGpCourseGetInfoRes Handle(LoginClient client, C2LGpCourseGetInfoReq request)
        {
            L2CGpCourseGetInfoRes response = new();

            foreach (var course in Server.AssetRepository.GPCourseInfoAsset.Courses)
            {
                CDataGPCourseInfo cDataGPCourseInfo = new CDataGPCourseInfo()
                {
                    CourseId = course.Value.Id,
                    CourseName = course.Value.Name,
                    DoubleCourseTarget = course.Value.Target,
                    PrioGroup = (byte)course.Value.PriorityGroup,
                    PrioSameTime = (byte)course.Value.PrioritySameTime,
                    AnnounceType = (byte)course.Value.AnnounceType,
                    EffectUIDs = course.Value.Effects
                };

                response.CourseInfo.Add(cDataGPCourseInfo);
            }

            foreach (var effect in Server.AssetRepository.GPCourseInfoAsset.Effects)
            {
                CDataGPCourseEffectParam cDataGPCourseEffectParam = new CDataGPCourseEffectParam()
                {
                    EffectUID = effect.Value.Uid,
                    EffectID = effect.Value.Id,
                    Param0 = effect.Value.Param0,
                    Param1 = effect.Value.Param1
                };

                response.Effects.Add(cDataGPCourseEffectParam);
            }

            return response;
        }
    }
}
