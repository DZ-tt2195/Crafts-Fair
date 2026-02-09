using UnityEngine;
using System;
using System.Collections.Generic;

public class Artificer : CardType
{
    public Artificer(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.ArtIcon, TokenType.SwordIcon, TokenType.TechIcon);
    }
}
