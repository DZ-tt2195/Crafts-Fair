using UnityEngine;
using System.Collections.Generic;

public class Total_Six : CardType
{
    public Total_Six(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SumOfLevels(tokensSubmitted, FindNumber.Exact, 6);
    }
}
