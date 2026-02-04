using System.Collections.Generic;
using System;

public class Upgrade_Three : CardType
{
    public Upgrade_Three(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        Dictionary<TokenType, int[]> playerTokens = player.GetTokenDict();
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
        {
            int number = playerTokens[token][3];
            player.UpDowngradeToken(number, (3, token), (4, token), logged);
        }
    }
}
