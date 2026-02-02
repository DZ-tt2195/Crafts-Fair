using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rise_Of_Kingdoms : CardType
{
    public Rise_Of_Kingdoms(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 4, 2);
    }
}
