using UnityEngine;
using System.Collections.Generic;

public class Sequential_Sword : CardType
{
    public Sequential_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return SequentialLevels(tokensSubmitted, TokenType.SwordIcon, 4);
    }
}
