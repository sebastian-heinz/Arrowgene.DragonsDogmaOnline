using System;
using System.Text;
using Arrowgene.Ddon.Shared.Crypto;

namespace Arrowgene.Ddon.Shared.Model
{
    public class GameToken
    {
        private static string TokenPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private const int TokenLength = 20;

        public static GameToken Generate(int accountId)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < TokenLength; i++)
            {
                int tokenPoolIndex = CryptoRandom.Instance.Next(0, TokenPool.Length - 1);
                sb.Append(TokenPool[tokenPoolIndex]);
            }

            GameToken token = new GameToken();
            token.Token = sb.ToString();
            token.Created = DateTime.Now;
            token.AccountId = accountId;
            return token;
        }

        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
    }
}
