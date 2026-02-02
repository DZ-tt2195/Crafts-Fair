using UnityEngine;
using System.Collections.Generic;

public class Many_Ones : CardType
{
    public Many_Ones(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return WithLevel(tokensSubmitted, FindNumber.Minimum, 1, 4);
    }
}
