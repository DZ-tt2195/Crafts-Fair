using UnityEngine;
using System.Collections.Generic;

public class Squire : CardType
{
    public Squire(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.SwordIcon);
    }
}
