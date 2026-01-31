using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rise_Of_Empires : CardType
{
    public Rise_Of_Empires(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return tokensSubmitted.Where(info => info.level == 6).ToList().Count >= 2;
    }
}
