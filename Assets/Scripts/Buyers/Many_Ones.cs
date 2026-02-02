using UnityEngine;
using System.Collections.Generic;

public class Many_Ones : CardType
{
    public Many_Ones(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 1, 4);
    }
}
