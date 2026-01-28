using System.Collections.Generic;
using System;

public class Advance_Two : CardType
{
    public Advance_Two(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        Dictionary<TokenType, int[]> playerTokens = player.GetAllTokens().Item2;
        foreach (TokenType value in Enum.GetValues(typeof(TokenType)))
        {
            int number = playerTokens[value][2];
            player.AdvanceRetreatToken(number, (2, value), (3, value), logged);
        }
    }
}
