using System.Collections.Generic;
using System;

public class Upgrade_Two : CardType
{
    public Upgrade_Two(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        Dictionary<TokenType, int[]> playerTokens = player.GetAllTokens().Item2;
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
        {
            int number = playerTokens[token][2];
            player.UpDowngradeToken(number, (2, token), (3, token), logged);
        }
    }
}
