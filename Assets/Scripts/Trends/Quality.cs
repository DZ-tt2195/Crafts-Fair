using UnityEngine;
using System.Collections.Generic;

public class Quality : CardType
{
    public Quality(CardData dataFile) : base(dataFile)
    {
    }

    public override void TrendEffect(Player player, int logged)
    {
        List<TextButtonInfo> addTokens = new()
        {
            new(AutoTranslate.ArtIcon(), () => AddThis(TokenType.ArtIcon)),
            new(AutoTranslate.HouseIcon(), () => AddThis(TokenType.HouseIcon)),
            new(AutoTranslate.ToolIcon(), () => AddThis(TokenType.ToolIcon)),
            new(AutoTranslate.TechIcon(), () => AddThis(TokenType.TechIcon))
        };
        MakeDecision.inst.ChooseTextButton(addTokens, AutoTranslate.Ask_Token_Type());

        void AddThis(TokenType type)
        {
            player.AddLoseToken(1, (4, type), logged);
        }
    }
}
