using Photon.Pun;
using UnityEngine;
using TMPro;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public enum FindNumber {Exact, Minimum, Maximum, Not}
public class Player : PhotonCompatible
{

#region Setup

    bool initialized = false;
    [ReadOnly] public bool endPause = true;
    [SerializeField] Transform keepHand;
    [SerializeField] TMP_Text crownText;
    public Dictionary<string, bool> uiDictionary = new();
    [SerializeField] List<TokenDisplay> allCoinDisplays = new();
    [SerializeField] List<TokenDisplay> allBoneDisplays = new();
    [SerializeField] List<TokenDisplay> allWeaponDisplays = new();
    [SerializeField] List<TokenDisplay> allTextDisplays = new();
    List<Card> myPlacards;
    int myScore;
    Dictionary<TokenType, int[]> myTokens;

    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();

        List<string> toAdd = new() { ConstantStrings.MyPlacards, ConstantStrings.MyDiscard, ConstantStrings.MyScore, TokenType.Art.ToString(), TokenType.House.ToString(), TokenType.Sword.ToString(), TokenType.Tech.ToString() };
        foreach (string next in toAdd)
            uiDictionary.Add(next, true);

        Invoke(nameof(Beginning), 1f);
    }

    void Beginning()
    {
        if (photonView.AmOwner && !initialized)
            DoFunction(() => SendName(PlayerPrefs.GetString(ConstantStrings.MyUserName)), RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SendName(string username)
    {
        initialized = true;
        this.name = username;
        SetToPlayerProps();

        Button resignButton = GameObject.Find("Resign Button").GetComponent<Button>();
        if (photonView.AmOwner)
        {
            CreateGame.inst.mainPlayer = this;
            resignButton.onClick.AddListener(() => TurnManager.inst.TextForEnding(OnlineTranslate.Online_Player_Resigned(this.name), GetThisPlayerPosition(PhotonNetwork.LocalPlayer)));
            StartTurn();
        }
    }
    void SetToPlayerProps()
    {
        myScore = TurnManager.inst.GetInt(ConstantStrings.MyScore, this);
        myPlacards = TurnManager.inst.GetCardList(ConstantStrings.MyPlacards, this);
        myTokens = new Dictionary<TokenType, int[]>();
        foreach (TokenType value in Enum.GetValues(typeof(TokenType)))
        {
            int[] array = TurnManager.inst.GetIntArray(value.ToString(), this);
            myTokens.Add(value, array);
        }        
    }

    #endregion

#region Hand
    public List<Card> GetPlacards() => myPlacards;
    public void DrawPlacardRPC(int amount, int logged = 0)
    {
        if (amount <= 0)
            return;

        List<Card> myDeck = TurnManager.inst.GetCardList(ConstantStrings.MyDeck, this);
        while (myDeck.Count < amount)
        {
            List<Card> myDiscard = TurnManager.inst.GetCardList(ConstantStrings.MyDiscard);
            myDiscard = myDiscard.Shuffle();
            myDeck.AddRange(myDiscard);
            TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyDiscard, new int[0]);
        }

        List<Card> toDraw = new();
        for (int i = 0; i < amount; i++)
        {
            Card card = myDeck[i];
            Log.inst.AddMyText(false, OnlineTranslate.Online_Draw_Placard(this.name, card.name), logged);
            toDraw.Add(card);
        }
        Log.inst.NewRollback(() => DrawPlacard(toDraw));
    }
    void DrawPlacard(List<Card> cardsToAdd)
    {
        List<Card> myDeck = TurnManager.inst.GetCardList(ConstantStrings.MyDeck, this);

        if (!Log.inst.forward)
        {
            for (int i = cardsToAdd.Count-1; i>= 0; i--)
            {
                Card card = cardsToAdd[i];
                card.transform.SetParent(null);
                myPlacards.Remove(card);
                myDeck.Insert(0, card);
            }
        }
        else
        {
            for (int i = 0; i < cardsToAdd.Count; i++)
            {
                Card card = cardsToAdd[i];
                myPlacards.Add(card);
                myDeck.Remove(card);
            }
        }
        myPlacards = myPlacards.OrderBy(card => card.dataFile.crownAmount).ThenBy(card => card.dataFile.cardName).ToList();
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyPlacards, TurnManager.inst.ConvertCardList(myPlacards)); uiDictionary[ConstantStrings.MyPlacards] = true;
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyDeck, TurnManager.inst.ConvertCardList(myDeck));
    }
    public void DiscardPlacardRPC(Card card, int logged = 0)
    {
        Log.inst.NewRollback(() => DiscardPlacard(card));
        Log.inst.AddMyText(false, OnlineTranslate.Online_Discard_Placard(this.name, card.name), logged);
    }
    void DiscardPlacard(Card card)
    {
        List<Card> myDiscard = TurnManager.inst.GetCardList(ConstantStrings.MyDiscard, this);

        if (!Log.inst.forward)
        {
            myPlacards.Add(card);
            myDiscard.Remove(card);
        }
        else
        {
            myPlacards.Remove(card);
            myDiscard.Add(card);
            card.transform.SetParent(null);
        }
        myPlacards = myPlacards.OrderBy(card => card.dataFile.crownAmount).ThenBy(card => card.dataFile.cardName).ToList();
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyPlacards, TurnManager.inst.ConvertCardList(myPlacards)); uiDictionary[ConstantStrings.MyPlacards] = true;
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyDiscard, TurnManager.inst.ConvertCardList(myDiscard)); uiDictionary[ConstantStrings.MyDiscard] = true;
    }

    #endregion

#region Resources

    public int GetScore() => myScore;
    public void ScoreRPC(int num, int logged = 0, bool important = false)
    {
        if (num == 0)
            return;
        if (num > 0)
            Log.inst.AddMyText(important, OnlineTranslate.Online_Add_Score(this.name, num.ToString()), logged);
        else
            Log.inst.AddMyText(important, OnlineTranslate.Online_Lose_Score(this.name, num.ToString()), logged);
        Log.inst.NewRollback(() => ChangeScore(num));
    }
    void ChangeScore(int num)
    {
        myScore += (!Log.inst.forward) ? -num : num;
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyScore, myScore); uiDictionary[ConstantStrings.MyScore] = true;
    }
    public void ChangeTokenRPC(int num, (int value, TokenType token) info, int logged = 0, bool important = false)
    {
        if (num == 0)
            return;
        if (num > 0)
            Log.inst.AddMyText(important, OnlineTranslate.Online_Add_Token(this.name, num.ToString(), Translator.ConvertToken(info)), logged);
        else
            Log.inst.AddMyText(important, OnlineTranslate.Online_Remove_Token(this.name, num.ToString(), Translator.ConvertToken(info)), logged);
        Log.inst.NewRollback(() => ChangeToken(num, info));
    }
    void ChangeToken(int num, (int value, TokenType token) info)
    {
        int[] tokenArray = myTokens[info.token];
        tokenArray[info.value] += (Log.inst.forward) ? num : -num;
        TurnManager.inst.WillChangePlayerProperty(this, info.token.ToString(), tokenArray); uiDictionary[info.token.ToString()] = true;
    }

    #endregion

#region Turns

    void Update()
    {
        if (photonView.AmOwner && Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                GetPlayers(true);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                PhotonNetwork.Disconnect();
        }
    }
    public void StartTurn()
    {
        //this.DoFunction(() => this.ChangeButtonColor(false));
        CreateGame.inst.SwitchToPlayer(CreateGame.inst.mainPlayer);
        InstantChangePlayerProp(this, ConstantStrings.Waiting, false);
        endPause = true;

        (string phase, Action action) = TurnManager.inst.GetTurnAction(this);
        if (phase != nameof(WaitForJoiners) && phase != nameof(DisplayTwists))
            Log.inst.AddMyText(true, AutoTranslate.Blank());

        Log.inst.NewDecisionContainer(() => action(), 0);
        Log.inst.NewDecisionContainer(() => EndTurn(), -1);
        Log.inst.PopStack();
    }
    void EndTurn()
    {
        Log.inst.inReaction.Add(Done);
        if (endPause)
        {
            string instructions = (Log.inst.undosInLog.Count >= 1) ? AutoTranslate.Pause_to_Undo() : AutoTranslate.Pause_to_Read();
            MakeDecision.inst.ChooseTextButton(new() { new(AutoTranslate.Done()) }, instructions,false);
        }

        void Done()
        {
            Log.inst.DoneWithTurn();
            InstantChangePlayerProp(this, ConstantStrings.Waiting, true);
        }
    }

    #endregion

#region UI
    public (int, Dictionary<TokenType, int[]>) GetAllTokens()
    {
        int totalTokens = 0;
        foreach (TokenType value in Enum.GetValues(typeof(TokenType)))
        {
            int[] tokenArray = myTokens[value];
            for (int i = 0; i<tokenArray.Length; i++)
                totalTokens += tokenArray[i];
        }
        return (totalTokens, myTokens);
    }
    public void UpdateUI(bool forcedUpdate)
    {
        List<string> uiKeys = uiDictionary.Keys.ToList();
        int myPosition = GetThisPlayerPosition(PhotonNetwork.LocalPlayer);
        int thisPlayerPosition = GetThisPlayerPosition(this.photonView.Owner);

        if (forcedUpdate)
        {
            SetToPlayerProps();
            foreach (var key in uiKeys)
                uiDictionary[key] = true;
        }

        if (uiDictionary[ConstantStrings.MyPlacards])
        {
            List<Vector2> handPositions = ObjectPositions(myPlacards.Count, -1125, 475, 225, -550, true);
            for (int i = 0; i < myPlacards.Count; i++)
            {
                Card nextCard = myPlacards[i];
                if (nextCard.transform.parent != keepHand)
                {
                    nextCard.transform.SetParent(keepHand);
                    nextCard.transform.localPosition = new(0, -1000);
                }
                nextCard.transform.SetSiblingIndex(i);
                nextCard.selectMe.SetBorder(false);
                nextCard.MoveCardRPC(handPositions[i], 0.25f, Vector3.one);

                if (myPosition == -1 || thisPlayerPosition == myPosition)
                    nextCard.FlipCardRPC(1, 0.25f);
            }
        }

        if (uiDictionary[ConstantStrings.MyDiscard])
        {
            foreach (Card card in TurnManager.inst.GetCardList(ConstantStrings.MyDiscard, this))
                card.transform.SetParent(null);
        }

        if (uiDictionary[TokenType.Art.ToString()])
            ApplyToken(TokenType.Art, allCoinDisplays);
        if (uiDictionary[TokenType.House.ToString()])
            ApplyToken(TokenType.House, allBoneDisplays);
        if (uiDictionary[TokenType.Sword.ToString()])
            ApplyToken(TokenType.Sword, allWeaponDisplays);
        if (uiDictionary[TokenType.Tech.ToString()])
            ApplyToken(TokenType.Tech, allTextDisplays);

        void ApplyToken(TokenType type, List<TokenDisplay> list)
        {
            int[] array = myTokens[type];
            for (int i = 1; i<array.Length; i++)
                list[i].ChangeInfo(i, type, array[i].ToString());
        }

        if (uiDictionary[ConstantStrings.MyScore])
        {
            crownText.text = KeywordTooltip.instance.EditText($"{GetScore()} {AutoTranslate.CrownIcon()}");
        }

        foreach (var key in uiKeys)
            uiDictionary[key] = false;
    }
    List<Vector2> ObjectPositions(int objectAmount, float start, float end, float gap, float fixedPosition, bool useX)
    {
        float midPoint = (start + end) / 2f;
        int maxFit = (int)((Mathf.Abs(start) + Mathf.Abs(end)) / gap);
        float offByOne = objectAmount - 1;

        List<Vector2> toReturn = new();
        for (int i = 0; i<objectAmount; i++)
        {
            float starting = (objectAmount <= maxFit) ? midPoint - (gap * (offByOne / 2f)) : start;
            float difference = (objectAmount <= maxFit) ? gap : gap * (maxFit / offByOne);

            if (useX)
                toReturn.Add(new(starting + difference * i, fixedPosition));
            else
                toReturn.Add(new(fixedPosition, starting + difference * i));
        }
        return toReturn;
    } 

#endregion

#region  Helpers
    public List<TokenDisplay> OfNumber(FindNumber toFind, int number)
    {
        List<TokenDisplay> toReturn = new();

        ApplyToken(myTokens[TokenType.Art], allCoinDisplays);
        ApplyToken(myTokens[TokenType.House], allBoneDisplays);
        ApplyToken(myTokens[TokenType.Sword], allWeaponDisplays);
        ApplyToken(myTokens[TokenType.Tech], allTextDisplays);

        void ApplyToken(int[] array, List<TokenDisplay> list)
        {
            for (int i = 1; i<array.Length; i++)
            {
                switch (toFind)
                {
                    case FindNumber.Exact:
                        if (array[i] == number) toReturn.Add(list[i]); break;
                    case FindNumber.Not:
                        if (array[i] != number) toReturn.Add(list[i]); break;
                    case FindNumber.Minimum:
                        if (array[i] >= number) toReturn.Add(list[i]); break;
                    case FindNumber.Maximum:
                        if (array[i] <= number) toReturn.Add(list[i]); break;
                }
            }
        }
        return toReturn;
    }

#endregion

}
