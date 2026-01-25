using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
public class TakeTurn : Turn
{
    public override void ForPlayer(Player player)
    {
        player.DrawPlacardRPC(1);
        Log.inst.NewDecisionContainer(() => TokenStuff(player));
        Log.inst.NewDecisionContainer(() => MakeSubmission(player, new(), null));
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

    void MakeSubmission(Player player, List<(int value, TokenType type)> submittedTokens, DecisionContainer rewind)
    {
        int minimum = 2;
        List<Card> playerPlacards = player.GetPlacards();
        List<TokenDisplay> tokensToSubmit = player.OfNumber(FindNumber.Minimum, 1);
        DecisionContainer restartContainer = rewind;

        if (rewind == null)
        {
            restartContainer = Log.inst.currentContainer;
            if (player.GetAllTokens().Item1 < minimum || playerPlacards.Count < minimum)
            {
                NoSubmission();
                return;
            }
        }

        List<Card> placardsToSubmit = new();
        foreach (Card card in playerPlacards)
        {
            if (submittedTokens.Count >= 2 && card.thisCard.CanSubmit(player, submittedTokens))
            {
                placardsToSubmit.Add(card);
                card.selectMe.SetBorder(true);
            }
            else
            {
                card.selectMe.SetBorder(false);
            }
        }

        List<TextButtonInfo> textOptions = new();
        if (submittedTokens.Count >= minimum && placardsToSubmit.Count >= minimum)
            textOptions.Add(new(AutoTranslate.Confirm(), SubmitEverything));
        if (submittedTokens.Count == 0)
            textOptions.Add(new(AutoTranslate.Decline(), NoSubmission));
        else
            textOptions.Add(new(AutoTranslate.Undo_Submission(), UndoAll));

        MakeDecision.inst.ChooseTextButton(textOptions, AutoTranslate.Ask_Submission(), false);
        MakeDecision.inst.ChooseDisplayOnScreen(tokensToSubmit, AutoTranslate.Ask_Submission(), SubmitToken, false);

        void NoSubmission()
        {
            Log.inst.AddMyText(false, OnlineTranslate.Online_No_Submission(player.name));            
        }

        void UndoAll()
        {
            Log.inst.InvokeUndo(rewind, false);
        }

        void SubmitEverything()
        {
            Log.inst.AddMyText(true, OnlineTranslate.Online_Make_Submission(player.name, submittedTokens.Count.ToString(), placardsToSubmit.Count.ToString()));
            int totalScore = 0;
            foreach (Card card in placardsToSubmit)
            {
                totalScore += card.dataFile.crownAmount;
                player.DiscardPlacardRPC(card, 1);
                card.selectMe.SetBorder(false);
            }
            player.ScoreRPC(totalScore, 0);
        }

        void SubmitToken((int value, TokenType type) info)
        {
            player.ChangeTokenRPC(-1, info);
            List<(int, TokenType)> newList = submittedTokens;
            newList.Add(info);
            Log.inst.NewDecisionContainer(() => MakeSubmission(player, newList, restartContainer));
        }
    }

    public override void MasterEnd()
    {
        PhotonCompatible.InstantChangeRoomProp(ConstantStrings.TurnNumber, TurnManager.inst.GetInt(ConstantStrings.TurnNumber)+1);                
    }
}
