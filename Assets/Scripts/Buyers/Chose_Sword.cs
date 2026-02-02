using UnityEngine;
using System.Collections.Generic;

public class Chose_Sword : CardType
{
    public Chose_Sword(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.SwordIcon);
    }
}
