using System.Collections.Generic;
using UnityEngine;

public class Crowds : CardType
{
    public Crowds(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return player.AllTotalTokens() == 0;
    }
}
