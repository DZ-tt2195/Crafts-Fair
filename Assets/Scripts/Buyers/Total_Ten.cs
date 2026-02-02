using UnityEngine;
using System.Collections.Generic;

public class Total_Ten : CardType
{
    public Total_Ten(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SumOfLevels(tokensSubmitted, FindNumber.Exact, 10);
    }
}
