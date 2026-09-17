using UnityEngine;
using System.Collections.Generic;

public class Farmer : CardType
{
    public Farmer(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return player.AllTotalTokens() >= 5;
    }
}
