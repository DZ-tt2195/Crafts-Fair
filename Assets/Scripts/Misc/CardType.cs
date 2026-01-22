using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CardType
{
    public CardData dataFile { get; private set; }

    public CardType(CardData dataFile)
    {
        this.dataFile = dataFile;
    }

#region Progress

    public virtual void MasterStart()
    {
    }

    public virtual void ForPlayer(Player player)
    {
    }

    public virtual void MasterEnd()
    {
    }

#endregion

#region  Placard

    public virtual bool CanSubmit(Player player, List<(int value, TokenType type)> tokensSubmitted)
    {
        return false;
    }

#endregion

}
