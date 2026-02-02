using UnityEngine;
using System.Collections.Generic;

public class Sequential_House : CardType
{
    public Sequential_House(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.HouseIcon, 4);
    }
}
