using System;

namespace Arrowgene.Ddon.Shared.Model;

public class GameToken
{
    public int AccountId { get; init; }
    public uint CharacterId { get; init; }
    public string Token { get; init; }
    public DateTime Created { get; init; }

    public static string GenerateLoginToken()
    {
        return Guid.NewGuid().ToString("N");
    }

    public static GameToken GenerateGameToken(int accountId, uint characterId)
    {
        return new GameToken
        {
            AccountId = accountId,
            CharacterId = characterId,
            Token = GenerateLoginToken(),
            Created = DateTime.UtcNow
        };
    }
}
