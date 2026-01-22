using System.Collections.Generic;
using UnityEngine;

public class Perfect_Fit : CardType
{
    public Perfect_Fit(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted)
    {
        return player.GetAllTokens().Item1 == 0;
    }
}
