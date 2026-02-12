using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Smelt : CardType
{
    public Smelt(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        player.AddLoseToken(1, (6, TokenType.ToolIcon), logged);
        Log.inst.NewDecisionContainer(() => RemoveToken(player, TokenType.ArtIcon, logged));
        Log.inst.NewDecisionContainer(() => RemoveToken(player, TokenType.HouseIcon, logged));
        Log.inst.NewDecisionContainer(() => RemoveToken(player, TokenType.BookIcon, logged));
    }
    void RemoveToken(Player player, TokenType type, int logged)
    {
        List<TokenDisplay> canLose = player.OfNumber(FindNumber.Minimum, new() {type}, Player.AllLevels(), 1);
        MakeDecision.inst.ChooseDisplayOnScreen(canLose, AutoTranslate.Ask_Lose(Translator.inst.Translate(type.ToString()), "1", "1"), LoseToken);

        void LoseToken((int level, TokenType type) info)
        {
            player.AddLoseToken(-1, info, logged);
        }
    }
}
