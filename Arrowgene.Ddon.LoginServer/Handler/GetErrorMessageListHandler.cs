using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler;

public sealed class GetErrorMessageListHandler(DdonLoginServer server) : LoginRequestPacketHandler<C2LGetErrorMessageListReq, L2CGetErrorMessageListRes>(server)
{
    /// <summary>
    ///     Total allowable packet size, in bytes.
    ///     Break up the list into multiple packets if we exceed this value.
    ///     This is slightly smaller than ushort.MaxValue (the hard cap) to give some wiggle room.
    /// </summary>
    private const uint PacketSizeLimit = ushort.MaxValue - 1000;

    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetErrorMessageListHandler));

    public override L2CGetErrorMessageListRes Handle(LoginClient client, C2LGetErrorMessageListReq packet)
    {
        L2CGetErrorMessageListNtc ntc = new();
        uint totalLength = 0;

        foreach (ErrorCode code in Enum.GetValues<ErrorCode>())
        {
            // An MtString of length N takes up N+2 bytes.
            // Each CDataErrorMessage is thus at least N+2+2+4+4 bytes.
            string message = code.ToString();

            if (Server.AssetRepository.ClientErrorCodes.TryGetValue(code, out ClientErrorCode asset)
                && asset.Message.TryGetValue("en", out string assetMessage))
                message = assetMessage;

            totalLength += (uint)(message.Length + 12);

            ntc.ErrorMessages.Add(new CDataErrorMessage
            {
                ErrorId = code,
                MessageId = 1,
                Message = message
            });

            if (totalLength >= PacketSizeLimit)
            {
                client.Send(ntc);
                ntc = new L2CGetErrorMessageListNtc();
                totalLength = 0;
            }
        }

        if (ntc.ErrorMessages.Count != 0) client.Send(ntc);
        L2CGetErrorMessageListRes res = new();
        return res;
    }
}
