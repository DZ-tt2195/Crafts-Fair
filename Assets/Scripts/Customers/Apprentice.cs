using UnityEngine;
using System.Collections.Generic;

public class Apprentice : CardType
{
    public Apprentice(Card card, CardData dataFile) : base(card, dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.ToolIcon.ToString());
    }
}
