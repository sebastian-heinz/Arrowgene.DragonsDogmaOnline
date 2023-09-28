using System;
using System.Text;
using Arrowgene.Ddon.Shared.Crypto;

namespace Arrowgene.Ddon.Shared.Model
{
    public class GameToken
    {
        private static string TokenPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const int LoginTokenLength = 20;
        public const int GameTokenLength = 20;

        public static string GenerateLoginToken()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < LoginTokenLength; i++)
            {
                int tokenPoolIndex = CryptoRandom.Instance.Next(0, TokenPool.Length - 1);
                sb.Append(TokenPool[tokenPoolIndex]);
            }
            return sb.ToString();
        }
        
        public static GameToken GenerateGameToken(int accountId, uint characterId)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < GameTokenLength; i++)
            {
                int tokenPoolIndex = CryptoRandom.Instance.Next(0, TokenPool.Length - 1);
                sb.Append(TokenPool[tokenPoolIndex]);
            }

            GameToken token = new GameToken();
            token.Token = sb.ToString();
            token.Created = DateTime.UtcNow;
            token.AccountId = accountId;
            token.CharacterId = characterId;
            return token;
        }

        public int AccountId { get; set; }
        public uint CharacterId { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
    }
}
