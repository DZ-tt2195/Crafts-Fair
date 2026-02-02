using UnityEngine;
using System.Collections.Generic;

public class Unnecessary_Surplus : CardType
{
    public Unnecessary_Surplus(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return player.GetAllTokens().Item1 >= 5;
    }
}
