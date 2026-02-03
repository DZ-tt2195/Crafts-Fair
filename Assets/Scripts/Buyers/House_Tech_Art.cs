using UnityEngine;
using System;
using System.Collections.Generic;

public class House_Tech_Art : CardType
{
    public House_Tech_Art(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.HouseIcon, TokenType.TechIcon, TokenType.ArtIcon);
    }
}
