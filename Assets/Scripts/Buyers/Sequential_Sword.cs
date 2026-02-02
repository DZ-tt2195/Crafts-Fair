using UnityEngine;
using System.Collections.Generic;

public class Sequential_Sword : CardType
{
    public Sequential_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return SequentialLevels(soldTokens, TokenType.SwordIcon, 4);
    }
}
