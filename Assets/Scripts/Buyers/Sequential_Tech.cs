using UnityEngine;
using System.Collections.Generic;

public class Sequential_Tech : CardType
{
    public Sequential_Tech(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SequentialLevels(tokensSubmitted, TokenType.TechIcon, 4);
    }
}
