using System;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class RequestPacketHandler<TClient, TReqStruct, TResStruct> : StructurePacketHandler<TClient, TReqStruct>
        where TClient : Client
        where TReqStruct : class, IPacketStructure, new()
        where TResStruct : ServerResponse, new()
    {
        protected RequestPacketHandler(DdonServer<TClient> server) : base(server)
        {
        }

        public abstract TResStruct Handle(TClient client, TReqStruct request);

        public sealed override void Handle(TClient client, StructurePacket<TReqStruct> request)
        {
            TResStruct response;
            try
            {
                response = Handle(client, request.Structure);
            }
            catch (ResponseErrorException ex)
            {
                response = new TResStruct();
                response.Error = (uint) ex.ErrorCode;
                client.Send(response);
            }
            catch (Exception)
            {
                response = new TResStruct();
                response.Error = (uint) ErrorCode.ERROR_CODE_FAIL;
                throw;
            }    
            client.Send(response);
        }

    }
}
