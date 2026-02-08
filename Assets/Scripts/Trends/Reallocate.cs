using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Reallocate : CardType
{
    public Reallocate(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (6, TokenType.TechIcon), logged);
        for (int i = 0; i<2; i++)
            Log.inst.NewDecisionContainer(() => Downgrade(player, logged));
    }
    void Downgrade(Player player, int logged)
    {
        List<TokenDisplay> canLose = player.OfNumber(FindNumber.Minimum, Player.AllTokens(), Player.AllLevelsBut(1), 1);
        MakeDecision.inst.ChooseDisplayOnScreen(canLose, AutoTranslate.Ask_Downgrade(AutoTranslate.TokenIcon()), DowngradeToken);

        void DowngradeToken((int level, TokenType type) info)
        {
            player.UpDowngradeToken(1, info, (info.level-1, info.type), logged);
        }
    }
}
