using UnityEngine;
using System.Collections.Generic;

public class Unnecessary_Surplus : CardType
{
    public Unnecessary_Surplus(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted)
    {
        return player.GetAllTokens().Item1 >= 5;
    }
}
