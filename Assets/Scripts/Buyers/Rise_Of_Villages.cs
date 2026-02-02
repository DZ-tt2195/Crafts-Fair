using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rise_Of_Villages : CardType
{
    public Rise_Of_Villages(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return WithLevel(tokensSubmitted, FindNumber.Minimum, 2, 2);
    }
}
