using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ClientErrorCodeCsvReader : CsvReader<ClientErrorCode>
    {
        protected override int NumExpectedItems => 5;
        
        protected override ClientErrorCode CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint messageId)) return null;
            if (!uint.TryParse(properties[1], out uint errorId)) return null;
            string errorCode = properties[2];
            string msgJp = properties[3];
            string msgEn = properties[3];
            return new ClientErrorCode
            {
                ErrorId = errorId,
                MessageId = messageId,
                ErrorCode = errorCode,
                MessageJp = msgJp,
                MessageEn = msgEn
            };
        }
    }
}
