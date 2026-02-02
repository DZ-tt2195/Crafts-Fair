using UnityEngine;
using System.Collections.Generic;

public class Chose_Sword : CardType
{
    public Chose_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.SwordIcon);
    }
}
