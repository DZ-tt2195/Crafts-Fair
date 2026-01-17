using Photon.Pun;
using UnityEngine;
using TMPro;
using MyBox;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public enum FindNumber {Exact, Minimum, Maximum}
public class Player : PhotonCompatible
{

#region Setup

    bool initialized = false;
    public bool endPause = true;
    public int myPosition { get; private set; }

    Button resignButton;
    [SerializeField] Transform keepHand;
    public Dictionary<string, bool> uiDictionary = new();
    [SerializeField] List<TokenDisplay> allCoinDisplays = new();
    [SerializeField] List<TokenDisplay> allBoneDisplays = new();
    [SerializeField] List<TokenDisplay> allWeaponDisplays = new();
    [SerializeField] List<TokenDisplay> allTextDisplays = new();

    protected override void Awake()
    {
        base.Awake();
        this.bottomType = this.GetType();
        resignButton = GameObject.Find("Resign Button").GetComponent<Button>();

        List<string> toAdd = new() { ConstantStrings.MyPlacards, ConstantStrings.MyDiscard, ConstantStrings.MyScore, ConstantStrings.Tokens };
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
        this.transform.SetParent(CreateGame.inst.canvas.transform);
        this.transform.localPosition = Vector3.zero;
        this.transform.SetAsFirstSibling();

        initialized = true;
        this.name = username;
        myPosition = (int)GetPlayerProperty(this, ConstantStrings.MyPosition);
        CreateGame.inst.listOfPlayers.Insert(myPosition, this);

        resignButton = GameObject.Find("Resign Button").GetComponent<Button>();
        if (photonView.AmOwner)
        {
            resignButton.onClick.AddListener(() => TurnManager.inst.TextForEnding("Player_Resigned", this.name, "", "", myPosition));
            StartTurn();
        }
    }

    #endregion

#region Hand

    public List<Card> GetPlacards() => TurnManager.inst.GetCardList(ConstantStrings.MyPlacards, this);

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
            Log.inst.AddMyText(false, "Draw_Card", this.name, card.name, "", logged);
            toDraw.Add(card);
        }
        Log.inst.NewRollback(() => DrawPlacard(toDraw));
    }

    void DrawPlacard(List<Card> cardsToAdd)
    {
        List<Card> myPlacards = GetPlacards();
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
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyPlacards, TurnManager.inst.ConvertCardList(myPlacards)); uiDictionary[ConstantStrings.MyPlacards] = true;
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyDeck, TurnManager.inst.ConvertCardList(myDeck));
    }

    public void DiscardPlacardRPC(Card card, int logged)
    {
        Log.inst.NewRollback(() => DiscardPlacard(card));
        Log.inst.AddMyText(false, "Discard_Card", this.name, card.name, "", logged);
    }

    void DiscardPlacard(Card card)
    {
        List<Card> myPlacards = GetPlacards();
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
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyPlacards, TurnManager.inst.ConvertCardList(myPlacards)); uiDictionary[ConstantStrings.MyPlacards] = true;
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyDiscard, TurnManager.inst.ConvertCardList(myDiscard)); uiDictionary[ConstantStrings.MyDiscard] = true;
    }

    #endregion

#region Resources

    public int GetScore() => TurnManager.inst.GetInt(ConstantStrings.MyScore, this);
    public void ScoreRPC(int num, int logged = 0)
    {
        if (num == 0)
            return;
        if (num > 0)
            Log.inst.AddMyText(false, "Add_Health_Player", this.name, "", num.ToString(), logged);
        else
            Log.inst.AddMyText(false, "Lose_Health_Player", this.name, "", Mathf.Abs(num).ToString(), logged);
        Log.inst.NewRollback(() => ChangeScore(num));
    }
    void ChangeScore(int num)
    {
        int total = TurnManager.inst.GetInt(ConstantStrings.MyScore, this);
        total += (!Log.inst.forward) ? -num : num;
        TurnManager.inst.WillChangePlayerProperty(this, ConstantStrings.MyScore, total); uiDictionary[ConstantStrings.MyScore] = true;
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
        InstantChangePlayerProp(this, ConstantStrings.Waiting, false);
        endPause = true;

        (string phase, Action action) = TurnManager.inst.GetTurnAction(this);
        if (phase != nameof(SetupWait))
            Log.inst.AddMyText(true, "Blank", "", "", "");

        Log.inst.NewDecisionContainer(() => action(), 0);
        Log.inst.NewDecisionContainer(() => EndTurn(), -1);
        Log.inst.PopStack();
    }

    void EndTurn()
    {
        Log.inst.inReaction.Add(Done);
        if (endPause)
        {
            string instructions = (Log.inst.undosInLog.Count >= 1) ? "Pause_to_Undo" : "Pause_to_Read";
            MakeDecision.inst.ChooseTextButton(new() { new("Done", "", "", "", Color.white) }, instructions, "", "", "", false);
        }

        void Done()
        {
            Log.inst.DoneWithTurn();
            InstantChangePlayerProp(this, ConstantStrings.Waiting, true);
        }
    }

    #endregion

#region UI

    public int[] GetCoins() => TurnManager.inst.GetIntArray(ConstantStrings.MyCoins, this);
    public int[] GetBones() => TurnManager.inst.GetIntArray(ConstantStrings.MyBones, this);
    public int[] GetWeapons() => TurnManager.inst.GetIntArray(ConstantStrings.MyWeapons, this);
    public int[] GetTexts() => TurnManager.inst.GetIntArray(ConstantStrings.MyTexts, this);
    public Dictionary<TokenType, int[]> GetAllTokens() => new Dictionary<TokenType, int[]>() {{ TokenType.Coin, GetCoins() },{ TokenType.Bone, GetBones() },{ TokenType.Weapon, GetWeapons() },{ TokenType.Text, GetTexts() }};
    public void UpdateUI(bool forcedUpdate)
    {
        List<string> uiKeys = uiDictionary.Keys.ToList();

        if (forcedUpdate)
        {
            foreach (var key in uiKeys)
                uiDictionary[key] = true;
        }

        List<Card> myHand = GetPlacards();
        if (uiDictionary[ConstantStrings.MyPlacards])
        {
            List<Vector2> handPositions = ObjectPositions(myHand.Count, -700, 475, 225, -550, true);

            int thisPlayerPosition = (int)GetPlayerProperty(PhotonNetwork.LocalPlayer, ConstantStrings.MyPosition.ToString());
            for (int i = 0; i < myHand.Count; i++)
            {
                Card nextCard = myHand[i];
                if (nextCard.transform.parent != keepHand)
                {
                    nextCard.transform.SetParent(keepHand);
                    nextCard.transform.localPosition = new(0, -1000);
                }
                nextCard.transform.SetSiblingIndex(i);
                nextCard.MoveCardRPC(handPositions[i], 0.25f, Vector3.one);

                if (thisPlayerPosition == -1 || thisPlayerPosition == myPosition)
                    nextCard.FlipCardRPC(1, 0.25f, 0);
            }
        }

        if (uiDictionary[ConstantStrings.MyDiscard])
        {
            foreach (Card card in TurnManager.inst.GetCardList(ConstantStrings.MyDiscard, this))
                card.transform.SetParent(null);
        }

        if (uiDictionary[ConstantStrings.Tokens])
        {
            ApplyToken(TokenType.Coin, GetCoins(), allCoinDisplays);
            ApplyToken(TokenType.Bone, GetBones(), allBoneDisplays);
            ApplyToken(TokenType.Weapon, GetWeapons(), allWeaponDisplays);
            ApplyToken(TokenType.Text, GetTexts(), allTextDisplays);

            void ApplyToken(TokenType type, int[] array, List<TokenDisplay> list)
            {
                for (int i = 0; i<array.Length; i++)
                    list[i].ChangeInfo(i, type, array[i].ToString());
            }
        }

        if (uiDictionary[ConstantStrings.MyScore])
        {
            //myUI.infoText.text = KeywordTooltip.instance.EditText("fill in information");
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
    
    public List<TokenDisplay> OfNumber(FindNumber toFind, int number)
    {
        List<TokenDisplay> toReturn = new();

        ApplyToken(GetCoins(), allCoinDisplays);
        ApplyToken(GetBones(), allBoneDisplays);
        ApplyToken(GetWeapons(), allWeaponDisplays);
        ApplyToken(GetTexts(), allTextDisplays);

        void ApplyToken(int[] array, List<TokenDisplay> list)
        {
            for (int i = 0; i<array.Length; i++)
            {
                switch (toFind)
                {
                    case FindNumber.Exact:
                        if (array[i] == number) toReturn.Add(list[i]); break;
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
