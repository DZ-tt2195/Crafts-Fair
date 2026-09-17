using UnityEngine;
using System;
using System.Collections.Generic;

public class Ringmaster : CardType
{
    public Ringmaster(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return HigherTypeVs(soldTokens, 4, TokenType.ArtIcon);
    }
}
