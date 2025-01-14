using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetErrorMessageListHandler : LoginRequestPacketHandler<C2LGetErrorMessageListReq, L2CGetErrorMessageListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetErrorMessageListHandler));

        public GetErrorMessageListHandler(DdonLoginServer server) : base(server)
        {
        }

        /// <summary>
        /// Total allowable packet size, in bytes.
        /// Break up the list into multiple packets if we exceed this value.
        /// This is slightly smaller than ushort.MaxValue (the hard cap) to give some wiggle room.
        /// </summary>
        private static uint PACKET_SIZE_LIMIT = ushort.MaxValue - 1000;

        public override L2CGetErrorMessageListRes Handle(LoginClient client, C2LGetErrorMessageListReq packet)
        {
            L2CGetErrorMessageListNtc ntc = new L2CGetErrorMessageListNtc();
            uint totalLength = 0;

            foreach (ErrorCode code in Enum.GetValues(typeof(ErrorCode)))
            {
                // An MtString of length N takes up N+2 bytes.
                // Each CDataErrorMessage is thus at least N+2+2+4+4 bytes.
                var message = code.ToString();

                if (Server.AssetRepository.ClientErrorCodes.TryGetValue(code, out var asset)
                    && asset.Message.TryGetValue("en", out string assetMessage))
                {
                    message = assetMessage;
                }

                totalLength += (uint)(message.Length + 12); 

                ntc.ErrorMessages.Add(new()
                {
                    ErrorId = code,
                    MessageId = 1,
                    Message = message
                });

                if (totalLength >= PACKET_SIZE_LIMIT)
                {
                    client.Send(ntc);
                    ntc = new();
                    totalLength = 0;
                }
            }

            if (ntc.ErrorMessages.Any())
            {
                client.Send(ntc);
            }
            L2CGetErrorMessageListRes res = new L2CGetErrorMessageListRes();
            return res;
        }
    }
}
