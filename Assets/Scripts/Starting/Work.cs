using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Work : CardType
{
    public Work(CardData dataFile) : base(dataFile)
    {
    }

    public override void ForPlayer(Player player)
    {
        Log.inst.NewDecisionContainer(() => TokenStuff(player));
        Log.inst.NewDecisionContainer(() => MakeSubmission(player, new(), new(), null));
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

    void MakeSubmission(Player player, List<(int value, TokenType type)> submittedTokens, List<Card> submittedPlacards, DecisionContainer rewind)
    {
        int minimum = 2;
        List<Card> playerPlacards = player.GetPlacards();
        List<TokenDisplay> tokensToSubmit = player.OfNumber(FindNumber.Minimum, 1);

        if (rewind == null)
        {
            rewind = Log.inst.currentContainer;
            if (player.GetAllTokens().Item1 < minimum || playerPlacards.Count < minimum)
            {
                NoSubmission();
                return;
            }
        }
        List<TextButtonInfo> textOptions = new();
        if (submittedTokens.Count == 4 && submittedPlacards.Count == 4)
        {
            Submit();
            return;
        }
        else if (submittedTokens.Count >= minimum && submittedPlacards.Count >= minimum)
        {
            textOptions.Add(new(AutoTranslate.Confirm(), Submit));
        }
        if (submittedTokens.Count == 0)
            textOptions.Add(new(AutoTranslate.Decline(), NoSubmission));
        else
            textOptions.Add(new(AutoTranslate.Undo_Submission(), UndoAll));

        MakeDecision.inst.ChooseTextButton(textOptions, AutoTranslate.Ask_Submission(), false);
        MakeDecision.inst.ChooseDisplayOnScreen(tokensToSubmit, AutoTranslate.Ask_Submission(), AddToken, false);

        List<Card> placardsToSubmit = new();
        foreach (Card card in playerPlacards)
        {
            if (submittedTokens.Count >= minimum && card.thisCard.CanSubmit(player, submittedTokens, submittedPlacards))
                placardsToSubmit.Add(card);
        }
        MakeDecision.inst.ChooseCardOnScreen(placardsToSubmit, AutoTranslate.Ask_Submission(), AddPlacard, false);

        void NoSubmission()
        {
            Log.inst.AddMyText(false, OnlineTranslate.Online_No_Submission(player.name));            
        }

        void UndoAll()
        {
            Log.inst.InvokeUndo(rewind, false);
        }

        void Submit()
        {
            Log.inst.AddMyText(true, OnlineTranslate.Online_Make_Submission(player.name, submittedTokens.Count.ToString(), submittedPlacards.Count.ToString()));
            int totalScore = 0;
            foreach (Card card in submittedPlacards)
                totalScore += card.dataFile.crownAmount;
            player.ScoreRPC(totalScore, 1);
        }

        void AddToken((int value, TokenType type) info)
        {
            player.ChangeTokenRPC(-1, info);
            List<(int, TokenType)> newList = submittedTokens;
            newList.Add(info);
            Log.inst.NewDecisionContainer(() => MakeSubmission(player, newList, submittedPlacards, rewind));
        }

        void AddPlacard(Card card)
        {
            player.DiscardPlacardRPC(card);
            List<Card> newList = submittedPlacards;
            newList.Add(card);
            Log.inst.NewDecisionContainer(() => MakeSubmission(player, submittedTokens, newList, rewind));
        }
    }
}
