using UnityEngine;
using System.Collections.Generic;

public class Chose_House : CardType
{
    public Chose_House(CardData dataFile) : base(dataFile)
    {
    }

    public override bool CanSubmit(Player player, List<(int level, TokenType type)> tokensSubmitted)
    {
        return TurnManager.inst.GetString(ConstantStrings.ChosenToken, player).Equals(TokenType.HouseIcon);
    }
}
