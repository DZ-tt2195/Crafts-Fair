using UnityEngine;
using System;
using System.Collections.Generic;

public class Art_Sword_Tech : CardType
{
    public Art_Sword_Tech(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TypesInOrder(soldTokens, TokenType.ArtIcon, TokenType.SwordIcon, TokenType.TechIcon);
    }
}
