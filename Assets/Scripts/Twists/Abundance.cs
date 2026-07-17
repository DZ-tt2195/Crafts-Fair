using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Abundance : CardType
{
    public Abundance(CardData dataFile) : base(dataFile)
    {
    }

    public override void TwistEffect(Player player, int logged)
    {
        int toMake = Mathf.FloorToInt(player.GetCoins() / 3f);
        for (int i = 1; i<=toMake; i++)
        {
            int number = i;
            Log.inst.NewDecisionContainer(() => AddToken(number));
        }
        void AddToken(int num)
        {
            List<TextButtonInfo> addTokens = new()
            {
                new(AutoTranslate.ArtIcon(), () => AddThis(TokenType.ArtIcon)),
                new(AutoTranslate.HouseIcon(), () => AddThis(TokenType.HouseIcon)),
                new(AutoTranslate.ToolIcon(), () => AddThis(TokenType.ToolIcon)),
                new(AutoTranslate.BookIcon(), () => AddThis(TokenType.BookIcon))
            };
            MakeDecision.inst.ChooseTextButton(addTokens, AutoTranslate.Ask_Create(AutoTranslate.TokenIcon(), num.ToString(), toMake.ToString()));
            void AddThis(TokenType type)
            {
                player.CreateLoseToken(1, (1, type), logged);
            }
        }
    }
}
