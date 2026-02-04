using System.Collections.Generic;
using System;
using System.Linq;
public class Upgrade_All : CardType
{
    public Upgrade_All(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        Dictionary<TokenType, int[]> playerTokens = player.GetTokenDict();
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
            Log.inst.NewDecisionContainer(() => AdvanceToken(player, token));
    }

    void AdvanceToken(Player player, TokenType token)
    {
        List<TokenDisplay> canAdvance = player.OfNumber(FindNumber.Minimum, new(){token}, 1).Where(display => display.info.level != 6).ToList();
        MakeDecision.inst.ChooseDisplayOnScreen(canAdvance, AutoTranslate.Ask_Upgrade(token.ToString()), AdvanceThis);

        void AdvanceThis((int level, TokenType type) info)
        {
            player.UpDowngradeToken(1, info, (info.level+1, info.type), 1);
        }
    }
}
