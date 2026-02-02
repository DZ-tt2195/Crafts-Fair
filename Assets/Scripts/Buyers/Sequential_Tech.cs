using UnityEngine;
using System.Collections.Generic;

public class Sequential_Tech : CardType
{
    public Sequential_Tech(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.TechIcon, 4);
    }
}
