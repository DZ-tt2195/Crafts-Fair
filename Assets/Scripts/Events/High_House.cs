using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class High_House : CardType
{
    public High_House(CardData dataFile) : base(dataFile)
    {
    }

    public override void EventEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (6, TokenType.HouseIcon), logged);
        for (int i = 0; i<2; i++)
            Log.inst.NewDecisionContainer(() => RemoveHouse(player, logged));
    }
    void RemoveHouse(Player player, int logged)
    {
        List<TokenDisplay> canLose = player.OfNumber(FindNumber.Minimum, new() {TokenType.HouseIcon}, 1);
        MakeDecision.inst.ChooseDisplayOnScreen(canLose, AutoTranslate.Ask_Lose(AutoTranslate.HouseIcon()), LoseToken);

        void LoseToken((int level, TokenType type) info)
        {
            player.AddLoseToken(-1, info, logged);
        }
    }
}
