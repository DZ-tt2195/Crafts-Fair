using System.Collections.Generic;
using UnityEngine;
using System;

public class Remake : CardType
{
    public Remake(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        List<TokenDisplay> canAdvance = player.OfNumber(FindNumber.Minimum, Player.AllTokens(), Player.AllLevels(), 1);
        MakeDecision.inst.ChooseDisplayOnScreen(canAdvance, AutoTranslate.Ask_Token_Type(), DuplicateThis);

        void DuplicateThis((int level, TokenType type) info)
        {
            player.AddLoseToken(1, info, logged);
        }
    }
}
