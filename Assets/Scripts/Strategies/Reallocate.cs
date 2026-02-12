using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Reallocate : CardType
{
    public Reallocate(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (6, TokenType.BookIcon), logged);
        int max = 2;
        for (int i = 1; i<=max; i++)
            Log.inst.NewDecisionContainer(() => Downgrade(player, i, max, logged));
    }
    void Downgrade(Player player, int num, int max, int logged)
    {
        List<TokenDisplay> canLose = player.OfNumber(FindNumber.Minimum, Player.AllTokens(), Player.AllLevelsBut(1), 1);
        MakeDecision.inst.ChooseDisplayOnScreen(canLose, AutoTranslate.Ask_Downgrade(AutoTranslate.TokenIcon(), num.ToString(), max.ToString()), DowngradeToken);

        void DowngradeToken((int level, TokenType type) info)
        {
            player.UpDowngradeToken(1, info, -1, logged);
        }
    }
}
