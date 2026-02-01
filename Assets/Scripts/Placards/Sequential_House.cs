using UnityEngine;
using System.Collections.Generic;

public class Sequential_House : CardType
{
    public Sequential_House(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SequentialLevels(tokensSubmitted, TokenType.HouseIcon, 4);
    }
}
