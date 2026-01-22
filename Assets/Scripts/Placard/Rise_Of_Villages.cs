using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rise_Of_Villages : CardType
{
    public Rise_Of_Villages(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted)
    {
        return tokensSubmitted.Where(info => info.value == 2).ToList().Count >= 2;
    }
}
