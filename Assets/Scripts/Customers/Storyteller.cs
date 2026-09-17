using UnityEngine;
using System;
using System.Collections.Generic;

public class Storyteller : CardType
{
    public Storyteller(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return soldTokens[TokenType.BookIcon][6] >= 1;
    }
}
