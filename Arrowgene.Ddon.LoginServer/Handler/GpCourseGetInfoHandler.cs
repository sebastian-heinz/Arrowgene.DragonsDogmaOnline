using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GpCourseGetInfoHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpCourseGetInfoHandler));

        private L2CGpCourseGetInfoRes _Response;

        public GpCourseGetInfoHandler(DdonLoginServer server) : base(server)
        {
            _Response = new L2CGpCourseGetInfoRes();

            foreach (var Course in server.AssetRepository.GPCourseInfoAsset.Courses)
            {
                CDataGPCourseInfo cDataGPCourseInfo = new CDataGPCourseInfo()
                {
                    CourseId = Course.Value.Id,
                    CourseName = Course.Value.Name,
                    DoubleCourseTarget = Course.Value.Target,
                    PrioGroup = (byte)Course.Value.PriorityGroup,
                    PrioSameTime = (byte)Course.Value.PrioritySameTime,
                    AnnounceType = (byte)Course.Value.AnnounceType,
                    EffectUIDs = Course.Value.Effects
                };

                _Response.CourseInfo.Add(cDataGPCourseInfo);
            }

            foreach (var Effect in server.AssetRepository.GPCourseInfoAsset.Effects)
            {
                CDataGPCourseEffectParam cDataGPCourseEffectParam = new CDataGPCourseEffectParam()
                {
                    EffectUID = Effect.Value.Uid,
                    EffectID = Effect.Value.Id,
                    Param0 = Effect.Value.Param0,
                    Param1 = Effect.Value.Param1
                };

                _Response.Effects.Add(cDataGPCourseEffectParam);
            }
        }

        public override PacketId Id => PacketId.C2L_GP_COURSE_GET_INFO_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            // client.Send(LoginDump.Dump_22);
            client.Send(_Response);
        }
    }
}
