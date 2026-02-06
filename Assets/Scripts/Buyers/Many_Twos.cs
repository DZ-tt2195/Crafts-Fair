using UnityEngine;
using System.Collections.Generic;

public class Many_Twos : CardType
{
    public Many_Twos(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 2, 4);
    }
}
