using UnityEngine;
using System;
using System.Collections.Generic;

public class Craftsman : CardType
{
    public Craftsman(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return soldTokens[TokenType.ToolIcon][6] >= 1;
    }
}
