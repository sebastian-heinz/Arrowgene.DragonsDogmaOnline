using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ClientErrorCodeCsv : CsvReaderWriter<CDataErrorMessage>
    {
        protected override int NumExpectedItems => 5;

        protected override CDataErrorMessage CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint messageId)) return null;
            if (!uint.TryParse(properties[1], out uint errorId)) return null;
            string errorCode = properties[2];
            string msgJp = properties[3];
            string msgEn = properties[3];
            return new CDataErrorMessage
            {
                ErrorId = (ErrorCode)errorId,
                MessageId = messageId,
                Message = errorCode,
                //MessageJp = msgJp,
                //MessageEn = msgEn
            };
        }
    }
}
