using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
public class TakeTurn : Turn
{
    public override void MasterStart()
    {
        int currentTurn = TurnManager.inst.GetInt(ConstantStrings.TurnNumber);
        Log.inst.MasterText(true, AutoTranslate.Blank());
        Log.inst.MasterText(true, OnlineTranslate.Online_Next_Turn(currentTurn.ToString()));
    }
    public override void ForPlayer(Player player)
    {
        Log.inst.NewDecisionContainer(() => ChooseToken(player));
        Log.inst.NewDecisionContainer(() => MakeSubmission(player, new(), null));
    }
    void ChooseToken(Player player)
    {
        List<TextButtonInfo> addTokens = new()
        {
            new(AutoTranslate.ArtIcon(), () => AddThis(TokenType.ArtIcon)),
            new(AutoTranslate.HouseIcon(), () => AddThis(TokenType.HouseIcon)),
            new(AutoTranslate.SwordIcon(), () => AddThis(TokenType.SwordIcon)),
            new(AutoTranslate.TechIcon(), () => AddThis(TokenType.TechIcon))
        };
        MakeDecision.inst.ChooseTextButton(addTokens, AutoTranslate.Ask_Token_Type());

        void AddThis(TokenType type)
        {
            TurnManager.inst.WillChangePlayerProperty(player, ConstantStrings.ChosenToken, type.ToString());
            Log.inst.AddMyText(true, OnlineTranslate.Online_Chose_Token(player.name, type.ToString()));
            player.AddRemoveToken(1, (1, type), 1);
            Log.inst.NewDecisionContainer(() => AdvanceToken(player, type));
        }
    }
    void AdvanceToken(Player player, TokenType token)
    {
        List<TokenDisplay> canAdvance = player.OfNumber(FindNumber.Minimum, 1).Where(display => display.info.level != 6 && display.info.type == token).ToList();
        MakeDecision.inst.ChooseDisplayOnScreen(canAdvance, AutoTranslate.Ask_Upgrade(token.ToString()), AdvanceThis);

        void AdvanceThis((int level, TokenType type) info)
        {
            player.UpDowngradeToken(1, info, (info.level+1, info.type), 1);
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
                card.selectMe.SetBorder(true, Color.yellow);
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
            TurnManager.inst.WillChangePlayerProperty(player, ConstantStrings.PlacardsSubmitted, 0);
            Log.inst.AddMyText(false, OnlineTranslate.Online_No_Submission(player.name));            
        }

        void UndoAll()
        {
            Log.inst.InvokeUndo(rewind, false);
        }

        void SubmitEverything()
        {
            Log.inst.AddMyText(true, OnlineTranslate.Online_Make_Submission(player.name, submittedTokens.Count.ToString(), placardsToSubmit.Count.ToString()));
            TurnManager.inst.WillChangePlayerProperty(player, ConstantStrings.PlacardsSubmitted, placardsToSubmit.Count);
            int totalScore = 0;
            foreach (Card card in placardsToSubmit)
            {
                totalScore += card.dataFile.crownAmount;
                player.DiscardPlacardRPC(card, 1);
                card.selectMe.SetBorder(false);
            }
            player.ScoreRPC(totalScore, 0, true);
        }

        void SubmitToken((int value, TokenType type) info)
        {
            player.AddRemoveToken(-1, info);
            List<(int, TokenType)> newList = submittedTokens;
            newList.Add(info);
            Log.inst.NewDecisionContainer(() => MakeSubmission(player, newList, restartContainer));
        }
    }
    public override void MasterEnd()
    {
        ExitGames.Client.Photon.Hashtable toChange = new();
        bool twistTriggers = false;

        foreach (Player player in CreateGame.inst.GetPlayers())
        {
            string selectedToken = TurnManager.inst.GetString(ConstantStrings.ChosenToken, player);
            Debug.Log($"{player.name}, {selectedToken}");

            string targetString = ConstantStrings.TokenCounter(selectedToken);
            if (toChange.ContainsKey(targetString))
            {
                int currentValue = (int)toChange[targetString];
                toChange[targetString] = currentValue-1;
            }
            else
            {
                int currentCounter = TurnManager.inst.GetInt(targetString);
                toChange[targetString] = currentCounter - 1;
            }
            if ((int)toChange[targetString] <= 0)
                twistTriggers = true;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(toChange);
        PhotonCompatible.InstantChangeRoomProp(ConstantStrings.TurnNumber, TurnManager.inst.GetInt(ConstantStrings.TurnNumber)+1);
        if (twistTriggers)
            PhotonCompatible.InstantChangeRoomProp(ConstantStrings.NextPhase, nameof(ResolveEvents));
    }
}
