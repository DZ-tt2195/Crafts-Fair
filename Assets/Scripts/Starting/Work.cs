using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Work : CardType
{
    public Work(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        Log.inst.NewDecisionContainer(() => TokenStuff(player), 0);
        Log.inst.NewDecisionContainer(() => Submit(player, new(), new()), 0);
    }

    void TokenStuff(Player player)
    {
        List<TextButtonInfo> addTokens = new()
        {
            new(AutoTranslate.Coin1(), () => player.ChangeTokenRPC(1, (1, TokenType.Coin))),
            new(AutoTranslate.Bone1(), () => player.ChangeTokenRPC(1, (1, TokenType.Bone))),
            new(AutoTranslate.Weapon1(), () => player.ChangeTokenRPC(1, (1, TokenType.Weapon))),
            new(AutoTranslate.Text1(), () => player.ChangeTokenRPC(1, (1, TokenType.Text)))
        };
        MakeDecision.inst.ChooseTextButton(addTokens, AutoTranslate.Add_Or_Advance());

        List<TokenDisplay> canAdvance = player.OfNumber(FindNumber.Minimum, 1).Where(display => display.info.Item1 != 6).ToList();
        MakeDecision.inst.ChooseDisplayOnScreen(canAdvance, AutoTranslate.Add_Or_Advance(), AdvanceThis);

        void AdvanceThis((int value, TokenType type) info)
        {
            player.ChangeTokenRPC(-1, info);
            player.ChangeTokenRPC(1, (info.value+1, info.type));
        }
    }

    void Submit(Player player, List<(int value, TokenType type)> submittedTokens, List<Card> submittedPlacards)
    {
        
    }
}
