using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ClientErrorCodeCsvReader : CsvReader<ClientErrorCode>
    {
        protected override int NumExpectedItems => 5;

        protected override ClientErrorCode CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int messageId)) return null;
            if (!int.TryParse(properties[1], out int errorId)) return null;
            string errorCode = properties[2];
            string msgJp = properties[3];
            string msgEn = properties[3];
            return new ClientErrorCode
            {
                Id = errorId,
                MessageId = messageId,
                ErrorCode = errorCode,
                MessageJp = msgJp,
                MessageEn = msgEn
            };
        }
    }
}
