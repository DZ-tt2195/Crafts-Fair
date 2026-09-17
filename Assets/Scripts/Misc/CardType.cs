using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CardType : GeneralEffects
{
    public Card cardObject {get; private set;}
    public CardData dataFile { get; private set; }

    public CardType(Card card, CardData dataFile)
    {
        this.cardObject = card;
        this.dataFile = dataFile;
    }

    public virtual void TwistEffect(Player player, int logged)
    {
    }

    public virtual bool CanSell(Player player, Dictionary<TokenType, int[]> soldTokens)
    {
        return false;
    }

}
