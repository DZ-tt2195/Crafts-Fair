using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Rise_Of_Empires : CardType
{
    public Rise_Of_Empires(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted, List<CardData> placardsSubmitted)
    {
        return tokensSubmitted.Where(info => info.value == 6).ToList().Count >= 2;
    }
}
