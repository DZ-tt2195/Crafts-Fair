using System.Collections.Generic;
using System;

public class Polish : CardType
{
    public Polish(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        Dictionary<TokenType, int[]> playerTokens = player.GetTokenDict();
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
        {
            int number = playerTokens[token][3];
            player.UpDowngradeToken(number, (3, token), (4, token), logged);
        }
    }
}
