using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rise_Of_Villages : CardType
{
    public Rise_Of_Villages(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return WithLevel(soldTokens, FindNumber.Minimum, 2, 2);
    }
}
