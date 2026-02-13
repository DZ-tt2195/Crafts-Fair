using System.Collections.Generic;
using System;
using System.Linq;

public class Consult : CardType
{
    public Consult(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        Dictionary<TokenType, int[]> playerTokens = player.GetTokenDict();
        foreach (TokenType token in Enum.GetValues(typeof(TokenType)))
            Log.inst.NewDecisionContainer(() => UpgradeToken(player, token));
    }

    void UpgradeToken(Player player, TokenType token)
    {
        List<TokenDisplay> canUpgrade = player.OfNumber(FindNumber.Minimum, new(){token}, Player.AllLevelsBut(TurnManager.inst.GetInt(ConstantStrings.MaxLevel)), 1);
        MakeDecision.inst.ChooseDisplayOnScreen(canUpgrade, AutoTranslate.Ask_Upgrade(token.ToString(), "1", "1"), UpgradeThis);

        void UpgradeThis((int level, TokenType type) info)
        {
            player.UpDowngradeToken(1, info, 1, 1);
        }
    }
}
