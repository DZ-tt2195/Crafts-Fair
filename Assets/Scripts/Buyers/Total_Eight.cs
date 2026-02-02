using UnityEngine;
using System.Collections.Generic;

public class Total_Eight : CardType
{
    public Total_Eight(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SumOfLevels(tokensSubmitted, FindNumber.Exact, 8);
    }
}
