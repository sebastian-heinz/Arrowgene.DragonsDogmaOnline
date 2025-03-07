using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class RequestPacketQueueHandler<TClient, TReqStruct, TResStruct> : StructurePacketHandler<TClient, TReqStruct>
        where TClient : Client
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(RequestPacketQueueHandler<TClient, TReqStruct, TResStruct>));

        protected RequestPacketQueueHandler(DdonServer<TClient> server) : base(server)
        {
        }

        public abstract PacketQueue Handle(TClient client, TReqStruct request);

        public sealed override void Handle(TClient client, StructurePacket<TReqStruct> request)
        {
            TResStruct response;
            try
            {
                PacketQueue queue = Handle(client, request.Structure);
                queue.Send();
            }
            catch (SQLiteException ex)
            {
                if (ex.ErrorCode == (int)SQLiteErrorCode.Busy)
                {
                    response = new TResStruct();
                    response.Error = (uint)ErrorCode.ERROR_CODE_DB_DEAD_LOCK;
                }
                else
                {
                    response = new TResStruct();
                    response.Error = (uint)ErrorCode.ERROR_CODE_DB_FAILURE;
                }
                client.Send(response);
                client.Close(); // Do not tolerate SqLiteExceptions because of desync issues.
                throw;
            }
            catch (NotImplementedException ex)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_NOT_IMPLEMENTED, ex.Message, ex);
            }
            catch (ResponseErrorException ex)
            {
                response = new TResStruct();
                response.Error = (uint)ex.ErrorCode;

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"{ex.ErrorCode} thrown when handling {typeof(TReqStruct).Name}");
                if (ex.Message.Length > 0)
                {
                    stringBuilder.AppendLine($"\tMessage: {ex.Message}");
                }
                stringBuilder.AppendLine(ex.StackTrace?.Split(Environment.NewLine).FirstOrDefault());
                Logger.Error(client, stringBuilder.ToString());

                client.Send(response);

                if (ex.Critical)
                {
                    client.Close();
                }
            }
            catch (Exception)
            {
                response = new TResStruct();
                response.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(response);
                throw;
            }
        }
    }
}
