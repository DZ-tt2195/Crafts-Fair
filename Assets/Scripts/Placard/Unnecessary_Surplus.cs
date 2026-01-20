using UnityEngine;
using System.Collections.Generic;

public class Unnecessary_Surplus : CardType
{
    public Unnecessary_Surplus(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int, TokenType)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        return player.GetAllTokens().Item1 >= 5;
    }
}
